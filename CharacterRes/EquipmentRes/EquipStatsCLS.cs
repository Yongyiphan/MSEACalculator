using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes.EquipmentRes
{
    public class EquipStatsCLS
    {
        //BASE STATS
        public int MS { get; set; } = 0;
        public int SS { get; set; } = 0;

        public int STR { get; set; } = 0;
        public int DEX { get; set; } = 0;
        public int INT { get; set; } = 0;
        public int LUK { get; set; } = 0;       
        public int DEF { get; set; } = 0;
        public int HP { get; set; } = 0;
        public string SpecialHP { get; set; } = "";
        public int MP { get; set; } = 0;
        public string SpecialMP { get; set; } = "";
        public int SPD { get; set; } = 0;
        public int JUMP { get; set; } = 0;
        public int ATK { get; set; } = 0;
        public int MATK { get; set; } = 0;

        //ADDITIONAL BASE STAT FOR WEAPONS
        public int IED { get; set; } = 0;
        public int BD { get; set; } = 0;
        public int ATKSPD { get; set; } = 0;
        public int DMG { get; set; } = 0;

        //FLAME STATS
        public int AllStat { get; set; } = 0;

        public EquipStatsCLS()
        {

        }

        public override bool Equals(object obj)
        {
            List<string> test = new List<string>();
            if (obj == null)
            {
                return false;
            }
            if(obj is EquipStatsCLS)
            {
                EquipStatsCLS cObj = (EquipStatsCLS)obj;
                test.Add(STR ==  cObj.STR ? "true" : "false" );
                test.Add(DEX ==  cObj.DEX ? "true" : "false" );
                test.Add(INT ==  cObj.INT ? "true" : "false" );
                test.Add(LUK ==  cObj.LUK ? "true" : "false" );
                test.Add(ATK ==  cObj.ATK ? "true" : "false" );
                test.Add(MATK ==  cObj.MATK ? "true" : "false" );
                test.Add(DEF ==  cObj.DEF ? "true" : "false" );
                test.Add(HP ==  cObj.HP ? "true" : "false" );
                test.Add(MP ==  cObj.MP ? "true" : "false" );
                test.Add(SPD ==  cObj.SPD ? "true" : "false" );
                test.Add(JUMP ==  cObj.JUMP ? "true" : "false" );
                test.Add(AllStat ==  cObj.AllStat ? "true" : "false" );

                
            }
            return test.Contains("false") ? false : true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
