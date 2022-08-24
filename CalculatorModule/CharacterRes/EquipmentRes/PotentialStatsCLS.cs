using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class PotentialStatsCLS
    {

        //Potential Stats
        
        public int PotID { get; set; } = 0;

        public string EquipSlot {get; set;}
        public string PotGrp { get; set; }
        public string Grade { get; set; }
        public string Prime { get; set; }
        public string DisplayStat { get; set; }
        public string StatIncrease { get; set; }
        public string StatType { get; set; }
        public int MinLvl { get; set; }
        public int MaxLvl { get; set; }
        public string StatValue { get; set; }
        
        public int Chance { get; set; }
        public int Duration { get; set; }
        public int ReflectDMG { get; set; }
        public int Tick { get; set; }

        public List<string> CubeType { get; set; } = new List<string>();


        //Cube Rates
        public PotentialRatesCLS Rates = new PotentialRatesCLS(); 

        public PotentialStatsCLS()
        {

        }

        public object ShallowCopy()
        {
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (obj is PotentialStatsCLS)
                {
                    PotentialStatsCLS cObj = (PotentialStatsCLS)obj;
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
