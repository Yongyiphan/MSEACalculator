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
        public Dictionary<int, List<string>> CurrentDict { get; set; }

        private string ClassWeaponSpec = "(" +
                "ClassName string," +
                "WeaponType string," +
                "PRIMARY KEY (ClassName, WeaponType)" +
                ");";
        public ClassWeaponTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = ClassWeaponSpec;
        }
        public async void RetrieveData()
        {
            switch (TableName)
            {
                case "ClassMainWeapon":
                    CurrentDict = await GetClassMWeaponCSVAsync();
                    break;
                case "ClassSecWeapon":
                    CurrentDict = await GetClassSWeaponCSVAsync();
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

        public static async Task<Dictionary<int, List<string>>> GetClassMWeaponCSVAsync()
        {
            Dictionary<int, List<string>> CWdict = new Dictionary<int, List<string>>();
            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "ClassMainWeapon.csv");

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
                    foreach (string CI in result.Skip(1))
                    {
                        if (CI == "")
                        {
                            return CWdict;
                        }
                        var temp = CI.Split(',');
                        var tempL = new List<string>() { temp[1], temp[2] };
                        CWdict.Add(counter, tempL);

                        counter++;
                    }
                }
            }


            return CWdict;
        }
        public static async Task<Dictionary<int, List<string>>> GetClassSWeaponCSVAsync()
        {
            Dictionary<int, List<string>> CWdict = new Dictionary<int, List<string>>();
            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CharacterPath + "ClassSecWeapon.csv");

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
                    foreach (string CI in result.Skip(1))
                    {
                        if (CI == "")
                        {
                            return CWdict;
                        }
                        var temp = CI.Split(',');
                        var tempL = new List<string>() { temp[1], temp[2] };
                        CWdict.Add(counter, tempL);

                        counter++;
                    }
                }
            }


            return CWdict;
        }




    }
}
