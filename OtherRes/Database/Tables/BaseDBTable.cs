using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes.Interface;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class BaseDBTable
    {
        public string TableParameters { get; set; }
        public string TableName { get; set; }

        public int ErrorCounter = 0;

        public BaseDBTable(string TableName, string TablePara = "")
        {
            this.TableName = TableName;
            TableParameters = TablePara;
        }


        public void InitTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            if (connection.State == ConnectionState.Open)
            {

                try
                {
                    //delete table if exist
                    string dropT = "DROP TABLE IF EXISTS " + TableName;
                    using (SqliteCommand dropCmd = new SqliteCommand(dropT, connection, transaction))
                    {
                        dropCmd.ExecuteNonQuery();
                    };
                    // create table if not exist

                    string createTable = "CREATE TABLE IF NOT EXISTS " + TableName + TableParameters;
                    using (SqliteCommand createCmd = new SqliteCommand(createTable, connection, transaction))
                    {
                        createCmd.ExecuteNonQuery();
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }

    
    
}
