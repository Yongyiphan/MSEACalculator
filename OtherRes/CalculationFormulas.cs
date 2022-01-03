using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes
{
    public class CalculationFormulas
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
            for (int i = 0; i < MaxLvl; i++)
            {
                MaxExp += i ^ 2 + 11;
            }

            return MaxExp;
        }

        public static int CalSymbolCost(int CurrentLevel, string mode)
        {
            int mod1 = 7130000;
            int mod2 = 2370000;
            int cost = 0;
            switch (mode)
            {
                case "Current":

                    cost = (CurrentLevel * mod1) + mod2;

                    break;
                case "Total":

                    for(int i = 0;i < CurrentLevel; i++)
                    {
                        cost += (i * mod1) + mod2;
                    }

                    break;
            }
            

            return cost;
        }
    }
}
