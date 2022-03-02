using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.UnionRes;
using Windows.Storage.Streams;
using Windows.Storage;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class UnionTable : BaseDBTable, ITableUpload
    {
        List<UnionCLS> UnionList { get; set; }

        private string[] unionETableSpecs = { "(" +
                    "Effect string," +
                    "EffectType string," +
                    "B int," +
                    "A int," +
                    "S int," +
                    "SS int," +
                    "SSS int," +
                    "PRIMARY KEY(Effect, EffectType)" +
                    ");" };

        public UnionTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = unionETableSpecs[0];
        }


        
        public async void RetrieveData()
        {
            UnionList = await GetUnionECSVAsync();
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

        public static async Task<List<UnionCLS>> GetUnionECSVAsync()
        {
            List<UnionCLS> unionList = new List<UnionCLS>();

            StorageFile UnionTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "UnionData.csv");

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
                        UnionCLS tempUnion = new UnionCLS();
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


    }
}
