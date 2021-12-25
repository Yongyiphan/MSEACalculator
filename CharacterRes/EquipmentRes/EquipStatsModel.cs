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

        public EquipStatsModel()
        {

        }

        public override bool Equals(object obj)
        {
            List<string> test = new List<string>();
            if (obj == null)
            {
                return false;
            }
            if(obj is EquipStatsModel)
            {
                test.Add(STR ==  ((EquipStatsModel)obj).STR ? "true" : "false" );
                test.Add(DEX ==  ((EquipStatsModel)obj).DEX ? "true" : "false" );
                test.Add(INT ==  ((EquipStatsModel)obj).INT ? "true" : "false" );
                test.Add(LUK ==  ((EquipStatsModel)obj).LUK ? "true" : "false" );
                test.Add(ATK ==  ((EquipStatsModel)obj).ATK ? "true" : "false" );
                test.Add(MATK ==  ((EquipStatsModel)obj).MATK ? "true" : "false" );
                test.Add(DEF ==  ((EquipStatsModel)obj).DEF ? "true" : "false" );
                test.Add(HP ==  ((EquipStatsModel)obj).HP ? "true" : "false" );
                test.Add(MP ==  ((EquipStatsModel)obj).MP ? "true" : "false" );
                test.Add(SPD ==  ((EquipStatsModel)obj).SPD ? "true" : "false" );
                test.Add(JUMP ==  ((EquipStatsModel)obj).JUMP ? "true" : "false" );
                test.Add(AllStat ==  ((EquipStatsModel)obj).AllStat ? "true" : "false" );

                
            }
            return test.Contains("false") ? false : true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
