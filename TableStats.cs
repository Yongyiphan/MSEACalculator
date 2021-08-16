using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator
{
    class TableStats
    {
        public string tableName { get; set; }
        public string tableSpecs { get; set; }
        public string initMode { get; set; }

        public TableStats() { }
        public TableStats(string TN, string TS, string mode)
        {
            this.tableName = TN;
            this.tableSpecs = TS;
            this.initMode = mode;

        }
    }
}
