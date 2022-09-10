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
            (List<PotentialStatsCLS>, string) Result;
            switch (TableName)
            {
                case "PotentialMainData":
                    Result = await GetPotentialCSVAsync(FileName: "PotentialData.csv", PotKey);
                    PotList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;
                case "PotentialBonusData":
                    Result = await GetPotentialCSVAsync(FileName: "BonusData.csv", PotKey);
                    PotList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;


                case "PotentialMainCube":
                    Result = await GetCubeRatesCSVAsync("PotentialCubeRatesData.csv", "All");
                    PotList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;
                case "PotentialBonusCube":
                    Result = await GetCubeRatesCSVAsync("BonusCubeRatesData.csv", "All");
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
                        insertCMD.Parameters.AddWithValue("@Grade", pot.Grade);
                        insertCMD.Parameters.AddWithValue("@Prime", pot.Prime);
                        insertCMD.Parameters.AddWithValue("@DisplayStat", pot.DisplayStat);
                        insertCMD.Parameters.AddWithValue("@StatType", pot.StatType);
                        insertCMD.Parameters.AddWithValue("@MinLvl", pot.MinLvl);
                        insertCMD.Parameters.AddWithValue("@MaxLvl", pot.MaxLvl);
                        insertCMD.Parameters.AddWithValue("@StatValue", pot.StatValue);
                        insertCMD.Parameters.AddWithValue("@Chance", pot.Chance);

                        //Main Pots Addons
                        insertCMD.Parameters.AddWithValue("@Tick", pot.Tick);
                        insertCMD.Parameters.AddWithValue("@CubeType", String.Join(";", pot.CubeType));

                        //Bonus Pots Addons
                        insertCMD.Parameters.AddWithValue("@Initial", pot.Rates.Initial);
                        insertCMD.Parameters.AddWithValue("@GameCube", pot.Rates.GameCube);
                        insertCMD.Parameters.AddWithValue("@CashCube", pot.Rates.CashCube);
                        insertCMD.Parameters.AddWithValue("@Probability", pot.Rates.Probability);
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
                        tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.PotentialCN, equipItem, TableKey);
                        counter += 1;
                        continue;
                    }

                    var temp = equipItem.Split(",");
                    counter += 1;

                    PotentialStatsCLS citem = new PotentialStatsCLS();
                    if (FileName.Contains("Bonus"))
                    {
                        citem.PotGrp = "Bonus";
                        citem.EquipSlot = temp[1];
                        citem.Grade = temp[2];
                        citem.Prime = temp[3];
                        citem.DisplayStat = temp[4];
                        citem.StatType = temp[5];

                        citem.MinLvl = Convert.ToInt32(temp[6]);
                        citem.MaxLvl = Convert.ToInt32(temp[7]);
                        citem.StatValue = temp[8];

                        citem.Chance = Convert.ToInt32(temp[9]);

                        PotentialList.Add(citem);
                        continue;

                    }
                    citem.PotGrp = "Main";
                    citem.EquipSlot = temp[1];
                    citem.Grade = temp[2];
                    citem.Prime = temp[3];
                    citem.DisplayStat = temp[4];
                    citem.StatType = temp[5];

                    citem.MinLvl = Convert.ToInt32(temp[6]);
                    citem.MaxLvl = Convert.ToInt32(temp[7]);
                    citem.StatValue = temp[8];
                    citem.Chance = Convert.ToInt32(temp[9]);
                    citem.Tick =  Convert.ToInt32(Convert.ToDouble(temp[10]));
                    string At11 = temp[11];
                    citem.CubeType = At11.Contains(";") ? At11.Split(';').ToList() : new List<string>() { "None" };

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
                        tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.PotentialCN, equipItem, TableKey);
                        counter += 1;
                        continue;
                    }


                    var temp = equipItem.Split(",");
                    counter += 1;

                    PotentialStatsCLS citem = new PotentialStatsCLS();
                    citem.EquipSlot = temp[1];
                    citem.Grade = temp[2];
                    citem.Prime = temp[3];
                    citem.DisplayStat = temp[4];

                    citem.MinLvl = Convert.ToInt32(temp[5]);
                    citem.MaxLvl = Convert.ToInt32(temp[6]);


                    if (FileName.Contains("Bonus"))
                    {

                        citem.Rates.Probability =  Convert.ToDouble(temp[7].Replace('%', ' '));

                        PotentialList.Add(citem);
                        continue;
                    }
                    citem.Rates.Initial = Convert.ToDouble(temp[7].Replace('%', ' '));
                    citem.Rates.GameCube = Convert.ToDouble(temp[8].Replace('%', ' '));
                    citem.Rates.CashCube = Convert.ToDouble(temp[9].Replace('%', ' '));


                    PotentialList.Add(citem);

                }

            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
            return (PotentialList, tableSpec);

        }


        public static Dictionary<string, Dictionary<string, Dictionary<(string, string), Dictionary<int, PotentialStatsCLS>>>> NewAllPotentialDB()
        {
            Dictionary<string, Dictionary<string, Dictionary<(string, string), Dictionary<int, PotentialStatsCLS>>>> FinalPotList = new Dictionary<string, Dictionary<string, Dictionary<(string, string), Dictionary<int, PotentialStatsCLS>>>>();
            
            Dictionary<string, Dictionary<(string, string), Dictionary<int, PotentialStatsCLS>>> PotList = new Dictionary<string, Dictionary<(string, string), Dictionary<int, PotentialStatsCLS>>>();

            string MainQuery = "SELECT " +
                    "PD.EquipSlot, PD.Grade, PD.Prime, PD.DisplayStat, PD.StatType, PD.MinLvl, PD.MaxLvl, PD.StatValue, PD.Chance, PD.Tick, PD.CubeType, PC.MinLvl, PC.MaxLvl, PC.Initial, PC.GameCube, PC.CashCube" +
                    "FROM PotentialMainData AS PD "+
                    "INNER JOIN PotentialBonusData AS PC " +
                    "ON PD.DisplayStat = PC.DisplayStat " +
                    "AND PD.EquipSlot = PC.EquipSlot " +
                    "AND PD.Prime = PC.Prime; ";

            string BonusQuery = "SELECT " +
                    "PD.EquipSlot, PD.Grade, PD.Prime, PD.DisplayStat, PD.StatType, PD.MinLvl, PD.MaxLvl, PD.StatValue, PD.Chance,PC.MinLvl, PC.MaxLvl, PC.Probability " +
                    "FROM PotentialBonusData AS PD "+
                    "INNER JOIN PotentialBonusCube AS PC " +
                    "ON PD.DisplayStat = PC.DisplayStat " +
                    "AND PD.EquipSlot = PC.EquipSlot " +
                    "AND PD.Prime = PC.Prime; ";


            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                using (SqliteCommand selectCMD = new SqliteCommand(MainQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        PotList.Clear();                        
                        int counter = 0;
                        while (reader.Read())
                        {
                            //Counter Starts from '1'
                            counter++;
                            string EquipSlot = reader.GetString(0);
                            if (PotList.ContainsKey(EquipSlot) == false)
                            {
                                PotList.Add(EquipSlot, new Dictionary<(string, string), Dictionary<int, PotentialStatsCLS>>());
                            }
                            string Grade = reader.GetString(1);
                            string Prime = reader.GetString(2);

                            (string, string) GradePrimePair = (Grade, Prime);

                            if (PotList[EquipSlot].ContainsKey(GradePrimePair) == false)
                            {
                                PotList[EquipSlot].Add(GradePrimePair, new Dictionary<int, PotentialStatsCLS>());
                            }


                            PotentialStatsCLS pot = new PotentialStatsCLS();
                            pot.PotID = counter;
                            pot.EquipSlot = EquipSlot;
                            pot.Grade = Grade;
                            pot.PotGrp = "Main";
                            pot.DisplayStat = reader.GetString(3);
                            pot.StatType = reader.GetString(4);
                            pot.MinLvl = reader.GetInt32(5);
                            pot.MaxLvl = reader.GetInt32(6);
                            pot.StatValue = reader.GetString(7);
                            pot.Chance = reader.GetInt32(8);
                            pot.Tick = reader.GetInt32(9);
                            pot.CubeType = reader.GetString(10).Split(';').ToList();

                            pot.Rates.MinLvl = reader.GetInt32(11);
                            pot.Rates.MaxLvl = reader.GetInt32(12);
                            pot.Rates.Initial = reader.GetDouble(13);
                            pot.Rates.GameCube = reader.GetDouble(14);
                            pot.Rates.CashCube = reader.GetDouble(15);

                            PotList[EquipSlot][GradePrimePair].Add(counter, pot);
                        }
                        FinalPotList.Add("Main", PotList);

                    }

                    selectCMD.CommandText = BonusQuery;
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        PotList.Clear();
                        int counter = 0;
                        while (reader.Read())
                        {
                            counter++;
                            string EquipSlot = reader.GetString(0);
                            if (PotList.ContainsKey(EquipSlot) == false)
                            {
                                PotList.Add(EquipSlot, new Dictionary<(string, string), Dictionary<int, PotentialStatsCLS>>());
                            }
                            string Grade = reader.GetString(1);
                            string Prime = reader.GetString(2);

                            (string, string) GradePrimePair = (Grade, Prime);

                            if (PotList[EquipSlot].ContainsKey(GradePrimePair) == false)
                            {
                                PotList[EquipSlot].Add(GradePrimePair, new Dictionary<int, PotentialStatsCLS>());
                            }


                            PotentialStatsCLS pot = new PotentialStatsCLS();
                            pot.PotID = counter;
                            pot.EquipSlot = EquipSlot;
                            pot.Grade = Grade;
                            pot.PotGrp = "Bonus";
                            pot.DisplayStat = reader.GetString(3);
                            pot.StatType = reader.GetString(4);
                            pot.MinLvl = reader.GetInt32(5);
                            pot.MaxLvl = reader.GetInt32(6);
                            pot.StatValue = reader.GetString(7);
                            pot.Chance = reader.GetInt32(8);

                            pot.Rates.MinLvl = reader.GetInt32(9);
                            pot.Rates.MaxLvl = reader.GetInt32(10);
                            pot.Rates.Probability = reader.GetDouble(11);
                            PotList[EquipSlot][GradePrimePair].Add(counter, pot);

                        }
                        FinalPotList.Add("Bonus", PotList);
                    }
                    
                }
            }



            return FinalPotList;
        }
        

    }
}
