using Dasync.Collections;
using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.StarforceRes;
using MSEACalculator.UnionRes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;


namespace MSEACalculator.OtherRes.Database
{
    public class DatabaseINIT
    {

        //Codes for First initialising of Data
        //From CSV (DefaultData folder) to Sqlite Database (Maplestory.db)
        public async static Task DBInit()
        {
            //string dbName = "Maplestory.db";
            await ApplicationData.Current.LocalFolder.CreateFileAsync("Maplestory.db", CreationCollisionOption.OpenIfExists);



            List<TableStats> staticTables = new List<TableStats>();


            ///////CHARACTER///////

            //TABLE FOR DEFAULT CHARACTER LIST
            string[] CharacterTableSpec = {"(" +
                            "ClassName string," +
                            "ClassType string," +
                            "Faction string," +
                            "MainStat string," +
                            "SecStat string," +
                            "UnionE string," +
                            "UnionET string," +
                            "PRIMARY KEY (ClassName)" +
                            ");" };
            staticTables.Add(new TableStats("AllCharacter", CharacterTableSpec[0], "AllCharacter"));



            //TABLE FOR UNION EFFECTS
            string[] unionETableSpecs = {"(" +
                "Effect string," +
                "EffectType string," +
                "B int," +
                "A int," +
                "S int," +
                "SS int," +
                "SSS int," +
                "PRIMARY KEY(Effect, EffectType)" +
                ");" }; 
            staticTables.Add(new TableStats("UnionEffects", unionETableSpecs[0], "UnionEffect"));

            string ClassWeaponSpec = "(" +
                "ClassName string," +
                "WeaponType string," +
                "PRIMARY KEY (ClassName, WeaponType)" +
                ");";

            staticTables.Add(new TableStats("ClassMainWeap", ClassWeaponSpec, "ClassMWeapon"));
            staticTables.Add(new TableStats("ClassSecWeap", ClassWeaponSpec, "ClassSWeapon"));




            ///////BOSSING///////


            string bossListSpec = "(" +
                "BossID int," +
                "BossName varchar(50) NOT NULL," +
                "Difficulty varchar(10) NOT NULL," +
                "EntryType varchar(10) NOT NULL," +
                "Meso int NOT NULL," +
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
            string[] equipTableSpec = {"(" +
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
                ");" };
            staticTables.Add(new TableStats("ArmorStats", equipTableSpec[0], "Armor"));


            //TABLE FOR ACCESSORIES
            string[] AccessoriesTableSpec = {"(" +
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
                "IED int," +
                "PRIMARY KEY (EquipName, EquipSet, ClassType, EquipSlot) " +
                ");" };
            staticTables.Add(new TableStats("AccessoriesData", AccessoriesTableSpec[0], "Accessories"));

            //TABLE FOR WEAPON
            string[] WeapTableSpec = {"(" +
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
                ");" };
            staticTables.Add(new TableStats("WeaponData", WeapTableSpec[0], "Weapon"));
            

            string[] SecWeapTableSpec = {"(" +
                "ClassName string," +
                "WeaponType string," +
                "EquipName string," +
                "EquipLevel int," +
                "MainStat int," +
                "SecStat int," +
                "ATK int," +
                "MATK int," +
                "AllStat int," +
                "DEF int," +
                "HP int," +
                "MP int," +
                "ATKSPD int," +
                "PRIMARY KEY (ClassName, WeaponType, EquipName, EquipLevel) " +
                ");" };
            staticTables.Add(new TableStats("SecondaryWeaponData", SecWeapTableSpec[0], "Secondary"));




            //Equipment Set Effects

            /////CALCULATIONS/////

            //TABLE FOR STARFORCE STATS
            string[] sfTableSpec = {"(" +
                "SFID int," +
                "JobStat int," +
                "NonWeapVDef int," +
                "OverallVDef int," +
                "CatAMaxHP int," +
                "WeapMaxMP int," +
                "WeapVATK int," +
                "WeapVMATK int," +
                "SJump int," +
                "SSpeed int," +
                "GloveVATK int, " +
                "GloveVMATK int," +
                "PRIMARY KEY (SFID)" +
                ");" };
            staticTables.Add(new TableStats("StarforceList", sfTableSpec[0], "StarForce"));

            //TABLE FOR STARFORCE STATS ADDONS
            string[] addSFstatSpec = {"(" +
                "SFID int," +
                "LevelRank int," +
                "VStat int," +
                "NonWeapVATK int," +
                "NonWeapVMATK int," +
                "WeapVATK int," +
                "WeapVMATK int," +
                "PRIMARY KEY (SFID, LevelRank)" +
                ");" };
            staticTables.Add(new TableStats("AddSFStat", addSFstatSpec[0], "AddStarForce"));

            string[] PotSpec = { "(" +
                    "EquipGrp string," +
                    "Grade string," +
                    "GradeT string," +
                    "StatT string," +
                    "Stat nvarchar," +
                    "MinLvl int," +
                    "MaxLvl int," +
                    "ValueI nvarchar," +
                    "Duration int," +
                    "PRIMARY KEY (EquipGrp, Grade, GradeT, Stat, MinLvl, MaxLvl, ValueI)" +
                    ");"};
            staticTables.Add(new TableStats("PotentialData", PotSpec[0], "Potential"));

            /////BLANK TABLES/////







            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                using (var trans = dbConnection.BeginTransaction())
                {
                    try
                    {
                        //INIT Blank Tables <- Tables with foreign key FIRST

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

        public async static Task BlankTablesInit()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("Maplestory.db", CreationCollisionOption.OpenIfExists);

            List<TableStats> blankTables = new List<TableStats>();


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


            string[] charEquipScrollSpec = {"(" +
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
                ");" };
            blankTables.Add(new TableStats("CharTEquipScroll", charEquipScrollSpec[0]));

            string[] charEquipFlameSpec = {"(" +
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
                ");" };
            blankTables.Add(new TableStats("CharTEquipFlame", charEquipFlameSpec[0]));


            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GVar.databasePath}"))
            {
                dbConnection.Open();
                using (var trans = dbConnection.BeginTransaction())
                {
                    try
                    {
                        //INIT Blank Tables <- Tables with foreign key FIRST
                        blankTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection, trans));

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
            if (connection.State == ConnectionState.Open)
            {
                int counter = 0;
                switch (insertType)
                {
                    case "Boss":
                        List<Boss> bosstable = await ImportCSV.GetBossCSVAsync();
                        // update to table


                        counter++;
                        string insertBoss = "INSERT INTO " + tableName + " (BossID, BossName, Difficulty, EntryType, Meso)" +
                        " VALUES (@BossID,@BossName,@Difficulty,@EntryType,@Meso)";
                        using (SqliteCommand insertBosscmd = new SqliteCommand(insertBoss, connection, transaction))
                        {
                            foreach (Boss bossItem in bosstable)
                            {
                                insertBosscmd.Parameters.Clear();
                                insertBosscmd.Parameters.AddWithValue("@BossID", bossItem.BossID);
                                insertBosscmd.Parameters.AddWithValue("@BossName", bossItem.BossName);
                                insertBosscmd.Parameters.AddWithValue("@Difficulty", bossItem.Difficulty);
                                insertBosscmd.Parameters.AddWithValue("@EntryType", bossItem.EntryType);
                                insertBosscmd.Parameters.AddWithValue("@Meso", bossItem.Meso);

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

                        break;

                    case "AllCharacter":

                        List<Character> charTable = await ImportCSV.GetCharCSVAsync();

                        
                        counter++;
                        string insertChar = "INSERT INTO " + tableName + "(ClassName, ClassType, Faction, MainStat, SecStat, UnionE, UnionET)" +
                            " VALUES (@CN, @CT, @Fac, @MS, @SS, @UE, @UET)";

                        using (SqliteCommand insertCMD = new SqliteCommand(insertChar, connection, transaction))
                        {
                            foreach (Character charItem in charTable)
                            {
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@CN", charItem.ClassName);
                                insertCMD.Parameters.AddWithValue("@CT", charItem.ClassType);
                                insertCMD.Parameters.AddWithValue("@Fac", charItem.Faction);
                                insertCMD.Parameters.AddWithValue("@UE", charItem.unionEffect);
                                insertCMD.Parameters.AddWithValue("@UET", charItem.unionEffectType);
                                insertCMD.Parameters.AddWithValue("@MS", charItem.MainStat);
                                insertCMD.Parameters.AddWithValue("@SS", charItem.SecStat);

                                try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(counter.ToString());
                                    Console.WriteLine(ex.ToString());
                                }
                            };

                            

                        }


                        break;
                    case "UnionEffect":
                        List<UnionModel> unionList = await ImportCSV.GetUnionECSVAsync();
                        
                        counter++;
                        string insertUnion = "INSERT INTO " + tableName + " (Effect, EffectType, B, A, S, SS, SSS)" +
                            " VALUES (@E, @ET, @B, @A, @S, @SS, @SSS);";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertUnion, connection, transaction))
                        {
                            foreach (UnionModel unionItem in unionList)
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



                        break;
                    case "Armor":

                        List<EquipModel> equipList = await ImportCSV.GetArmorCSVAsync();

                        counter++;
                        string insertEquip = "INSERT INTO " + tableName + "(" +
                            " EquipSet, ClassType, EquipSlot, EquipLevel, MainStat, SecStat, AllStat, HP, MP, DEF, ATK, MATK, SPD, JUMP, IED)" +
                            " VALUES (@Set, @Job, @Slot, @Lvl, @MS, @SS, @AllS, @HP, @MP, @DEF, @ATK, @MATK, @SPD,@JUMP,@IED);";

                        using (SqliteCommand insertCMD = new SqliteCommand(insertEquip, connection, transaction))
                        {
                            foreach (EquipModel equipItem in equipList)
                            {   
                                insertCMD.Parameters.Clear();
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

                        break;
                    case "EquipSlot":

                        //Dictionary<EquipSlot,EquipType>
                        Dictionary<string, string> EquipmentDict = new Dictionary<string, string>(){
                            {"Ring1", "Ring" },{"Ring2", "Ring" },{"Ring3", "Ring" },{"Ring4", "Ring" },
                            {"Pendant1", "Pendant" },{"Pendant2", "Pendant" },
                            {"Face Accessory", "Accessory" },{"Eye Decor", "Accessory" },{"Earring", "Accessory" },{"Belt", "Accessory" },
                            {"Shoulder", "Accessory"},
                            {"Badge", "Misc" },{"Medal", "Misc" },{"Pocket", "Misc" },
                            {"Heart", "Heart" },
                            {"Weapon", "Weapon" },{"Secondary", "Secondary" },{"Emblem", "Emblem" },
                            {"Hat", "Armor" },{"Top", "Armor" },{"Bottom", "Armor" },{"Overall", "Armor" },{"Cape", "Armor" },{"Shoes", "Armor" },
                            {"Gloves", "Gloves" }
                        };


                        
                        counter++;
                        string insertES = "INSERT INTO " + tableName + " VALUES(@ES, @ET);";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertES, connection, transaction))
                        {
                            foreach (string Eitem in EquipmentDict.Keys)
                            {
                                insertCMD.Parameters.Clear();
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

                        break;
                    case "Accessories":
                        List<EquipModel> AccessoriesList = await ImportCSV.GetAccessoriesCSVAsync();



                        string insertAcc = "INSERT INTO " + tableName + " (" +
                        "EquipName, EquipSet,ClassType, EquipSlot, EquipLevel, " +
                        "MainStat, SecStat,AllStat, HP, MP, DEF, ATK, MATK, SPD, JUMP, IED) VALUES" +
                        "(@EN, @ESet,@CT, @ESlot, @EL" +
                        ",@MS, @SS, @AS, @HP, @MP, @DEF, @ATK, @MATK, @SPD, @JUMP, @IED);";


                        using (SqliteCommand insertCMD = new SqliteCommand(insertAcc, connection, transaction))
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
                        break;
                    case "Weapon":
                        List<EquipModel> WeapList = await ImportCSV.GetWeaponCSVAsync();


                        string insertWeap = "INSERT INTO " + tableName + " (" +
                            "EquipSet, WeaponType, EquipLevel, ATKSPD, MainStat, SecStat, HP, DEF, ATK, MATK, SPD, BDMG, IED) VALUES" +
                            "(@ES, @WT, @EL, @AS, @MS, @SS,@HP, @DEF, @ATK, @MATK, @SPD, @BDMG, @IED);";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertWeap, connection, transaction))
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
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                    Console.WriteLine(counter.ToString());
                                }
                            }

                        }
                        break;

                    case "Secondary":
                        List<EquipModel> SecList = await ImportCSV.GetSecondaryCSVAsync();


                        string insertSec = "INSERT INTO " + tableName + " (" +
                            "ClassName, WeaponType, EquipName, EquipLevel, MainStat, SecStat, ATK, MATK, AllStat, DEF, HP, MP, ATKSPD) VALUES" +
                            "(@ES, @WT, @EN, @EL, @MS, @SS, @ATK, @MATK, @AS, @DEF, @HP, @MP, @ATKSPD);";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertSec, connection, transaction))
                        {
                            foreach (EquipModel model in SecList)
                            {
                                counter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@ES", model.ClassType);
                                insertCMD.Parameters.AddWithValue("@WT", model.WeaponType);
                                insertCMD.Parameters.AddWithValue("@EN", model.EquipName);
                                insertCMD.Parameters.AddWithValue("@EL", model.EquipLevel);
                                insertCMD.Parameters.AddWithValue("@AS", model.BaseStats.AllStat);
                                insertCMD.Parameters.AddWithValue("@MS", model.BaseStats.MS);
                                insertCMD.Parameters.AddWithValue("@SS", model.BaseStats.SS);
                                insertCMD.Parameters.AddWithValue("@HP", model.BaseStats.HP);
                                insertCMD.Parameters.AddWithValue("@MP", model.BaseStats.MP);
                                insertCMD.Parameters.AddWithValue("@DEF", model.BaseStats.DEF);
                                insertCMD.Parameters.AddWithValue("@ATK", model.BaseStats.ATK);
                                insertCMD.Parameters.AddWithValue("@MATK", model.BaseStats.MATK);
                                insertCMD.Parameters.AddWithValue("@ATKSPD", model.BaseStats.ATKSPD);


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

                        break;

                    case "ClassMWeapon":

                        Dictionary<int, List<string>> CMWDict = await ImportCSV.GetClassMWeaponCSVAsync();

                        string insertMW = "INSERT INTO " + tableName + "(" +
                            "ClassName, WeaponType) VALUES (" +
                            "@CN, @WT );";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertMW, connection, transaction))
                        {
                            foreach (int CN in CMWDict.Keys)
                            {
                                counter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@CN", CMWDict[CN][0]);
                                insertCMD.Parameters.AddWithValue("@WT", CMWDict[CN][1]);

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

                        break;
                    case "ClassSWeapon":

                        Dictionary<int, List<string>> CSWDict =  await ImportCSV.GetClassSWeaponCSVAsync();

                        string insertSM = "INSERT INTO " + tableName + "(" +
                            "ClassName, WeaponType) VALUES (" +
                            "@CN, @WT );";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertSM, connection, transaction))
                        {
                            foreach (int CN in CSWDict.Keys)
                            {
                                counter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@CN", CSWDict[CN][0]);
                                insertCMD.Parameters.AddWithValue("@WT", CSWDict[CN][1]);

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

                        break;

                    case "StarForce":

                        List<SFGain> SFList = await ImportCSV.GetSFCSVAsync();

                        string insertSF =  "INSERT INTO " + tableName + "(" +
                            "SFID, JobStat, NonWeapVDef, OverallVDef, CatAMaxHP, WeapMaxMP, WeapVATK, WeapVMATK, SJump, SSpeed, GloveVATK, GloveVMATK ) VALUES " +
                            "(@SFID, @JS, @NWD, @OD, @CMH, @WMP, @WATK, @WMATK, @SJ, @SS, @GATK, @GMATK );";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertSF, connection, transaction))
                        {
                            foreach(SFGain sF in SFList)
                            {
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@SFID", sF.SFLevel);
                                insertCMD.Parameters.AddWithValue("@JS", sF.JobStat);
                                insertCMD.Parameters.AddWithValue("@NWD", sF.NonWeapVDef);
                                insertCMD.Parameters.AddWithValue("@OD", sF.OverallVDef);
                                insertCMD.Parameters.AddWithValue("@CMH", sF.CatAMaxHP);
                                insertCMD.Parameters.AddWithValue("@WMP", sF.WeapMaxMP);
                                insertCMD.Parameters.AddWithValue("@WATK", sF.WeapVATK);
                                insertCMD.Parameters.AddWithValue("@WMATK", sF.WeapVMATK);
                                insertCMD.Parameters.AddWithValue("@SJ", sF.SJump);
                                insertCMD.Parameters.AddWithValue("@SS", sF.SSpeed);
                                insertCMD.Parameters.AddWithValue("GATK", sF.GloveVATK);
                                insertCMD.Parameters.AddWithValue("@GMATK", sF.GloveVMATK);

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

                        break;

                    case "AddStarForce":
                        List<SFGain> ASFList = await ImportCSV.GetAddSFCSVAsync();

                        string insertASF = "INSERT INTO " + tableName + "(" +
                            "SFID, LevelRank, VStat, NonWeapVATK, NonWeapVMATK, WeapVATK, WeapVMATK) VALUES " +
                            "(@SFID, @LR, @VS, @NWATK, @NWMATK, @WATK, @WMATK);";
                        using (SqliteCommand insertCMD = new SqliteCommand(insertASF, connection, transaction))
                        {
                            foreach (SFGain sF in ASFList)
                            {
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@SFID", sF.SFLevel);
                                insertCMD.Parameters.AddWithValue("@LR", sF.LevelRank);
                                insertCMD.Parameters.AddWithValue("@VS", sF.VStat);
                                insertCMD.Parameters.AddWithValue("@NWATK", sF.NonWeapATK);
                                insertCMD.Parameters.AddWithValue("@NWMATK", sF.NonWeapMATK);
                                insertCMD.Parameters.AddWithValue("@WATK", sF.WeapVATK);
                                insertCMD.Parameters.AddWithValue("@WMATK", sF.WeapVMATK);
                               


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
                        break;
                    case "Potential":

                        List<PotentialStats> PotList = await ImportCSV.GetPotentialCSVAsync();

                        string insertPot = "INSERT INTO " + tableName +"(" +
                            "EquipGrp, Grade, GradeT, StatT, Stat, MinLvl, MaxLvl, ValueI, Duration) VALUES " +
                            "(@EG, @G, @GT, @ST, @S, @MinL, @MaxL, @VI, @D);";
                        using(SqliteCommand insertCMD =  new SqliteCommand(insertPot, connection, transaction))
                        {
                            foreach (PotentialStats pot in PotList)
                            {
                                counter+=1;
                                foreach (string i in pot.EquipGrpL)
                                {
                                    string e;
                                    if ( i.Trim() == "Shoulderpad")
                                    {
                                        e = "Shoulder";
                                    }
                                    else
                                    {
                                        e = i.Trim();
                                    }

                                    insertCMD.Parameters.Clear();
                                    insertCMD.Parameters.AddWithValue("@EG", e);
                                    insertCMD.Parameters.AddWithValue("@G", pot.Grade);
                                    insertCMD.Parameters.AddWithValue("@GT", pot.Prime);
                                    insertCMD.Parameters.AddWithValue("@ST", pot.StatType);
                                    insertCMD.Parameters.AddWithValue("@S", pot.StatIncrease);
                                    insertCMD.Parameters.AddWithValue("@MinL", pot.MinLvl);
                                    insertCMD.Parameters.AddWithValue("@MaxL", pot.MaxLvl);
                                    insertCMD.Parameters.AddWithValue("@VI", pot.StatValue);
                                    insertCMD.Parameters.AddWithValue("@D", pot.Duration);

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
                    default:
                        break;
                }

            }

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
