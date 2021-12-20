using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class EquipModel
    {
        public string EquipName { get; set; }
        public string EquipSet { get; set; }
        public string ClassType { get; set; }
        public string WeaponType { get; set; }
        public string EquipSlot { get; set; }
        public int EquipLevel { get; set; }
        public string StatType { get; set; } //PERC OR FLAT 
        public int SlotCount { get; set; } = 0;


        public EquipStatsModel BaseStats { get; set; } = new EquipStatsModel();
        public EquipStatsModel ScrollStats { get; set; } = new EquipStatsModel();
        public EquipStatsModel FlameStats { get; set; } = new EquipStatsModel();

        public bool SpellTraced { get; set; } = false;

        public int SpellTracePerc { get; set; } = -1;
        public int StarForce { get; set; } = 0;

        //DEFAULT CONSTRUCTOR
        public EquipModel() { }

        public override bool Equals(object obj)
        {
            List<string> test = new List<string>();
            if(obj == null)
            {
                return false;
            }
            else
            {
                if(obj is EquipModel)
                {
                    test.Add(EquipSet == ((EquipModel)obj).EquipSet ? "true" : "false");
                    test.Add(EquipSlot == ((EquipModel)obj).EquipSlot ? "true" : "false");
                    test.Add(SlotCount == ((EquipModel)obj).SlotCount ? "true" : "false");
                    test.Add(ScrollStats.Equals(((EquipModel)obj).ScrollStats) ? "true" : "false");
                    test.Add(FlameStats.Equals((((EquipModel)obj).FlameStats)) ? "true" : "false");

                     
                }
            }

            return test.Contains("false") ? false : true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
