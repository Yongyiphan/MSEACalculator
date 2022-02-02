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
            for (int i = 0; i < MaxLvl; i++)
            {
                MaxExp += i ^ 2 + 11;
            }

            return MaxExp;
        }

        public static int CalCurrentLimit(int level)
        {
            return (level * level) + 11;
        }


    }
}
