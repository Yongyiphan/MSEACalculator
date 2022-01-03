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
        public string StatIncrease { get; set; }
        public string StatType { get; set; }
        public int StatValue { get; set; }
        
        public int LevelRank { get; set; }
        public string Prime { get; set; }
        


        public PotentialStats()
        {

        }

    }
}
