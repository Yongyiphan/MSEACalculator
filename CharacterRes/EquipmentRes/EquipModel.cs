using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class EquipModel
    {
        //BASE STATS
        public int MS { get; set; }
        public int SS { get; set; }
        public int DEF { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int SPD { get; set; }
        public int JUMP { get; set; }

        //ADDITIONAL BASE STAT FOR WEAPONS
        public int ATK { get; set; }
        public int IED { get; set; }
        public int BD { get; set; }
        public int ATKSPD { get; set; }

        //FLAME STATS
        public int FAllStat { get; set; }
        public int FSTR { get; set; }
        public int FDEX { get; set; }
        public int FLUK { get; set; }
        public int FINT { get; set; }
        public int FDEF { get; set; }
        public int FHP { get; set; }
        public int FMP { get; set; }
        public int FSPD { get; set; }
        public int FJUMP { get; set; }


        public int FATK { get; set; }
        public int FIED { get; set; }
        public int FBD { get; set; }

        //SCROLLING
        public int Slots { get; set; }
        public string scroll { get; set; }


    }
}
