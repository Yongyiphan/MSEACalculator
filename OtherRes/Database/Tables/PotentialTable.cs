using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class PotentialTable : BaseDBTable, ITableUpload
    {
        List<PotentialStatsCLS> PotList { get; set; }

        public PotentialTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }

        public async void RetrieveData()
        {
            switch (TableName)
            {
                case "PotentialData":
                    PotList = await ImportCSV.GetPotentialCSVAsync();
                    break;
                case "PotentialBonusData":
                    PotList = await ImportCSV.GetPotentialBonusCSVAsync();
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
    }
}
