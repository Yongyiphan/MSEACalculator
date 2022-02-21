using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes.Interface
{
    public interface ITableUpload
    {

        void RetrieveData();
        void UploadTable(SqliteConnection connection, SqliteTransaction transaction);


    }
}
