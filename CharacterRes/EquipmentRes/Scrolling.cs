using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class Scrolling
    {

        public int MainStat { get; set; }
        public int HP { get; set; }
        public int DEF { get; set; }
        public int ATK { get; set; }

        public Scrolling()
        {

        }

        //ARMOR | Accessories = HP =0, DEF =0
        public Scrolling(int MS, int HP, int DEF)
        {
            this.MainStat = MS;
            this.HP = HP;
            this.DEF = DEF;
        }

        //HEART & GLOVES
        public Scrolling(int ATK)
        {
            this.ATK = ATK;
        }

        //WEAPON
        public Scrolling(int MS, int ATk)
        {
            this.MainStat= MS;
            this.ATK= ATk;
        }

        public List<string> ScrollTypes { get; set; } = new List<string> { "Spell Trace", "Chaos Scroll", "Miracle CS", "CSOG", "ISCOG" };

        public List<int> slots { get; set; } = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        //public Dictionary<string, Dictionary<int, Dictionary<int, Scrolling>>> SpellTrace { get; set; } = new Dictionary<string, Dictionary<int, Dictionary<int, Scrolling>>>()
        //{
        //    ["Armor"] = new Dictionary<int, Dictionary<int, Scrolling>>
        //    {
        //        [1] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(1,5,1),
        //            [70] = new Scrolling(2,15,2),
        //            [30] = new Scrolling(3,30,4)
        //        },
        //        [2] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(2,20,2),
        //            [70] = new Scrolling(3,40,4),
        //            [30] = new Scrolling(5,70,7)
        //        },
        //        [3] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(3,30,3),
        //            [70] = new Scrolling(4,70,5),
        //            [30] = new Scrolling(7,120,10)
        //        }

        //    },
        //    ["Gloves"] = new Dictionary<int, Dictionary<int, Scrolling>>
        //    {
        //        [1] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(0,0,3),
        //            [70] = new Scrolling(1),
        //            [30] = new Scrolling(2)
        //        },
        //        [2] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(1),
        //            [70] = new Scrolling(2),
        //            [30] = new Scrolling(3)
        //        }
        //    },
        //    ["Accessories"] = new Dictionary<int, Dictionary<int, Scrolling>>
        //    {
        //        [1] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(1,0,0),
        //            [70] = new Scrolling(2,0,0),
        //            [30] = new Scrolling(3,0,0)
        //        },
        //        [2] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(1, 0, 0),
        //            [70] = new Scrolling(2, 0, 0),
        //            [30] = new Scrolling(4, 0, 0)
        //        },
        //        [3] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(2, 0, 0),
        //            [70] = new Scrolling(3, 0, 0),
        //            [30] = new Scrolling(5, 0, 0)
        //        }
        //    },
        //    ["Heart"] = new Dictionary<int, Dictionary<int, Scrolling>>
        //    {
        //        [1] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(1),
        //            [70] = new Scrolling(2),
        //            [30] = new Scrolling(3)
        //        },
        //        [2] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(2),
        //            [70] = new Scrolling(3),
        //            [30] = new Scrolling(5)
        //        }
        //        ,
        //        [3] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(3),
        //            [70] = new Scrolling(4),
        //            [30] = new Scrolling(7)
        //        }
        //    },
        //    ["Weapon"] = new Dictionary<int, Dictionary<int, Scrolling>>
        //    {
        //        [1] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(1),
        //            [70] = new Scrolling(2),
        //            [30] = new Scrolling(1,3),
        //            [15] = new Scrolling(2,5)
        //        },
        //        [2] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(2),
        //            [70] = new Scrolling(1,3),
        //            [30] = new Scrolling(2,5),
        //            [15] = new Scrolling(3,7)
        //        }
        //        ,
        //        [3] = new Dictionary<int, Scrolling>
        //        {
        //            [100] = new Scrolling(1,3),
        //            [70] = new Scrolling(2,5),
        //            [30] = new Scrolling(3,7),
        //            [15] = new Scrolling(4,9)
        //        }
        //    }

        //};
    }
}
