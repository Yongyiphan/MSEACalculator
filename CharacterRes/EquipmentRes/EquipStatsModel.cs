using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class EquipStatsModel
    {
        //BASE STATS
        public int MS { get; set; } = 0;
        public int SS { get; set; } = 0;
        public int STR { get; set; } = 0;
        public int DEX { get; set; } = 0;
        public int LUK { get; set; } = 0;
        public int INT { get; set; } = 0;
        public int DEF { get; set; } = 0;
        public int HP { get; set; } = 0;
        public string SpecialHP { get; set; }
        public int MP { get; set; } = 0;
        public string SpecialMP { get; set; }
        public int SPD { get; set; } = 0;
        public int JUMP { get; set; } = 0;
        public int ATK { get; set; } = 0;
        public int MATK { get; set; } = 0;

        //ADDITIONAL BASE STAT FOR WEAPONS
        public int IED { get; set; }
        public int BD { get; set; }
        public int ATKSPD { get; set; }
        public int DMG { get; set; }

        //FLAME STATS
        public int AllStat { get; set; }

        public EquipStatsModel()
        {

        }

    }
}
