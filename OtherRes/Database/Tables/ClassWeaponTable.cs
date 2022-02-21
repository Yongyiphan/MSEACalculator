using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class ClassWeaponTable : BaseDBTable, ITableUpload
    {
        public Dictionary<int, List<string>> CurrentDict { get; set; }
        public ClassWeaponTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }
        public async void RetrieveData()
        {
            switch (TableName)
            {
                case "ClassMainWeapon":
                    CurrentDict = await ImportCSV.GetClassMWeaponCSVAsync();
                    break;
                case "ClassSecWeapon":
                    CurrentDict = await ImportCSV.GetClassSWeaponCSVAsync();
                    break;

            }
        }


        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                
                string insertMW = ComFunc.InsertSQLStringBuilder(TableName,TableParameters);
                using (SqliteCommand insertCMD = new SqliteCommand(insertMW, connection, transaction))
                {
                    foreach (int CN in CurrentDict.Keys)
                    {
                        ErrorCounter++;
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@ClassName", CurrentDict[CN][0]);
                        insertCMD.Parameters.AddWithValue("@WeaponType", CurrentDict[CN][1]);

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
