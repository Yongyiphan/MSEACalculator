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
        public int MaxHP { get; set; } = 0;
        public int DEF { get; set; } = 0;
        public int ATK { get; set; } = 0;

        public ScrollingModelCLS()
        {
        }


        public List<string> ScrollTypes { get; set; } = new List<string> { "Spell Trace", "Chaos Scroll", "Miracle CS", "CSOG", "ISCOG" };

        public List<string> SpellTracePercTypes { get; set; } = new List<string> { "100%", "70%", "30%", "15%" };

        public List<int> SpellTracePerc { get; set; } = new List<int>() { 100, 70, 30, 15 };

        public List<string> SpellTraceStat { get; set; } = new List<string> { "STR", "DEX", "INT", "LUK", "HP" };

        public List<int> Slots { get; set; } = Enumerable.Range(0, 12).ToList();


    }
}
