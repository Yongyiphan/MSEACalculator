using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class EquipSlotTable : BaseDBTable, ITableUpload
    {
        Dictionary<string, string> EquipmentDict { get; set; }

        private string EquipSlotTableSpec = "(" +
                "EquipSlot string," +
                "EquipType string," +
                "PRIMARY KEY(EquipSlot)" +
                ");";
        public EquipSlotTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {
            TableParameters = EquipSlotTableSpec;
        }

        public void RetrieveData()
        {
            //ASSIGNED ACCORDING TO DATABASE TABLES
            EquipmentDict  = new Dictionary<string, string>(){
                            {"Ring1", "Ring" },{"Ring2", "Ring" },{"Ring3", "Ring" },{"Ring4", "Ring" },
                            {"Pendant1", "Pendant" },{"Pendant2", "Pendant" },
                            {"Face Accessory", "Accessory" },{"Eye Accessory", "Accessory" },{"Earrings", "Accessory" },{"Belt", "Accessory" },
                            {"Shoulder", "Accessory"},
                            {"Badge", "Accessory" },{"Medal", "Accessory" },{"Pocket", "Accessory" },
                            {"Heart", "Accessory" },{"Emblem", "Accessory" },
                            {"Weapon", "Weapon" },{"Secondary", "Secondary" },
                            {"Hat", "Armor" },{"Top", "Armor" },{"Bottom", "Armor" },{"Overall", "Armor" },{"Cape", "Armor" },{"Shoes", "Armor" },
                            {"Gloves", "Armor" }
                        };
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            
            if (ComFunc.IsOpenConnection(connection))
            {
                ErrorCounter++;
                string insertES = ComFunc.InsertSQLStringBuilder(TableName, TableParameters);
                using (SqliteCommand insertCMD = new SqliteCommand(insertES, connection, transaction))
                {
                    foreach (string Eitem in EquipmentDict.Keys)
                    {
                        insertCMD.Parameters.Clear();
                        insertCMD.Parameters.AddWithValue("@EquipSlot", Eitem);
                        insertCMD.Parameters.AddWithValue("@EquipType", EquipmentDict[Eitem]);

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

        public static Dictionary<string, string> GetEquipSlotDB()
        {
            Dictionary<string, string> equipSlotDict = new Dictionary<string, string>();

            using (SqliteConnection dbCon = new SqliteConnection($"Filename = {GVar.databasePath}"))
            {
                dbCon.Open();

                string selectQuery = "SELECT * FROM EquipSlot";
                using (SqliteCommand selectCMD = new SqliteCommand(selectQuery, dbCon))
                {
                    using (SqliteDataReader reader = selectCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipSlotDict.Add(reader.GetString(0), reader.GetString(1));
                        }


                    }
                }
            }

            return equipSlotDict;
        }


    }
}
