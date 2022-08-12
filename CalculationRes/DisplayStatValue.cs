using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes
{
    public class DisplayStatValue
    {


        public string Key;
        public string DTotalStat { get; set; }
        public string DBaseStat { get; set; }
        public string DScrollStat { get; set; }
        public string DFlameStat { get; set; }
        public string DStarforceStat { get; set; }

        public int TotalStat { get; set; }
        public int BaseStat { get; set; }
        public int ScrollStat { get; set; }
        public int FlameStat { get; set; }
        public int StarforceStat { get; set; }

        public void ReOrgInput()
        {
            char[] toTrim = { '+', '%', ' ', ':' };


            string Perc = GVar.SpecialStatType.Contains(Key) || Key == "IED" ? "%" : "";


            if (string.IsNullOrEmpty(DBaseStat) == false)
            {
                BaseStat = Convert.ToInt32(DBaseStat.Trim(toTrim));
                DBaseStat = String.Format("{0}{1}", BaseStat, Perc);
            }
            if (string.IsNullOrEmpty(DScrollStat) == false)
            {
                ScrollStat = Convert.ToInt32(DScrollStat.Trim(toTrim));
                DScrollStat = String.Format(" +{0}{1}", ScrollStat, Perc);
            }
            if (string.IsNullOrEmpty(DFlameStat) == false)
            {
                FlameStat = Convert.ToInt32(DFlameStat.Trim(toTrim));
                DFlameStat = String.Format(" +{0}{1}", FlameStat, Perc);
            }
            if (string.IsNullOrEmpty(DStarforceStat) == false)
            {

                StarforceStat = Convert.ToInt32(DStarforceStat.Trim(toTrim));
                DStarforceStat = String.Format(" +{0}{1}", StarforceStat, Perc);
            }

            DTotalStat = String.Format(": {0}{1} ", TotalStat, Perc);
        }

        public int ReturnTotal()
        {
            ReOrgInput();
            
            TotalStat = BaseStat + ScrollStat + FlameStat + StarforceStat;
            return TotalStat;
        }



    }
}
