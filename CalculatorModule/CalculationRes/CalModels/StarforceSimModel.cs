using MSEACalculator.OtherRes.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSEACalculator.OtherRes.Database.StructCollections;

namespace MSEACalculator.CalculationRes.CalModels
{
    public class StarforceSimModel
    {
        public Dictionary<string, Dictionary<int, StarforceCLS>> StarforceStatStore { get => StarForceTable.GetAllStarforceDB(); }

        public Dictionary<string, Dictionary<int, SFRates>> SuccessRates { get => StarForceTable.SFSuccessRatesDB(); }



    }
}
