using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class EquipCLS
    {
        public string EquipName { get; set; }
        public string EquipSet { get; set; }
        public string ClassType { get; set; }
        public string WeaponType { get; set; }
        public string EquipSlot { get; set; }
        public int EquipLevel { get; set; }
        public string StatType { get; set; } //PERC OR FLAT 
        public int SlotCount { get; set; } = 0;


        public EquipStatsCLS BaseStats { get; set; } = new EquipStatsCLS();
        public EquipStatsCLS ScrollStats { get; set; } = new EquipStatsCLS();
        public EquipStatsCLS FlameStats { get; set; } = new EquipStatsCLS();

        public int MPotGrade { get; set; } = 0;
        public Dictionary<string, PotentialStatsCLS> MainPot { get; set; } = new Dictionary<string, PotentialStatsCLS>
        {
            {"First", new PotentialStatsCLS() },
            {"Second", new PotentialStatsCLS() },
            {"Third", new PotentialStatsCLS() },
        };

        public int APotGrade { get; set;} = 0;  
        public Dictionary<string, PotentialStatsCLS> AddPot { get; set; } = new Dictionary<string, PotentialStatsCLS>
        {
            {"First", new PotentialStatsCLS() },
            {"Second", new PotentialStatsCLS() },
            {"Third", new PotentialStatsCLS() },
        };

        public bool SpellTraced { get; set; } = false;

        //STORE BY INDEX
        public int SpellTracePerc { get; set; } = -1;
        public int StarForce { get; set; } = 0;

        //DEFAULT CONSTRUCTOR
        public EquipCLS() { }

        public override bool Equals(object obj)
        {
            List<string> test = new List<string>();
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (obj is EquipCLS)
                {
                    EquipCLS cObj = (EquipCLS)obj;
                    test.Add(EquipSet == cObj.EquipSet ? "true" : "false");
                    test.Add(EquipSlot == cObj.EquipSlot ? "true" : "false");
                    test.Add(SlotCount == cObj.SlotCount ? "true" : "false");
                    test.Add(ScrollStats.Equals(cObj.ScrollStats) ? "true" : "false");
                    test.Add(FlameStats.Equals(cObj.FlameStats) ? "true" : "false");
                    //test.Add(new HashSet<PotentialStats>(MainPot).Equals(new HashSet<PotentialStats>(((EquipModel)obj).MainPot)) ? "true" : "false");
                    test.Add(MainPot.Values.ToList().Select(x => x.PotID).ToList().SequenceEqual(cObj.MainPot.Values.ToList().Select(x => x.PotID).ToList()) ? "true" : "false");


                }
            }

            return test.Contains("false") ? false : true;
        }

        public object ShallowCopy()
        {
            return this.MemberwiseClone();
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
