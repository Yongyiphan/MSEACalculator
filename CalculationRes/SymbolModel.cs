using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes.Database;
using MSEACalculator.OtherRes.Database.Tables;
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

        public int MaxSymbolExp { get; set; }
        public int MaxArcaneForce { get; set; }

        public SymbolModel()
        {
            MaxSymbolExp = CalForm.CalMaxExp(GVar.MaxArcaneSymbolLevel);
            MaxArcaneForce = CalForm.CalArcaneStatsForce(GVar.MaxArcaneSymbolLevel, "General")["ArcaneForce"] * ArcaneList.Count;
        }


    }
}
