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


        private bool _SGDCheck = true;
        public bool SGDCheck
        {
            get { return _SGDCheck; }
            set { _SGDCheck = value;
                if(value)
                {
                    CMode = "SGD";
                }
                OnPropertyChanged(nameof(SGDCheck));
            }
        }

        private bool _BCheck = false;

        public bool BCheck
        {
            get { return _BCheck; }
            set { _BCheck = value;
                if (value)
                {
                    CMode = "B";
                }
                OnPropertyChanged(nameof(BCheck));
            }
        }

        private bool _MesoCheck = false;
        public bool MesoCheck
        {
            get { return _MesoCheck; }
            set { _MesoCheck = value;
                if (value)
                {
                    CMode = "Meso";
                }
                OnPropertyChanged(nameof(MesoCheck));
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

        private string _MoneyOutSGD;
        public string MoneyOutSGD
        {
            get => _MoneyOutSGD;
            set
            {
                _MoneyOutSGD = value;
                OnPropertyChanged(nameof(MoneyOutSGD));
            }
        }
        private string _MoneyOutB;
        public string MoneyOutB
        {
            get => _MoneyOutB;
            set { _MoneyOutB = value;
                OnPropertyChanged(nameof(MoneyOutB));
            }
        }
        private string _MoneyOutMeso;
        public string MoneyOutMeso
        {
            get => _MoneyOutMeso;
            set { _MoneyOutMeso = value;
                OnPropertyChanged(nameof(MoneyOutMeso));
            }
        }


        public CustomCommand ConvertCMD { get; set; }
        public ConversionQMViewModel()
        {
            ConvertCMD = new CustomCommand(ConvertMoney, CanConvert);
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
            MoneyOutB = string.Format("{0:n4}",result["B"]);
            MoneyOutMeso = string.Format("{0:n0}",result["Meso"]);
            ResetInput();

        }



        private void ResetInput()
        {
            MesoRate = string.Empty;
            MoneyIn = string.Empty;
        }
    }
}
