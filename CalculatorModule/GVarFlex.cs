using MSEACalculator.CalculationRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator
{
    public class GVarFlex
    {
        public int MaxCrytalCount { get; private set; } = 180;
        public int MaxArcaneSymbolLevel { get; private set; } = 20;
        public int MaxAuthenticSymbolLevel { get; private set; } = 11;
        public int ArcaneTransferSymbolExp { get; private set; } = 2467;
        public int AuthenticTransferSymbolExp { get; private set; } = 0;
        public int MaxSymbolExp { get; set; }

        public List<string> ArcaneSymbolCoinExchange { get; set; } = new List<string> { "Lachelein", "Arcana" }; 
        public int minLevel { get; private set; } = 1;
        public int maxLevel { get; private set; } = 300;


        public GVarFlex()

        {
            MaxSymbolExp  = CalForm.CalMaxExp(MaxArcaneSymbolLevel);

        }


    }
}
