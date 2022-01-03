using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CharacterRes
{
    public class ArcaneSymbol
    {

        //To be keyed in
        public string Name { get; set; }

        public int CurrentLevel { get; set; } = 1;
        public int CurrentExp { get; set; } = 1;
       


        //Generated when Compiled
        public int CurrentLimit { get; set; }
        public int AccumulatedExp { get; set; }
        public int MaxExp { get; set; } = GVar.MaxSymbolExp;

        //Result:
        public int DaysLeft { get; set; }
        public int New { get; set; }
        public int NewCExp { get; set; }

        //Example
        //ChuChu
        //CurrentLevel : 1
        //CurrentExp/CurrentLimit: 885/12 

        //Results:
        //New Level = 13
        //CurrentExp/CurrentLimit : 103/180
        //AccumulatedExp / TotalExp : 885/2679
        //TotalCost: 826,280,000

        //Arcane Catalyst

        public ArcaneSymbol()
        {
            CurrentLimit = CurrentLevel ^ 2 + 11;

            
        }

        public List<string> Symbols { get; set; } = new List<string> { "Vanishing Journey", "Chew Chew", "Lachelein", "Arcana", "Moras","Esfera" };
    }
}
