using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.CalculationRes.ViewModels
{
    public class QMConversionVM : INPCObject
    {

        /// <summary>
        /// Conversion Types
        /// SGD to Meso
        /// SGD to ACash
        /// 
        /// Mesos to Maple Points
        /// Mesos to SGD (in B or in M)
        /// 
        /// </summary>


        public List<string> ConversionMode{ get => new List<string>(){ "SGD to Meso", "ACash to SGD", "Meso to Maple Points"}; }

        private string _SelectedMode;
        public string SelectedMode
        {
            get => _SelectedMode;
            set
            {
                _SelectedMode = value;
                UpdateInputText();
                ConvertCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedMode));
            }
        }

        private void UpdateInputText()
        {
            switch (SelectedMode)
            {
                case "SGD to Meso":
                    RateText = "SGD/B";
                    MMOnText = "In B";
                    MMOffText = "In Meso";
                    ValueInText = Reverse ? "Meso In" : "SGD In";
                    ValueOutText = Reverse ? "SGD Out" : "Meso Out";
                    break;
                case "ACash to SGD":
                    RateText = "SGD/K";
                    MMOnText = "In K";
                    MMOffText = "";
                    ValueInText = Reverse ? "SGD In" : "Acash In";
                    ValueOutText = Reverse ? "Acash Out" : "SGD Out";
                    break;
                case "Meso to Maple Points":
                    RateText = "Maple Point/100M";
                    MMOnText = "In B";
                    MMOffText = "In Meso";
                    ValueInText = Reverse ? "Maple Points In" : "Meso In";
                    ValueOutText = Reverse ? "Meso Out" : "Maple Points Out";
                    break;
            }

        }
        private bool _Reverse;
        public bool Reverse
        {
            get => _Reverse;
            set
            {
                _Reverse = value;
                UpdateInputText();
                OnPropertyChanged(nameof(Reverse));
            }
        }

        private string _RateText;
        public string RateText
        {
            get => _RateText;
            set
            {
                _RateText = value;
                OnPropertyChanged(nameof(RateText));
            }
        }

        public decimal Rates { get;set; }

        private string _Rate;
        public string Rate
        {
            get => _Rate;
            set
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    (bool, decimal) Result = IsDecimal(value);
                    if (Result.Item1)
                    {
                        _Rate = Result.Item2.ToString();
                        Rates = Result.Item2;
                    }
                    else
                    {
                        ComFunc.ErrorDia("Enter valid rate");
                    }
                }
                ConvertCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(Rate));
            }
        }


        private string _ValueInText;
        public string ValueInText
        {
            get => _ValueInText;
            set
            {
                _ValueInText = value;
                OnPropertyChanged(nameof(ValueInText));
            }
        }

        public decimal InputValue { get; set; }

        private string _ValueIn;
        public string ValueIn
        {
            get => _ValueIn;
            set
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    (bool, decimal) Result = IsDecimal(value);
                    if (Result.Item1)
                    {
                        _ValueIn = Result.Item2.ToString();
                        InputValue = Result.Item2;
                    }
                    else
                    {
                        ComFunc.ErrorDia("Enter valid Value");
                    }
                }

                ConvertCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ValueIn));
            }
        }

        private bool _MesoMod = true;
        public bool MesoMod
        {
            get => _MesoMod; set
            {
                _MesoMod = value;
                if (CanConvert())
                {
                    ConvertMoney();
                }
                OnPropertyChanged(nameof(MesoMod));
            }
        }

        private string _MMOnText;
        public string MMOnText
        {
            get => _MMOnText;
            set
            {
                _MMOnText = value;
                OnPropertyChanged(nameof(MMOnText));
            }
        }
        private string _MMOffText;
        public string MMOffText
        {
            get => _MMOffText;
            set
            {
                _MMOffText = value;
                OnPropertyChanged(nameof(MMOffText));
            }
        }


        private string _ValueOutText;
        public string ValueOutText
        {
            get => _ValueOutText;
            set
            {
                _ValueOutText = value;
                OnPropertyChanged(nameof(ValueOutText));
            }
        }


        private string _ValueOut;
        public string ValueOut
        {
            get => _ValueOut;
            set
            {
                _ValueOut = value;
                OnPropertyChanged(nameof(ValueOut));
            }
        }



        public CustomCommand ConvertCMD { get; set; }
        public CustomCommand ResetCMD { get; set; }
        public QMConversionVM()
        {
            ConvertCMD = new CustomCommand(ConvertMoney, CanConvert);
            ResetCMD = new CustomCommand(() => ResetInput());
            VMInit();

            OnPropertyChanged(nameof(ConversionMode));
        }


        private void VMInit()
        {
            SelectedMode = "SGD to Meso";
            Reverse = false;
            MesoMod = false;
        }

        private bool CanConvert()
        {
            return string.IsNullOrEmpty(SelectedMode) || string.IsNullOrEmpty(Rate) || string.IsNullOrEmpty(ValueIn) ? false : true;
        }

        private void ConvertMoney()
        {
            switch (SelectedMode)
            {
                case "SGD to Meso":
                    SGDtoMeso();
                    break;
                case "ACash to SGD":
                    SGDtoACash();
                    break;
                case "Meso to Maple Points":
                    MesoToMP();
                    break;
            }
        }
        
        
        private void SGDtoMeso()
        {
            //!Reverse -> SGD to Meso
            //Value In = SGD
            //Value Out = Meso (In B/M)
            //SGD/Rates * Multiplier

            //Reverse -> Meso to SGD
            //Value In = Meso
            //Value Out = SGD
            //Meso * Rates / Multiplier
        
            decimal B = Reverse ? MesoMod ? 1 : 1000000000 : Rates;
            decimal C = Reverse ? Rates : MesoMod ? 1 : 1000000000;
            

            decimal Result = (InputValue / B) * C;
            ValueOut = Reverse ? Result.ToString("N2") : MesoMod ? Result.ToString("N9") : Result.ToString("N0");

        }

        private void SGDtoACash()
        {
            //No Reverse = Input / Meso * Rate
            //Reverse = Input / Rate * Meso

            decimal B = Reverse ? Rates : MesoMod ? 1 : 1000;
            decimal C = Reverse ?  MesoMod ? 1 : 1000 : Rates;

            decimal Result = (InputValue / B) * C;
            if (Math.Floor(Result) == 0) 
            {
                Result *= 1000;
            }

            ValueOut = Reverse ? Result.ToString("N0") : Result.ToString("N2"); 

        }

        private void MesoToMP()
        {
            decimal tax = 0.01m;
            //reverse = mp to meso
            // => No Tax
            // => Input / Rate * 100 000 000 
            // --> In M / 1 000 000
            // --> In B / 1 000 000 000
            // Convert to meso then 100m
            if (Reverse)
            {
                decimal Result = Math.Floor(InputValue / Rates) * (decimal)Math.Pow(10, 8);
                Result = MesoMod ? Result /= (decimal) Math.Pow(10, 9) : Result /= (decimal) Math.Pow(10, 0);
                ValueOut = MesoMod ? Result.ToString("N9") : Result.ToString("N0");
            }
            else
            {
                decimal M = MesoMod ? 0.1m : 1000000000;
                decimal Result = Math.Ceiling(InputValue / M * Rates * (1-tax));
                ValueOut = Result.ToString();
            }
            

        }


        private void ResetInput()
        {

        }


        private (bool, decimal Output) IsDecimal(string Input)
        {
            if (decimal.TryParse(Input, out decimal result))
            {
                return (true, result);
            }
            return (false, default(decimal));
        }
    }
}
