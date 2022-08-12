using Microsoft.Data.Sqlite;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Database.Tables
{
    public class SpellTraceTable : BaseDBTable, ITableUpload
    {



        public SpellTraceTable(string TableName, string TablePara = "") : base(TableName, TablePara)
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
    }
}
