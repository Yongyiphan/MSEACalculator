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
            Dictionary<string, CharacterCLS> Result = new Dictionary<string, CharacterCLS>();
            CharacterCLS TChar = null;
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();
                string BaseTrackQ = "SELECT * FROM TrackCharacter";
                using(SqliteCommand cmd = new SqliteCommand(BaseTrackQ, dbCon))
                {
                    using(SqliteDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            TChar = BaseList[result.GetString(0)];  
                            TChar.UnionRank=result.GetString(1);
                            TChar.Level= result.GetInt32(2);
                            TChar.Starforce= result.GetInt32(3);
                            Result.Add(TChar.ClassName, TChar);
                        }
                    }
                }

                string ScrollET = "SELECT * FROM TrackCharEquipScroll";
                using (SqliteCommand cmd = new SqliteCommand(ScrollET, dbCon))
                {
                    using (SqliteDataReader result = cmd.ExecuteReader())
                    {
                        string CharName = result.GetString(0);
                        EquipCLS Equip = new EquipCLS();
                        Equip.ClassType = result.GetString(1);
                        Equip.EquipListSlot = result.GetString(2);
                        Equip.EquipSet = result.GetString(3);
                        Equip.EquipSlot = ComFunc.ReturnRingPend(Equip.EquipListSlot);
                        string SlotCat = ComFunc.ReturnSetCat(Equip.EquipSlot);

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
                        Equip.ScrollStats = Stat;
                        if(!Result[CharName].EquipmentList.Any(x=> x.EquipListSlot == Equip.EquipListSlot))
                        {
                            Result[CharName].EquipmentList.Add(Equip);
                        }
                        else
                        {
                            Result[CharName].EquipmentList.Single(x => x.EquipListSlot == Equip.EquipListSlot).ScrollStats = Stat;
                        }

                    }
                }
                string FlameET = "SELECT * FROM TrackCharEquipFlame";
                using (SqliteCommand cmd = new SqliteCommand(FlameET, dbCon))
                {
                    using (SqliteDataReader result = cmd.ExecuteReader())
                    {
                        string CharName = result.GetString(0);
                        EquipCLS Equip = new EquipCLS();
                        Equip.ClassType = result.GetString(1);
                        Equip.EquipListSlot = result.GetString(2);
                        Equip.EquipSet = result.GetString(3);
                        Equip.EquipSlot = ComFunc.ReturnRingPend(Equip.EquipListSlot);
                        string SlotCat = ComFunc.ReturnSetCat(Equip.EquipSlot);

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
                        Equip.FlameStats = Stat;
                        if(!Result[CharName].EquipmentList.Any(x=> x.EquipListSlot == Equip.EquipListSlot))
                        {
                            Result[CharName].EquipmentList.Add(Equip);
                        }
                        else
                        {
                            Result[CharName].EquipmentList.Single(x => x.EquipListSlot == Equip.EquipListSlot).FlameStats = Stat;
                        }

                    }
                }
            }


            return Result;
        }
        

        
        
        
        

    }
}
