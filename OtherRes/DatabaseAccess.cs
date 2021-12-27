using Dasync.Collections;
using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes;
using MSEACalculator.StarforceRes;
using MSEACalculator.UnionRes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MSEACalculator
{
    public class DatabaseAccess
    {

        //Codes for First initialising of Data
        //From CSV (DefaultData folder) to Sqlite Database (Maplestory.db)
        public async static Task databaseInit()
        {
            //string dbName = "Maplestory.db";
            await ApplicationData.Current.LocalFolder.CreateFileAsync("Maplestory.db", CreationCollisionOption.OpenIfExists);
            
            

            List<TableStats> staticTables = new List<TableStats>();
            List<TableStats> blankTables = new List<TableStats>();


            ///////CHARACTER///////

            //TABLE FOR DEFAULT CHARACTER LIST
            string CharacterTableSpec = "(" +
                            "ClassName string," +
                            "ClassType string," +
                            "Faction string," +
                            "MainStat string," +
                            "SecStat string," +
                            "UnionE string," +
                            "UnionET string," +
                            "PRIMARY KEY (ClassName)" +
                            ");";
            staticTables.Add(new TableStats("AllCharacter", CharacterTableSpec, "AllCharacter"));

            //TABLE FOR UNION EFFECTS
            string unionETableSpecs = "(" +
                "Effect string," +
                "EffectType string," +
                "B int," +
                "A int," +
                "S int," +
                "SS int," +
                "SSS int," +
                "PRIMARY KEY(Effect, EffectType)" +
                ");";
            staticTables.Add(new TableStats("UnionEffects", unionETableSpecs, "UnionEffect"));

            string WeapToClassSpec = "(" +
                "ClassName string," +
                "WeaponType string," +
                "PRIMARY KEY (ClassName, WeaponType)" +
                ");";

            staticTables.Add(new TableStats("ClassMWeapon", WeapToClassSpec,"ClassMWeapon"));



            ///////BOSSING///////


            string bossListSpec = "(" +
                "BossID int," +
                "BossName varchar(50) NOT NULL," +
                "Difficulty varchar(10) NOT NULL," +
                "EntryType varchar(10) NOT NULL," +
                "EntryLimit int NOT NULL," +
                "BossCrystal int NOT NULL," +
                "Meso varchar NOT NULL," +
                "PRIMARY KEY(BossID)" +
                ");";
            staticTables.Add(new TableStats("BossList", bossListSpec, "Boss"));


            


            //////EVENT///////

            ///////INVENTORY////////

            //TABLE FOR EQUIP SLOT AND TYPES
            string equipSlotTS = "(" +
                "EquipSlot string," +
                "EquipType string," +
                "PRIMARY KEY(EquipSlot)" +
                ");";
            staticTables.Add(new TableStats("EquipSlot", equipSlotTS, "EquipSlot"));

            //TABLE FOR ALL ARMOR
            string equipTableSpec = "(" +
                "EquipSet string," +
                "ClassType string," +
                "EquipSlot string," +
                "EquipLevel int," +
                "MainStat int," +
                "SecStat int," +
                "AllStat int," +
                "HP int," +
                "MP int," +
                "DEF int," +
                "ATK int," +
                "MATK int," +
                "SPD int," +
                "JUMP int," +
                "IED int," +
                "PRIMARY KEY (EquipSet, ClassType, EquipSlot)" +
                ");";
            staticTables.Add(new TableStats("ArmorStats", equipTableSpec, "Armor"));


            //TABLE FOR ACCESSORIES
            string AccessoriesTableSpec = "(" +
                "EquipName string," +
                "EquipSet string," +
                "ClassType  string," +
                "EquipSlot string," +
                "EquipLevel int," +
                "MainStat int," +
                "SecStat int," +
                "AllStat int," +
                "HP string," +
                "MP string," +
                "DEF int," +
                "ATK int," +
                "MATK int," +
                "SPD int," +
                "JUMP int," +
                "IED int,"+
                "PRIMARY KEY (EquipName, EquipSet, ClassType, EquipSlot) " +
                ");";
            staticTables.Add(new TableStats("AccessoriesData", AccessoriesTableSpec, "Accessories"));

            //TABLE FOR WEAPON
            string WeapTableSpec = "(" +
                "EquipSet string," +
                "WeaponType string," +
                "EquipLevel int," +
                "ATKSPD int," +
                "MainStat int," +
                "SecStat int," +
                "HP int," +
                "DEF int," +
                "ATK int," +
                "MATK int," +
                "SPD int," +
                "BDMG int," +
                "IED int," +
                "PRIMARY KEY (EquipSet, WeaponType) " +
                ");";
            staticTables.Add(new TableStats("WeaponData", WeapTableSpec, "Weapon"));

            //Equipment Set Effects

            /////CALCULATIONS/////
            
            //TABLE FOR STARFORCE STATS
            string sfTableSpec = "(" +
                "SFID int NOT NULL, " +
                "MainStat int," +
                "NonWeaponDef int NOT NULL," +
                "OverallDef int NOT NULL," +
                "MaxHP int NOT NULL," +
                "MaxMP int NOT NULL," +
                "ATK int," +
                "NonWeaponATK int," +
                "ShoeSpeed int NOT NULL," +
                "ShoeJump int NOT NULL," +
                "GlovesATK int NOT NULL," +
                "PRIMARY KEY(SFID)" +
                ");";
            staticTables.Add(new TableStats("StarforceList", sfTableSpec, "StarForce"));

            //TABLE FOR STARFORCE STATS ADDONS
            string addSFstatSpec = "(" +
                "SFID int NOT NULL," +
                "StatType varchar(50) NOT NULL," +
                "'128~137' int NOT NULL," +
                "'138~149' int NOT NULL," +
                "'150~159' int NOT NULL," +
                "'160~199' int NOT NULL," +
                "'200' int NOT NULL" +
                ");";
            staticTables.Add(new TableStats("AddSFStat", addSFstatSpec, "AddStarForce"));


            /////BLANK TABLES/////
            


            //TABLE FOR CHARACTER TO TRACK
            string charTrackSpec = "(" +
                "CharName string," +
                "UnionRank string," +
                "Level int," +
                "Starforce int," +
                "PRIMARY KEY(CharName)" +
                ");";
            blankTables.Add(new TableStats("CharacterTrack", charTrackSpec));

            //TABLE FOR CHAR'S BOSS TRACKING
            string bossMesoGainsTableSpec = "(" +
                "CharName string," +
                "BossName string," +
                "BossID int," +
                "PRIMARY KEY(CharName, BossID)," +
                "FOREIGN KEY (CharName) REFERENCES CharacterTrack(CharName) ON DELETE CASCADE," +
                "FOREIGN KEY (BossID) REFERENCES BossList(BossID)" +
                ");";
            blankTables.Add(new TableStats("BossMesoGains", bossMesoGainsTableSpec));


            string charEquipScrollSpec = "(" +
                "CharName string," +
                "EquipSlot string," +
                "EquipSet string," +
                "STR int," +
                "DEX int," +
                "INT int," +
                "LUK int," +
                "HP int," +
                "MP int," +
                "DEF int," +
                "ATK int," +
                "MATK int," +
                "SPD int," +
                "JUMP int," +
                "PRIMARY KEY (CharName, EquipSlot, EquipSet)," +
                "FOREIGN KEY (CharName) REFERENCES CharacterTrack(CharName) ON DELETE CASCADE" +
                ");";
            blankTables.Add( new TableStats("CharTEquipScroll", charEquipScrollSpec));
            
            string charEquipFlameSpec = "(" +
                "CharName string," +
                "EquipSlot string," +
                "EquipSet string," +
                "STR int," +
                "DEX int," +
                "INT int," +
                "LUK int," +
                "HP int," +
                "MP int," +
                "DEF int," +
                "ATK int," +
                "MATK int," +
                "SPD int," +
                "JUMP int," +
                "AllStat int," +
                "BossDMG, int" +
                "DMG int," +
                "PRIMARY KEY (CharName, EquipSlot, EquipSet)," +
                "FOREIGN KEY (CharName) REFERENCES CharacterTrack(CharName) ON DELETE CASCADE" +
                ");";
            blankTables.Add(new TableStats("CharTEquipFlame", charEquipFlameSpec));

            

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GlobalVars.databasePath}"))
            {
                dbConnection.Open();
                using(var trans = dbConnection.BeginTransaction())
                {
                    try
                    {
                        //INIT Blank Tables <- Tables with foreign key FIRST
                        blankTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection, trans));

                        staticTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection, trans));

                        await Task.WhenAll(staticTables.ParallelForEachAsync(item => Task.Run(() => InitCSVData(item.initMode, item.tableName, dbConnection, trans))));

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        trans.Rollback();
                    }
                    
                }


            }


        }

        //public static bool testDBCon()
        //{
        //    using(SqliteConnection dbConnection = new SqliteConnection($"Filename={GlobalVars.databasePath}"))
        //    {
        //        dbConnection.Open();
        //        return true;
        //    }

        //    return false;
        //}

        private static void initTable(string tableName, string tableParameters, SqliteConnection connection, SqliteTransaction transaction)
        {
            if (connection.State == ConnectionState.Open)
            {
                //delete table if exist
                string dropT = "DROP TABLE IF EXISTS " + tableName;
                using (SqliteCommand dropCmd = new SqliteCommand(dropT, connection, transaction))
                {
                    dropCmd.ExecuteNonQuery();
                };
                // create table if not exist

                string createTable = "CREATE TABLE IF NOT EXISTS " + tableName + tableParameters;
                using (SqliteCommand createCmd = new SqliteCommand(createTable, connection, transaction))
                {
                    createCmd.ExecuteNonQuery();
                };
            }

        }


        public static async Task InitCSVData(string insertType, string tableName, SqliteConnection connection, SqliteTransaction transaction)
        {
            int counter = 0;
            switch (insertType)
            {
                case "Boss":
                    List<Boss> bosstable = await GetBossCSVAsync();
                    // update to table

                    if(connection.State == ConnectionState.Open)
                    {
                        foreach (Boss bossItem in bosstable)
                        {
                            counter++;
                            string insertBoss = "INSERT INTO " + tableName + " (BossID, BossName, Difficulty, EntryType, EntryLimit, BossCrystal, Meso)" +
                            " VALUES (@BossID,@BossName,@Difficulty,@EntryType,@EntryLimit,@BossCrystal,@Meso)";
                            using (SqliteCommand insertBosscmd = new SqliteCommand(insertBoss, connection, transaction))
                            {
                                insertBosscmd.Parameters.AddWithValue("@BossID", bossItem.BossID);
                                insertBosscmd.Parameters.AddWithValue("@BossName", bossItem.name);
                                insertBosscmd.Parameters.AddWithValue("@Difficulty", bossItem.difficulty);
                                insertBosscmd.Parameters.AddWithValue("@EntryType", bossItem.entryType);
                                insertBosscmd.Parameters.AddWithValue("@EntryLimit", bossItem.entryLimit);
                                insertBosscmd.Parameters.AddWithValue("@BossCrystal", bossItem.bossCrystalCount);
                                insertBosscmd.Parameters.AddWithValue("@Meso", bossItem.meso);

                                try
                                {
                                    insertBosscmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(counter.ToString());
                                    Console.WriteLine(ex.ToString());
                                }

                            }

                        }                            
                    }
                    break;
                case "StarForce":

                    List<SFGain> SFtable = await GetSFCSVAsync();

                    if(connection.State == ConnectionState.Open)
                    {
                        foreach (SFGain sfitem in SFtable)
                        {
                            counter++;
                            if (sfitem.StarForceLevel < 16)
                            {
                                string insertSF1to15 = "INSERT INTO " + tableName +
                                    " VALUES (@SFID, @MainStat, @NonWeapDef, @OveralDef, @MaxHP, @MaxMP, @ATK, @NWATK, @SS, @SJ, @GATK )";
                                SqliteCommand insertSFcmd = new SqliteCommand(insertSF1to15, connection, transaction);
                                insertSFcmd.Parameters.AddWithValue("@SFID", sfitem.StarForceLevel);
                                insertSFcmd.Parameters.AddWithValue("@MainStat", sfitem.MainStat);
                                insertSFcmd.Parameters.AddWithValue("@NonWeapDef", sfitem.NonWeapDef);
                                insertSFcmd.Parameters.AddWithValue("@OveralDef", sfitem.OverallDef);
                                insertSFcmd.Parameters.AddWithValue("@MaxHP", sfitem.MaxHP);
                                insertSFcmd.Parameters.AddWithValue("@MaxMP", sfitem.MaxMP);
                                insertSFcmd.Parameters.AddWithValue("@ATK", sfitem.WATK);
                                insertSFcmd.Parameters.AddWithValue("@NWATK", sfitem.NonWATK);
                                insertSFcmd.Parameters.AddWithValue("@SS", sfitem.Speed);
                                insertSFcmd.Parameters.AddWithValue("@SJ", sfitem.Jump);
                                insertSFcmd.Parameters.AddWithValue("@GATK", sfitem.GloveAtk);

                                try
                                {
                                    insertSFcmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(counter.ToString());
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                            else
                            {
                                string insertSF16to25 = "INSERT INTO " + tableName +
                                    " VALUES (@SFID, @MainStat, @NonWeapDef, @OveralDef, @MaxHP, @MaxMP, @ATK, @NWATK, @SS, @SJ, @GATK )";
                                SqliteCommand insertSF2cmd = new SqliteCommand(insertSF16to25, connection, transaction);
                                insertSF2cmd.Parameters.AddWithValue("@SFID", sfitem.StarForceLevel);
                                insertSF2cmd.Parameters.AddWithValue("@MainStat", (object)sfitem.MainStat ?? DBNull.Value);
                                insertSF2cmd.Parameters.AddWithValue("@NonWeapDef", sfitem.NonWeapDef);
                                insertSF2cmd.Parameters.AddWithValue("@OveralDef", sfitem.OverallDef);
                                insertSF2cmd.Parameters.AddWithValue("@MaxHP", sfitem.MaxHP);
                                insertSF2cmd.Parameters.AddWithValue("@MaxMP", sfitem.MaxMP);
                                insertSF2cmd.Parameters.AddWithValue("@ATK", (object)sfitem.WATK ?? DBNull.Value);
                                insertSF2cmd.Parameters.AddWithValue("@NWATK", (object)sfitem.NonWATK ?? DBNull.Value);
                                insertSF2cmd.Parameters.AddWithValue("@SS", sfitem.Speed);
                                insertSF2cmd.Parameters.AddWithValue("@SJ", sfitem.Jump);
                                insertSF2cmd.Parameters.AddWithValue("@GATK", sfitem.GloveAtk);
                                

                                try
                                {
                                    insertSF2cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                    Console.WriteLine(counter.ToString());
                                }

                            }
                        }
                                

                        
                    }
                    break;
                case "AddStarForce":

                    List<SFGain> AddSFtable = await GetSFCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {   
                        int startCount = 15, endCount = AddSFtable.Count() + 1;

                        for(int i  = 0; i < endCount; i++)
                        {
                            counter++;
                            SFGain tempSFI = AddSFtable[startCount];


                            void addRow(SFGain sfitem, string stattype, List<int> templist)
                            {
                                string insertRow = "INSERT INTO " + tableName +
                                    " VALUES ( @SFID, @ST, @zero, @one, @two, @three, @four ) ";
                                SqliteCommand insertRowCmd = new SqliteCommand(insertRow, connection, transaction);
                                insertRowCmd.Parameters.AddWithValue("@SFID", sfitem.StarForceLevel);
                                insertRowCmd.Parameters.AddWithValue("@ST", stattype);
                                insertRowCmd.Parameters.AddWithValue("@zero", templist[0]);
                                insertRowCmd.Parameters.AddWithValue("@one", templist[1]);
                                insertRowCmd.Parameters.AddWithValue("@two", templist[2]);
                                insertRowCmd.Parameters.AddWithValue("@three", templist[3]);
                                insertRowCmd.Parameters.AddWithValue("@four", templist[4]);

                                try
                                {
                                    insertRowCmd.ExecuteNonQuery();

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(counter.ToString());  
                                    Console.WriteLine(ex.ToString());  
                                }

                                
                            };
                            if (tempSFI.StarForceLevel < 23)
                            {
                                addRow(tempSFI, nameof(tempSFI.MainStat), tempSFI.MainStatL);
                                addRow(tempSFI, nameof(tempSFI.NonWATK), tempSFI.NonWATKL);
                                addRow(tempSFI, nameof(tempSFI.WATK), tempSFI.WATKL);
                            }
                            if (tempSFI.StarForceLevel > 22)
                            {
                                addRow(tempSFI, nameof(tempSFI.NonWATK), tempSFI.NonWATKL);
                                addRow(tempSFI, nameof(tempSFI.WATK), tempSFI.WATKL);
                            }

                        }


                    }
                    break;
                case "AllCharacter":

                    List<Character> charTable = await GetCharCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {

                        foreach (Character charItem in charTable)
                        {
                            counter++;
                            string insertChar = "INSERT INTO " + tableName + "(ClassName, ClassType, Faction, MainStat, SecStat, UnionE, UnionET)" +
                                " VALUES (@CN, @CT, @Fac, @MS, @SS, @UE, @UET)";

                            SqliteCommand insertCharCmd = new SqliteCommand(insertChar, connection, transaction);

                            insertCharCmd.Parameters.AddWithValue("@CN", charItem.ClassName);
                            insertCharCmd.Parameters.AddWithValue("@CT", charItem.ClassType);
                            insertCharCmd.Parameters.AddWithValue("@Fac", charItem.Faction);
                            insertCharCmd.Parameters.AddWithValue("@UE", charItem.unionEffect);
                            insertCharCmd.Parameters.AddWithValue("@UET", charItem.unionEffectType);
                            insertCharCmd.Parameters.AddWithValue("@MS", charItem.MainStat);
                            insertCharCmd.Parameters.AddWithValue("@SS", charItem.SecStat);

                            try
                            {
                                insertCharCmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(counter.ToString());
                                Console.WriteLine(ex.ToString());
                            }
                            
                        }
                    }

                    break;
                case "UnionEffect":
                    List<UnionModel> unionList = await GetUnionECSVAsync();
                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (UnionModel unionItem in unionList)
                        {
                            counter++;
                            string insertUnion = "INSERT INTO " + tableName + " (Effect, EffectType, B, A, S, SS, SSS)" +
                                " VALUES (@E, @ET, @B, @A, @S, @SS, @SSS);";
                            using (SqliteCommand insertCMD = new SqliteCommand(insertUnion, connection, transaction))
                            {
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@E", unionItem.Effect);
                                insertCMD.Parameters.AddWithValue("@ET", unionItem.EffectType);
                                insertCMD.Parameters.AddWithValue("@B", unionItem.RankB);
                                insertCMD.Parameters.AddWithValue("@A", unionItem.RankA);
                                insertCMD.Parameters.AddWithValue("@S", unionItem.RankS);
                                insertCMD.Parameters.AddWithValue("@SS", unionItem.RankSS);
                                insertCMD.Parameters.AddWithValue("@SSS", unionItem.RankSSS);

                                try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                    Console.WriteLine(counter.ToString());
                                }
                            }
                        }
                    }


                    break;
                case "Armor":

                    List<EquipModel> equipList = await GetArmorCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (EquipModel equipItem in equipList)
                        {
                            counter++;
                            string insertEquip = "INSERT INTO " + tableName + "(" +
                                " EquipSet, ClassType, EquipSlot, EquipLevel, MainStat, SecStat, AllStat, HP, MP, DEF, ATK, MATK, SPD, JUMP, IED)" +
                                " VALUES (@Set, @Job, @Slot, @Lvl, @MS, @SS, @AllS, @HP, @MP, @DEF, @ATK, @MATK, @SPD,@JUMP,@IED);";

                            using (SqliteCommand insertCMD = new SqliteCommand(insertEquip, connection, transaction))
                            {
                                insertCMD.Parameters.AddWithValue("@Set", equipItem.EquipSet);
                                insertCMD.Parameters.AddWithValue("@Job", equipItem.ClassType);
                                insertCMD.Parameters.AddWithValue("@Slot", equipItem.EquipSlot);
                                insertCMD.Parameters.AddWithValue("@Lvl", equipItem.EquipLevel);
                                insertCMD.Parameters.AddWithValue("@MS", equipItem.BaseStats.MS);
                                insertCMD.Parameters.AddWithValue("@SS", equipItem.BaseStats.SS);
                                insertCMD.Parameters.AddWithValue("@HP", equipItem.BaseStats.HP);
                                insertCMD.Parameters.AddWithValue("@MP", equipItem.BaseStats.MP);
                                insertCMD.Parameters.AddWithValue("@DEF", equipItem.BaseStats.DEF);
                                insertCMD.Parameters.AddWithValue("@ATK", equipItem.BaseStats.ATK);
                                insertCMD.Parameters.AddWithValue("@MATK", equipItem.BaseStats.MATK);
                                insertCMD.Parameters.AddWithValue("@AllS", equipItem.BaseStats.AllStat);
                                insertCMD.Parameters.AddWithValue("@SPD", equipItem.BaseStats.SPD);
                                insertCMD.Parameters.AddWithValue("@JUMP", equipItem.BaseStats.JUMP);
                                insertCMD.Parameters.AddWithValue("@IED", equipItem.BaseStats.IED);

                                try
                                {
                                    insertCMD.ExecuteNonQuery();

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(counter.ToString());
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                        }
                    }

                    break;
                case "EquipSlot":

                    //Dictionary<EquipSlot,EquipType>
                    Dictionary<string, string> EquipmentDict = new Dictionary<string, string>()
                    {
                        {"Ring1", "Ring" },{"Ring2", "Ring" },{"Ring3", "Ring" },{"Ring4", "Ring" },
                        {"Pendant1", "Pendant" },{"Pendant2", "Pendant" },
                        {"Face Accessory", "Accessory" },{"Eye Decor", "Accessory" },{"Earring", "Accessory" },{"Belt", "Accessory" },
                        {"Shoulder", "Accessory"},
                        {"Badge", "Misc" },{"Medal", "Misc" },{"Pocket", "Misc" },{"Heart", "Misc" },
                        {"Weapon", "Weapon" },{"Secondary", "Weapon" },{"Emblem", "Weapon" },
                        {"Hat", "Armor" },{"Top", "Armor" },{"Btm", "Armor" },{"Overall", "Armor" },{"Cape", "Armor" },{"Shoes", "Armor" },
                        {"Gloves", "Gloves" }

                    };

                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (string Eitem in EquipmentDict.Keys)
                        {   
                            counter++;
                            string insertQuery = "INSERT INTO " + tableName + " VALUES(@ES, @ET);";
                            using (SqliteCommand insertCMD = new SqliteCommand(insertQuery, connection, transaction))
                            {
                                insertCMD.Parameters.AddWithValue("@ES", Eitem);
                                insertCMD.Parameters.AddWithValue("@ET", EquipmentDict[Eitem]);

                                try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(counter.ToString());
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }

                    break;
                case "Accessories":
                    List<EquipModel> AccessoriesList = await GetAccessoriesCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {

                        string insertQuery = "INSERT INTO " + tableName + " (" +
                        "EquipName, EquipSet,ClassType, EquipSlot, EquipLevel, " +
                        "MainStat, SecStat,AllStat, HP, MP, DEF, ATK, MATK, SPD, JUMP, IED) VALUES" +
                        "(@EN, @ESet,@CT, @ESlot, @EL" +
                        ",@MS, @SS, @AS, @HP, @MP, @DEF, @ATK, @MATK, @SPD, @JUMP, @IED);";

                        
                        using (SqliteCommand insertCMD = new SqliteCommand(insertQuery, connection, transaction))
                        {
                            foreach (EquipModel Aitem in AccessoriesList)
                            {   
                                counter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@EN", Aitem.EquipName);
                                insertCMD.Parameters.AddWithValue("@ESet", Aitem.EquipSet);
                                insertCMD.Parameters.AddWithValue("@ESlot", Aitem.EquipSlot);
                                insertCMD.Parameters.AddWithValue("@CT", Aitem.ClassType);
                                insertCMD.Parameters.AddWithValue("@EL", Aitem.EquipLevel);

                                insertCMD.Parameters.AddWithValue("@MS", Aitem.BaseStats.MS);
                                insertCMD.Parameters.AddWithValue("@SS", Aitem.BaseStats.SS);
                                insertCMD.Parameters.AddWithValue("@AS", Aitem.BaseStats.AllStat);
                                insertCMD.Parameters.AddWithValue("@HP", Aitem.BaseStats.SpecialHP);
                                insertCMD.Parameters.AddWithValue("@MP", Aitem.BaseStats.SpecialMP);
                                insertCMD.Parameters.AddWithValue("@DEF", Aitem.BaseStats.DEF);
                                insertCMD.Parameters.AddWithValue("@ATK", Aitem.BaseStats.ATK);
                                insertCMD.Parameters.AddWithValue("@MATK", Aitem.BaseStats.MATK);
                                insertCMD.Parameters.AddWithValue("@SPD", Aitem.BaseStats.SPD);
                                insertCMD.Parameters.AddWithValue("@JUMP", Aitem.BaseStats.JUMP);
                                insertCMD.Parameters.AddWithValue("@IED", Aitem.BaseStats.IED);

                                try
                                {

                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(counter.ToString());
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                        }


                    }
                    break;
                case "Weapon":
                    List<EquipModel> WeapList = await GetWeaponCSVAsync();

                    if(connection.State == ConnectionState.Open)
                    {
                        string insertQuery = "INSERT INTO " + tableName + " (" +
                            "EquipSet, WeaponType, EquipLevel, ATKSPD, MainStat, SecStat, HP, DEF, ATK, MATK, SPD, BDMG, IED) VALUES" +
                            "(@ES, @WT, @EL, @AS, @MS, @SS,@HP, @DEF, @ATK, @MATK, @SPD, @BDMG, @IED);";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertQuery, connection, transaction))
                        {
                            foreach (EquipModel model in WeapList)
                            {
                                counter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@ES", model.EquipSet);
                                insertCMD.Parameters.AddWithValue("@WT", model.WeaponType);
                                insertCMD.Parameters.AddWithValue("@EL", model.EquipLevel);
                                insertCMD.Parameters.AddWithValue("@AS", model.BaseStats.ATKSPD);
                                insertCMD.Parameters.AddWithValue("@MS", model.BaseStats.MS);
                                insertCMD.Parameters.AddWithValue("@SS", model.BaseStats.SS);
                                insertCMD.Parameters.AddWithValue("@HP", model.BaseStats.HP);
                                insertCMD.Parameters.AddWithValue("@DEF", model.BaseStats.DEF);
                                insertCMD.Parameters.AddWithValue("@ATK", model.BaseStats.ATK);
                                insertCMD.Parameters.AddWithValue("@MATK", model.BaseStats.MATK);
                                insertCMD.Parameters.AddWithValue("@SPD", model.BaseStats.SPD);
                                insertCMD.Parameters.AddWithValue("@BDMG", model.BaseStats.BD);
                                insertCMD.Parameters.AddWithValue("@IED", model.BaseStats.IED);

                                try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                    Console.WriteLine(counter.ToString());
                                }
                            }
                        }
                    }

                    break;

                case "ClassMWeapon":

                    Dictionary<int, List<string>> CWDict = await GetClassMWeaponCSVAsync();
                    if (connection.State == ConnectionState.Open) {

                        string insertQ = "INSERT INTO " + tableName + "(" +
                            "ClassName, WeaponType) VALUES (" +
                            "@CN, @WT );";
                        using(SqliteCommand insertCMD = new SqliteCommand(insertQ, connection, transaction))
                        {
                            foreach (int CN in CWDict.Keys)
                            {
                                counter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@CN", CWDict[CN][0]);
                                insertCMD.Parameters.AddWithValue("@WT", CWDict[CN][1]);

                                try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                    Console.WriteLine(counter.ToString());
                                }
                            }
                        }

                    }


                    break;

                   
                default:
                    break;
            }

        }

        private static async Task<List<Boss>> GetBossCSVAsync()
        {

            List<Boss> AllBossList = new List<Boss>();

            StorageFile statTable = await GlobalVars.storageFolder.GetFileAsync(GlobalVars.CalculationsPath + "BossListData.csv");

            var stream = await statTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string bossEntry in result.Skip(1))
                    {
                        if (bossEntry == "")
                        {
                            return AllBossList;
                        }

                        var temp = bossEntry.Split(",");
                        counter += 1;
                        Boss tempboss = new Boss(counter, temp[0], temp[1], temp[2], Convert.ToInt32(temp[3]), Convert.ToInt32(temp[4]), Convert.ToInt32(temp[5]));
                        //Boss Contructor BossID | Name | Difficulty | Entry Type | Entry Limit | BossCrystalCount | Meso


                        AllBossList.Add(tempboss);


                    }

                }
            }



            return AllBossList;
        }

        private static async Task<List<SFGain>> GetSFCSVAsync()
        {
            List<SFGain> SFList = new List<SFGain>();

            StorageFile statTable = await GlobalVars.storageFolder.GetFileAsync(GlobalVars.CalculationsPath + "statGains.csv");

            var stream = await statTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string SFEntry in result.Skip(1))
                    {
                        if (SFEntry == "")
                        {
                            return SFList;
                        }

                        var sfitem = SFEntry.Split(",");
                        if (counter >= 15)
                        {
                            counter += 1;
                            SFGain tempSFitem2 = new SFGain();

                            tempSFitem2.StarForceLevel = counter;
                            tempSFitem2.MainStatL = sfitem[1].Split(";").ToList().Select(s => int.Parse(s)).ToList();
                            tempSFitem2.NonWeapDef = Convert.ToInt32(sfitem[2]);
                            tempSFitem2.OverallDef = Convert.ToInt32(sfitem[3]);
                            tempSFitem2.MaxHP = Convert.ToInt32(sfitem[4]);
                            tempSFitem2.MaxMP = Convert.ToInt32(sfitem[5]);
                            tempSFitem2.WATKL = sfitem[6].Split(";").ToList().Select(s => int.Parse(s)).ToList();
                            tempSFitem2.NonWATKL = sfitem[7].Split(";").ToList().Select(s => int.Parse(s)).ToList();
                            tempSFitem2.Speed = Convert.ToInt32(sfitem[8]);
                            tempSFitem2.Jump = Convert.ToInt32(sfitem[9]);
                            tempSFitem2.GloveAtk = Convert.ToInt32(sfitem[10]);


                            SFList.Add(tempSFitem2);
                        }
                        else
                        {
                            counter += 1;
                            SFGain tempSFitem1 = new SFGain();
                            tempSFitem1.StarForceLevel = counter;
                            tempSFitem1.MainStat = Convert.ToInt32(sfitem[1]);
                            tempSFitem1.NonWeapDef = Convert.ToInt32(sfitem[2]);
                            tempSFitem1.OverallDef = Convert.ToInt32(sfitem[3]);
                            tempSFitem1.MaxHP = Convert.ToInt32(sfitem[4]);
                            tempSFitem1.MaxMP = Convert.ToInt32(sfitem[5]);
                            tempSFitem1.WATK = Convert.ToInt32(sfitem[6]);
                            tempSFitem1.NonWATK = Convert.ToInt32(sfitem[7]);
                            tempSFitem1.Speed = Convert.ToInt32(sfitem[8]);
                            tempSFitem1.Jump = Convert.ToInt32(sfitem[9]);
                            tempSFitem1.GloveAtk = Convert.ToInt32(sfitem[10]);

                            SFList.Add(tempSFitem1);

                        }

                    }

                }
            }



            return SFList;
        }

        private static async Task<List<Character>> GetCharCSVAsync()
        {
            List<Character> characterList = new List<Character>();


            StorageFile charTable = await GlobalVars.storageFolder.GetFileAsync(GlobalVars.CharacterPath + "CharacterData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string characterItem in result.Skip(1))
                    {
                        if (characterItem == "")
                        {
                            return characterList;
                        }

                        var temp = characterItem.Split(",");
                        counter += 1;
                        Character tempChar = new Character();
                        tempChar.ClassName = temp[1];
                        tempChar.Faction = temp[2];
                        tempChar.ClassType = temp[3];
                        tempChar.MainStat = temp[4];
                        tempChar.SecStat = temp[5];
                        tempChar.unionEffect = temp[6];
                        tempChar.unionEffectType = temp[7];

                        characterList.Add(tempChar);

                    }
                }
            }


            return characterList;
        }

        private static async Task<List<UnionModel>> GetUnionECSVAsync()
        {
            List<UnionModel> unionList = new List<UnionModel>();

            StorageFile UnionTable = await GlobalVars.storageFolder.GetFileAsync(GlobalVars.CharacterPath + "UnionData.csv");

            var stream = await UnionTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string unionItems in result.Skip(1))
                    {
                        if (unionItems == "")
                        {
                            return unionList;
                        }
                        var temp = unionItems.Split(",");
                        counter += 1;
                        UnionModel tempUnion = new UnionModel();
                        tempUnion.Effect = temp[1];
                        tempUnion.RankB = Convert.ToInt32(temp[2]);
                        tempUnion.RankA = Convert.ToInt32(temp[3]);
                        tempUnion.RankS = Convert.ToInt32(temp[4]);
                        tempUnion.RankSS = Convert.ToInt32(temp[5]);
                        tempUnion.RankSSS = Convert.ToInt32(temp[6]);
                        tempUnion.EffectType = temp[7];


                        //Stat = temp[0],
                        //    StatType = temp[1],
                        //    RankB = Convert.ToInt32(temp[2]),
                        //    RankA = Convert.ToInt32(temp[3]),
                        //    RankS = Convert.ToInt32(temp[4]),
                        //    RankSS = Convert.ToInt32(temp[5]),
                        //    RankSSS = Convert.ToInt32(temp[6])
                        unionList.Add(tempUnion);
                    }
                }
            }

            return unionList;
        }

        private static async Task<List<EquipModel>> GetArmorCSVAsync()
        {
            List<EquipModel> equipList = new List<EquipModel>();

            StorageFile charTable = await GlobalVars.storageFolder.GetFileAsync(GlobalVars.EquipmentPath + "ArmorData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string equipItem in result.Skip(1))
                    {
                        if (equipItem == "")
                        {
                            return equipList;
                        }

                        var temp = equipItem.Split(",");
                        counter += 1;
                        EquipModel equip = new EquipModel();
                        equip.EquipSet = temp[1];
                        equip.ClassType = temp[2];
                        equip.EquipSlot = temp[3];
                        equip.EquipLevel = Convert.ToInt32(temp[4]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[5]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[7]);
                        equip.BaseStats.HP = Convert.ToInt32(temp[8]);
                        equip.BaseStats.MP = Convert.ToInt32(temp[9]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[10]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[11]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[12]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[13]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[14]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[15]);

                        equipList.Add(equip);

                    }
                }
            }
            return equipList;

        }

        private static async Task<List<EquipModel>> GetAccessoriesCSVAsync()
        {
            List<EquipModel> AccessoriesList = new List<EquipModel>();

            StorageFile charTable = await GlobalVars.storageFolder.GetFileAsync(GlobalVars.EquipmentPath + "AccessoriesData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string accItem in result.Skip(1))
                    {
                        if (accItem == "")
                        {
                            return AccessoriesList;
                        }

                        var temp = accItem.Split(",");

                        EquipModel equip = new EquipModel();
                        equip.EquipName = temp[1];
                        equip.EquipSet  = temp[2];
                        equip.ClassType = temp[3];
                        equip.EquipSlot = temp[4];

                        equip.EquipLevel = Convert.ToInt32(temp[5]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[7]);
                        equip.BaseStats.AllStat = Convert.ToInt32(temp[8]);

                        equip.BaseStats.SpecialHP = temp[9];
                        equip.BaseStats.SpecialMP = temp[10];
                        equip.BaseStats.DEF = Convert.ToInt32(temp[11]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[12]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[13]);
                        
                        equip.BaseStats.SPD = Convert.ToInt32(temp[14]);
                        equip.BaseStats.JUMP = Convert.ToInt32(temp[15]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[16]);


                        AccessoriesList.Add(equip);
                        
                        
                    }
                }
            }


            return AccessoriesList;
        }

        private static async Task<List<EquipModel>> GetWeaponCSVAsync()
        {
            List<EquipModel> WeaponList = new List<EquipModel>();

            StorageFile charTable = await GlobalVars.storageFolder.GetFileAsync(GlobalVars.EquipmentPath + "WeaponData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string weapItem in result.Skip(1))
                    {
                        if (weapItem == "")
                        {
                            return WeaponList;
                        }

                        var temp = weapItem.Split(",");

                        EquipModel equip = new EquipModel();
                        equip.EquipSet = temp[1];
                        equip.WeaponType = temp[2];
                        equip.EquipLevel = Convert.ToInt32(temp[3]);
                        equip.BaseStats.ATKSPD = Convert.ToInt32(temp[4]);
                        equip.BaseStats.MS = Convert.ToInt32(temp[5]);
                        equip.BaseStats.SS = Convert.ToInt32(temp[6]);
                        equip.BaseStats.ATK = Convert.ToInt32(temp[7]);
                        equip.BaseStats.MATK = Convert.ToInt32(temp[8]);
                        equip.BaseStats.SPD = Convert.ToInt32(temp[9]);
                        equip.BaseStats.HP = Convert.ToInt32(temp[10]);
                        equip.BaseStats.DEF = Convert.ToInt32(temp[11]);
                        equip.BaseStats.BD = Convert.ToInt32(temp[12]);
                        equip.BaseStats.IED = Convert.ToInt32(temp[13]);
                                                
                        WeaponList.Add(equip);


                    }
                }
            }


            return WeaponList;


        }

        private static async Task<Dictionary<int, List<string>>> GetClassMWeaponCSVAsync()
        {
            Dictionary<int, List<string>> CWdict = new Dictionary<int, List<string>>();
            StorageFile charTable = await GlobalVars.storageFolder.GetFileAsync(GlobalVars.CharacterPath + "ClassMainWeapon.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string CI in result.Skip(1))
                    {
                        if (CI == "")
                        {
                            return CWdict;
                        }
                        var temp = CI.Split(',');
                        var tempL = new List<string>() { temp[1], temp[2] };
                        CWdict.Add(counter, tempL);

                        counter++;
                    }
                }
            }


            return CWdict;
        }



        ///////Retrieving Data from Maplestory.db

        public static Dictionary<int, Boss> GetBossDB()
        {
            Dictionary<int, Boss> bossDict = new Dictionary<int, Boss>();

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename = {GlobalVars.databasePath}"))
            {
                dbConnection.Open();
                string getBossCmd = "SELECT * FROM BossList";

                SqliteCommand selectCmd = new SqliteCommand(getBossCmd, dbConnection);

                SqliteDataReader query = selectCmd.ExecuteReader();


                while (query.Read())
                {
                    Boss tempBoss = new Boss();
                    tempBoss.BossID = query.GetInt16(0);
                    tempBoss.name = query.GetString(1);
                    tempBoss.difficulty = query.GetString(2);
                    tempBoss.entryType = query.GetString(3);
                    tempBoss.entryLimit = query.GetInt16(4);
                    tempBoss.bossCrystalCount = query.GetInt16(5);
                    tempBoss.meso = query.GetInt32(6);

                    bossDict.Add(query.GetInt32(0), tempBoss);
                }



                dbConnection.Close();

            }


            return bossDict;
        }

        public static Dictionary<string, Character> GetAllCharDB()
        {
            Dictionary<string, Character> charDict = new Dictionary<string, Character>();

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GlobalVars.databasePath}"))
            {
                dbConnection.Open();
                string getCharCmd = "SELECT * FROM AllCharacter";

                using (SqliteCommand selectCmd = new SqliteCommand(getCharCmd, dbConnection))
                {
                    using (SqliteDataReader query = selectCmd.ExecuteReader())
                    {
                        while (query.Read())
                        {
                            Character tempChar = new Character();
                            tempChar.ClassName = query.GetString(0);
                            tempChar.ClassType = query.GetString(1);
                            tempChar.Faction = query.GetString(2);
                            tempChar.unionEffect = query.GetString(3);
                            tempChar.unionEffectType = query.GetString(4);
                            tempChar.MainStat = query.GetString(5);
                            tempChar.SecStat = query.GetString(6);

                            charDict.Add(query.GetString(0), tempChar);
                        }
                    }
                }
            }
            return charDict;
        }

        public static Dictionary<string, Character> GetAllCharTrackDB()
        {
            Dictionary<string, Character> charDict = new Dictionary<string, Character>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GlobalVars.databasePath}"))
            {
                dbCon.Open();

                string getCharCMD = "SELECT * FROM CharacterTrack";

                using (SqliteCommand selectCMD = new SqliteCommand(getCharCMD, dbCon))
                {
                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            Character tempChar = new Character();
                            tempChar.ClassName = result.GetString(0);
                            tempChar.unionRank = result.GetString(1);
                            tempChar.level = result.GetInt32(2);

                            charDict.Add(result.GetString(0), tempChar);
                        }
                    }
                }
            }
            return charDict;
        }

        public static List<EquipModel> GetAllArmorDB()
        {
            List<EquipModel> equipList = new List<EquipModel>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename ={GlobalVars.databasePath}"))
            {
                dbCon.Open();

                string getArmor = "SELECT * FROM ArmorStats";

                using (SqliteCommand selectCMD = new SqliteCommand(getArmor, dbCon))
                {
                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {

                        while (result.Read())
                        {
                            EquipModel tempEquip = new EquipModel();
                            tempEquip.EquipSet = result.GetString(0);
                            tempEquip.ClassType = result.GetString(1);
                            tempEquip.EquipSlot = result.GetString(2);
                            tempEquip.EquipLevel = result.GetInt32(3);
                            tempEquip.BaseStats.MS = result.GetInt32(4);
                            tempEquip.BaseStats.SS = result.GetInt32(5);
                            tempEquip.BaseStats.HP = result.GetInt32(6);
                            tempEquip.BaseStats.MP = result.GetInt32(7);
                            tempEquip.BaseStats.ATK = result.GetInt32(8);
                            tempEquip.BaseStats.MATK = result.GetInt32(9);
                            tempEquip.BaseStats.DEF = result.GetInt32(10);
                            tempEquip.BaseStats.SPD = result.GetInt32(11);
                            tempEquip.BaseStats.JUMP = result.GetInt32(12);
                            tempEquip.BaseStats.IED = result.GetInt32(13);

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

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GlobalVars.databasePath}"))
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

        public static List<EquipModel> GetAllAccessoriesDB()
        {
            List<EquipModel> accModel = new List<EquipModel>();

            using(SqliteConnection dbCon = new SqliteConnection($"Filename = {GlobalVars.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM AccessoriesData";

                using(SqliteCommand selectCMD = new SqliteCommand( selectQuery, dbCon))
                {
                    using(SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipModel equipModel = new EquipModel();
                            equipModel.EquipName = reader.GetString(1);
                            equipModel.EquipSet = reader.GetString(2);
                            equipModel.ClassType = reader.GetString(3);
                            equipModel.EquipSlot = reader.GetString(4);

                            equipModel.EquipLevel = reader.GetInt32(5);
                            equipModel.BaseStats.MS = reader.GetInt32(6);
                            equipModel.BaseStats.SS = reader.GetInt32(7);
                            equipModel.BaseStats.AllStat = reader.GetInt32(8);
                            equipModel.BaseStats.SpecialHP = reader.GetString(9);
                            equipModel.BaseStats.SpecialMP = reader.GetString(10);
                            equipModel.BaseStats.DEF = reader.GetInt32(11);
                            equipModel.BaseStats.ATK = reader.GetInt32(12);
                            equipModel.BaseStats.MATK = reader.GetInt32(13);
                            equipModel.BaseStats.SPD = reader.GetInt32(14);
                            equipModel.BaseStats.JUMP = reader.GetInt32(15);
                            equipModel.BaseStats.IED = reader.GetInt32(16);

                            accModel.Add(equipModel);
                        }
                    }
                }


            }



            return accModel;
        }
        
        public static List<EquipModel> GetAllWeaponDB()
        {
            List<EquipModel> weapModel = new List<EquipModel>();

            using(SqliteConnection dbCon = new SqliteConnection($"Filename = {GlobalVars.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM WeaponData";

                using(SqliteCommand selectCMD = new SqliteCommand( selectQuery, dbCon))
                {
                    using(SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EquipModel equipModel = new EquipModel();
                            equipModel.EquipSet = reader.GetString(1);
                            equipModel.WeaponType = reader.GetString(2);

                            equipModel.EquipLevel = reader.GetInt32(3);
                            equipModel.BaseStats.ATKSPD= reader.GetInt32(4);
                            equipModel.BaseStats.MS = reader.GetInt32(5);
                            equipModel.BaseStats.SS = reader.GetInt32(6);
                            equipModel.BaseStats.HP = reader.GetInt32(7);
                            equipModel.BaseStats.DEF = reader.GetInt32(8);
                            equipModel.BaseStats.ATK = reader.GetInt32(9);

                            equipModel.BaseStats.SPD= reader.GetInt32(10);
                            equipModel.BaseStats.BD = reader.GetInt32(11);
                            equipModel.BaseStats.IED = reader.GetInt32(12);
                            
                            
                            equipModel.BaseStats.MATK = reader.GetInt32(12);

                            weapModel.Add(equipModel);
                        }
                    }
                }


            }



            return weapModel;
        }


        ///<Summary>
        ///Insertion into Database
        /// </Summary>
        public static bool insertCharTrack(Character character)
        {
            bool insertPassed;

            string insertQueryStr = "INSERT INTO CharacterTrack (CharName, UnionRank, Level, Starforce ) VALUES (@CN, @UR, @Lvl, @SF)";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GlobalVars.databasePath}"))
            {
                dbCon.Open();

                try
                {
                    using (SqliteCommand insertCMD = new SqliteCommand(insertQueryStr, dbCon))
                    {
                        insertCMD.Parameters.AddWithValue("@CN", character.ClassName);
                        insertCMD.Parameters.AddWithValue("@UR", character.unionRank);
                        insertCMD.Parameters.AddWithValue("@Lvl", character.level);
                        insertCMD.Parameters.AddWithValue("@SF", character.starforce);

                        insertCMD.ExecuteNonQuery();
                    }

                    insertPassed = true;
                    return insertPassed;
                }
                catch (SqliteException)
                {
                    return insertPassed = false;
                }
            }
        }

        public static List<Boss> getCharBossList(string character)
        {


            List<Boss> bossList = new List<Boss>();

            string getMesoTrackQuery = @"SELECT " +
                "BossList.BossID," +
                "BossList.BossName, " +
                "BossList.Difficulty," +
                "BossList.EntryType," +
                "BossList.EntryLimit," +
                "BossList.BossCrystal, " +
                "BossList.Meso " +
                "FROM BossList " +
                "INNER JOIN BossMesoGains ON BossMesoGains.BossID = BossList.BossID " +
                "WHERE BossMesoGains.charName = @charName";


            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GlobalVars.databasePath}"))
            {
                dbCon.Open();
                using (SqliteCommand selectCMD = new SqliteCommand())
                {
                    selectCMD.CommandText = getMesoTrackQuery;
                    selectCMD.Connection = dbCon;
                    selectCMD.Parameters.AddWithValue("@charName", character);

                    using (SqliteDataReader result = selectCMD.ExecuteReader())
                    {
                        while (result.Read())
                        {

                            Boss tempBoss = new Boss()
                            {
                                BossID = result.GetInt16(0),
                                name = result.GetString(1),
                                difficulty = result.GetString(2),
                                entryType = result.GetString(3),
                                entryLimit = result.GetInt16(4),
                                bossCrystalCount = result.GetInt16(5),
                                meso = result.GetInt32(6)

                            };
                            bossList.Add(tempBoss);
                        }
                    }

                }
            }

            return bossList;
        }

        public static bool insertCharTBossList(string charName, string bossName, int bossID)
        {
            bool insertPassed = false;

            string insertQueryStr = "INSERT INTO BossMesoGains (charName, BossName, BossID) VALUES (@charName, @bossName, @bossID)";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename={GlobalVars.databasePath}"))
            {
                dbCon.Open();
                try
                {
                    using (SqliteCommand insertCMD = new SqliteCommand(insertQueryStr, dbCon))
                    {
                        insertCMD.Parameters.AddWithValue("@charName", charName);
                        insertCMD.Parameters.AddWithValue("@bossName", bossName);
                        insertCMD.Parameters.AddWithValue("@bossID", bossID);

                        insertCMD.ExecuteNonQuery();
                    }


                    insertPassed = true;
                    return insertPassed;
                }
                catch (SqliteException)
                {
                    insertPassed = false;
                    return insertPassed;
                }
            }

        }

        public static bool insertCharTEquip(EquipModel selectedEquip)
        {
            bool insertPassed = false;

            using (SqliteConnection dbCon = new SqliteConnection(GlobalVars.CONN_STRING))
            {
                dbCon.Open();
                using (var transaction = dbCon.BeginTransaction())
                {
                    try
                    {

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        insertPassed = false;
                    }
                }
            }

            return insertPassed;
        }

        /// <summary>
        /// Deletion from Database
        /// </summary>
        public static bool deleteCharBossList(string charName, int bossID)
        {
            bool deletePassed = false;
            string deleteQuery = "DELETE FROM BossMesoGains WHERE charName =  @CN AND BossID = @BID";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename ={GlobalVars.databasePath}"))
            {
                dbCon.Open();
                try
                {
                    using (SqliteCommand deleteCMD = new SqliteCommand(deleteQuery, dbCon))
                    {
                        deleteCMD.Parameters.AddWithValue("@CN", charName);
                        deleteCMD.Parameters.AddWithValue("@BID", bossID);

                        deleteCMD.ExecuteNonQuery();
                    }

                    deletePassed = true;
                    return deletePassed;
                }
                catch (SqliteException)
                {
                    deletePassed = false;
                    return deletePassed;
                }
            }

        }

        public static bool deleteCharT(Character character)
        {
            bool deletePassed = false;
            using (SqliteConnection dbCon = new SqliteConnection(GlobalVars.CONN_STRING))
            {
                
                dbCon.Open();
                string deleteQuery = "DELETE FROM CharacterTrack WHERE CharName = @CN";
                using (var transaction = dbCon.BeginTransaction())
                {
                    try
                    {
                        using(SqliteCommand deleteCMD = new SqliteCommand(deleteQuery, dbCon, transaction))
                        {
                            deleteCMD.Parameters.AddWithValue("@CN", character.ClassName);
                            deleteCMD.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        return deletePassed = true;
                    }
                    catch(SqliteException e)
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                        deletePassed = false;
                    }
                }
            }

            return deletePassed;
        }



    }


}



//public async static Task writetoEventJson(string m, List<EventRecords> eventRecords)
//{
//    string jsonFP = Path.Combine(ApplicationData.Current.LocalFolder.Path, @"Data\");

//    string filename = "Event.json";
//    switch (m)
//    {
//        case "init":

//            string jsonString = JsonSerializer.Serialize<List<EventRecords>>(eventRecords);
//            using (FileStream createStream = File.Create(filename))
//            {
//                await JsonSerializer.SerializeAsync(createStream, eventRecords);

//            }

//            File.WriteAllText(jsonFP,filename);

//                break;

//        case "update":
//            break;
//    }



//}
