using Microsoft.Data.Sqlite;
using MSEACalculator.CalculationRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class HyperStatTable : BaseDBTable, ITableUpload
    {
        public List<HyperStatCLS> CurrentList = new List<HyperStatCLS>();
        public HyperStatTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
        }

        public async void RetrieveData()
        {
            string FileName = "";
            string TableConstraint = "";
            switch (TableName)
            {
                case "HyperStat":
                    FileName = "HyperStat.csv";
                    TableConstraint = "PRIMARY KEY (Stat, HSLevel)";
                    break;
                case "HyperStatCost":
                    FileName = "HyperStatCost.csv";
                    TableConstraint = "All";
                    break;
                case "HyperStatDistribution":
                    FileName = "HyperStatDistribution.csv";
                    TableConstraint = "PRIMARY KEY (Level)";
                    break;
            }
            (List<HyperStatCLS>, string) Result = await GetHyperStatAsync(FileName, TableConstraint);
            CurrentList = Result.Item1;
            TableParameters = Result.Item2;
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                string insertHS = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                using (SqliteCommand insertCMD = new SqliteCommand(insertHS, connection, transaction))
                {
                    foreach (HyperStatCLS hl in CurrentList)
                    {
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@Stat", hl.Stat);
                        insertCMD.Parameters.AddWithValue("@HSLevel", hl.Level);
                        insertCMD.Parameters.AddWithValue("@StatIncrease", hl.StatIncrease);
                        insertCMD.Parameters.AddWithValue("@TotalStatIncrease", hl.TotalStatIncrease);

                        insertCMD.Parameters.AddWithValue("@Cost", hl.Costs);
                        insertCMD.Parameters.AddWithValue("@TotalCost", hl.TotalCosts);

                        insertCMD.Parameters.AddWithValue("@Level", hl.Level);
                        insertCMD.Parameters.AddWithValue("@Points", hl.PointsGiven);
                        insertCMD.Parameters.AddWithValue("@TotalPoints", hl.TotalPointsGiven);
                        try
                        {
                            insertCMD.ExecuteNonQuery();
                        }
                        catch (Exception E)
                        {
                            Console.WriteLine(ErrorCounter);
                            Console.WriteLine(E.Message);
                        }
                    }
                }
            }
        }
        //Hyper Stat
        private static async Task<(List<HyperStatCLS>, string)> GetHyperStatAsync(string FileName, string constraint)
        {
            List<HyperStatCLS> HSList = new List<HyperStatCLS>();
            string tableSpec = "";
            int counter = 0;
            List<string> result = await ComFunc.CSVStringAsync(GVar.CalculationsPath, FileName);
            try
            {


                foreach (string hstat in result)
                {
                    if (hstat == "")
                    {
                        return (HSList, tableSpec);
                    }
                    if (counter == 0)
                    {
                        tableSpec = ComFunc.TableSpecStringBuilder(TableColNames.HyperStatCN, hstat, constraint);
                        counter++;
                        continue;
                    }
                    var temp = hstat.Split(",");
                    HyperStatCLS citem = new HyperStatCLS();
                    switch (FileName)
                    {
                        case "HyperStat.csv":
                            citem.Stat = temp[1];
                            citem.Level = Convert.ToInt32(temp[2]);
                            citem.TotalStatIncrease = Convert.ToDouble(temp[3]);
                            citem.StatIncrease = Convert.ToDouble(temp[4]);
                            break;
                        case "HyperStatCost.csv":
                            citem.Level = Convert.ToInt32(temp[1]);
                            citem.Costs= Convert.ToInt32(temp[2]);
                            citem.TotalCosts = Convert.ToInt32(temp[3]);
                            break;
                        case "HyperStatDistribution.csv":
                            citem.Level = Convert.ToInt32(temp[1]);
                            citem.PointsGiven = Convert.ToInt32(temp[2]);
                            citem.TotalPointsGiven = Convert.ToInt32(temp[2]);
                            break;
                    }


                    counter++;
                    HSList.Add(citem);

                }
            }
            catch (Exception)
            {
                Console.WriteLine(counter);
            }

            return (HSList, tableSpec);
        }
    }
}
