using Microsoft.Data.Sqlite;
using MSEACalculator.CalculationRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using static MSEACalculator.OtherRes.Database.StructCollections;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class StarForceTable : BaseDBTable, ITableUpload
    {

        public List<StarforceCLS> CurrentList { get; set; }
        public Dictionary<string, Dictionary<(int, int), int>> SFLimits { get; set; }

        //public List<StarforceRatesCLS> SFRates { get; set; }
        public List<SFRates> SFRates { get; set; }




        public StarForceTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
        }
        public async void RetrieveData()
        {
            (List<StarforceCLS>, string) Result;

            switch (TableName)
            {
                case "SFNormalData":
                    Result = await GetNormalSFAsync("NormalEquipSF.csv", "PRIMARY KEY (SFID, MinLvl, MaxLvl)");
                    CurrentList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;
                case "SFSuperiorData":
                    Result = await GetSuperiorSFAsync("SuperiorItemsSF.csv", "PRIMARY KEY(SFID, MinLvl, MaxLvl)");
                    CurrentList = Result.Item1;
                    TableParameters = Result.Item2;
                    break;
                case "SFSuccessRates":
                    (List<SFRates>, string) Rates = await GetSFSuccessAsync("SFSuccessRates.csv", "PRIMARY KEY (Title, Attempt)");
                    SFRates = Rates.Item1;
                    TableParameters = Rates.Item2;
                    break;

                case "SFLimits":
                    (Dictionary<string, Dictionary<(int, int), int>>, string) Limits = await GetStarLimitAsync("StarLimit.csv", "PRIMARY KEY (Title, MinLvl, MaxLvl)");
                    SFLimits = Limits.Item1;
                    TableParameters = Limits.Item2;
                    break;
            }
        }


        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                string insertSF = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                int SFID = 0;
                using (SqliteCommand insertCMD = new SqliteCommand(insertSF, connection, transaction))
                {
                    switch (TableName)
                    {
                        case "SFSuccessRates":
                            foreach (SFRates Rates in SFRates)
                            {
                                ErrorCounter++;
                                insertCMD.Parameters.Clear();

                                insertCMD.Parameters.AddWithValue("@Title", Rates.StarforceType);
                                insertCMD.Parameters.AddWithValue("@Attempt", Rates.Attempt);
                                insertCMD.Parameters.AddWithValue("@Success", Rates.Success);
                                insertCMD.Parameters.AddWithValue("@Maintain", Rates.Maintain);
                                insertCMD.Parameters.AddWithValue("@Decrease", Rates.Decrease);
                                insertCMD.Parameters.AddWithValue("@Destroy", Rates.Destroy);
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
                            break;
                        case "SFLimits":
                            foreach (string title in SFLimits.Keys)
                            {
                                foreach ((int, int) MinMaxLvl in SFLimits[title].Keys)
                                {
                                    ErrorCounter++;
                                    insertCMD.Parameters.Clear();

                                    insertCMD.Parameters.AddWithValue("@Title", title);
                                    insertCMD.Parameters.AddWithValue("@MinLvl", MinMaxLvl.Item1);
                                    insertCMD.Parameters.AddWithValue("@MaxLvl", MinMaxLvl.Item2);
                                    insertCMD.Parameters.AddWithValue("@MaxStars", SFLimits[title][MinMaxLvl]);

                                    SFID++;
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
                            break;
                        default:
                            foreach (StarforceCLS citem in CurrentList)
                            {
                                ErrorCounter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@SFID", citem.SFLevel);
                                insertCMD.Parameters.AddWithValue("@MinLvl", citem.MinLvl);
                                insertCMD.Parameters.AddWithValue("@MaxLvl", citem.MaxLvl);
                                insertCMD.Parameters.AddWithValue("@JobStat", citem.JobStat);
                                insertCMD.Parameters.AddWithValue("@NonWeaponVDEF", citem.NonWeapVDef);
                                insertCMD.Parameters.AddWithValue("@OverallVDEF", citem.OverallVDef);
                                insertCMD.Parameters.AddWithValue("@CatAMaxHP", citem.CatAMaxHP);
                                insertCMD.Parameters.AddWithValue("@WeaponMaxMP", citem.WeapMaxMP);
                                insertCMD.Parameters.AddWithValue("@WeaponVATK", citem.WeapVATK);
                                insertCMD.Parameters.AddWithValue("@WeaponVMATK", citem.WeapVMATK);
                                insertCMD.Parameters.AddWithValue("@ShoeSpeed", citem.SSpeed);
                                insertCMD.Parameters.AddWithValue("@ShoeJump", citem.SJump);
                                insertCMD.Parameters.AddWithValue("@GloveVATK", citem.GloveVATK);
                                insertCMD.Parameters.AddWithValue("@GloveVMATK", citem.GloveVMATK);
                                insertCMD.Parameters.AddWithValue("@VStat", citem.VStat);
                                insertCMD.Parameters.AddWithValue("@NonWeaponATK", citem.NonWeapATK);
                                insertCMD.Parameters.AddWithValue("@NonWeaponMATK", citem.NonWeapMATK);

                                //Superior Addons

                                insertCMD.Parameters.AddWithValue("@VDEF", citem.VDef);
                                insertCMD.Parameters.AddWithValue("@VATK", citem.VATK);
                                try
                                {
                                    insertCMD.ExecuteNonQuery();
                                }
                                catch (Exception E)
                                {
                                    Console.WriteLine(ErrorCounter.ToString());
                                    Console.WriteLine(E.Message);
                                }
                            }
                            break;
                    }
                }
            }
        }

        //Method for SF
        private static async Task<(List<StarforceCLS>, string)> GetNormalSFAsync(string FileName, string TableConstraint)
        {

            List<StarforceCLS> SFList = new List<StarforceCLS>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.CalculationsPath, FileName);
            string tableSpec = "";
            int counter = 0;

            foreach (string sl in result)
            {
                if (sl == "")
                {
                    return (SFList, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.StarforceCN, sl, TableConstraint);
                    counter++;
                    continue;
                }
                var temp = sl.Split(",");
                StarforceCLS citem = new StarforceCLS();

                citem.SFLevel =  Convert.ToInt32(temp[1]);
                citem.MinLvl = Convert.ToInt32(temp[2]);
                citem.MaxLvl = Convert.ToInt32(temp[3]);
                citem.JobStat = Convert.ToInt32(temp[4]);

                citem.NonWeapVDef = Convert.ToInt32(temp[5]);
                citem.OverallVDef = Convert.ToInt32(temp[6]);

                citem.CatAMaxHP = Convert.ToInt32(temp[7]);
                citem.WeapMaxMP = Convert.ToInt32(temp[8]);

                citem.WeapVATK = Convert.ToInt32(temp[9]);
                citem.WeapVMATK = Convert.ToInt32(temp[10]);

                citem.SSpeed = Convert.ToInt32(temp[11]);
                citem.SJump = Convert.ToInt32(temp[12]);

                citem.GloveVATK = Convert.ToInt32(temp[13]);
                citem.GloveVMATK = Convert.ToInt32(temp[14]);

                citem.VStat = Convert.ToInt32(temp[15]);
                citem.NonWeapATK = Convert.ToInt32(temp[16]);
                citem.NonWeapMATK = Convert.ToInt32(temp[17]);

                SFList.Add(citem);
                counter++;
            }
            return (SFList, tableSpec);
        }

        private static async Task<(List<StarforceCLS>, string)> GetSuperiorSFAsync(string FileName, string TableConstraint)
        {

            List<StarforceCLS> SFList = new List<StarforceCLS>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.CalculationsPath, FileName);
            string tableSpec = "";
            int counter = 0;

            foreach (string sl in result)
            {
                if (sl == "")
                {
                    return (SFList, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.StarforceCN, sl, TableConstraint);
                    counter++;
                    continue;
                }
                var temp = sl.Split(",");
                StarforceCLS citem = new StarforceCLS();

                citem.SFLevel =  Convert.ToInt32(temp[1]);
                citem.MinLvl = Convert.ToInt32(temp[2]);
                citem.MaxLvl = Convert.ToInt32(temp[3]);

                citem.VDef = Convert.ToInt32(temp[4]);
                citem.VStat = Convert.ToInt32(temp[5]);
                citem.VATK = Convert.ToInt32(temp[6]);

                SFList.Add(citem);
                counter++;


            }
            return (SFList, tableSpec);
        }

        //Method for Star Limits
        private static async Task<(Dictionary<string, Dictionary<(int, int), int>>, string)> GetStarLimitAsync(string FileName, string TableConstraint)
        {

            Dictionary<string, Dictionary<(int, int), int>> StarLimit = new Dictionary<string, Dictionary<(int, int), int>>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.CalculationsPath, FileName);
            string tableSpec = "";
            int counter = 0;

            foreach (string sl in result)
            {
                if (sl == "")
                {
                    return (StarLimit, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.StarforceCN, sl, TableConstraint);
                    counter++;
                    continue;
                }
                var temp = sl.Split(",");
                string title = temp[1];
                (int, int) MinMaxLvl = (Convert.ToInt32(temp[2]), Convert.ToInt32(temp[3]));
                if (StarLimit.ContainsKey(title) == false)
                {
                    StarLimit.Add(title, new Dictionary<(int, int), int>());
                }
                if (StarLimit[title].ContainsKey(MinMaxLvl) == false)
                {
                    StarLimit[title].Add(MinMaxLvl, default(int));
                }
                StarLimit[title][MinMaxLvl] = Convert.ToInt32(temp[4]);

                counter++;

            }


            return (StarLimit, tableSpec);
        }

        //Method for Success Rates
        private static async Task<(List<SFRates>, string)> GetSFSuccessAsync(string FileName, string TableConstraint)
        {

            List<SFRates> Rates = new List<SFRates>();
            List<string> result = await ComFunc.CSVStringAsync(GVar.CalculationsPath, FileName);
            string tableSpec = "";
            int counter = 0;

            foreach (string sl in result)
            {
                if (sl == "")
                {
                    return (Rates, tableSpec);
                }
                if (counter == 0)
                {
                    tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.StarforceCN, sl, TableConstraint);
                    counter++;
                    continue;
                }
                var temp = sl.Split(",");

                SFRates citem = new SFRates();
                citem.StarforceType = temp[1];

                citem.Attempt = Convert.ToInt32(temp[2]);
                citem.Success = Convert.ToInt32(temp[3]);
                citem.Maintain = Convert.ToDouble(temp[4]);
                citem.Decrease = Convert.ToDouble(temp[5]);
                citem.Destroy = Convert.ToDouble(temp[6]);

                Rates.Add(citem);

                counter++;

            }


            return (Rates, tableSpec);
        }


        //DB Retrieval
        public static Dictionary<string, Dictionary<int, StarforceCLS>> NewGetAllStarforceDB()
        {
            Dictionary<string, Dictionary<int, StarforceCLS>> FinalSFList = new Dictionary<string, Dictionary<int, StarforceCLS>>();
            Dictionary<int, StarforceCLS> SFList = new Dictionary<int, StarforceCLS>();
            string NormalQuery = "SELECT * FROM SFNormalData";
            string SuperiorQuery = "SELECT * FROM SFSuperiorData";

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                using (SqliteCommand selectCMD = new SqliteCommand(NormalQuery, dbCon))
                {
                    SFList.Clear();
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int SFID = reader.GetInt32(0);
                            int MinLvl = reader.GetInt32(1);
                            int MaxLvl = reader.GetInt32(2);

                            StarforceCLS sf = new StarforceCLS();
                            sf.SFLevel = SFID;
                            sf.MinLvl = MinLvl;
                            sf.MaxLvl = MaxLvl;
                            sf.JobStat = reader.GetInt32(3);
                            sf.NonWeapVDef = reader.GetInt32(4);
                            sf.OverallVDef = reader.GetInt32(5);
                            sf.CatAMaxHP = reader.GetInt32(6);
                            sf.WeapMaxMP = reader.GetInt32(7);

                            sf.SSpeed = reader.GetInt32(10);
                            sf.SJump = reader.GetInt32(11);
                            sf.GloveVATK = reader.GetInt32(12);
                            sf.GloveVMATK = reader.GetInt32(13);


                            if (SFID > 15)
                            {
                                (int, int) MinMax = (MinLvl, MaxLvl);
                                if (SFList.ContainsKey(SFID))
                                {
                                    SFList[SFID].WeapVATKL.Add(MinMax, reader.GetInt32(8));
                                    SFList[SFID].WeapVMATKL.Add(MinMax, reader.GetInt32(9));
                                    SFList[SFID].VStatL.Add(MinMax, reader.GetInt32(14));
                                    SFList[SFID].NonWeapVATKL.Add(MinMax, reader.GetInt32(15));
                                    SFList[SFID].NonWeapVMATKL.Add(MinMax, reader.GetInt32(16));
                                    continue;
                                }
                                sf.WeapVATKL.Add(MinMax, reader.GetInt32(8));
                                sf.WeapVMATKL.Add(MinMax, reader.GetInt32(9));
                                sf.VStatL.Add(MinMax, reader.GetInt32(14));
                                sf.NonWeapVATKL.Add(MinMax, reader.GetInt32(15));
                                sf.NonWeapVMATKL.Add(MinMax, reader.GetInt32(16));
                            }
                            else
                            {
                                sf.WeapVATK = reader.GetInt32(8);
                                sf.WeapVMATK = reader.GetInt32(9);
                            }

                            SFList.Add(SFID, sf);

                        }
                        FinalSFList.Add("Normal", SFList);
                    }

                    selectCMD.CommandText = SuperiorQuery;
                    SFList.Clear();
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int SFID = reader.GetInt32(0);
                            StarforceCLS sf = new StarforceCLS();
                            sf.SFLevel = SFID;
                            (int, int) MinMax = (reader.GetInt32(1), reader.GetInt32(2));
                            if (SFList.ContainsKey(SFID))
                            {
                                SFList[SFID].VDefL.Add(MinMax, reader.GetInt32(3));
                                SFList[SFID].VStatL.Add(MinMax, reader.GetInt32(4));
                                SFList[SFID].WeapVATKL.Add(MinMax, reader.GetInt32(5));
                                continue;
                            }
                            sf.VDefL.Add(MinMax, reader.GetInt32(3));
                            sf.VStatL.Add(MinMax, reader.GetInt32(4));
                            sf.WeapVATKL.Add(MinMax, reader.GetInt32(5));

                            SFList.Add(SFID, sf);
                        }
                        FinalSFList.Add("Superior", SFList);
                    }
                }
            }




            return FinalSFList;
        }
        public static Dictionary<string, Dictionary<int, SFRates>> SFSuccessRatesDB()
        {
            Dictionary<string, Dictionary<int, SFRates>> Rates = new Dictionary<string, Dictionary<int, SFRates>>();
            string RatesQuery = "SELECT * FROM SFSuccessRates";
            using (SqliteConnection dbCon = new SqliteConnection(GVar.CONN_STRING))
            {
                using (SqliteCommand selectCMD = new SqliteCommand(RatesQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string EquipType = reader.GetString(0);
                            int Attempt = reader.GetInt32(1);
                            SFRates SFRates = new SFRates();
                            SFRates.StarforceType = EquipType;
                            SFRates.Attempt = Attempt;
                            SFRates.Success = reader.GetInt32(2);
                            SFRates.Maintain = reader.GetDouble(3);
                            SFRates.Decrease = reader.GetDouble(4);
                            SFRates.Destroy = reader.GetDouble(5);


                            if (Rates.ContainsKey(EquipType))
                            {
                                Rates[EquipType].Add(Attempt, SFRates);
                            }
                            Rates.Add(EquipType, new Dictionary<int, SFRates>());

                        }
                    }
                }
            }

            return Rates;
        }
        public static Dictionary<string, Dictionary<(int, int), int>> SFLimitsDB()
        {
            Dictionary<string, Dictionary<(int, int), int>> Limits = new Dictionary<string, Dictionary<(int, int), int>>();

            string LimitQuery = "SELECT * FROM SFLimits";
            using (SqliteConnection dbCon = new SqliteConnection(GVar.CONN_STRING))
            {
                using (SqliteCommand selectCMD = new SqliteCommand(LimitQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string EquipType = reader.GetString(0);
                            int MinLvl = reader.GetInt32(1);
                            int MaxLvl = reader.GetInt32(2);
                            (int, int) MinMax = (MinLvl, MaxLvl);
                            int Stars = reader.GetInt32(3);

                            if (Limits.ContainsKey(EquipType))
                            {
                                Limits[EquipType].Add(MinMax, Stars);
                            }
                            Limits.Add(EquipType, new Dictionary<(int, int), int>());

                        }
                    }
                }
            }

            return Limits;
        }



    }
}
