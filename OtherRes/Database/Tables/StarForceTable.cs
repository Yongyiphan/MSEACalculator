using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using MSEACalculator.StarforceRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class StarForceTable : BaseDBTable, ITableUpload
    {
        public List<SFGainCLS> SFList { get; set; } 
        public StarForceTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }
        public async void RetrieveData()
        {
            switch (TableName)
            {
                case "StarForceBaseData":
                    SFList = await ImportCSV.GetSFCSVAsync();
                    break;
                case "StarForceAddData":
                    SFList = await ImportCSV.GetAddSFCSVAsync();
                    break;
            }
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                switch (TableName)
                {
                    case "StarForceBaseData":
                        string insertSF = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                        using (SqliteCommand insertCMD = new SqliteCommand(insertSF, connection, transaction))
                        {
                            foreach (SFGainCLS sF in SFList)
                            {
                                ErrorCounter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@SFID", sF.SFLevel);
                                insertCMD.Parameters.AddWithValue("@JobStat", sF.JobStat);

                                insertCMD.Parameters.AddWithValue("@NonWeapVDef", sF.NonWeapVDef);
                                insertCMD.Parameters.AddWithValue("@OverallVDef", sF.OverallVDef);
                                
                                insertCMD.Parameters.AddWithValue("@CatAMaxHP", sF.CatAMaxHP);
                                insertCMD.Parameters.AddWithValue("@WeapMaxMP", sF.WeapMaxMP);
                                
                                insertCMD.Parameters.AddWithValue("@WeapVATK", sF.WeapVATK);
                                insertCMD.Parameters.AddWithValue("@WeapVMATK", sF.WeapVMATK);
                                
                                insertCMD.Parameters.AddWithValue("@SJump", sF.SJump);
                                insertCMD.Parameters.AddWithValue("@SSpeed", sF.SSpeed);
                                
                                insertCMD.Parameters.AddWithValue("GloveVATK", sF.GloveVATK);
                                insertCMD.Parameters.AddWithValue("@GloveVMATK", sF.GloveVMATK);

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
                    case "StarForceAddData":
                        string insertASF = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                        using (SqliteCommand insertCMD = new SqliteCommand(insertASF, connection, transaction))
                        {
                            foreach (SFGainCLS sF in SFList)
                            {   
                                ErrorCounter++;
                                insertCMD.Parameters.Clear();
                                insertCMD.Parameters.AddWithValue("@SFID", sF.SFLevel);
                                insertCMD.Parameters.AddWithValue("@LevelRank", sF.LevelRank);
                                insertCMD.Parameters.AddWithValue("@VStat", sF.VStat);
                                insertCMD.Parameters.AddWithValue("@NonWeapVATK", sF.NonWeapATK);
                                insertCMD.Parameters.AddWithValue("@NonWeapVMATK", sF.NonWeapMATK);
                                insertCMD.Parameters.AddWithValue("@WeapVATK", sF.WeapVATK);
                                insertCMD.Parameters.AddWithValue("@WeapVMATK", sF.WeapVMATK);



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
                }
            }
        }

        
    }
}
