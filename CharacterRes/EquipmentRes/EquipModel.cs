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
        public string EquipSlot { get; set; }
        public int EquipLevel { get; set; }
        public string StatType { get; set; } //PERC OR FLAT 
        public int SlotCount { get; set; } = 0;

        public EquipStatsModel BaseStats { get; set; } = new EquipStatsModel();
        public EquipStatsModel ScrollStats { get; set; } = new EquipStatsModel();
        public EquipStatsModel FlameStats { get; set; } = new EquipStatsModel();



        //DEFAULT CONSTRUCTOR
        public EquipModel() { }

    
    }
}
