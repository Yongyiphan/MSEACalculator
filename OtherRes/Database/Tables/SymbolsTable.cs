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

        private string[] ArcaneSymSpec = { "(" +
                        "Name string, " +
                        "SubMap string, " +
                        "CurrentLevel int," +
                        "CurrentExp int," +
                        "CurrentLimit int, " +
                        "BaseGain int," +
                        "PQName string, " +
                        "PQGain int, " +
                        "PQGainLimit int, " +
                        "SymbolExchangeRate int," +
                        "MaxLvl int," +
                        "CostLvlMod int," +
                        "CostMod int," +
                        "PRIMARY KEY(Name)" +
                        ");" };
        public SymbolsTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = ArcaneSymSpec[0];
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
                    CostMod = 2370000,
                    MaxLvl = 20
                },
                new ArcaneSymbolCLS
                {
                    Name = "Chew Chew",
                    BaseSymbolGain = 4,
                    SubMap = "Yum Yum",
                    PQName = "Hungry Muto",
                    PQGainLimit = 15,
                    SymbolExchangeRate = 1,
                    CostLvlMod = 6600000,
                    CostMod = 12440000,
                    MaxLvl = 20
                },
                new ArcaneSymbolCLS
                {
                    Name = "Lachelein",
                    BaseSymbolGain = 8,
                    PQName = "Dream Breaker",
                    PQGainLimit = 500,
                    SymbolExchangeRate = 30,
                    CostLvlMod = 6600000,
                    CostMod = 12440000,
                    MaxLvl = 20
                },
                new ArcaneSymbolCLS
                {
                    Name = "Arcana",
                    BaseSymbolGain = 8,
                    PQName = "Spirit Savior",
                    PQGainLimit = 30,
                    SymbolExchangeRate = 3,
                    CostLvlMod = 6600000,
                    CostMod = 12440000,
                    MaxLvl = 20
                },
                new ArcaneSymbolCLS
                {
                    Name = "Moras",
                    BaseSymbolGain = 8,
                    PQSymbolsGain = 6,
                    CostLvlMod = 6600000,
                    CostMod = 12440000,
                    MaxLvl = 20
                },
                new ArcaneSymbolCLS
                {
                    Name = "Esfera",
                    BaseSymbolGain = 8,
                    PQSymbolsGain = 6,
                    CostLvlMod = 6600000,
                    CostMod = 12440000,
                    MaxLvl = 20
                }

            };
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
                        insertCMD.Parameters.AddWithValue("@PQName", (object)symbol.PQName ?? string.Empty);
                        insertCMD.Parameters.AddWithValue("@PQGain", (object)symbol.PQSymbolsGain ?? DBNull.Value);
                        insertCMD.Parameters.AddWithValue("@PQGainLimit", (object)symbol.PQGainLimit ?? DBNull.Value);
                        insertCMD.Parameters.AddWithValue("@SymbolExchangeRate", (object)symbol.SymbolExchangeRate ?? DBNull.Value);
                        insertCMD.Parameters.AddWithValue("@MaxLvl", symbol.MaxLvl);
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

        public static List<ArcaneSymbolCLS> GetAllArcaneSymbolDB()
        {
            List<ArcaneSymbolCLS> symbolList = new List<ArcaneSymbolCLS>();
            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM ArcaneSymbolData";

                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ArcaneSymbolCLS symbol = new ArcaneSymbolCLS();
                            symbol.Name               =  reader.GetString(0);
                            symbol.SubMap             =  reader.GetString(1);
                            symbol.CurrentLevel       =  reader.GetInt32(2);
                            symbol.CurrentExp         =  reader.GetInt32(3);
                            symbol.CurrentLimit       =  reader.GetInt32(4);
                            symbol.BaseSymbolGain     =  reader.GetInt32(5);
                            symbol.PQName             =  reader.GetString(6);
                            symbol.PQSymbolsGain      =  reader.GetInt32(7);
                            symbol.PQGainLimit        =  reader.GetInt32(8);
                            symbol.SymbolExchangeRate =  reader.GetInt32(9);
                            symbol.MaxLvl             =  reader.GetInt32(10);
                            symbol.CostLvlMod         =  reader.GetInt32(11);
                            symbol.CostMod            = reader.GetInt32(12);
                            symbol.MaxExp = CalculationRes.CalForm.CalMaxExp(symbol.MaxLvl);
                            symbol.Description = "Arcane";
                            
                            symbolList.Add(symbol);


                        }
                    }
                }


            }

            return symbolList;
        }

    }
}
