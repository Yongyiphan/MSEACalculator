using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using Dasync.Collections;
using MSEACalculator.StarforceRes;
using MSEACalculator.BossRes;
using MSEACalculator.EventRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.MesoRes;

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
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");
            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={dbpath}"))
            {
                dbConnection.Open();

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
                    "PRIMARY KEY(BossID));";
                string tableNameBOss = "BossList";
                TableStats BossTable = new TableStats(tableNameBOss, bossListSpec, "Boss");
                staticTables.Add(BossTable);


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
                    "PRIMARY KEY(SFID));";
                string tableNameSF = "StarforceList";
                TableStats SFTable = new TableStats(tableNameSF, sfTableSpec, "StarForce");
                staticTables.Add(SFTable);

                //TABLE FOR STARFORCE STATS ADDONS
                string addSFstatSpec = "(" +
                    "SFID int NOT NULL," +
                    "StatType varchar(50) NOT NULL," +
                    "'128~137' int NOT NULL," +
                    "'138~149' int NOT NULL," +
                    "'150~159' int NOT NULL," +
                    "'160~199' int NOT NULL," +
                    "'200' int NOT NULL);";
                string tableNameAddSFtable = "AddSFStat";
                TableStats AddSFtable = new TableStats(tableNameAddSFtable, addSFstatSpec, "AddStarForce");
                staticTables.Add(AddSFtable);


                //TABLE FOR DEFAULT CHARACTER LIST
                string characterTableSpec = "(" +
                    "ClassName string," +
                    "ClassType string," +
                    "Faction string," +
                    "PRIMARY KEY (className));";

                string tableNameChar = "DefaultCharacter";
                TableStats CharacterTable = new TableStats(tableNameChar, characterTableSpec, "Character");
                staticTables.Add(CharacterTable);

                //TABLE FOR UNION

                //BLANK TABLES
                //TABLE FOR CHARACTER INIT 
                




                staticTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection));

                await Task.WhenAll(staticTables.ParallelForEachAsync(item => Task.Run(() => InitCSVData(item.initMode, item.tableName, dbConnection))));


                 
                //EMPTY TABLES



                dbConnection.Close();


            }

        }
        private static void initTable(string tableName, string tableParameters, SqliteConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                //delete table if exist
                string dropT = "DROP TABLE IF EXISTS " + tableName;
                SqliteCommand dropCmd = new SqliteCommand(dropT, connection);
                dropCmd.ExecuteReader();
                // create table if not exist

                string createTable = "CREATE TABLE IF NOT EXISTS " + tableName + tableParameters;
                SqliteCommand createCmd = new SqliteCommand(createTable, connection);
                createCmd.ExecuteReader();
            }

        }

        public static async Task InitCSVData(string type, string tableName, SqliteConnection connection)
        {
            switch (type)
            {
                case "Boss":
                    Dictionary<int, Boss> bosstable = await GetBossCSVAsync();
                    // update to table

                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (Boss bossItem in bosstable.Values)
                        {
                            string insertBoss = "INSERT INTO " + tableName + " (BossID, BossName, Difficulty, EntryType, EntryLimit, BossCrystal, Meso)" +
                            " VALUES (@BossID,@BossName,@Difficulty,@EntryType,@EntryLimit,@BossCrystal,@Meso)";
                            SqliteCommand insertBosscmd = new SqliteCommand(insertBoss, connection);
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
                    break;
                case "StarForce":

                    Dictionary<int, SFGain> SFtable = await GetSFCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (SFGain sfitem in SFtable.Values)
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

                    Dictionary<int, SFGain> AddSFtable = await GetSFCSVAsync();

                    if (connection.State == ConnectionState.Open)
                    {
                        int startCount = 16, endCount = AddSFtable.Values.Count() + 1;

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
                            if (startCount < 23)
                            {
                                addRow(tempSFI, nameof(tempSFI.MainStat), tempSFI.MainStatL);
                            }
                            addRow(tempSFI, nameof(tempSFI.NonWATK), tempSFI.NonWATKL);
                            addRow(tempSFI, nameof(tempSFI.WATK), tempSFI.WATKL);


                            startCount += 1;
                        }


                    }
                    break;
                case "Character":

                    Dictionary<int, Character> charTable = await GetCharCSVAsync();

                    if(connection.State == ConnectionState.Open)
                    {
                        foreach(Character charItem in charTable.Values)
                        {
                            string insertChar = "INSERT INTO " + tableName + "(ClassName, ClassType, Faction)" +
                                " VALUES (@CN, @CT, @Fac)";

                            SqliteCommand insertCharCmd = new SqliteCommand(insertChar, connection);

                            insertCharCmd.Parameters.AddWithValue("@CN", charItem.className);
                            insertCharCmd.Parameters.AddWithValue("@CT", charItem.classType);
                            insertCharCmd.Parameters.AddWithValue("@Fac", charItem.faction);

                            insertCharCmd.ExecuteNonQuery();
                        }
                    }

                    break;
                default:
                    break;
            }

        }

        public static async Task<Dictionary<int, Boss>> GetBossCSVAsync()
        {

            Dictionary<int, Boss> AllBossList = new Dictionary<int, Boss>();



            //string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\Data\statGains.csv";
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            StorageFile statTable = await storageFolder.GetFileAsync(@"\DefaultData\BossListData.csv");

            var stream = await statTable.OpenAsync(Windows.Storage.FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
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
                        Boss tempboss = new Boss();

                        tempboss.BossID = counter;
                        tempboss.name = temp[0];
                        tempboss.difficulty = temp[1];
                        tempboss.entryType = temp[2];
                        tempboss.entryLimit = Convert.ToInt32(temp[3]);
                        tempboss.bossCrystalCount = Convert.ToInt32(temp[4]);
                        tempboss.meso = Convert.ToInt32(temp[5]);
                        AllBossList.Add(counter, tempboss);


                    }

                }
            }



            return AllBossList;
        }

        public static async Task<Dictionary<int, SFGain>> GetSFCSVAsync()
        {
            Dictionary<int, SFGain> SFList = new Dictionary<int, SFGain>();

            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            StorageFile statTable = await storageFolder.GetFileAsync(@"\DefaultData\statGains.csv");

            var stream = await statTable.OpenAsync(Windows.Storage.FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
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


                            SFList.Add(counter, tempSFitem2);
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

                            SFList.Add(counter, tempSFitem1);

                        }

                    }

                }
            }



            return SFList;
        }
        
        public static async Task<Dictionary<int, Character>> GetCharCSVAsync()
        {
            Dictionary<int, Character> characterList = new Dictionary<int, Character>();

            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            StorageFile charTable = await storageFolder.GetFileAsync(@"\DefaultData\CharacterList.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    int counter = 0;
                    foreach (string characterItem in result)
                    {
                        if (characterItem == "")
                        {
                            return characterList;
                        }

                        var temp = characterItem.Split(",");
                        counter += 1;
                        Character tempChar = new Character(temp[0], temp[1], temp[2]);
                        characterList.Add(counter, tempChar);

                    }
                }
            }


                return characterList;
        }

        //Code for First initialisation of Data
        //From CSV to Json





        //Retrieving Data from Maplestory.db

        public static Dictionary<int, Boss> GetBossDB()
        {
            Dictionary<int, Boss> bossDict = new Dictionary<int, Boss>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename = {dbpath}"))
            {
                dbConnection.Open();
                string getBossCmd = "SELECT * FROM BossList";

                SqliteCommand selectCmd = new SqliteCommand(getBossCmd, dbConnection);

                SqliteDataReader query = selectCmd.ExecuteReader();

                
                while (query.Read())
                {
                    Boss tempBoss = new Boss();
                    tempBoss.BossID = query.GetInt16(0);
                    tempBoss.name= query.GetString(1);
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

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={dbpath}"))
            {
                dbConnection.Open();
                string getCharCmd = "SELECT * FROM DefaultCharacter";

                SqliteCommand selectCmd = new SqliteCommand(getCharCmd, dbConnection);

                SqliteDataReader query = selectCmd.ExecuteReader();


                while (query.Read())
                {
                    Character tempChar = new Character();
                    tempChar.className = query.GetString(0);
                    tempChar.classType = query.GetString(1);
                    tempChar.faction = query.GetString(2);


                    charDict.Add(query.GetString(0),tempChar);
                }

                dbConnection.Close();

            }
            return charDict;
        }

        public static Dictionary<string, Character> getCharTrackDict()
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");

            Dictionary<string, Character> charTrackList = new Dictionary<string, Character>(); 

            using (SqliteConnection dbCon = new SqliteConnection($"Filename ={dbpath}"))
            {
                dbCon.Open();

                string getCharTrackQuery = "SELECT * FROM Character";

                SqliteCommand selectCharsCmd = new SqliteCommand(getCharTrackQuery, dbCon);

                SqliteDataReader queryResult = selectCharsCmd.ExecuteReader();

                while (queryResult.Read())
                {
                    Character tempChar = new Character();
                    tempChar.className = queryResult.GetString(0);
                    tempChar.classType = queryResult.GetString(1);
                    tempChar.faction = queryResult.GetString(2);

                    charTrackList.Add(tempChar.className, tempChar);
                }


                dbCon.Close();

            }



            return charTrackList;
        }


        public static Dictionary<string, Character> getCharTrackMeso(Dictionary<string, Character> charTrackDict)
        {

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");

            using(SqliteConnection dbCon = new SqliteConnection($"Filename = {dbpath}"))
            {
                dbCon.Open();

                foreach(Character character in charTrackDict.Values)
                {

                    string getMesoTrackQuery = "SELECT * FROM BossMesoGains WHERE charName = '" + character.className + "'";

                    SqliteCommand selectCMD = new SqliteCommand(getMesoTrackQuery, dbCon);

                    SqliteDataReader result = selectCMD.ExecuteReader();

                    List<string> bossList = new List<string>(); 

                    while (result.Read())
                    {
                        bossList.Add(result.GetString(1));
                    }
                    character.bossList = bossList;

                }


                dbCon.Close();
            }


            return charTrackDict;
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
