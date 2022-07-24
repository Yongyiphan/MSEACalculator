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

        public static Dictionary<string, CharacterCLS> GetAllCharDB()
        {
            Dictionary<string, CharacterCLS> charDict = new Dictionary<string, CharacterCLS>();

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                string getCharCmd =                 
                    "SELECT AC.ClassName, AC.ClassType, AC.Faction, AC.MainStat, AC.SecStat, AC.UEffect, AC.UEffectType, ClassMainWeapon.Weapon, ClassSecWeapon.Secondary "+
                    "FROM ClassCharacterData AS AC " +
                    "INNER JOIN ClassMainWeapon ON AC.ClassName = ClassMainWeapon.ClassName " +
                    "INNER JOIN ClassSecWeapon ON AC.ClassName = ClassSecWeapon.ClassName";

                using (SqliteCommand selectCmd = new SqliteCommand(getCharCmd, dbConnection))
                {
                    using (SqliteDataReader query = selectCmd.ExecuteReader())
                    {
                        while (query.Read())
                        {
                            string ClassName = query.GetString(0);
                            string MainWeapon = query.GetString(7);
                            string SecWeapon = query.GetString(8);
                            if (charDict.ContainsKey(query.GetString(0)))
                            {
                                charDict[ClassName].MainWeapon.Add(MainWeapon);
                                charDict[ClassName].SecondaryWeapon.Add(SecWeapon);
                                continue;
                            }

                            CharacterCLS tempChar = new CharacterCLS();
                            tempChar.ClassName = ClassName;
                            tempChar.ClassType = query.GetString(1);
                            tempChar.Faction = query.GetString(2);
                            tempChar.MainStat = query.GetString(3);
                            tempChar.SecStat = query.GetString(4);
                            tempChar.UnionEffect = query.GetString(5);
                            tempChar.UnionEffectType = query.GetString(6);

                            tempChar.MainWeapon = new List<string> {MainWeapon};
                            tempChar.SecondaryWeapon = new List<string> { SecWeapon};

                            charDict.Add(ClassName, tempChar);
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

                string getCharCMD = "SELECT * FROM TrackCharacter";
                try
                {
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
                catch (Exception)
                {
                    return charDict;
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
                        using (SqliteDataReader result = cmd.ExecuteReader())
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
                    using (SqliteCommand cmd = new SqliteCommand(PotET, dbCon, tran))
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

        //Generator Function to retrieve desired DB Content

        //Armor DB Function
        public static List<ArcaneSymbolCLS> GetAllArcaneSymbolDB()
        {
            List<ArcaneSymbolCLS> symbolList = new List<ArcaneSymbolCLS>();
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM ArcaneSymbolData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ArcaneSymbolCLS symbol = new ArcaneSymbolCLS();
                            symbol.Name               =  reader.GetString(0);
                            symbol.SubMap             =  reader.GetString(1);
                            symbol.CurrentLevel       =  reader.GetInt32(2);
                            symbol.CurrentExp         =  reader.GetInt32(3);
                            symbol.CurrentLimit       =  reader.GetInt32(4);
                            symbol.BaseSymbolGain     =  reader.GetInt32(5);
                            symbol.PQName             =  reader.GetString(6);
                            symbol.PQSymbolsGain      =  reader.GetInt32(7);
                            symbol.PQGainLimit        =  reader.GetInt32(8);
                            symbol.SymbolExchangeRate =  reader.GetInt32(9);
                            symbol.MaxLvl             =  reader.GetInt32(10);
                            symbol.CostLvlMod         =  reader.GetInt32(11);
                            symbol.CostMod            =  reader.GetInt32(12);
                            symbol.MaxExp = CalculationRes.CalForm.CalMaxExp(symbol.MaxLvl);
                            symbol.Description = "Arcane";

                            symbolList.Add(symbol);


                        }
                    }
                }


            }

            return symbolList;
        }

        public static Dictionary<string, List<EquipCLS>> GetEquipmentDB()
        {
            Dictionary<string, List<EquipCLS>> EquipList = new Dictionary<string, List<EquipCLS>>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                string Query = "SELECT* FROM EquipAccessoriesData; " +
                "SELECT* FROM EquipArmorData; " +
                "SELECT* FROM EquipMedalData; " +
                "SELECT* FROM EquipAndroidData; " +
                "SELECT* FROM EquipSecondaryData; " +
                "SELECT* FROM EquipWeaponData;";
                SqliteCommand selectCMD = new SqliteCommand();
                selectCMD.Connection = dbCon;
                selectCMD.CommandText = "SELECT* FROM EquipArmorData; ";
                using (SqliteDataReader result = selectCMD.ExecuteReader())
                {
                    int ErrorCounter = 0;
                    while (result.Read())
                    {
                        ErrorCounter++;
                        string EquipSlot = result.GetString(0);

                        EquipCLS citem = new EquipCLS();
                        citem.EquipSlot = EquipSlot;
                        citem.ClassType = result.GetString(1);
                        citem.EquipName = result.GetString(2);
                        citem.EquipSet = result.GetString(3);
                        citem.EquipLevel = result.GetInt32(4);

                        citem.BaseStats.STR = result.GetInt32(5);
                        citem.BaseStats.DEX = result.GetInt32(6);
                        citem.BaseStats.INT = result.GetInt32(7);
                        citem.BaseStats.LUK = result.GetInt32(8);
                        citem.BaseStats.MaxHP = result.GetInt32(9);
                        citem.BaseStats.MaxMP = result.GetInt32(10);
                        citem.BaseStats.DEF = result.GetInt32(11);
                        citem.BaseStats.ATK = result.GetInt32(12);
                        citem.BaseStats.MATK = result.GetInt32(13);
                        citem.BaseStats.IED = result.GetInt32(14);
                        citem.BaseStats.SPD = result.GetInt32(15);
                        citem.BaseStats.JUMP = result.GetInt32(16);
                        citem.BaseStats.NoUpgrades = result.GetInt32(17);

                        try
                        {
                            if (EquipList.Keys.Contains(EquipSlot))
                            {
                                EquipList[EquipSlot].Add(citem);
                                continue;
                            }
                            EquipList.Add(EquipSlot, new List<EquipCLS>() { citem });
                        }
                        catch (Exception E)
                        {
                            Console.WriteLine(E.Message);
                        }
                    }

                }
;
                selectCMD.CommandText = "SELECT* FROM EquipAccessoriesData;";
                using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string EquipSlot = reader.GetString(0);
                            EquipCLS citem = new EquipCLS();
                            citem.EquipSlot = EquipSlot;
                            citem.ClassType = reader.GetString(1);
                            citem.EquipName = reader.GetString(2);
                            citem.EquipSet = reader.GetString(3);
                            citem.Category = reader.GetString(4);
                            citem.EquipLevel = reader.GetInt32(5);

                            citem.BaseStats.STR = reader.GetInt32(6);
                            citem.BaseStats.DEX = reader.GetInt32(7);
                            citem.BaseStats.INT = reader.GetInt32(8);
                            citem.BaseStats.LUK = reader.GetInt32(9);
                            citem.BaseStats.AllStat = reader.GetInt32(10);
                            citem.BaseStats.MaxHP = reader.GetInt32(11);
                            citem.BaseStats.MaxMP = reader.GetInt32(12);
                            citem.BaseStats.HP = reader.GetString(13);
                            citem.BaseStats.MP = reader.GetString(14);
                            citem.BaseStats.DEF = reader.GetInt32(15);
                            citem.BaseStats.ATK = reader.GetInt32(16);
                            citem.BaseStats.MATK = reader.GetInt32(17);
                            citem.BaseStats.IED = reader.GetInt32(18);
                            citem.BaseStats.SPD = reader.GetInt32(19);
                            citem.BaseStats.JUMP = reader.GetInt32(20);
                            citem.BaseStats.NoUpgrades = reader.GetInt32(21);
                            citem.BaseStats.Rank = reader.GetInt32(22);

                            if (EquipList.Keys.Contains(EquipSlot))
                            {
                                EquipList[EquipSlot].Add(citem);
                                continue;
                            }
                            EquipList.Add(EquipSlot, new List<EquipCLS>() { citem });

                        }
                    }
                selectCMD.CommandText = "SELECT* FROM EquipMedalData;";
;
                selectCMD.CommandText = "SELECT* FROM EquipAndroidData;";
;
                selectCMD.CommandText = "SELECT* FROM EquipSecondaryData;";
;
                selectCMD.CommandText = "SELECT* FROM EquipWeaponData;";
;

            }


            return EquipList;
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



        public static Dictionary<string, List<EquipCLS>> GetArmorDB()
        {
            //<EquipSlot, List of Equip>
            Dictionary<string, List<EquipCLS>> ArmorList = new Dictionary<string, List<EquipCLS>>();
            int ErrorCounter = 0;

            using (SqliteConnection dbCon = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbCon.Open();

                string getArmor = "SELECT * FROM EquipArmorData";

                using (SqliteCommand selectCMD = new SqliteCommand(getArmor, dbCon))
                {
                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {

                        while (result.Read())
                        {
                            ErrorCounter++;
                            string EquipSlot = result.GetString(0);

                            EquipCLS citem = new EquipCLS();
                            citem.EquipSlot = EquipSlot;
                            citem.ClassType = result.GetString(1);
                            citem.EquipName = result.GetString(2);
                            citem.EquipSet = result.GetString(3);
                            citem.EquipLevel = result.GetInt32(4);

                            citem.BaseStats.STR = result.GetInt32(5);
                            citem.BaseStats.DEX = result.GetInt32(6);
                            citem.BaseStats.INT = result.GetInt32(7);
                            citem.BaseStats.LUK = result.GetInt32(8);
                            citem.BaseStats.MaxHP = result.GetInt32(9);
                            citem.BaseStats.MaxMP = result.GetInt32(10);
                            citem.BaseStats.DEF = result.GetInt32(11);
                            citem.BaseStats.ATK = result.GetInt32(12);
                            citem.BaseStats.MATK = result.GetInt32(13);
                            citem.BaseStats.IED = result.GetInt32(14);
                            citem.BaseStats.SPD = result.GetInt32(15);
                            citem.BaseStats.JUMP = result.GetInt32(16);
                            citem.BaseStats.NoUpgrades = result.GetInt32(17);

                            if (ArmorList.Keys.Contains(EquipSlot))
                            {
                                ArmorList[EquipSlot].Add(citem);
                                continue;
                            }
                            ArmorList.Add(EquipSlot, new List<EquipCLS>() { citem });


                        }
                    }
                }

            }

            return ArmorList;
        }

        //Accessory DB Function
        public static Dictionary<string, List<EquipCLS>> GetAccessoriesDB()
        {

            Dictionary<string, List<EquipCLS>> AccList = new Dictionary<string, List<EquipCLS>>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM EquipAccessoriesData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string EquipSlot = reader.GetString(0);
                            EquipCLS citem = new EquipCLS();
                            citem.EquipSlot = EquipSlot;
                            citem.ClassType = reader.GetString(1);
                            citem.EquipName = reader.GetString(2);
                            citem.EquipSet = reader.GetString(3);
                            citem.Category = reader.GetString(4);
                            citem.EquipLevel = reader.GetInt32(5);

                            citem.BaseStats.STR = reader.GetInt32(6);
                            citem.BaseStats.DEX = reader.GetInt32(7);
                            citem.BaseStats.INT = reader.GetInt32(8);
                            citem.BaseStats.LUK = reader.GetInt32(9);
                            citem.BaseStats.AllStat = reader.GetInt32(10);
                            citem.BaseStats.MaxHP = reader.GetInt32(11);
                            citem.BaseStats.MaxMP = reader.GetInt32(12);
                            citem.BaseStats.HP = reader.GetString(13);
                            citem.BaseStats.MP = reader.GetString(14);
                            citem.BaseStats.DEF = reader.GetInt32(15);
                            citem.BaseStats.ATK = reader.GetInt32(16);
                            citem.BaseStats.MATK = reader.GetInt32(17);
                            citem.BaseStats.IED = reader.GetInt32(18);
                            citem.BaseStats.SPD = reader.GetInt32(19);
                            citem.BaseStats.JUMP = reader.GetInt32(20);
                            citem.BaseStats.NoUpgrades = reader.GetInt32(21);
                            citem.BaseStats.Rank = reader.GetInt32(22);

                            if (AccList.Keys.Contains(EquipSlot))
                            {
                                AccList[EquipSlot].Add(citem);
                                continue;
                            }
                            AccList.Add(EquipSlot, new List<EquipCLS>() { citem });

                        }
                    }
                }


            }


            return AccList;
        }

public static Dictionary<string, List<EquipCLS>> GetMedalDB()
        {
            //<EquipSlot, List of Equip>
            Dictionary<string, List<EquipCLS>> ArmorList = new Dictionary<string, List<EquipCLS>>();
            int ErrorCounter = 0;

            using (SqliteConnection dbCon = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbCon.Open();

                string getArmor = "SELECT * FROM EquipArmorData";

                using (SqliteCommand selectCMD = new SqliteCommand(getArmor, dbCon))
                {
                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {

                        while (result.Read())
                        {
                            ErrorCounter++;
                            string EquipSlot = result.GetString(0);

                            EquipCLS citem = new EquipCLS();
                            citem.EquipSlot = EquipSlot;
                            citem.ClassType = result.GetString(1);
                            citem.EquipName = result.GetString(2);
                            citem.EquipSet = result.GetString(3);
                            citem.EquipLevel = result.GetInt32(4);

                            citem.BaseStats.STR = result.GetInt32(5);
                            citem.BaseStats.DEX = result.GetInt32(6);
                            citem.BaseStats.INT = result.GetInt32(7);
                            citem.BaseStats.LUK = result.GetInt32(8);
                            citem.BaseStats.MaxHP = result.GetInt32(9);
                            citem.BaseStats.MaxMP = result.GetInt32(10);
                            citem.BaseStats.DEF = result.GetInt32(11);
                            citem.BaseStats.ATK = result.GetInt32(12);
                            citem.BaseStats.MATK = result.GetInt32(13);
                            citem.BaseStats.IED = result.GetInt32(14);
                            citem.BaseStats.SPD = result.GetInt32(15);
                            citem.BaseStats.JUMP = result.GetInt32(16);
                            citem.BaseStats.NoUpgrades = result.GetInt32(17);

                            if (ArmorList.Keys.Contains(EquipSlot))
                            {
                                ArmorList[EquipSlot].Add(citem);
                                continue;
                            }
                            ArmorList.Add(EquipSlot, new List<EquipCLS>() { citem });


                        }
                    }
                }

            }

            return ArmorList;
        }


        //Weapon DB Function
        public static List<EquipCLS> GetWeaponDB()
        {
            List<EquipCLS> weapModel = new List<EquipCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM EquipWeaponData";

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

                            equipModel.BaseStats.STR = reader.GetInt32(4);
                            equipModel.BaseStats.DEX = reader.GetInt32(5);
                            equipModel.BaseStats.INT = reader.GetInt32(6);
                            equipModel.BaseStats.LUK = reader.GetInt32(7);

                            equipModel.BaseStats.MaxHP = reader.GetInt32(8);
                            equipModel.BaseStats.DEF = reader.GetInt32(9);
                            equipModel.BaseStats.ATK = reader.GetInt32(10);
                            equipModel.BaseStats.MATK = reader.GetInt32(11);

                            equipModel.BaseStats.SPD = reader.GetInt32(12);
                            equipModel.BaseStats.BD = reader.GetInt32(13);
                            equipModel.BaseStats.IED = reader.GetInt32(14);



                            weapModel.Add(equipModel);
                        }
                    }
                }


            }



            return weapModel;
        }

        //Secondary DB Function
        public static List<EquipCLS> GetSecondaryDB()
        {
            List<EquipCLS> SecModel = new List<EquipCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM EquipSecondaryData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipCLS equipModel = new EquipCLS();

                            equipModel.ClassType = reader.GetString(0);
                            equipModel.EquipName = reader.GetString(1);
                            equipModel.WeaponType = reader.GetString(2);
                            equipModel.EquipLevel = reader.GetInt32(3);

                            equipModel.BaseStats.STR = reader.GetInt32(4);
                            equipModel.BaseStats.DEX = reader.GetInt32(5);
                            equipModel.BaseStats.INT = reader.GetInt32(6);
                            equipModel.BaseStats.LUK = reader.GetInt32(7);

                            equipModel.BaseStats.ATK = reader.GetInt32(8);
                            equipModel.BaseStats.MATK = reader.GetInt32(9);
                            equipModel.BaseStats.AllStat = reader.GetInt32(10);
                            equipModel.BaseStats.DEF = reader.GetInt32(11);
                            equipModel.BaseStats.MaxHP = reader.GetInt32(12);
                            equipModel.BaseStats.MaxMP = reader.GetInt32(13);

                            equipModel.BaseStats.ATKSPD = reader.GetInt32(14);



                            SecModel.Add(equipModel);
                        }
                    }
                }


            }



            return SecModel;
        }





        //Calculation

        public static Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>> GetAllPotentialDB()
        {
            Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>> FinalResult = new Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>>();
            List<PotentialStatsCLS> UnsortedPotentialList = new List<PotentialStatsCLS>();
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
                            pot.EquipSlot = reader.GetString(1);
                            if (!FinalResult.ContainsKey(pot.EquipSlot))
                            {
                                FinalResult.Add(pot.EquipSlot, new Dictionary<string, List<PotentialStatsCLS>>());
                            }
                            pot.Grade = reader.GetString(2);
                            if (!FinalResult[pot.EquipSlot].ContainsKey(pot.Grade))
                            {
                                FinalResult[pot.EquipSlot].Add(pot.Grade, new List<PotentialStatsCLS>());
                            }
                            pot.Prime = reader.GetString(3);
                            pot.StatType = reader.GetString(4);
                            pot.StatIncrease = reader.GetString(5);
                            pot.MinLvl = reader.GetInt32(6);
                            pot.MaxLvl = reader.GetInt32(7);
                            pot.StatValue = reader.GetString(8);

                            pot.DisplayStat = pot.StatValue == "0" ? pot.StatIncrease.ToString() : String.Format("{0} +{1}", pot.StatIncrease.TrimEnd('%'), pot.StatValue);
                            UnsortedPotentialList.Add(pot);



                            //FinalResult[pot.EquipGrp].Add(pot);
                        }
                    }
                }


            }

            foreach (PotentialStatsCLS pot in UnsortedPotentialList)
            {
                int potPos = GVar.PotentialGrade.IndexOf(pot.Grade);
                string nextPot;
                switch (pot.Grade)
                {
                    case "Rare":
                        FinalResult[pot.EquipSlot][pot.Grade].Add(pot);
                        if (pot.Prime == "Prime")
                        {
                            nextPot = GVar.PotentialGrade[potPos + 1];
                            FinalResult[pot.EquipSlot][nextPot].Add(pot);

                        }
                        break;

                    case "Epic":
                        FinalResult[pot.EquipSlot][pot.Grade].Add(pot);
                        nextPot = GVar.PotentialGrade[potPos + 1];
                        FinalResult[pot.EquipSlot][nextPot].Add(pot);

                        break;

                    case "Unique":
                        FinalResult[pot.EquipSlot][pot.Grade].Add(pot);
                        nextPot = GVar.PotentialGrade[potPos + 1];
                        FinalResult[pot.EquipSlot][nextPot].Add(pot);
                        break;

                    case "Legendary":
                        FinalResult[pot.EquipSlot][pot.Grade].Add(pot);
                        break;
                }

            }

            return FinalResult;
        }
        public static Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>> GetAllBonusPotentialDB()
        {
            Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>> FinalResult = new Dictionary<string, Dictionary<string, List<PotentialStatsCLS>>>();

            List<PotentialStatsCLS> UnsortedPotentialList = new List<PotentialStatsCLS>();
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT ROWID, * FROM PotentialBonusData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PotentialStatsCLS pot = new PotentialStatsCLS();
                            pot.PotID = reader.GetInt32(0);
                            pot.EquipSlot = reader.GetString(1);
                            if (!FinalResult.ContainsKey(pot.EquipSlot))
                            {
                                FinalResult.Add(pot.EquipSlot, new Dictionary<string, List<PotentialStatsCLS>>());
                            }
                            pot.Grade = reader.GetString(2);
                            if (!FinalResult[pot.EquipSlot].ContainsKey(pot.Grade))
                            {
                                FinalResult[pot.EquipSlot].Add(pot.Grade, new List<PotentialStatsCLS>());
                            }
                            pot.Prime = reader.GetString(3);
                            pot.StatType = reader.GetString(4);
                            pot.StatIncrease = reader.GetString(5);
                            pot.MinLvl = reader.GetInt32(6);
                            pot.MaxLvl = reader.GetInt32(7);
                            pot.StatValue = reader.GetString(8);

                            pot.DisplayStat = pot.StatValue == "0" ? pot.StatIncrease.ToString() : String.Format("{0} +{1}", pot.StatIncrease.TrimEnd('%'), pot.StatValue);
                            UnsortedPotentialList.Add(pot);
                        }
                    }
                }


            }
            foreach (PotentialStatsCLS pot in UnsortedPotentialList)
            {
                int potPos = GVar.PotentialGrade.IndexOf(pot.Grade);
                string nextPot;
                switch (pot.Grade)
                {
                    case "Rare":
                        FinalResult[pot.EquipSlot][pot.Grade].Add(pot);
                        if (pot.Prime == "Prime")
                        {
                            nextPot = GVar.PotentialGrade[potPos + 1];
                            FinalResult[pot.EquipSlot][nextPot].Add(pot);

                        }
                        break;

                    case "Epic":
                        FinalResult[pot.EquipSlot][pot.Grade].Add(pot);
                        nextPot = GVar.PotentialGrade[potPos + 1];
                        FinalResult[pot.EquipSlot][nextPot].Add(pot);

                        break;

                    case "Unique":
                        FinalResult[pot.EquipSlot][pot.Grade].Add(pot);
                        nextPot = GVar.PotentialGrade[potPos + 1];
                        FinalResult[pot.EquipSlot][nextPot].Add(pot);
                        break;

                    case "Legendary":
                        FinalResult[pot.EquipSlot][pot.Grade].Add(pot);
                        break;
                }
            }


            return FinalResult;
        }

        public static List<StarforceCLS> GetAllStarforceDB()
        {
            List<StarforceCLS> result = new List<StarforceCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM StarForceBaseData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetInt32(0) == 16)
                            {
                                break;
                            }

                            StarforceCLS SF = new StarforceCLS();

                            SF.SFLevel     =  reader.GetInt32(0);
                            SF.JobStat     =  reader.GetInt32(1);
                            SF.NonWeapVDef =  reader.GetInt32(2);
                            SF.OverallVDef =  reader.GetInt32(3);
                            SF.CatAMaxHP   =  reader.GetInt32(4);
                            SF.WeapMaxMP   =  reader.GetInt32(5);
                            SF.WeapVATK    =  reader.GetInt32(6);
                            SF.WeapVMATK   =  reader.GetInt32(7);
                            SF.SJump       =  reader.GetInt32(8);
                            SF.SSpeed      =  reader.GetInt32(9);
                            SF.GloveVATK   = reader.GetInt32(10);
                            SF.GloveVMATK  = reader.GetInt32(11);

                            result.Add(SF);
                        }
                    }
                }
                string addQuery = "SELECT b.SFID, b.NonWeapVDef, b.OverallVDef, a.LevelRank, a.VStat, a.NonWeapVATK, a.NonWeapVMATK, a.WeapVATK, a.WeapVMATK " +
                    "FROM StarForceBaseData AS b " +
                    "INNER JOIN StarForceAddData as a ON " +
                    "b.SFID = a.SFID";

                using (SqliteCommand selectCMD = new SqliteCommand(addQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {

                        StarforceCLS SF = null;
                        bool startNew = false;
                        bool toAdd = false;
                        int prevLvl = 0;

                        while (reader.Read())
                        {
                            int clvl = reader.GetInt32(0);
                            if (prevLvl == 0)
                            {
                                prevLvl = clvl;
                                startNew = true;
                                goto StartNew;
                            }

                        StartNew:
                            if (startNew)
                            {
                                SF = new StarforceCLS();
                                SF.SFLevel = clvl;
                                SF.NonWeapVDef = reader.GetInt32(1);
                                SF.OverallVDef = reader.GetInt32(2);
                                startNew = false;
                            }


                            if (prevLvl !=  clvl)
                            {
                                prevLvl = clvl;
                                toAdd = true;
                                goto ToAdd;
                            }
                            else
                            {
                                int lvlrank = reader.GetInt32(3);
                                SF.LevelRank = lvlrank;
                                //SF.VStatL.Add(lvlrank, reader.GetInt32(4));
                                //SF.NonWeapVATKL.Add(lvlrank, reader.GetInt32(5));
                                //SF.NonWeapVMATKL.Add(lvlrank, reader.GetInt32(6));
                                //SF.WeapVATKL.Add(lvlrank, reader.GetInt32(7));
                                //SF.WeapVMATKL.Add(lvlrank, reader.GetInt32(8));
                            }

                        ToAdd:
                            if (toAdd)
                            {
                                result.Add(SF);
                                toAdd = false;
                                startNew = true;
                                goto StartNew;
                            }
                        }

                        result.Add(SF);
                    }
                }


            }

            return result;
        }

        public static List<StarforceCLS> GetAllSuperiorStarforceDB()
        {
            List<StarforceCLS> result = new List<StarforceCLS>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();


                string addQuery = "SELECT * FROM StarforceSuperiorData";

                using (SqliteCommand selectCMD = new SqliteCommand(addQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {

                        StarforceCLS SF = null;
                        bool startNew = false;
                        bool toAdd = false;
                        int prevLvl = 0;

                        while (reader.Read())
                        {
                            int clvl = reader.GetInt32(0);
                            if (prevLvl == 0)
                            {
                                prevLvl = clvl;
                                startNew = true;
                                goto StartNew;
                            }

                        StartNew:
                            if (startNew)
                            {
                                SF = new StarforceCLS();
                                SF.SFLevel = clvl;
                                SF.VDef = reader.GetInt32(4);
                                startNew = false;
                            }


                            if (prevLvl !=  clvl)
                            {
                                prevLvl = clvl;
                                toAdd = true;
                                goto ToAdd;
                            }
                            else
                            {
                                int lvlrank = reader.GetInt32(1);
                                SF.LevelRank = lvlrank;
                                //SF.VStatL.Add(lvlrank, reader.GetInt32(2));
                                //SF.WeapVATKL.Add(lvlrank, reader.GetInt32(3));
                            }
                        ToAdd:
                            if (toAdd)
                            {
                                result.Add(SF);
                                toAdd = false;
                                startNew = true;
                                goto StartNew;
                            }
                        }

                        result.Add(SF);
                    }
                }


            }

            return result;
        }



    }
}
