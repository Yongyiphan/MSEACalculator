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
    class DatabaseAccess
    {

        //Codes for First initialising of Data
        //From CSV (DefaultData folder) to Sqlite Database (Maplestory.db)
        public async static Task databaseInit()
        {
            //string dbName = "Maplestory.db";
            await ApplicationData.Current.LocalFolder.CreateFileAsync("Maplestory.db", CreationCollisionOption.OpenIfExists);
            
            

            List<TableStats> staticTables = new List<TableStats>();
            List<TableStats> blankTables = new List<TableStats>();

            //TABLE FOR BOSS DETAILS

            string bossListSpec = "(" +
                "BossID int," +
                "BossName varchar(50) NOT NULL," +
                "Difficulty varchar(10) NOT NULL," +
                "EntryType varchar(10) NOT NULL," +
                "EntryLimit int NOT NULL," +
                "BossCrystal int NOT NULL," +
                "Meso int NOT NULL," +
                "PRIMARY KEY(BossID)" +
                ");";
            staticTables.Add(new TableStats("BossList", bossListSpec, "Boss"));


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


            //TABLE FOR DEFAULT CHARACTER LIST
            string CharacterTableSpec = "(" +
                "ClassName string," +
                "ClassType string," +
                "Faction string," +
                "UnionE string," +
                "UnionET string," +
                "MainStat string," +
                "SecStat string," +
                "PRIMARY KEY (ClassName)" +
                ");";
            staticTables.Add(new TableStats("AllCharacter", CharacterTableSpec, "AllCharacter"));


            //TABLE FOR UNION EFFECTS
            string unionETableSpecs = "(" +
                "Stat string," +
                "StatType string," +
                "B int," +
                "A int," +
                "S int," +
                "SS int," +
                "SSS int," +
                "PRIMARY KEY(Stat, StatType)" +
                ");";
            staticTables.Add(new TableStats("UnionEffects", unionETableSpecs, "UnionEffect"));

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
                "HP int," +
                "MP int," +
                "ATK int," +
                "MATK int," +
                "DEF int," +
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
                "EquipSlot string," +
                "EquipLevel int," +
                "AllStat int," +
                "HP string," +
                "MP string," +
                "WATK int," +
                "MATK int," +
                "DEF int," +
                "SPD int," +
                "JUMP int," +
                "PRIMARY KEY (EquipName, EquipSet, EquipSlot) " +
                ");";
            staticTables.Add(new TableStats("AccessoriesData", AccessoriesTableSpec, "Accessories"));


            //Equipment Set Effects


            //BLANK TABLES
            //TABLE FOR CHARACTER TO TRACK
            string charTrackSpec = "(" +
                "CharName string," +
                "UnionRank string," +
                "Level int," +
                "PRIMARY KEY(CharName)" +
                ");";

            string charTrackTableName = "CharacterTrack";
            TableStats charTrackTable = new TableStats(charTrackTableName, charTrackSpec);
            blankTables.Add(charTrackTable);

            //TABLE FOR CHAR'S BOSS TRACKING
            string bossMesoGainsTableSpec = "(" +
                "CharName string," +
                "BossName string," +
                "BossID int," +
                "PRIMARY KEY(CharName, BossID)," +
                "FOREIGN KEY (CharName) REFERENCES CharacterTrack(CharName) ON DELETE CASCADE," +
                "FOREIGN KEY (BossID) REFERENCES BossList(BossID)" +
                ");";

            string bossMesoTableName = "BossMesoGains";
            TableStats bossMesoGainsTable = new TableStats(bossMesoTableName, bossMesoGainsTableSpec);
            blankTables.Add(bossMesoGainsTable);


            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={GlobalVars.databasePath}"))
            {
                dbConnection.Open();

                //INIT Blank Tables <- Tables with foreign key FIRST
                blankTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection));

                staticTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection));

                await Task.WhenAll(staticTables.ParallelForEachAsync(item => Task.Run(() => InitCSVData(item.initMode, item.tableName, dbConnection))));



                //EMPTY TABLES



                dbConnection.Close();


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

        private static void initTable(string tableName, string tableParameters, SqliteConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                //delete table if exist
                string dropT = "DROP TABLE IF EXISTS " + tableName;
                using (SqliteCommand dropCmd = new SqliteCommand(dropT, connection))
                {
                    dropCmd.ExecuteNonQuery();
                };
                // create table if not exist

                string createTable = "CREATE TABLE IF NOT EXISTS " + tableName + tableParameters;
                using (SqliteCommand createCmd = new SqliteCommand(createTable, connection))
                {
                    createCmd.ExecuteNonQuery();
                };
            }

        }


        public static async Task InitCSVData(string insertType, string tableName, SqliteConnection connection)
        {
            switch (insertType)
            {
                case "Boss":
                    List<Boss> bosstable = await GetBossCSVAsync();
                    // update to table

                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (Boss bossItem in bosstable)
                        {
                            string insertBoss = "INSERT INTO " + tableName + " (BossID, BossName, Difficulty, EntryType, EntryLimit, BossCrystal, Meso)" +
                            " VALUES (@BossID,@BossName,@Difficulty,@EntryType,@EntryLimit,@BossCrystal,@Meso)";
                            using (SqliteCommand insertBosscmd = new SqliteCommand(insertBoss, connection))
                            {
                                insertBosscmd.Parameters.AddWithValue("@BossID", bossItem.BossID);
                                insertBosscmd.Parameters.AddWithValue("@BossName", bossItem.name);
                                insertBosscmd.Parameters.AddWithValue("@Difficulty", bossItem.difficulty);
                                insertBosscmd.Parameters.AddWithValue("@EntryType", bossItem.entryType);
                                insertBosscmd.Parameters.AddWithValue("@EntryLimit", bossItem.entryLimit);
                                insertBosscmd.Parameters.AddWithValue("@BossCrystal", bossItem.bossCrystalCount);
                                insertBosscmd.Parameters.AddWithValue("@Meso", bossItem.meso);

                                insertBosscmd.ExecuteNonQuery();

                            }

                        }
                    }
                    break;
                case "StarForce":

                    List<SFGain> SFtable = await GetSFCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (SFGain sfitem in SFtable)
                        {
                            if (sfitem.StarForceLevel < 16)
                            {
                                string insertSF1to15 = "INSERT INTO " + tableName + "(SFID, MainStat, NonWeaponDef, OverallDef, MaxHP, MaxMP, ATK, NonWeaponATK, ShoeSpeed, ShoeJump, GlovesATK)" +
                                    " VALUES (@SFID, @MainStat, @NonWeapDef, @OveralDef, @MaxHP, @MaxMP, @ATK, @NWATK, @SS, @SJ, @GATK )";
                                SqliteCommand insertSFcmd = new SqliteCommand(insertSF1to15, connection);
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
                                insertSFcmd.ExecuteNonQuery();
                            }
                            else
                            {
                                string insertSF16to25 = "INSERT INTO " + tableName +
                                    " VALUES (@SFID, @MainStat, @NonWeapDef, @OveralDef, @MaxHP, @MaxMP, @ATK, @NWATK, @SS, @SJ, @GATK )";
                                SqliteCommand insertSF2cmd = new SqliteCommand(insertSF16to25, connection);
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
                                insertSF2cmd.ExecuteNonQuery();

                            }
                        }
                    }
                    break;
                case "AddStarForce":

                    List<SFGain> AddSFtable = await GetSFCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {
                        int startCount = 15, endCount = AddSFtable.Count() + 1;

                        while (startCount < endCount)
                        {
                            SFGain tempSFI = AddSFtable[startCount];


                            void addRow(SFGain sfitem, string stattype, List<int> templist)
                            {
                                string insertRow = "INSERT INTO " + tableName +
                                    " VALUES ( @SFID, @ST, @zero, @one, @two, @three, @four ) ";
                                SqliteCommand insertRowCmd = new SqliteCommand(insertRow, connection);
                                insertRowCmd.Parameters.AddWithValue("@SFID", sfitem.StarForceLevel);
                                insertRowCmd.Parameters.AddWithValue("@ST", stattype);
                                insertRowCmd.Parameters.AddWithValue("@zero", templist[0]);
                                insertRowCmd.Parameters.AddWithValue("@one", templist[1]);
                                insertRowCmd.Parameters.AddWithValue("@two", templist[2]);
                                insertRowCmd.Parameters.AddWithValue("@three", templist[3]);
                                insertRowCmd.Parameters.AddWithValue("@four", templist[4]);
                                insertRowCmd.ExecuteNonQuery();
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



                            startCount += 1;
                        }


                    }
                    break;
                case "AllCharacter":

                    List<Character> charTable = await GetCharCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (Character charItem in charTable)
                        {
                            string insertChar = "INSERT INTO " + tableName + "(ClassName, ClassType, Faction, UnionE, UnionET, MainStat, SecStat)" +
                                " VALUES (@CN, @CT, @Fac, @UE, @UET, @MS, @SS)";

                            SqliteCommand insertCharCmd = new SqliteCommand(insertChar, connection);

                            insertCharCmd.Parameters.AddWithValue("@CN", charItem.className);
                            insertCharCmd.Parameters.AddWithValue("@CT", charItem.classType);
                            insertCharCmd.Parameters.AddWithValue("@Fac", charItem.faction);
                            insertCharCmd.Parameters.AddWithValue("@UE", charItem.unionEffect);
                            insertCharCmd.Parameters.AddWithValue("@UET", charItem.unionEffectType);
                            insertCharCmd.Parameters.AddWithValue("@MS", charItem.MainStat);
                            insertCharCmd.Parameters.AddWithValue("@SS", charItem.SecStat);


                            insertCharCmd.ExecuteNonQuery();
                        }
                    }

                    break;
                case "UnionEffect":
                    List<UnionModel> unionList = await GetUnionECSVAsync();
                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (UnionModel unionItem in unionList)
                        {

                            string insertUnion = "INSERT INTO " + tableName + " (Stat, StatType, B, A, S, SS, SSS)" +
                                " VALUES (@Stat, @ST, @B, @A, @S, @SS, @SSS);";
                            using (SqliteCommand insertCMD = new SqliteCommand(insertUnion, connection))
                            {
                                insertCMD.Parameters.AddWithValue("@Stat", unionItem.Stat);
                                insertCMD.Parameters.AddWithValue("@ST", unionItem.StatType);
                                insertCMD.Parameters.AddWithValue("@B", unionItem.RankB);
                                insertCMD.Parameters.AddWithValue("@A", unionItem.RankA);
                                insertCMD.Parameters.AddWithValue("@S", unionItem.RankS);
                                insertCMD.Parameters.AddWithValue("@SS", unionItem.RankSS);
                                insertCMD.Parameters.AddWithValue("@SSS", unionItem.RankSSS);

                                insertCMD.ExecuteNonQuery();
                            }
                        }
                    }


                    break;
                case "Armor":

                    List<EquipModel> equipList = await GetEquipCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (EquipModel equipItem in equipList)
                        {
                            string insertEquip = "INSERT INTO " + tableName + "(EquipSet, ClassType, EquipSlot, EquipLevel, MainStat, SecStat, HP, MP, ATK, MATK, DEF, SPD, JUMP,IED)" +
                                " VALUES (@Set, @Job, @Slot,@Lvl, @MS, @SS, @HP, @MP, @ATK, @MATK,@DEF, @SPD,@JUMP,@IED);";

                            using (SqliteCommand insertCMD = new SqliteCommand(insertEquip, connection))
                            {
                                insertCMD.Parameters.AddWithValue("@Set", equipItem.EquipSet);
                                insertCMD.Parameters.AddWithValue("@Job", equipItem.JobType);
                                insertCMD.Parameters.AddWithValue("@Slot", equipItem.EquipSlot);
                                insertCMD.Parameters.AddWithValue("@Lvl", equipItem.EquipLevel);
                                insertCMD.Parameters.AddWithValue("@MS", equipItem.MS);
                                insertCMD.Parameters.AddWithValue("@SS", equipItem.SS);
                                insertCMD.Parameters.AddWithValue("@HP", equipItem.HP);
                                insertCMD.Parameters.AddWithValue("@MP", equipItem.MP);
                                insertCMD.Parameters.AddWithValue("@ATK", equipItem.ATK);
                                insertCMD.Parameters.AddWithValue("@MATK", equipItem.MATK);
                                insertCMD.Parameters.AddWithValue("@DEF", equipItem.DEF);
                                insertCMD.Parameters.AddWithValue("@SPD", equipItem.SPD);
                                insertCMD.Parameters.AddWithValue("@JUMP", equipItem.JUMP);
                                insertCMD.Parameters.AddWithValue("@IED", equipItem.IED);

                                insertCMD.ExecuteNonQuery();
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
                            string insertQuery = "INSERT INTO " + tableName + " VALUES(@ES, @ET);";
                            using (SqliteCommand insertCMD = new SqliteCommand(insertQuery, connection))
                            {
                                insertCMD.Parameters.AddWithValue("@ES", Eitem);
                                insertCMD.Parameters.AddWithValue("@ET", EquipmentDict[Eitem]);

                                insertCMD.ExecuteNonQuery();
                            }
                        }
                    }

                    break;
                case "Accessories":
                    List<EquipModel>  AccessoriesList = await GetAccessoriesCSVAsync();

                    if(connection.State == ConnectionState.Open)
                    {
                        foreach(EquipModel Aitem in AccessoriesList) 
                        {
                            
                            string insertQuery = "INSERT INTO " + tableName + " (" +
                            "EquipName, EquipSet, EquipSlot, EquipLevel, " +
                            "AllStat, HP, MP, WATK, MATK, DEF, SPD, JUMP) VALUES" +
                            "(@EN, @ES, @ESLOT, @EL, @AS, @HP, @MP, @WATK, @MATK, @DEF, @SPD, @JUMP);";

                            using(SqliteCommand insertCMD = new SqliteCommand(insertQuery, connection))
                            {
                                insertCMD.Parameters.AddWithValue("@EN", Aitem.EquipName);
                                insertCMD.Parameters.AddWithValue("@ES", Aitem.EquipSet);
                                insertCMD.Parameters.AddWithValue("@ESLOT", Aitem.EquipSlot);
                                insertCMD.Parameters.AddWithValue("@EL", Aitem.EquipLevel);
                                insertCMD.Parameters.AddWithValue("@AS", Aitem.AllStat);
                                insertCMD.Parameters.AddWithValue("@HP", Aitem.SpecialHP);
                                insertCMD.Parameters.AddWithValue("@MP", Aitem.SpecialMP);
                                insertCMD.Parameters.AddWithValue("@WATK", Aitem.ATK);
                                insertCMD.Parameters.AddWithValue("@MATK", Aitem.MATK);
                                insertCMD.Parameters.AddWithValue("@DEF", Aitem.DEF);
                                insertCMD.Parameters.AddWithValue("@SPD", Aitem.SPD);
                                insertCMD.Parameters.AddWithValue("@JUMP", Aitem.JUMP);

                                insertCMD.ExecuteNonQuery();
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

            StorageFile statTable = await GlobalVars.storageFolder.GetFileAsync(@"\DefaultData\BossListData.csv");

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

            StorageFile statTable = await GlobalVars.storageFolder.GetFileAsync(@"\DefaultData\statGains.csv");

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


            StorageFile charTable = await GlobalVars.storageFolder.GetFileAsync(@"\DefaultData\CharacterData.csv");

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
                        Character tempChar = new Character(temp[0], temp[1], temp[2], temp[3], temp[4], temp[5], temp[6]);
                        characterList.Add(tempChar);

                    }
                }
            }


            return characterList;
        }

        private static async Task<List<UnionModel>> GetUnionECSVAsync()
        {
            List<UnionModel> unionList = new List<UnionModel>();

            StorageFile UnionTable = await GlobalVars.storageFolder.GetFileAsync(@"\DefaultData\UnionEffect.csv");

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
                        UnionModel tempUnion = new UnionModel(temp[0],
                            temp[1],
                            Convert.ToInt32(temp[2]),
                            Convert.ToInt32(temp[3]),
                            Convert.ToInt32(temp[4]),
                            Convert.ToInt32(temp[5]),
                            Convert.ToInt32(temp[6]));


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

        private static async Task<List<EquipModel>> GetEquipCSVAsync()
        {
            List<EquipModel> equipList = new List<EquipModel>();

            StorageFile charTable = await GlobalVars.storageFolder.GetFileAsync(@"\DefaultData\ArmorData.csv");

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
                        equip.EquipSet = temp[0];
                        equip.JobType = temp[1];
                        equip.EquipSlot = temp[2];
                        equip.EquipLevel = Convert.ToInt32(temp[3]);
                        equip.MS = Convert.ToInt32(temp[4]);
                        equip.SS = Convert.ToInt32(temp[5]);
                        equip.HP = Convert.ToInt32(temp[6]);
                        equip.ATK = Convert.ToInt32(temp[7]);
                        equip.MATK = Convert.ToInt32(temp[8]);
                        equip.DEF = Convert.ToInt32(temp[9]);
                        equip.SPD = Convert.ToInt32(temp[10]);
                        equip.JUMP = Convert.ToInt32(temp[11]);
                        equip.IED = Convert.ToInt32(temp[12]);

                        equipList.Add(equip);

                    }
                }
            }
            return equipList;

        }

        private static async Task<List<EquipModel>> GetAccessoriesCSVAsync()
        {
            List<EquipModel> AccessoriesList = new List<EquipModel>();

            StorageFile charTable = await GlobalVars.storageFolder.GetFileAsync(@"\DefaultData\AccessoriesData.csv");

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
                        equip.EquipName = temp[0];
                        equip.EquipSet = temp[1];
                        equip.EquipSlot = temp[2];
                        equip.EquipLevel = Convert.ToInt32(temp[3]);
                        equip.AllStat = Convert.ToInt32(temp[4]);
                        equip.SpecialHP = temp[5];
                        equip.SpecialMP = temp[6];
                        equip.ATK = Convert.ToInt32(temp[7]);
                        equip.MATK = Convert.ToInt32(temp[8]);
                        equip.DEF = Convert.ToInt32(temp[9]);
                        equip.SPD = Convert.ToInt32(temp[10]);
                        equip.JUMP = Convert.ToInt32(temp[11]);
                        AccessoriesList.Add(equip);
                        
                        
                    }
                }
            }


            return AccessoriesList;
        }

        //Retrieving Data from Maplestory.db

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
                            tempChar.className = query.GetString(0);
                            tempChar.classType = query.GetString(1);
                            tempChar.faction = query.GetString(2);
                            tempChar.unionEffect = query.GetString(3);
                            tempChar.unionEffectType = query.GetString(4);


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
                            tempChar.className = result.GetString(0);
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
                            tempEquip.JobType = result.GetString(1);
                            tempEquip.EquipSlot = result.GetString(2);
                            tempEquip.MS = result.GetInt32(3);
                            tempEquip.SS = result.GetInt32(4);
                            tempEquip.HP = result.GetInt32(5);
                            tempEquip.MP = result.GetInt32(6);
                            tempEquip.ATK = result.GetInt32(7);
                            tempEquip.MATK = result.GetInt32(8);
                            tempEquip.DEF = result.GetInt32(9);
                            tempEquip.SPD = result.GetInt32(10);
                            tempEquip.JUMP = result.GetInt32(11);
                            tempEquip.IED = result.GetInt32(12);

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
                            equipModel.EquipName = reader.GetString(0);
                            equipModel.EquipSet = reader.GetString(1);
                            equipModel.EquipSlot = reader.GetString(2);
                            equipModel.EquipLevel = reader.GetInt32(3);
                            equipModel.AllStat = reader.GetInt32(4);
                            equipModel.SpecialHP = reader.GetString(5);
                            equipModel.SpecialMP = reader.GetString(6);
                            equipModel.ATK = reader.GetInt32(7);
                            equipModel.MATK = reader.GetInt32(8);
                            equipModel.DEF = reader.GetInt32(9);
                            equipModel.SPD = reader.GetInt32(10);
                            equipModel.JUMP = reader.GetInt32(11);

                            accModel.Add(equipModel);
                        }
                    }
                }


            }



            return accModel;
        }


        //Insert into Maplestory.db
        public static bool insertCharTrack(Character character)
        {
            bool insertPassed;

            string insertQueryStr = "INSERT INTO CharacterTrack (charName, unionRank, level) VALUES (@CN, @UR, @Lvl)";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GlobalVars.databasePath}"))
            {
                dbCon.Open();

                try
                {
                    using (SqliteCommand insertCMD = new SqliteCommand(insertQueryStr, dbCon))
                    {
                        insertCMD.Parameters.AddWithValue("@CN", character.className);
                        insertCMD.Parameters.AddWithValue("@UR", character.unionRank);
                        insertCMD.Parameters.AddWithValue("@Lvl", character.level);

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

        public static bool insertCharBossList(string charName, string bossName, int bossID)
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
