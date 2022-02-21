using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class AllCharacterTable : BaseDBTable, ITableUpload
    {
        List<CharacterCLS> CharacterList { get; set; }
        public AllCharacterTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }

        public async void RetrieveData()
        {
            CharacterList = await ImportCSV.GetCharCSVAsync();
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            int counter = 0;
            if(ComFunc.IsOpenConnection(connection))
            {
                counter++;
                string insertChar = ComFunc.InsertSQLStringBuilder(TableName,TableParameters);

                using (SqliteCommand insertCMD = new SqliteCommand(insertChar, connection, transaction))
                {
                    foreach (CharacterCLS charItem in CharacterList)
                    {
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@ClassName", charItem.ClassName);
                        insertCMD.Parameters.AddWithValue("@ClassType", charItem.ClassType);
                        insertCMD.Parameters.AddWithValue("@Faction", charItem.Faction);

                        insertCMD.Parameters.AddWithValue("@MainStat", charItem.MainStat);
                        insertCMD.Parameters.AddWithValue("@SecStat", charItem.SecStat);
                        
                        insertCMD.Parameters.AddWithValue("@UnionE", charItem.UnionEffect);
                        insertCMD.Parameters.AddWithValue("@UnionET", charItem.UnionEffectType);

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

            }
        }
    }
}
