using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.CharacterRes;
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

        public static Dictionary<int, BossCLS> GetBossDB()
        {
            Dictionary<int, BossCLS> bossDict = new Dictionary<int, BossCLS>();

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbConnection.Open();
                string getBossCmd = "SELECT * FROM BossList";

                SqliteCommand selectCmd = new SqliteCommand(getBossCmd, dbConnection);

                SqliteDataReader query = selectCmd.ExecuteReader();


                while (query.Read())
                {
                    BossCLS tempBoss = new BossCLS();
                    tempBoss.BossID = query.GetInt16(0);
                    tempBoss.BossName = query.GetString(1);
                    tempBoss.Difficulty = query.GetString(2);
                    tempBoss.EntryType = query.GetString(3);
                    tempBoss.Meso = query.GetInt32(4);

                    bossDict.Add(query.GetInt32(0), tempBoss);
                }



                dbConnection.Close();

            }


            return bossDict;
        }

        public static Dictionary<string, CharacterCLS> GetAllCharDB()
        {
            Dictionary<string, CharacterCLS> charDict = new Dictionary<string, CharacterCLS>();

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                string getCharCmd = "SELECT " +
                    "AC.ClassName, AC.ClassType, AC.Faction, AC.MainStat, AC.SecStat, AC.UnionE, AC.UnionET, ClassMainWeap.WeaponType, ClassSecWeap.WeaponType " +
                    "FROM AllCharacter AS AC " +
                    "INNER JOIN ClassMainWeap ON AC.ClassName = ClassMainWeap.ClassName " +
                    "INNER JOIN ClassSecWeap ON AC.ClassName = ClassSecWeap.ClassName";

                using (SqliteCommand selectCmd = new SqliteCommand(getCharCmd, dbConnection))
                {
                    using (SqliteDataReader query = selectCmd.ExecuteReader())
                    {
                        while (query.Read())
                        {
                            if (charDict.ContainsKey(query.GetString(0)))
                            {
                                charDict[query.GetString(0)].MainWeapon.Add(query.GetString(7));
                                charDict[query.GetString(0)].SecondaryWeapon.Add(query.GetString(8));
                                continue;
                            }

                            CharacterCLS tempChar = new CharacterCLS();
                            tempChar.ClassName = query.GetString(0);
                            tempChar.ClassType = query.GetString(1);
                            tempChar.Faction = query.GetString(2);
                            tempChar.MainStat = query.GetString(3);
                            tempChar.SecStat = query.GetString(4);
                            tempChar.UnionEffect = query.GetString(5);
                            tempChar.UnionEffectType = query.GetString(6);

                            tempChar.MainWeapon = new List<string> { query.GetString(7) };
                            tempChar.SecondaryWeapon = new List<string> { query.GetString(8) };

                            charDict.Add(query.GetString(0), tempChar);
                        }
                    }
                }
            }
            return charDict;
        }

        public static Dictionary<string, CharacterCLS> GetAllCharTrackDB()
        {
            Dictionary<string, CharacterCLS> charDict = new Dictionary<string, CharacterCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string getCharCMD = "SELECT * FROM CharacterTrack";

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

        public static List<EquipCLS> GetAllArmorDB()
        {
            List<EquipCLS> equipList = new List<EquipCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbCon.Open();

                string getArmor = "SELECT * FROM ArmorStats";

                using (SqliteCommand selectCMD = new SqliteCommand(getArmor, dbCon))
                {
                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {

                        while (result.Read())
                        {
                            EquipCLS tempEquip = new EquipCLS();
                            tempEquip.EquipSet = result.GetString(0);
                            tempEquip.ClassType = result.GetString(1);
                            tempEquip.EquipSlot = result.GetString(2);
                            tempEquip.EquipLevel = result.GetInt32(3);
                            tempEquip.BaseStats.MS = result.GetInt32(4);
                            tempEquip.BaseStats.SS = result.GetInt32(5);
                            tempEquip.BaseStats.AllStat = result.GetInt32(6);
                            tempEquip.BaseStats.HP = result.GetInt32(7);
                            tempEquip.BaseStats.MP = result.GetInt32(8);
                            tempEquip.BaseStats.DEF = result.GetInt32(9);
                            tempEquip.BaseStats.ATK = result.GetInt32(10);
                            tempEquip.BaseStats.MATK = result.GetInt32(11);

                            tempEquip.BaseStats.SPD = result.GetInt32(12);
                            tempEquip.BaseStats.JUMP = result.GetInt32(13);
                            tempEquip.BaseStats.IED = result.GetInt32(14);

                            equipList.Add(tempEquip);
                        }
                    }
                }

            }

            return equipList;
        }

        public static Dictionary<string, string> GetEquipSlotDB()
        {
            Dictionary<string, string> equipSlotDict = new Dictionary<string, string>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM EquipSlot";
                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipSlotDict.Add(reader.GetString(0), reader.GetString(1));
                        }


                    }
                }
            }

            return equipSlotDict;
        }

        public static List<EquipCLS> GetAllAccessoriesDB()
        {
            List<EquipCLS> accModel = new List<EquipCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM AccessoriesData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipCLS equipModel = new EquipCLS();
                            equipModel.EquipName = reader.GetString(0);
                            equipModel.EquipSet = reader.GetString(1);
                            equipModel.ClassType = reader.GetString(2);
                            equipModel.EquipSlot = reader.GetString(3);

                            equipModel.EquipLevel = reader.GetInt32(4);
                            equipModel.BaseStats.MS = reader.GetInt32(5);
                            equipModel.BaseStats.SS = reader.GetInt32(6);
                            equipModel.BaseStats.AllStat = reader.GetInt32(7);
                            equipModel.BaseStats.SpecialHP = reader.GetString(8);
                            equipModel.BaseStats.SpecialMP = reader.GetString(9);
                            equipModel.BaseStats.DEF = reader.GetInt32(10);
                            equipModel.BaseStats.ATK = reader.GetInt32(11);
                            equipModel.BaseStats.MATK = reader.GetInt32(12);
                            equipModel.BaseStats.SPD = reader.GetInt32(13);
                            equipModel.BaseStats.JUMP = reader.GetInt32(14);
                            equipModel.BaseStats.IED = reader.GetInt32(15);

                            accModel.Add(equipModel);
                        }
                    }
                }


            }



            return accModel;
        }

        public static List<EquipCLS> GetAllWeaponDB()
        {
            List<EquipCLS> weapModel = new List<EquipCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM WeaponData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipCLS equipModel = new EquipCLS();
                            equipModel.EquipSet = reader.GetString(0);
                            equipModel.WeaponType = reader.GetString(1);

                            equipModel.EquipLevel = reader.GetInt32(2);
                            equipModel.BaseStats.ATKSPD = reader.GetInt32(3);
                            equipModel.BaseStats.MS = reader.GetInt32(4);
                            equipModel.BaseStats.SS = reader.GetInt32(5);
                            equipModel.BaseStats.HP = reader.GetInt32(6);
                            equipModel.BaseStats.DEF = reader.GetInt32(7);
                            equipModel.BaseStats.ATK = reader.GetInt32(8);
                            equipModel.BaseStats.MATK = reader.GetInt32(9);

                            equipModel.BaseStats.SPD = reader.GetInt32(10);
                            equipModel.BaseStats.BD = reader.GetInt32(11);
                            equipModel.BaseStats.IED = reader.GetInt32(12);



                            weapModel.Add(equipModel);
                        }
                    }
                }


            }



            return weapModel;
        }
        
        public static List<EquipCLS> GetAllSecondaryDB()
        {
            List<EquipCLS> SecModel = new List<EquipCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM SecondaryWeaponData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipCLS equipModel = new EquipCLS();
                            equipModel.ClassType = reader.GetString(0);
                            equipModel.WeaponType = reader.GetString(1);
                            equipModel.EquipName = reader.GetString(2);
                            equipModel.EquipLevel = reader.GetInt32(3);

                            equipModel.BaseStats.MS = reader.GetInt32(4);
                            equipModel.BaseStats.SS = reader.GetInt32(5);
                            equipModel.BaseStats.ATK = reader.GetInt32(6);
                            equipModel.BaseStats.MATK = reader.GetInt32(7);
                            equipModel.BaseStats.AllStat = reader.GetInt32(8);
                            equipModel.BaseStats.DEF = reader.GetInt32(9);
                            equipModel.BaseStats.HP = reader.GetInt32(10);
                            equipModel.BaseStats.MP = reader.GetInt32(11);
                            equipModel.BaseStats.ATKSPD = reader.GetInt32(12);



                            SecModel.Add(equipModel);
                        }
                    }
                }


            }



            return SecModel;
        }

        public static List<PotentialStatsCLS> GetAllPotential()
        {
            List<PotentialStatsCLS> potentialList = new List<PotentialStatsCLS>();
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT ROWID, * FROM PotentialData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PotentialStatsCLS pot = new PotentialStatsCLS();
                            pot.PotID = reader.GetInt32(0);
                            pot.EquipGrp = reader.GetString(1);
                            pot.Grade = reader.GetString(2);
                            pot.Prime = reader.GetString(3);
                            pot.StatType = reader.GetString(4);
                            pot.StatIncrease = reader.GetString(5);
                            pot.MinLvl = reader.GetInt32(6);
                            pot.MaxLvl = reader.GetInt32(7);
                            pot.StatValue = reader.GetString(8);

                            potentialList.Add(pot);

                        }
                    }
                }


            }

            return potentialList;
        }
        public static List<ArcaneSymbolCLS> GetAllArcaneSymbol()
        {
            List<ArcaneSymbolCLS> symbolList = new List<ArcaneSymbolCLS>();
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM ArcaneSymbol";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ArcaneSymbolCLS symbol = new ArcaneSymbolCLS();
                            symbol.Name = reader.GetString(0);
                            symbol.SubMap = reader.GetString(1);
                            symbol.CurrentLevel = reader.GetInt32(2);
                            symbol.CurrentExp = reader.GetInt32(3);
                            symbol.CurrentLimit = reader.GetInt32(4);
                            symbol.BaseSymbolGain = reader.GetInt32(5);
                            symbol.PQSymbolsGain = reader.GetInt32(6);
                            symbol.PQGainLimit = reader.GetInt32(7);
                            symbol.SymbolExchangeRate = reader.GetInt32(8);
                            symbol.CostLvlMod = reader.GetInt32(9);
                            symbol.CostMod = reader.GetInt32(10);

                            symbolList.Add(symbol);


                        }
                    }
                }


            }

            return symbolList;
        }


    }
}
