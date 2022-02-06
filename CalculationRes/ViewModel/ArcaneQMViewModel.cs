using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MSEACalculator.CalculationRes.ViewModel
{
    public class ArcaneQMViewModel : INPCObject
    {

        private List<ArcaneSymbol> _ArcaneSymbolList = new List<ArcaneSymbol>(GVar.Symbols);
        public List<ArcaneSymbol> ArcaneSymbolList
        {
            get { return _ArcaneSymbolList; }
            set { _ArcaneSymbolList = value; }
        }


        private ObservableCollection<ArcaneSymbol> _DisplayArcaneSymbolList;

        public ObservableCollection<ArcaneSymbol> DisplayArcaneSymbolList
        {
            get { return _DisplayArcaneSymbolList; }
            set { _DisplayArcaneSymbolList = value; }
        }


        private ArcaneSymbol _SArcaneSymbol;

        public ArcaneSymbol SArcaneSymbol
        {
            get { return _SArcaneSymbol; }
            set { _SArcaneSymbol = value;

                if (ComFunc.notNULL(SArcaneSymbol)){
                    toggleInput();
                    ResetDailyS = ResetPQS = false;
                    ShowSubMap = SArcaneSymbol.SubMap == null ? Visibility.Collapsed : Visibility.Visible;
                    CLvl = SymbolLvls.Single(x => x == SArcaneSymbol.CurrentLevel);
                    CExp = "1";


                }
                OnPropertyChanged(nameof(SArcaneSymbol));
            }
        }

        private Visibility _ShowPQ = Visibility.Collapsed;

        public Visibility ShowPQ
        {
            get { return _ShowPQ; }
            set { _ShowPQ = value;
                OnPropertyChanged(nameof(ShowPQ));
            }
        }

        private Visibility _ShowFlex = Visibility.Collapsed;

        public Visibility ShowFlex
        {
            get { return _ShowFlex; }
            set { _ShowFlex = value;
                OnPropertyChanged(nameof(ShowFlex));
            }
        }

        private Visibility _ShowSubMap = Visibility.Collapsed;

        public Visibility ShowSubMap
        {
            get { return _ShowSubMap; }
            set { _ShowSubMap = value;
                OnPropertyChanged(nameof(ShowSubMap));
            }
        }

        public string GainsType { get; set; } = "";

        private bool _isSubMap = false;
        public bool isSubMap
        {
            get { return _isSubMap; }
            set { _isSubMap = value;
                if (ComFunc.notNULL(isSubMap) && ComFunc.notNULL(SArcaneSymbol))
                {
                    
                    SArcaneSymbol.unlockSubMap =  isSubMap;
                }
                OnPropertyChanged(nameof(isSubMap));
            }
        }

        public List<int> SymbolLvls { get; set; } = new List<int>();

        private int _CurrentLvl;
        public int CLvl
        {
            get { return _CurrentLvl;}
            set { _CurrentLvl = value;

                if (ComFunc.notNULL(SArcaneSymbol))
                {
                    CLimit = CalForm.CalCurrentLimit(CLvl).ToString();
                    SArcaneSymbol.CurrentLevel = CLvl;
                }

         

                OnPropertyChanged(nameof(CLvl));
            }
        }


        private int _CLvlIndex = -1;
        public int CLvlIndex
        {
            get { return _CLvlIndex; }
            set { _CLvlIndex = value;
                OnPropertyChanged(nameof(CLvlIndex));
            }
        }


        private string _CurrentExp;
        public string CExp
        {
            get { return _CurrentExp; }
            set { _CurrentExp = value;

                if (int.TryParse(value, out int result) == false && value != String.Empty)
                {
                    CExp = "1";
                    ComFunc.errorDia("Enter valid number");
                    CExp = "1";
                }
                else
                {
                    if (ComFunc.notNULL(SArcaneSymbol) && value != String.Empty)
                    {
                        SArcaneSymbol.CurrentExp = int.Parse(CExp);
                    }
                }
                
                OnPropertyChanged(nameof(CExp));
            }
        }



        private string _CurrentLimit;
        public string CLimit
        {
            get { return _CurrentLimit; }
            set
            {
                _CurrentLimit = value;
                OnPropertyChanged(nameof(CLimit));
            }
        }




        private bool _ResetDailyS = false;
        public bool ResetDailyS
        {
            get { return _ResetDailyS; }
            set { _ResetDailyS = value;
                OnPropertyChanged(nameof(ResetDailyS));
            }
        }


        private bool _ResetPQS = false;
        public bool ResetPQS
        {
            get { return _ResetPQS; }
            set {
                _ResetPQS = value;
                OnPropertyChanged(nameof(ResetPQS));
            }
        }

        private string _PQGains;

        public string PQGains
        {
            get { return _PQGains; }
            set { _PQGains = value;

                if (int.TryParse(value, out int result) == false && value != String.Empty)
                {
                    
                    ComFunc.errorDia("Enter valid number");
                    PQGains = "0";
                }
                OnPropertyChanged(nameof(PQGains));
            }
        }


        public CustomCommand AddSymbolCMD { get; set; }

        public ArcaneQMViewModel()
        {
            
            InitVar();
            AddSymbolCMD = new CustomCommand(AddSymbol, CanAddSymbol);
        }

        private bool CanAddSymbol()
        {
            if(ComFunc.notNULL(SArcaneSymbol) == ComFunc.notNULL(CExp) == ComFunc.notNULL(CLvl) == true)
            {
                return true;
            }
            return false;
        }

        private void AddSymbol()
        {
            //CALCULATE MATH TIME.....
            //GET NEW LEVL
            //GET NEW EXP / NEW LIMIT
            //GET DAYS TO COMPLETION

            int dailyGains = ResetDailyS ? SArcaneSymbol.BaseSymbolGain : 0;
            
            switch(GainsType)
            {
                case "PQ":
                    dailyGains += SArcaneSymbol.PartyQuestSymbols;
                    break;
                case "Flex":
                    int tempGain = int.Parse(PQGains)/SArcaneSymbol.SymbolExchangeRate;

                    dailyGains += tempGain;
                    break;
            }



            Dictionary<string, int> CalculatedValues = CalForm.CalNewLvlExp(SArcaneSymbol.CurrentLevel, SArcaneSymbol.CurrentExp, dailyGains, SArcaneSymbol.unlockSubMap);

            


        }

        public void InitVar()
        {
            for (int i = 1;i< GVar.MaxSymbolLvl; i++)
            {
                SymbolLvls.Add(i);
            }
        }

        public void CalculateSymbol(ArcaneSymbol sym)
        {

        }


        private void toggleInput()
        {

            if (SArcaneSymbol.PartyQuestSymbols != 0)
            {
                ShowPQ = Visibility.Visible;
                ShowFlex = Visibility.Collapsed;
                GainsType = "PQ";
            }
            else
            {
                ShowFlex = Visibility.Visible;
                ShowPQ = Visibility.Collapsed;
                GainsType = "Flex";
            }

        }
        
    }
}
