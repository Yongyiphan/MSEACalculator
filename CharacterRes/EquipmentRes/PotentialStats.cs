using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class PotentialStats
    {
        

        public string Grade { get; set; }
        public List<string> EquipGrpL { get; set; }
        public string EquipGrp {get; set;}
        public string StatIncrease { get; set; }
        public string StatType { get; set; }

        public string StatValue { get; set; }
        
        public int MinLvl { get; set; }
        public int MaxLvl { get; set; }
        public string Prime { get; set; }
        public int Duration { get; set; }
        
        public string DisplayStat { get; set; }


        public PotentialStats()
        {

        }

    }
}
