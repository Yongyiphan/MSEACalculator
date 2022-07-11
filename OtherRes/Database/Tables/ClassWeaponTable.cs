using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Diagnostics;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class ClassWeaponTable : BaseDBTable, ITableUpload
    {

        //public Dictionary<int, List<string>> CurrentDict { get; set; }
        public List<(string, string)> CurrentList { get; set; }




        public ClassWeaponTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
        }

        public async void RetrieveData()
        {
            (List<(string, string)>, string) Result;

            switch (TableName)
            {
                case "ClassMainWeapon":
                    Result = await GetClassWeaponCSVAsync("ClassMainWeapon.csv", "All");
                    CurrentList = Result.Item1;
                    TableParameters = Result.Item2;

                    break;
                case "ClassSecWeapon":
                    Result = await GetClassWeaponCSVAsync("ClassSecWeapon.csv", "All");
                    CurrentList = Result.Item1;
                    TableParameters = Result.Item2;

                    break;

            }


        }


        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {

                string insertMW = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                try
                {
                    using (SqliteCommand insertCMD = new SqliteCommand(insertMW, connection, transaction))
                    {
                        foreach ((string, string) CN in CurrentList)
                        {
                            ErrorCounter++;
                            insertCMD.Parameters.Clear();
                            insertCMD.Parameters.AddWithValue("@ClassName", CN.Item1);
                            insertCMD.Parameters.AddWithValue("@Weapon", CN.Item2);
                            insertCMD.Parameters.AddWithValue("@Secondary", CN.Item2);

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
                catch (Exception E)
                {
                    Console.WriteLine(E);
                };
            }
        }
        public static async Task<(List<(string, string)>, string)> GetClassWeaponCSVAsync(string FileName, string TableConstraints)
        {
            //Dictionary<int, List<string>> CWdict = new Dictionary<int, List<string>>();
            List<(string, string)> CWdict = new List<(string, string)>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.CharacterPath, FileName);

            string TableSpec = "";


            int counter = 0;
            foreach (string CI in result)
            {
                if (CI == "")
                {
                    return (CWdict, TableSpec);
                }
                if (counter == 0)
                {
                    TableSpec = ComFunc.TableSpecStringBuilder(TableColNames.CharacterCN, CI, TableConstraints);
                    counter += 1;
                    continue;
                }
                var temp = CI.Split(',');
                CWdict.Add((temp[1], temp[2]));

                counter++;
            }


            return (CWdict, TableSpec);
        }


    }
}
