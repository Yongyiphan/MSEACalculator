using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes.ViewModel
{
    public class ArcaneQMViewModel : INPCObject
    {


        private ArcaneSymbol _VJ = GVar.Symbols[0];

        public ArcaneSymbol VJ
        {
            get { return _VJ; }
            set { _VJ = value;

                OnPropertyChanged(nameof(VJ));
            }
        }

        private ArcaneSymbol _ChuChu = GVar.Symbols[1];
        public ArcaneSymbol ChuChu
        {
            get { return _ChuChu; }
            set { _ChuChu = value; }
        }

        private ArcaneSymbol _Lachelein = GVar.Symbols[2];
        public ArcaneSymbol Lachelein
        {
            get { return _Lachelein; }
            set { _Lachelein = value; }
        }

        private ArcaneSymbol _Arcana = GVar.Symbols[3];
        public ArcaneSymbol MyProperty
        {
            get { return _Arcana; }
            set { _Arcana = value; }
        }

        private ArcaneSymbol _Moras = GVar.Symbols[4];
        public ArcaneSymbol Moras
        {
            get { return _Moras; }
            set { _Moras = value; }
        }

        private ArcaneSymbol _Esfera = GVar.Symbols[5];
        public ArcaneSymbol Esfera
        {
            get { return _Esfera; }
            set { _Esfera = value; }
        }

        private int _ExtraStatT = 0;

        public int ExtraStatT
        {
            get { return _ExtraStatT; }
            set { _ExtraStatT = value; OnPropertyChanged(nameof(ExtraStatT)); }
        }


        public ArcaneQMViewModel()
        {

        }

        public void CalculateSymbol(ArcaneSymbol sym)
        {

        }

    }
}
