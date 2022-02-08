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

        public static int CalAccEXp(int cLvl, int cExp)
        {
            int currentLimit = CalCurrentLimit(cLvl);
            int totalExp = 0;
            for (int i = 1; i < cLvl + 1; i++)
            {
                totalExp += CalCurrentLimit(i);
            }
            totalExp += cExp - currentLimit;

            return totalExp;
        }

        public static int CalDaysLeft(int accExp, decimal symGain)
        {

            int remainingExp = GVar.MaxSymbolExp - accExp;

            decimal DaysLeft = Math.Ceiling(remainingExp / symGain);

            return (int)DaysLeft;
        }

        public static Dictionary<string, int> CalNewLvlExp(int accExp)
        {
            

            Dictionary<string, int> dictStore = new Dictionary<string, int>();

            //int currentLimit = CalCurrentLimit(cLvl);
            //int remainingExp, totalExp = 0;
            //for(int i = 1; i < cLvl + 1; i++)
            //{
            //    totalExp += CalCurrentLimit(i);
            //}

            //totalExp += cExp - CalCurrentLimit(cLvl);
            int currentLimit = CalCurrentLimit(1);
            int cLvl = 1;


            dictStore["CurrentTotalExp"] = accExp;

            
            while(accExp > currentLimit)
            {
                cLvl++;
                accExp -= currentLimit;
                currentLimit = CalCurrentLimit(cLvl);
            }

            dictStore["NewLevel"] = cLvl;
            dictStore["NewLimit"] = currentLimit;
            dictStore["RemainingExp"] = accExp;




            return dictStore;
        }


        public static Dictionary<string,int> CalArcaneStatsForce(int symbolLvl, string mode)
        {
            Dictionary<string, int> dictStore = new Dictionary<string, int>();
            int cForce = 0;
            int cStat = 0;
            int baseForce = 30;
            int addForce = 10;

            int addStat = 0;
            int baseStat = 0;

            
            switch (mode)
            {
                case "Demon Avenger":
                    baseStat = 5250;
                    addStat = 1750;
                    break;

                case "Xenon":
                    baseStat = 117;
                    addStat = 39;
                    break;

                case "General":
                    baseStat = 300;
                    addStat = 100;
                    break;
            }
            
            if (symbolLvl > 1)
            {
                for(int i = 1; i<symbolLvl; i++)
                {
                    cForce += addForce;
                    cStat += addStat;
                }
            }

            dictStore["ArcaneForce"] = cForce + baseForce;
            dictStore["Stat"] = cStat + baseStat;


            return dictStore;
        }


    }
}
