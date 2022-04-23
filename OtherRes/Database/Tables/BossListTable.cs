using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class BossListTable : BaseDBTable, ITableUpload
    {
        public List<BossCLS> BossList { get; set; }

        private string BossListSpec = "(" +
                "BossID int," +
                "BossName string NOT NULL," +
                "Difficulty string NOT NULL," +
                "EntryType string NOT NULL," +
                "Meso int NOT NULL," +
                "PRIMARY KEY(BossID)" +
                ");";
        public BossListTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            base.TableParameters = BossListSpec;
        }
        public async void RetrieveData()
        {
            BossList = await GetBossCSVAsync();
        }
        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if(ComFunc.IsOpenConnection(connection))
            {
                string insertBoss = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                using (SqliteCommand insertBosscmd = new SqliteCommand(insertBoss, connection, transaction))
                {
                    foreach (BossCLS bossItem in BossList)
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
                            Console.WriteLine(ErrorCounter.ToString());
                            Console.WriteLine(ex.ToString());
                        }

                    }
                }
            }
            
        }

        public static async Task<List<BossCLS>> GetBossCSVAsync()
        {

            List<BossCLS> AllBossList = new List<BossCLS>();

            StorageFile statTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "BossListData.csv");

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
                        BossCLS tempboss = new BossCLS();
                        tempboss.BossID = counter;
                        tempboss.BossName = temp[1];
                        tempboss.Difficulty = temp[2];
                        tempboss.EntryType = temp[3];
                        tempboss.Meso = Convert.ToInt32(temp[4].Replace(",", ""));



                        AllBossList.Add(tempboss);


                    }

                }
            }



            return AllBossList;
        }

        public static Dictionary<int, BossCLS> GetBossDB()
        {
            Dictionary<int, BossCLS> bossDict = new Dictionary<int, BossCLS>();

            using (SqliteConnection dbConnection = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbConnection.Open();
                string getBossCmd = "SELECT * FROM BossListData";

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


    }
}
