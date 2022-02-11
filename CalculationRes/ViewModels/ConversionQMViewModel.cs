using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes.ViewModels
{
    public class ConversionQMViewModel : INPCObject
    {

        public List<string> ConversionMode { get; set; } = new List<string>
        {
            "SGD", "B", "Meso"
        };

        private int _DMode = 0;
        public int DefaultMode
        {
            get=> _DMode;
            set
            {
                _DMode = value;
                OnPropertyChanged(nameof(DefaultMode));
            }
        }

        private string _CMode = "SGD";
        public string CMode
        {
            get { return _CMode; }
            set { _CMode = value;
                OnPropertyChanged(nameof(CMode));
            }

        }

        private string _MesoRate;
        public string MesoRate
        {
            get => _MesoRate;
            set
            {
                _MesoRate = value;
                if (decimal.TryParse(value, out decimal result) == false && value !=  string.Empty)
                {
                    ComFunc.errorDia("Enter valid meso rate");
                }
                OnPropertyChanged(nameof(MesoRate));
            }
        }

        private string _MoneyIn;
        public string MoneyIn
        {
            get { return _MoneyIn; }
            set { _MoneyIn = value;
                if (decimal.TryParse(value, out decimal result) == false && value != string.Empty)
                {
                    ComFunc.errorDia("Enter value number");
                }
                ConvertCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(MoneyIn));
            }
        }

        private string _MoneyOutSGD = "0";
        public string MoneyOutSGD
        {
            get => _MoneyOutSGD;
            set
            {
                _MoneyOutSGD = value;
                OnPropertyChanged(nameof(MoneyOutSGD));
            }
        }
        private string _MoneyOutMeso = "0";
        public string MoneyOutMeso
        {
            get => _MoneyOutMeso;
            set { _MoneyOutMeso = value;
                OnPropertyChanged(nameof(MoneyOutMeso));
            }
        }


        public CustomCommand ConvertCMD { get; set; }
        public CustomCommand ResetCMD { get; set; }
        public ConversionQMViewModel()
        {
            ConvertCMD = new CustomCommand(ConvertMoney, CanConvert);
            ResetCMD = new CustomCommand(() => ResetInput());
        }

        private bool CanConvert()
        {
            if (decimal.TryParse(MoneyIn,out decimal Iresult) && decimal.TryParse(MesoRate, out decimal Rresult))
            {
                return true;
            }
            return false;
        }

        private void ConvertMoney()
        {


            Dictionary<string, decimal> result = CalForm.CalMesoConversion(decimal.Parse(MesoRate), decimal.Parse(MoneyIn), CMode);
            MoneyOutSGD = string.Format("{0:N2}", result["SGD"]);
            MoneyOutMeso = string.Format("{0:n0}",result["Meso"]);
            

        }



        private void ResetInput()
        {
            MesoRate = string.Empty;
            MoneyIn = string.Empty;
            DefaultMode = 0;
            MoneyOutSGD = "0";
            MoneyOutMeso = "0";
        }
    }
}
