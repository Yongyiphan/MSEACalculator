using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes.Database;
using MSEACalculator.OtherRes.Database.Tables;
using MSEACalculator.OtherRes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes
{
    public class SymbolModel
    {
        public List<ArcaneSymbolCLS>  ArcaneList { get => SymbolsTable.GetAllArcaneSymbolDB(); }


        public int MaxArcaneForce { get; set; }

        public SymbolModel()
        {
            MaxArcaneForce = CalForm.CalArcaneStatsForce(Singleton.Instance.GFlex.MaxArcaneSymbolLevel)["ArcaneForce"] * ArcaneList.Count;
        }


    }
}
