using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class PotentialStats
    {
        
        public int PotID { get; set; } = 0;

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
        public double Chance { get; set; }
        
        public string DisplayStat { get; set; }


        public PotentialStats()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (obj is PotentialStats)
                {
                    PotentialStats cObj = (PotentialStats)obj;
                    if (PotID ==  cObj.PotID)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
