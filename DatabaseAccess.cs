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

namespace MSEACalculator
{
    public static class DatabaseAccess
    {
        public async static Task databaseInit()
        {
            //string dbName = "Maplestory.db";
            await ApplicationData.Current.LocalFolder.CreateFileAsync("Maplestory.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Maplestory.db");
            using (SqliteConnection dbConnection = new SqliteConnection($"Filename ={dbpath}"))
            {
                dbConnection.Open();

                List<TableStats> staticTables = new List<TableStats>();
                List<TableStats> foreignTables = new List<TableStats>();

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
                string sftableSpec = "(" +
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
                TableStats SFTable = new TableStats(tableNameSF, sftableSpec, "StarForce");
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

                //TABLE FOR EVENTS
                //string eventMspecs = "(" +
                //    "StartDate  text, " +
                //    "EndDate text);";



                //For table without foreign key reference
                staticTables.ForEach(x => initTable(x.tableName, x.tableSpecs, dbConnection));

                await Task.WhenAll(staticTables.ParallelForEachAsync(item => Task.Run(() => InitCSVData(item.initMode, item.tableName, dbConnection))));



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

        private async static Task InitCSVData(string type, string tableName, SqliteConnection connection)
        {
            switch (type)
            {
                case "Boss":
                    Dictionary<int, Boss> bosstable = await CommonFunc.GetBossListAsync();
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

                    Dictionary<int, SFGain> SFtable = await CommonFunc.GetSFListAsync();

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

                    Dictionary<int, SFGain> AddSFtable = await CommonFunc.GetSFListAsync();

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
                default:
                    break;
            }

        }

        //Dont use async <= need results on the same thread
        public async static Task writetoEventJson(string m, List<EventRecords> eventRecords)
        {
            string jsonFP = Path.Combine(ApplicationData.Current.LocalFolder.Path, @"Data\");

            string filename = "Event.json";
            switch (m)
            {
                case "init":

                    string jsonString = JsonSerializer.Serialize<List<EventRecords>>(eventRecords);
                    using (FileStream createStream = File.Create(filename))
                    {
                        await JsonSerializer.SerializeAsync(createStream, eventRecords);

                    }

                    File.WriteAllText(jsonFP,filename);

                        break;

                case "update":
                    break;
            }



        }
    }
}
