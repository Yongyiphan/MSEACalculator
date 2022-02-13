using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class ScrollingModelCLS
    {

        public int MainStat { get; set; } = 0;
        public int HP { get; set; } = 0;
        public int DEF { get; set; } = 0;
        public int ATK { get; set; } = 0;

        public ScrollingModelCLS()
        {
        }


        public List<string> ScrollTypes { get; set; } = new List<string> { "Spell Trace", "Chaos Scroll", "Miracle CS", "CSOG", "ISCOG" };

        public List<string> SpellTracePercTypes { get; set; } = new List<string> { "100%", "70%", "30%", "15%" };

        public List<int> Slots { get; set; } = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };


    }
}
