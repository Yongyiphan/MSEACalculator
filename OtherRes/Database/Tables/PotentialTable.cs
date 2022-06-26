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

       
        public PotentialTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
        }

        public async void RetrieveData()
        {
            string PotKey = "PRIMARY KEY (EquipSlot, Grade, Prime, DisplayStat, MinLvl, MaxLvl, StatValue)";
            string RateKey = "PRIMARY KEY (EquipSlot, Grade, Prime, DisplayStat, MinLvl, MaxLvl)";
            (List<PotentialStatsCLS>, string) Result;
            switch (TableName)
            {
                case "PotentialData":
                    Result = await GetPotentialCSVAsync(FileName: "PotentialData.csv", "PRIMARY KEY (EquipSlot, Grade, Prime, DisplayStat, MinLvl, MaxLvl, StatValue, ReflectDMG, CubeType)");
                    PotList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;
                case "BonusData":
                    Result = await GetPotentialCSVAsync(FileName: "BonusData.csv", PotKey);
                    PotList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;


                case "PotentialCube":
                    Result = await GetCubeRatesCSVAsync("PotentialCubeRatesData.csv", RateKey);
                    PotList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;
                case "BonusCube":
                    Result = await GetCubeRatesCSVAsync("BonusCubeRatesData.csv", RateKey);
                    PotList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;

            }
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                string insertPot = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                int potIDc = 0;
                using (SqliteCommand insertCMD = new SqliteCommand(insertPot, connection, transaction))
                {
                    foreach (PotentialStatsCLS pot in PotList)
                    {
                        ErrorCounter+=1;
                        if (pot.EquipSlot.Trim() == "Shoulderpad")
                        {
                            pot.EquipSlot = "Shoulder";
                        }
                        insertCMD.Parameters.Clear();
                        //insertCMD.Parameters.AddWithValue("@P", potIDc);
                        insertCMD.Parameters.AddWithValue("@EquipSlot", pot.EquipSlot);
                        insertCMD.Parameters.AddWithValue("@PotentialGroup", pot.Grade);
                        insertCMD.Parameters.AddWithValue("@Grade", pot.Prime);
                        insertCMD.Parameters.AddWithValue("@Prime", pot.StatType);
                        insertCMD.Parameters.AddWithValue("@DisplayStat", pot.DisplayStat);
                        insertCMD.Parameters.AddWithValue("@Stat", pot.MinLvl);
                        insertCMD.Parameters.AddWithValue("@MinLvl", pot.MinLvl);
                        insertCMD.Parameters.AddWithValue("@MaxLvl", pot.MinLvl);
                        insertCMD.Parameters.AddWithValue("@StatType", pot.MaxLvl);
                        insertCMD.Parameters.AddWithValue("@StatValue", pot.StatValue);
                        insertCMD.Parameters.AddWithValue("@Chance", pot.Chance);
                        insertCMD.Parameters.AddWithValue("@Duration", pot.Duration);
                        insertCMD.Parameters.AddWithValue("@ReflectDMG", pot.ReflectDMG);
                        insertCMD.Parameters.AddWithValue("@Tick", pot.Tick);
                        insertCMD.Parameters.AddWithValue("@CubeType", String.Join(";", pot.CubeType));
                        insertCMD.Parameters.AddWithValue("@Initial", pot.Initial);
                        insertCMD.Parameters.AddWithValue("@GameCube", pot.InGame);
                        insertCMD.Parameters.AddWithValue("@CashCube", pot.CashCube);
                        potIDc++;
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


        public static async Task<(List<PotentialStatsCLS>, string)> GetPotentialCSVAsync(string FileName, string TableKey)
        {
            List<PotentialStatsCLS> PotentialList = new List<PotentialStatsCLS>();
            List<string> FileList = new List<string>();            
                string tableSpec = "";
                int counter = 0;
            try
            {
                FileList = await ComFunc.CSVStringAsync(GVar.CalculationsPath, FileName);

                foreach (string equipItem in FileList)
                {
                    if (equipItem == "")
                    {
                        return (PotentialList, tableSpec);
                    }
                    if (counter == 0)
                    {
                        tableSpec = ComFunc.TableSpecStringBuilder(equipItem, TableKey);
                        counter += 1;
                        continue;
                    }

                    var temp = equipItem.Split(",");
                    counter += 1;

                    PotentialStatsCLS citem = new PotentialStatsCLS();
                    if (FileName.Contains("Bonus"))
                    {
                        citem.EquipSlot = temp[1];
                        citem.Grade = temp[2];
                        citem.Prime = temp[3];
                        citem.DisplayStat = temp[4];
                        citem.StatType = temp[5];

                        citem.MinLvl = Convert.ToInt32(temp[6]);
                        citem.MaxLvl = Convert.ToInt32(temp[7]);
                        citem.Duration = Convert.ToInt32(Convert.ToDouble(temp[8]));
                        citem.Chance = Convert.ToInt32(temp[9]);

                        PotentialList.Add(citem);
                        continue;

                    }
                    citem.EquipSlot = temp[1];
                    citem.Grade = temp[2];
                    citem.Prime = temp[3];
                    citem.DisplayStat = temp[4];
                    citem.StatType = temp[5];

                    citem.MinLvl = Convert.ToInt32(temp[6]);
                    citem.MaxLvl = Convert.ToInt32(temp[7]);
                    citem.StatValue = temp[8];
                    citem.Chance = Convert.ToInt32(temp[9]);
                    //citem.Duration = Convert.ToInt32(temp[10]);
                    citem.Duration = Convert.ToInt32(Convert.ToDouble(temp[10]));
                    citem.ReflectDMG = Convert.ToInt32(temp[11]);
                    citem.Tick =  Convert.ToInt32(Convert.ToDouble(temp[12]));

                    if (temp[13].Contains(";"))
                    {

                    citem.CubeType = temp[13].Split(';').ToList();
                    }
                    else
                    {
                        citem.CubeType = new List<string>() { "None" };
                    }



                    PotentialList.Add(citem);

                }

            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message + counter.ToString());
            }
                return (PotentialList, tableSpec);
        }



        public static async Task<(List<PotentialStatsCLS>, string)> GetCubeRatesCSVAsync(string FileName, string TableKey)
        {
            List<PotentialStatsCLS> PotentialList = new List<PotentialStatsCLS>();

            StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + FileName);
            string tableSpec = "";
            var stream = await charTable.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;
            try
            {


                using (var inputStream = stream.GetInputStreamAt(0))
                {
                    using (var dataReader = new DataReader(inputStream))
                    {
                        uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                        string text = dataReader.ReadString(numBytesLoaded);

                        var result = text.Split("\r\n");
                        int counter = 0;
                        foreach (string equipItem in result)
                        {
                            if (equipItem == "")
                            {
                                return (PotentialList, tableSpec);
                            }
                            if (counter == 0)
                            {
                                tableSpec = ComFunc.TableSpecStringBuilder(equipItem, TableKey);
                                counter += 1;
                                continue;
                            }


                            var temp = equipItem.Split(",");
                            counter += 1;

                            PotentialStatsCLS citem = new PotentialStatsCLS();
                            if (FileName.Contains("Bonus"))
                            {
                                citem.EquipSlot = temp[1];
                                citem.Grade = temp[2];
                                citem.Prime = temp[3];
                                citem.DisplayStat = temp[4];
                                citem.StatType = temp[5];

                                citem.MinLvl = Convert.ToInt32(temp[6]);
                                citem.MaxLvl = Convert.ToInt32(temp[7]);
                                
                                citem.Probability = Convert.ToDouble(temp[8]);

                                PotentialList.Add(citem);
                                continue;
                            }
                            citem.EquipSlot = temp[1];
                            citem.Grade = temp[2];
                            citem.Prime = temp[3];
                            citem.DisplayStat = temp[4];
                            citem.StatType = temp[5];

                            citem.MinLvl = Convert.ToInt32(temp[6]);
                            citem.MaxLvl = Convert.ToInt32(temp[7]);
                            citem.StatValue = temp[8];
                            citem.Initial = Convert.ToDouble(temp[8]);
                            citem.InGame = Convert.ToDouble(temp[9]);
                            citem.CashCube = Convert.ToDouble(temp[10]);


                            PotentialList.Add(citem);

                        }
                    }
                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
            return (PotentialList, tableSpec);

        }


        //public static async Task<List<PotentialStatsCLS>> GetPotentialCSVAsync()
        //{
        //    List<PotentialStatsCLS> PotentialList = new List<PotentialStatsCLS>();

        //    StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "PotentialData.csv");

        //    var stream = await charTable.OpenAsync(FileAccessMode.Read);

        //    ulong size = stream.Size;

        //    using (var inputStream = stream.GetInputStreamAt(0))
        //    {
        //        using (var dataReader = new DataReader(inputStream))
        //        {
        //            uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
        //            string text = dataReader.ReadString(numBytesLoaded);

        //            var result = text.Split("\r\n");
        //            foreach (string potItem in result.Skip(1))
        //            {
        //                if (potItem == "")
        //                {
        //                    Console.WriteLine("Hello");
        //                    return PotentialList;
        //                }

        //                var temp = potItem.Split(",");

        //                PotentialStatsCLS Pot = new PotentialStatsCLS();

        //                Pot.EquipGrpL = temp[1].Contains(";") ? temp[1].Split(';').ToList() : new List<string> { temp[1] };
        //                Pot.Grade = temp[2];
        //                Pot.Prime = temp[3];
        //                Pot.StatIncrease = temp[4].TrimEnd('%');
        //                Pot.StatType = temp[5];
        //                Pot.Chance = Convert.ToDouble(temp[6]);
        //                Pot.Duration = Convert.ToInt32(temp[7]);
        //                Pot.MinLvl = Convert.ToInt32(temp[8]);
        //                Pot.MaxLvl = Convert.ToInt32(temp[9]);
        //                Pot.StatValue = temp[10];

        //                PotentialList.Add(Pot);


        //            }
        //        }
        //    }


        //    return PotentialList;


        //}
        //public static async Task<List<PotentialStatsCLS>> GetPotentialBonusCSVAsync()
        //{
        //    List<PotentialStatsCLS> PotentialList = new List<PotentialStatsCLS>();

        //    StorageFile charTable = await GVar.storageFolder.GetFileAsync(GVar.CalculationsPath + "BonusPotentialData.csv");

        //    var stream = await charTable.OpenAsync(FileAccessMode.Read);

        //    ulong size = stream.Size;

        //    using (var inputStream = stream.GetInputStreamAt(0))
        //    {
        //        using (var dataReader = new DataReader(inputStream))
        //        {
        //            uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
        //            string text = dataReader.ReadString(numBytesLoaded);

        //            var result = text.Split("\r\n");
        //            foreach (string potItem in result.Skip(1))
        //            {
        //                if (potItem == "")
        //                {
        //                    Console.WriteLine("Hello");
        //                    return PotentialList;
        //                }

        //                var temp = potItem.Split(",");

        //                PotentialStatsCLS Pot = new PotentialStatsCLS();

        //                Pot.EquipGrpL = temp[1].Contains(";") ? temp[1].Split(';').ToList() : new List<string> { temp[1] };
        //                Pot.Grade = temp[2];
        //                Pot.Prime = temp[3];
        //                Pot.StatIncrease = temp[4].TrimEnd('%');
        //                Pot.StatType = temp[5];
        //                Pot.Chance = Convert.ToDouble(temp[6]);
        //                Pot.Duration = Convert.ToInt32(temp[7]);
        //                Pot.MinLvl = Convert.ToInt32(temp[8]);
        //                Pot.MaxLvl = Convert.ToInt32(temp[9]);
        //                Pot.StatValue = temp[10];

        //                PotentialList.Add(Pot);


        //            }
        //        }
        //    }


        //    return PotentialList;


        //}



        //E.G
        //Hat => Rare => List
        //}
    }
}
