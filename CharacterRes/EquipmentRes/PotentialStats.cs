using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class PotentialStats
    {
        public int FirstLineStat { get; set; }
        public int SecondLineStat { get; set;}
        public int ThirdLineStat { get; set;}

        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public string ThirdLine { get; set; }



        public string Grade { get; set; }
        public List<string> EquipGrpL { get; set; }
        public string EquipGrp {get; set;}
        public string StatIncrease { get; set; }
        public string StatType { get; set; }

        public string StatValue { get; set; }
        
        public int MinLvl { get; set; }
        public int MaxLvl { get; set; }
        public string Prime { get; set; }


        


        public PotentialStats()
        {

        }

    }
}
