using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.CalculationRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database
{
    public class DBRetrieve
    {

        ///////Retrieving FULL Data from Maplestory.db


        
        public static Dictionary<string, CharacterCLS> GetAllCharTrackDB()
        {
            Dictionary<string, CharacterCLS> charDict = new Dictionary<string, CharacterCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string getCharCMD = "SELECT * FROM TrackCharacter";

                using (SqliteCommand selectCMD = new SqliteCommand(getCharCMD, dbCon))
                {
                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            CharacterCLS tempChar = new CharacterCLS();
                            tempChar.ClassName = result.GetString(0);
                            tempChar.UnionRank = result.GetString(1);
                            tempChar.Level = result.GetInt32(2);
                            tempChar.Starforce = result.GetInt32(3);

                            charDict.Add(result.GetString(0), tempChar);
                        }
                    }
                }
            }
            return charDict;
        }

        public static Dictionary<string, CharacterCLS> CompleteCharTrackRetrieve(Dictionary<string, CharacterCLS> BaseList)
        {
            Dictionary<string, CharacterCLS> FinalResult = new Dictionary<string, CharacterCLS>();
            
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                using (SqliteTransaction tran = dbCon.BeginTransaction())
                {


                    string BaseTrackQ = "SELECT * FROM TrackCharacter";
                    using (SqliteCommand cmd = new SqliteCommand(BaseTrackQ, dbCon, tran))
                    {
                        using (SqliteDataReader result = cmd.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                CharacterCLS TChar = BaseList[result.GetString(0)];
                                TChar.UnionRank=result.GetString(1);
                                TChar.Level= result.GetInt32(2);
                                TChar.Starforce= result.GetInt32(3);
                                FinalResult.Add(TChar.ClassName, TChar);
                            }
                        }
                    }

                    string ET = "SELECT * FROM TrackCharEquip";

                    using (SqliteCommand cmd = new SqliteCommand(ET, dbCon, tran))
                    {
                        using (SqliteDataReader result = cmd.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                string CharName = result.GetString(0); 
                                EquipCLS Equip = new EquipCLS();
                                Equip.ClassType = result.GetString(1);
                                Equip.EquipListSlot = result.GetString(2);
                                Equip.StarForce = result.GetInt32(4);

                                Equip.EquipSlot = ComFunc.ReturnRingPend(Equip.EquipListSlot);
                                if (Equip.EquipSlot == "Secondary" || GVar.AccEquips.Contains(Equip.EquipSlot))
                                {
                                    Equip.EquipName = result.GetString(3);
                                }
                                else 
                                { 
                                    Equip.EquipSet = result.GetString(3);
                                }

                                Dictionary<string, EquipCLS> CurrentEList = FinalResult[CharName].EquipmentList;
                                if (!CurrentEList.ContainsKey(Equip.EquipListSlot))
                                {
                                    FinalResult[CharName].EquipmentList.Add(Equip.EquipListSlot, Equip);
                                }
                                else
                                {
                                    FinalResult[CharName].EquipmentList[Equip.EquipListSlot] = Equip;  
                                }
                            }
                        }
                    }

                    string WeapET = "SELECT * FROM TrackCharWeapons";
                    using (SqliteCommand cmd = new SqliteCommand(WeapET, dbCon, tran))
                    {
                        using(SqliteDataReader result = cmd.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                string CharName = result.GetString(0);
                                if (FinalResult.ContainsKey(CharName))
                                {
                                    FinalResult[CharName].CurrentMainWeapon =  result.GetString(1);
                                    FinalResult[CharName].CurrentSecondaryWeapon = result.GetString(2);
                                }
                            }
                            
                        }
                    }

                    string ScrollET = "SELECT * FROM TrackCharEquipScroll";
                    using (SqliteCommand cmd = new SqliteCommand(ScrollET, dbCon, tran))
                    {
                        using (SqliteDataReader result = cmd.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                string CharName = result.GetString(0);

                                string EquipSlot = result.GetString(2);
                                string EquipSet = result.GetString(3);

                                int SlotCount = result.GetInt32(4);
                                int ScrollPerc = result.GetInt32(5);


                                if (FinalResult.ContainsKey(CharName))
                                {
                                    if (FinalResult[CharName].EquipmentList.ContainsKey(EquipSlot))
                                    {
                                        EquipStatsCLS Stat = new EquipStatsCLS();
                                        Stat.STR = result.GetInt32(6);
                                        Stat.DEX = result.GetInt32(7);
                                        Stat.INT = result.GetInt32(8);
                                        Stat.LUK = result.GetInt32(9);
                                        Stat.MaxHP = result.GetInt32(10);
                                        Stat.MaxMP = result.GetInt32(11);
                                        Stat.DEF = result.GetInt32(12);
                                        Stat.ATK = result.GetInt32(13);
                                        Stat.MATK = result.GetInt32(14);
                                        Stat.SPD = result.GetInt32(15);
                                        Stat.JUMP = result.GetInt32(16);

                                        EquipCLS current = FinalResult[CharName].EquipmentList[EquipSlot];
                                        current.SlotCount = SlotCount;
                                        current.SpellTracePerc = ScrollPerc;
                                        current.ScrollStats = Stat;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }


                            }
                        }
                    }
                    string FlameET = "SELECT * FROM TrackCharEquipFlame";
                    using (SqliteCommand cmd = new SqliteCommand(FlameET, dbCon, tran))
                    {
                        using (SqliteDataReader result = cmd.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                string CharName = result.GetString(0);

                                string EquipSlot = result.GetString(2);

                                if (FinalResult.ContainsKey(CharName))
                                {

                                    if (FinalResult[CharName].EquipmentList.ContainsKey(EquipSlot))
                                    {
                                        EquipStatsCLS Stat = new EquipStatsCLS();
                                        Stat.STR = result.GetInt32(4);
                                        Stat.DEX = result.GetInt32(5);
                                        Stat.INT = result.GetInt32(6);
                                        Stat.LUK = result.GetInt32(7);
                                        Stat.MaxHP = result.GetInt32(8);
                                        Stat.MaxMP = result.GetInt32(9);
                                        Stat.DEF = result.GetInt32(10);
                                        Stat.ATK = result.GetInt32(11);
                                        Stat.MATK = result.GetInt32(12);
                                        Stat.SPD = result.GetInt32(13);
                                        Stat.JUMP = result.GetInt32(14);
                                        Stat.AllStat = result.GetInt32(15);
                                        Stat.BD = result.GetInt32(16);
                                        Stat.DMG = result.GetInt32(17);

                                        FinalResult[CharName].EquipmentList[EquipSlot].FlameStats = Stat;

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }

                            }
                        }
                    }
                    string PotET = "SELECT * FROM TrackCharEquipPot";
                    using(SqliteCommand cmd = new SqliteCommand(PotET, dbCon, tran))
                    {
                        using(SqliteDataReader result = cmd.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                string CharName = result.GetString(0);
                                string EquipSlot = result.GetString(2);

                                if(FinalResult.ContainsKey(CharName))
                                {
                                    if (FinalResult[CharName].EquipmentList.ContainsKey(EquipSlot))
                                    {
                                        EquipCLS current = FinalResult[CharName].EquipmentList[EquipSlot];
                                        current.MPotGrade = result.GetInt32(4);
                                        current.MainPot["First"].PotID = result.GetInt32(5);
                                        current.MainPot["Second"].PotID = result.GetInt32(6);
                                        current.MainPot["Third"].PotID = result.GetInt32(7);

                                        current.APotGrade = result.GetInt32(8);
                                        current.AddPot["First"].PotID = result.GetInt32(9);
                                        current.AddPot["Second"].PotID= result.GetInt32(10);
                                        current.AddPot["Third"].PotID = result.GetInt32(11);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }

           
            return FinalResult;
        }







    }
}
