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

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class StarForceTable : BaseDBTable, ITableUpload
    {

        public List<StarforceCLS> CurrentList { get; set; }
        public Dictionary<string, Dictionary<(int, int), int>> SFLimits { get; set; }

        public List<StarforceRatesCLS> SFRates { get; set; }
       

        List<string> FileNames = new List<string>()
        {
            "NormalEquipSF.csv",
            "SFSuccessRates.csv",
            "StarLimit.csv",
            "SuperiorItemsSF.csv"

        };

        
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
                    (List<StarforceRatesCLS>, string) Rates = await GetSFSuccessAsync("SFSuccessRates.csv", "PRIMARY KEY (Title, Attempt)");
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
                using(SqliteCommand insertCMD = new SqliteCommand(insertSF, connection, transaction))
                {
                    switch (TableName)
                    {
                        case "SFSuccessRates":
                            foreach (StarforceRatesCLS Rates in SFRates)
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
                            foreach(StarforceCLS citem in CurrentList)
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
                                catch(Exception E)
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
        private static async Task<(List<StarforceRatesCLS>, string)> GetSFSuccessAsync(string FileName, string TableConstraint)
        {

            List<StarforceRatesCLS> Rates = new List<StarforceRatesCLS>();
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

                StarforceRatesCLS citem = new StarforceRatesCLS();
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


    }
}
