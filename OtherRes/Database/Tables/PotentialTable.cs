using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class PotentialTable : BaseDBTable, ITableUpload
    {
        List<PotentialStatsCLS> PotList { get; set; }

        private string[] PotSpec = { "(" +
                        "EquipGrp string," +
                        "Grade string," +
                        "GradeT string," +
                        "StatT string," +
                        "Stat nvarchar," +
                        "MinLvl int," +
                        "MaxLvl int," +
                        "ValueI nvarchar," +
                        "Duration int," +
                        "Chance double," +
                        "PRIMARY KEY (EquipGrp, Grade, GradeT, Stat, MinLvl, MaxLvl, ValueI)" +
                        ");" };

        public PotentialTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = PotSpec[0];
        }

        public async void RetrieveData()
        {
            switch (TableName)
            {
                case "PotentialData":
                    PotList = await GetPotentialCSVAsync();
                    break;
                case "PotentialBonusData":
                    PotList = await GetPotentialBonusCSVAsync();
                    break;
            }
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                string insertPot = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                //int potIDc = 0;
                using (SqliteCommand insertCMD = new SqliteCommand(insertPot, connection, transaction))
                {
                    foreach (PotentialStatsCLS pot in PotList)
                    {
                        ErrorCounter+=1;
                        foreach (string i in pot.EquipGrpL)
                        {
                            string e;
                            if (i.Trim() == "Shoulderpad")
                            {
                                e = "Shoulder";
                            }
                            else
                            {
                                e = i.Trim();
                            }

                            insertCMD.Parameters.Clear();
                            //insertCMD.Parameters.AddWithValue("@P", potIDc);
                            insertCMD.Parameters.AddWithValue("@EquipGrp", e);
                            insertCMD.Parameters.AddWithValue("@Grade", pot.Grade);
                            insertCMD.Parameters.AddWithValue("@GradeT", pot.Prime);
                            insertCMD.Parameters.AddWithValue("@StatT", pot.StatType);
                            insertCMD.Parameters.AddWithValue("@Stat", pot.StatIncrease);
                            insertCMD.Parameters.AddWithValue("@MinLvl", pot.MinLvl);
                            insertCMD.Parameters.AddWithValue("@MaxLvl", pot.MaxLvl);
                            insertCMD.Parameters.AddWithValue("@ValueI", pot.StatValue);
                            insertCMD.Parameters.AddWithValue("@Duration", pot.Duration);
                            insertCMD.Parameters.AddWithValue("@Chance", pot.Chance);
                            //potIDc++;
                            try
                            {
                                insertCMD.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ErrorCounter.ToString());
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        public static async Task<List<PotentialStatsCLS>> GetPotentialCSVAsync()
        {
            List<PotentialStatsCLS> PotentialList = new List<PotentialStatsCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "PotentialData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string potItem in result.Skip(1))
                    {
                        if (potItem == "")
                        {
                            Console.WriteLine("Hello");
                            return PotentialList;
                        }

                        var temp = potItem.Split(",");

                        PotentialStatsCLS Pot = new PotentialStatsCLS();

                        Pot.EquipGrpL = temp[1].Contains(";") ? temp[1].Split(';').ToList() : new List<string> { temp[1] };
                        Pot.Grade = temp[2];
                        Pot.Prime = temp[3];
                        Pot.StatIncrease = temp[4].TrimEnd('%');
                        Pot.StatType = temp[5];
                        Pot.Chance = Convert.ToDouble(temp[6]);
                        Pot.Duration = Convert.ToInt32(temp[7]);
                        Pot.MinLvl = Convert.ToInt32(temp[8]);
                        Pot.MaxLvl = Convert.ToInt32(temp[9]);
                        Pot.StatValue = temp[10];

                        PotentialList.Add(Pot);


                    }
                }
            }


            return PotentialList;


        }
        public static async Task<List<PotentialStatsCLS>> GetPotentialBonusCSVAsync()
        {
            List<PotentialStatsCLS> PotentialList = new List<PotentialStatsCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "BonusPotentialData.csv");

            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string text = dataReader.ReadString(numBytesLoaded);

                    var result = text.Split("\r\n");
                    foreach (string potItem in result.Skip(1))
                    {
                        if (potItem == "")
                        {
                            Console.WriteLine("Hello");
                            return PotentialList;
                        }

                        var temp = potItem.Split(",");

                        PotentialStatsCLS Pot = new PotentialStatsCLS();

                        Pot.EquipGrpL = temp[1].Contains(";") ? temp[1].Split(';').ToList() : new List<string> { temp[1] };
                        Pot.Grade = temp[2];
                        Pot.Prime = temp[3];
                        Pot.StatIncrease = temp[4].TrimEnd('%');
                        Pot.StatType = temp[5];
                        Pot.Chance = Convert.ToDouble(temp[6]);
                        Pot.Duration = Convert.ToInt32(temp[7]);
                        Pot.MinLvl = Convert.ToInt32(temp[8]);
                        Pot.MaxLvl = Convert.ToInt32(temp[9]);
                        Pot.StatValue = temp[10];

                        PotentialList.Add(Pot);


                    }
                }
            }


            return PotentialList;


        }




        public static List<PotentialStatsCLS> GetAllPotentialDB()
        {
            List<PotentialStatsCLS> potentialList = new List<PotentialStatsCLS>();
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT ROWID, * FROM PotentialData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PotentialStatsCLS pot = new PotentialStatsCLS();
                            pot.PotID = reader.GetInt32(0);
                            pot.EquipGrp = reader.GetString(1);
                            pot.Grade = reader.GetString(2);
                            pot.Prime = reader.GetString(3);
                            pot.StatType = reader.GetString(4);
                            pot.StatIncrease = reader.GetString(5);
                            pot.MinLvl = reader.GetInt32(6);
                            pot.MaxLvl = reader.GetInt32(7);
                            pot.StatValue = reader.GetString(8);

                            potentialList.Add(pot);

                        }
                    }
                }


            }

            return potentialList;
        }
        public static List<PotentialStatsCLS> GetAllBonusPotentialDB()
        {
            List<PotentialStatsCLS> potentialList = new List<PotentialStatsCLS>();
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT ROWID, * FROM PotentialBonusData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PotentialStatsCLS pot = new PotentialStatsCLS();
                            pot.PotID = reader.GetInt32(0);
                            pot.EquipGrp = reader.GetString(1);
                            pot.Grade = reader.GetString(2);
                            pot.Prime = reader.GetString(3);
                            pot.StatType = reader.GetString(4);
                            pot.StatIncrease = reader.GetString(5);
                            pot.MinLvl = reader.GetInt32(6);
                            pot.MaxLvl = reader.GetInt32(7);
                            pot.StatValue = reader.GetString(8);

                            potentialList.Add(pot);

                        }
                    }
                }


            }

            return potentialList;
        }

    }
}
