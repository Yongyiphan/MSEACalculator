using Microsoft.Data.Sqlite;
using MSEACalculator.BossRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class BossListTable : BaseDBTable, ITableUpload
    {
        public List<BossCLS> BossList { get; set; }
        public BossListTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }
        public async void RetrieveData()
        {
            BossList = await ImportCSV.GetBossCSVAsync();
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
    }
}
