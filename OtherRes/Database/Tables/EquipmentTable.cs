using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class EquipmentTable : BaseDBTable, ITableUpload
    {
        public EquipmentTable(string TableName, string TablePara = "") : base(TableName, TablePara)
        {

        }

        public void RetrieveData()
        {
            throw new NotImplementedException();
        }

        public void UploadTable(SqliteConnection connection, SqliteTransaction transaction)
        {
            throw new NotImplementedException();
        }


        //Method to get Armor Data
        //Method to get Acc Data
        //Method to get Main Weapon Data
        //Method to get Sec Weapon Data
    }
}
