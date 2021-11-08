using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.BossRes
{
    public class Boss
    {
        public int BossID { get; set; }
        public string name { get; set; }
        public string difficulty { get; set; }
        public string entryType { get; set; }
        public int entryLimit { get; set; }
        public int bossCrystalCount { get; set; }
        public int meso { get; set; }





        public Boss() { }

        public Boss(string name, string D, int M)
        {
            this.name = name;
            this.difficulty = D;
            this.meso = M;
        }

        public Boss(int ID, string name, string D, string ET, int EL, int BCC,int M)
        {
            this.BossID = ID;
            this.name = name;
            this.difficulty = D;
            this.entryType = ET;
            this.entryLimit = EL;
            this.meso = M;
            this.bossCrystalCount = BCC;


        }

        
    }
}
