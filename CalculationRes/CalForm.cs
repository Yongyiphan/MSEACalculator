using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes
{
    public class CalForm
    {
        

        public static int calSFStatBoost(int BaseStat, int ProportionPerc)
        {
            int proportion = ProportionPerc/100;
            int result = (BaseStat / proportion) + 1;

            return result;


        }



        public static int CalMaxExp(int MaxLvl)
        {
            int MaxExp = 0;
            for (int i = 1; i < MaxLvl; i++)
            {
                MaxExp += CalCurrentLimit(i);
            }

            return MaxExp;
        }

        public static int CalCurrentLimit(int level)
        {
            return (level * level) + 11;
        }

        public static Dictionary<string, int> CalNewLvlExp(int cLvl, int cExp, int symGain, bool subMap)
        {
            Func<int, int, int> CalDaysLeft = (div, ctotalExp) =>

            {
                int symDiff = GVar.MaxSymbolExp - ctotalExp;


                return symDiff/div + 1;
            };

            Dictionary<string, int> dictStore = new Dictionary<string, int>();
            int mod = subMap == true ? 2 : 1;

            int newLvl = cLvl;
            int newExp, totalExp = 0;
            for(int i = 1; i < cLvl + 1; i++)
            {
                totalExp += CalCurrentLimit(i);
            }

            totalExp += cExp>CalCurrentLimit(cLvl) ? cExp : cExp - CalCurrentLimit(cLvl);



            dictStore["CurrentTotalExp"] = 1;



            dictStore["NewLevel"] = 1;
            dictStore["NewLimit"] = 1;
            dictStore["RemainingExp"] = 1;




            return dictStore;
        }



    }
}
