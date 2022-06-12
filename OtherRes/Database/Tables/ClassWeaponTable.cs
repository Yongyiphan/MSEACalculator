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

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class ClassWeaponTable : BaseDBTable, ITableUpload
    {

        //public Dictionary<int, List<string>> CurrentDict { get; set; }
        public List<(string, string)> CurrentList { get; set; }



        public string TableConstraints = "PRIMARY KEY (ClassName, WeaponType)";

        public ClassWeaponTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
        }
        public async void RetrieveData()
        {
            switch (TableName)
            {
                case "ClassMainWeapon":
                    var Result1 = await GetClassMWeaponCSVAsync(TableConstraints);
                    CurrentList = Result1.Item1;
                    TableParameters = Result1.Item2;

                    break;
                case "ClassSecWeapon":
                    var Result2 = await GetClassSWeaponCSVAsync(TableConstraints);
                    CurrentList = Result2.Item1;
                    TableParameters = Result2.Item2;
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
                    foreach ((string, string) CN in CurrentList)
                    {
                        ErrorCounter++;
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@ClassName", CN.Item1);
                        insertCMD.Parameters.AddWithValue("@WeaponType", CN.Item2);

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

        public static async Task<(List<(string, string)>, string)> GetClassMWeaponCSVAsync(string TableConstraints)
        {
            //Dictionary<int, List<string>> CWdict = new Dictionary<int, List<string>>();
            List<(string, string)> CWdict = new List<(string, string)>();
            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "ClassMainWeapon.csv");

            string TableSpec = "";
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
                    foreach (string CI in result)
                    {
                        if (CI == "")
                        {
                            return (CWdict, TableSpec);
                        }
                        if (counter == 0)
                        {
                            TableSpec = ComFunc.TableSpecStringBuilder(CI, TableConstraints);
                            counter += 1;
                            continue;
                        }
                        var temp = CI.Split(',');
                        CWdict.Add((temp[1], temp[2]));

                        counter++;
                    }
                }
            }


            return (CWdict, TableSpec);
        }
        public static async Task<(List<(string, string)>, string)> GetClassSWeaponCSVAsync(string TableConstraints)
        {
            //Dictionary<int, List<string>> CWdict = new Dictionary<int, List<string>>();
            List<(string, string)> CWdict = new List<(string, string)>();
            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "ClassSecWeapon.csv");

            string TableSpec = "";
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
                    foreach (string CI in result)
                    {
                        if (CI == "")
                        {
                            return (CWdict, TableSpec);
                        }
                        if (counter == 0)
                        {
                            TableSpec = ComFunc.TableSpecStringBuilder(CI, TableConstraints);
                            counter += 1;
                            continue;
                        }
                        var temp = CI.Split(',');
                        CWdict.Add((temp[1], temp[2]));

                        counter++;
                    }
                }
            }


            return (CWdict, TableSpec);
        }



    }
}
