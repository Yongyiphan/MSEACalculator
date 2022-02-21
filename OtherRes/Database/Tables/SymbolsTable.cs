using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    //REUSE FOR ARCNAE AND AUTHENTIC SYMBOL
    public class SymbolsTable : BaseDBTable, ITableUpload
    {
        public List<ArcaneSymbolCLS> SymbolList { get; set; }
        public SymbolsTable(string TableName, string TablePara) : base(TableName, TablePara)
        {
        }

        public void RetrieveData()
        {
            SymbolList = new List<ArcaneSymbolCLS> 
            {
                new ArcaneSymbolCLS
                {
                    Name = "Vanishing Journey",
                    BaseSymbolGain = 8,
                    SubMap = "Reverse City",
                    PQSymbolsGain = 6,
                    CostLvlMod = 7130000,
                    CostMod = 2370000
                },
                new ArcaneSymbolCLS
                {
                    Name = "Chew Chew",
                    BaseSymbolGain = 4,
                    SubMap = "Yum Yum",
                    PQGainLimit = 15,
                    SymbolExchangeRate = 1,
                    CostLvlMod = 6600000,
                    CostMod = 12440000
                },
                new ArcaneSymbolCLS
                {
                    Name = "Lachelein",
                    BaseSymbolGain = 8,
                    PQGainLimit = 500,
                    SymbolExchangeRate = 30,
                    CostLvlMod = 6600000,
                    CostMod = 12440000
                },
                new ArcaneSymbolCLS
                {
                    Name = "Arcana",
                    BaseSymbolGain = 8,
                    PQGainLimit = 30,
                    SymbolExchangeRate = 3,
                    CostLvlMod = 6600000,
                    CostMod = 12440000
                },
                new ArcaneSymbolCLS
                {
                    Name = "Moras",
                    BaseSymbolGain = 8,
                    PQSymbolsGain = 6,
                    CostLvlMod = 6600000,
                    CostMod = 12440000
                },
                new ArcaneSymbolCLS
                {
                    Name = "Esfera",
                    BaseSymbolGain = 8,
                    PQSymbolsGain = 6,
                    CostLvlMod = 6600000,
                    CostMod = 12440000
                }

            };
        }
        public void RetrieveDataAsync()
        {
            throw new NotImplementedException();
        }
        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (ComFunc.IsOpenConnection(connection))
            {
                string insertSymbol = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                using (SqliteCommand insertCMD = new SqliteCommand(insertSymbol, connection, transaction))
                {
                    foreach (ArcaneSymbolCLS symbol in SymbolList)
                    {
                        ErrorCounter++;
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@Name", symbol.Name);
                        insertCMD.Parameters.AddWithValue("@SubMap", symbol.SubMap);
                        insertCMD.Parameters.AddWithValue("@CurrentLevel", symbol.CurrentLevel);
                        insertCMD.Parameters.AddWithValue("@CurrentExp", symbol.CurrentExp);
                        insertCMD.Parameters.AddWithValue("@CurrentLimit", CalculationRes.CalForm.CalCurrentLimit(symbol.CurrentLevel));
                        insertCMD.Parameters.AddWithValue("@BaseGain", symbol.BaseSymbolGain);
                        insertCMD.Parameters.AddWithValue("@PQGain", (object)symbol.PQSymbolsGain ?? DBNull.Value);
                        insertCMD.Parameters.AddWithValue("@PQGainLimit", (object)symbol.PQGainLimit ?? DBNull.Value);
                        insertCMD.Parameters.AddWithValue("@SymbolExchangeRate", (object)symbol.SymbolExchangeRate ?? DBNull.Value);
                        insertCMD.Parameters.AddWithValue("@CostLvlMod", symbol.CostLvlMod);
                        insertCMD.Parameters.AddWithValue("@CostMod", symbol.CostMod);

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
