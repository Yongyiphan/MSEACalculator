using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes
{
    public class StarforceRatesCLS
    {
        public StarforceRatesCLS()
        {

        }

        public string StarforceType { get; set; }
        public int Attempt { get; set; }
        public int Success { get; set; }
        public double Maintain { get; set; }
        public double Decrease { get; set; }
        public double Destroy { get; set; }

    }
}
