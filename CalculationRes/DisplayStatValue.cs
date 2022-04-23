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
                char[] toTrim = {'+', '%', ' ', ':' };
                BaseStat = Convert.ToInt32(DBaseStat.Trim(toTrim));
                ScrollStat = Convert.ToInt32(DScrollStat.Trim(toTrim));
                FlameStat = Convert.ToInt32(DFlameStat.Trim(toTrim));
                StarforceStat = Convert.ToInt32(DStarforceStat.Trim(toTrim));

                if (GVar.SpecialStatType.Contains(Key) || Key == "IED")
                {
                    DBaseStat      = String.Format("{0}%",  BaseStat);
                    DScrollStat    = String.Format(" +{0}%", ScrollStat);
                    DStarforceStat = String.Format(" +{0}%", StarforceStat);
                    DFlameStat     = String.Format(" +{0}%", FlameStat);

                    DTotalStat     = String.Format(": {0}% ", TotalStat);
                }
                else
                {
                    DBaseStat      = String.Format("{0}",  BaseStat);
                    DScrollStat    = String.Format(" +{0}", ScrollStat);
                    DStarforceStat = String.Format(" +{0}", StarforceStat);
                    DFlameStat     = String.Format(" +{0}", FlameStat);

                    DTotalStat     = String.Format(": {0} ", TotalStat);

                }

            }

            public int ReturnTotal()
            {
                ReOrgInput();
                TotalStat = BaseStat + ScrollStat + FlameStat + StarforceStat;
                return TotalStat;
            } 



    }
}
