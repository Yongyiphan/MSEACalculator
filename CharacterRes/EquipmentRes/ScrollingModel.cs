﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class ScrollingModel
    {

        public int MainStat { get; set; }
        public int HP { get; set; }
        public int DEF { get; set; }
        public int ATK { get; set; }

        public ScrollingModel()
        {
        }

        //ARMOR | Accessories = HP =0, DEF =0
        public ScrollingModel(int MS, int HP, int DEF)
        {
            this.MainStat = MS;
            this.HP = HP;
            this.DEF = DEF;
        }

        //HEART & GLOVES
        public ScrollingModel(int ATK)
        {
            this.ATK = ATK;
        }

        //WEAPON
        public ScrollingModel(int MS, int ATk)
        {
            this.MainStat= MS;
            this.ATK= ATk;
        }

        public List<string> ScrollTypes { get; set; } = new List<string> { "Spell Trace", "Chaos Scroll", "Miracle CS", "CSOG", "ISCOG" };

        public List<string> SpellTraceTypes { get; set; } = new List<string> { "100%", "70%", "30%", "15%" };

        public List<int> Slots { get; set; } = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };



    }
}
