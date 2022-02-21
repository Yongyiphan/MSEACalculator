using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.UnionRes;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class UnionTable : BaseDBTable, ITableUpload
    {
        List<UnionCLS> UnionList { get; set; }

        public UnionTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }
        public async void RetrieveData()
        {
            UnionList = await ImportCSV.GetUnionECSVAsync();
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            ErrorCounter++;
            if (ComFunc.IsOpenConnection(connection))
            {
                string insertUnion = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);

                using (SqliteCommand insertCMD = new SqliteCommand(insertUnion, connection, transaction))
                {
                    foreach (UnionCLS unionItem in UnionList)
                    {
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@Effect", unionItem.Effect);
                        insertCMD.Parameters.AddWithValue("@EffectType", unionItem.EffectType);
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
                            Console.WriteLine(ErrorCounter.ToString());
                        }
                    }
                }
            }
            
        }
    }
}
