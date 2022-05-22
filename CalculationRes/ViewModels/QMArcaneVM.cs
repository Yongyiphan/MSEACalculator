using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using MSEACalculator.OtherRes.Patterns;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MSEACalculator.CalculationRes.ViewModels
{
    public class QMArcaneVM : INPCObject
    {
        public SymbolModel SymbolM { get; set; } = new SymbolModel();
        public CheckTypes Vali { get; set; } = new CheckTypes();

        public GVarFlex GFlex { get; set; } = Singleton.Instance.GFlex;

        private List<ArcaneSymbolCLS> _SymbolList;
        public List<ArcaneSymbolCLS> SymbolList
        {
            get => _SymbolList;
            set
            {
                _SymbolList = value;
                OnPropertyChanged(nameof(SymbolList));
            }
        }
        


        private ObservableCollection<ArcaneSymbolCLS> _DisplayArcaneSymbolList = new ObservableCollection<ArcaneSymbolCLS>();

        public ObservableCollection<ArcaneSymbolCLS> DisplayArcaneSymbolList
        {
            get { return _DisplayArcaneSymbolList; }
            set { _DisplayArcaneSymbolList = value;
                OnPropertyChanged(nameof(DisplayArcaneSymbolList));
            }
        }

        private ArcaneSymbolCLS _CurrentSymbol;
        public ArcaneSymbolCLS CSymbol
        {
            get => _CurrentSymbol;
            set
            {
                _CurrentSymbol = value;
                if (CSymbol != null)
                {
                    toggleInput(CSymbol);
                    ResetDailyS = CSymbol.IsGainsDaily;

                    switch (GainsType)
                    {
                        case "PQ":
                            ResetPQS = CSymbol.IsGainsPQ;
                            break;
                        case "Flex":
                            PQGains = CSymbol.PQCoins.ToString();
                            break;
                    }
                    
                    ShowSubMap = CSymbol.SubMap == "None" ? Visibility.Collapsed : Visibility.Visible;
                    isSubMap = CSymbol.unlockSubMap;
                    if (SymbolLvls == null || SymbolLvls.Max() != CSymbol.MaxLvl)
                    {
                        SymbolLvls = Enumerable.Range(1, CSymbol.MaxLvl).ToList();
                        OnPropertyChanged(nameof(SymbolLvls));
                    }

                    CLvl = SymbolLvls.Single(x => x == CSymbol.CurrentLevel);
                    CExp = CSymbol.CurrentExp.ToString();
                    AddSymbolCMD.RaiseCanExecuteChanged();
                    DelSymbolCMD.RaiseCanExecuteChanged();


                }
                OnPropertyChanged(nameof(CSymbol));
            }
        }

        //VISIBILITY CONTROL   
        #region

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


        public Visibility ShowCustomLevel { get; set; } = Visibility.Collapsed;
        
        #endregion
        public string GainsType { get; set; } = "";

        private bool _isSubMap = false;
        public bool isSubMap
        {
            get { return _isSubMap; }
            set { _isSubMap = value;
                OnPropertyChanged(nameof(isSubMap));
            }
        }

        public List<int> SymbolLvls { get; set; } 

        private int _CurrentLvl;
        public int CLvl
        {
            get { return _CurrentLvl; }
            set { _CurrentLvl = value;

                if (CSymbol != null)
                {
                    CLimit = CalForm.CalCurrentLimit(CLvl).ToString();

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

        private bool _resetOne = false;

        public bool ResetOne
        {
            get { return _resetOne; }
            set
            {
                _resetOne = value;
                if (ResetOne)
                {
                    CLvlIndex = 0;
                    CExp = String.Empty;
                    ResetOne = false;
                }
                OnPropertyChanged(nameof(ResetOne));
            }
        }


        private string _CurrentExp;
        public string CExp
        {
            get { return _CurrentExp; }
            set { _CurrentExp = value;

                if (Vali.NotInt(value))
                {
                    ComFunc.ErrorDia("Enter valid number");
                    CExp = "1";
                }
                else
                {
                    if (CExp != String.Empty && CSymbol != null)
                    {
                        int tempExp = int.Parse(CExp);
                        if (tempExp > CSymbol.MaxExp || tempExp<1)
                        {
                            ComFunc.ErrorDia(String.Format("Enter number within 1 to {0}",CSymbol.MaxExp));
                            CExp = "1";
                        }
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


        private string _PQGains = "0";
        public string PQGains
        {
            get { return _PQGains; }
            set { _PQGains = value;

                if (Vali.NotInt(PQGains))
                {
                    ComFunc.ErrorDia("Enter valid number");
                    PQGains = "0";
                }
                else
                {
                    if (PQGains !=  String.Empty)
                    {
                        if (int.Parse(PQGains) > CSymbol?.PQGainLimit)
                        {
                            ComFunc.ErrorDia(String.Format("Enter number less than {0}", CSymbol?.PQGainLimit));
                            PQGains = "0";
                        }
                    }
                }
                OnPropertyChanged(nameof(PQGains));
            }
        }

        public string EndType { get; set; } = "Max";

        private int _CustomLvl;

        public int CustomLvl
        {
            get { return _CustomLvl; }
            set
            {
                _CustomLvl = value;
                OnPropertyChanged(nameof(CustomLvl));
            }
        }
        public List<int> CustomLvlList { get; set; } = new List<int>();



        private string _DaysLeft;
        public string DaysLeft
        {
            get => _DaysLeft;
            set
            {
                _DaysLeft = value;
                OnPropertyChanged(nameof(DaysLeft));
            }
        }

        private int _CurrentAF = 0;
        public int CurrentAF
        {
            get => _CurrentAF;
            set
            {
                _CurrentAF = value;
                OnPropertyChanged(nameof(CurrentAF));
            }
        }

        public int TotalAF { get; set;}

        private bool _ArcaenCat;
        public bool ArcaneCatT
        {
            get => _ArcaenCat;
            set
            {
                _ArcaenCat = value;
                
                ReEvalSymbolArcaneCatalyst();
                
                OnPropertyChanged(nameof(ArcaneCatT));
            }
        }

        public List<string> ExtraStatType { get; set; } = new List<string>() { "General", "Xenon", "Demon Avenger" };

        private int _ExtraStatIndex = 0;
        public int ExtraStatIndex
        {
            get => _ExtraStatIndex;
            set
            {
                _ExtraStatIndex = value;
                UpdateDisList();
                OnPropertyChanged(nameof(ExtraStatIndex));
            }
        }

        private int _TotalStat = 0;
        public int TotalStat
        {
            get { return _TotalStat; }
            set { _TotalStat = value;
                OnPropertyChanged(nameof(TotalStat));
            }
        }

        //NOT IMPLEMENTED
        private string _GSkills;
        public string GSkills
        {
            get { return _GSkills; }
            set { _GSkills = value;
                OnPropertyChanged(nameof(GSkills));
            }
        }

        private string _HyperSkills;
        public string HyperSkills
        {
            get => _HyperSkills;
            set
            {
                _HyperSkills= value;
               
                OnPropertyChanged(nameof(HyperSkills));
            }
        }
        //NOT IMPLEMENTED




        public CustomTCommand<string> EndGoal { get; set; }
        public CustomCommand AddSymbolCMD { get; set; }
        public CustomCommand DelSymbolCMD { get; set; }
        public CustomCommand ResetCMD { get; set; }

        public QMArcaneVM()
        {

            InitVar();
            EndGoal = new CustomTCommand<string>((s) => ToggleEG(s), condition: () => { return DisplayArcaneSymbolList.Count > 0 ? true :false; });
            AddSymbolCMD = new CustomCommand(AddSymbol, canExecute: ()=> { return Vali.NotNull(CSymbol) == Vali.NotNull(CExp) == Vali.NotNull(CLvl) == true ? true : false; } );
            DelSymbolCMD = new CustomCommand(DelSymbol, canExecute: () => { return CSymbol != null && DisplayArcaneSymbolList?.Count >0 ? true : false; });
            ResetCMD = new CustomCommand(ResetBtn);
        }

        private void ToggleEG(string TargetLimit)
        {
            ArcaneCatT = false;
            EndType = TargetLimit;

            
            if (TargetLimit == "Custom")
            {
                ShowCustomLevel = Visibility.Visible;
                OnPropertyChanged(nameof(ShowCustomLevel));
                OnPropertyChanged(nameof(SymbolLvls));
            }
            else
            {
                ReEvalSymbolArcaneCatalyst();
            }
        }

       
        public void DelSymbol()
        {
            
            DisplayArcaneSymbolList.RemoveAt(DisplayArcaneSymbolList.ToList().FindIndex(x => x.Name == CSymbol.Name));
            if (DisplayArcaneSymbolList.Count>0)
            {
                UpdateDisList();
            }
            else
            {
                ResetBtn();
            }
            ResetInputFields();
        }


        private void ResetBtn()
        {
            InitVar();
            DisplayArcaneSymbolList.Clear();
            
            ResetInputFields();
        }

        
        private void AddSymbol()
        {
            //CALCULATE MATH TIME.....
            //GET NEW LEVL
            //GET NEW EXP / NEW LIMIT
            //GET DAYS TO COMPLETION
            ArcaneSymbolCLS cSymbol = CSymbol;
            cSymbol.CurrentLevel =  CLvl;
            cSymbol.CurrentExp = int.Parse(CExp);
            decimal dailyGains = ResetDailyS ? Convert.ToDecimal(cSymbol.BaseSymbolGain) : 0;
            cSymbol.IsGainsDaily = ResetDailyS;
            cSymbol.IsGainsPQ = ResetPQS;
            cSymbol.unlockSubMap = isSubMap;

            int mod = isSubMap == true ? 2 : 1;
            dailyGains *= mod;

            switch (GainsType)
            {
                case "PQ":
                    if (ResetPQS)
                    {
                        dailyGains += cSymbol.PQSymbolsGain;
                    }
                    break;
                case "Flex":
                    if (PQGains != null && PQGains != String.Empty)
                    {

                        int PQCoins = int.Parse(PQGains);
                        if (PQCoins > cSymbol.PQGainLimit)
                        {
                            PQCoins = cSymbol.PQGainLimit;
                        }
                        cSymbol.PQCoins =  PQCoins;

                        decimal PQSymbols = Decimal.Divide(PQCoins, cSymbol.SymbolExchangeRate);
                        dailyGains += PQSymbols;
                    }
                    break;
            }

            //CALCULATION BEGIN
            int accExp = CalForm.CalAccEXp(cSymbol.CurrentLevel, cSymbol.CurrentExp);
            cSymbol.SymbolGainRate = dailyGains;
            Dictionary<string, int> CalculatedValues = CalForm.CalNewLvlExp(accExp);


            cSymbol.CurrentLevel = CalculatedValues["NewLevel"];
            cSymbol.CurrentLimit = CalculatedValues["NewLimit"];
            cSymbol.AccumulatedExp = CalculatedValues["CurrentTotalExp"];
            CSymbol.BeforeCatalyst = cSymbol.AccumulatedExp;
            cSymbol.CurrentExp = CalculatedValues["RemainingExp"];

            
            cSymbol.DaysLeft = CalForm.CalDaysLeft(cSymbol.AccumulatedExp, cSymbol.SymbolGainRate, ReEvalExpCap(cSymbol));

            //CALCULATION END
            cSymbol.CostSpent = CalForm.CalCostSymbol(1, cSymbol.CurrentLevel, cSymbol.CostLvlMod, cSymbol.CostMod);
            cSymbol.CostToSpend = CalForm.CalCostSymbol(cSymbol.CurrentLevel, cSymbol.MaxLvl , cSymbol.CostLvlMod, cSymbol.CostMod);

            if (cSymbol.DaysLeft == -1)
            {
                ComFunc.ErrorDia("Impossible to complete symbol. Add method to get more symbols");
            }

            bool added = DisplayArcaneSymbolList.ToList().Any(x => x.Name == cSymbol.Name);
            if (!added)
            {
                DisplayArcaneSymbolList.Add(cSymbol);
            }
            //else
            //{
            //    ObservableCollection<ArcaneSymbolCLS> tempL = new ObservableCollection<ArcaneSymbolCLS>(DisplayArcaneSymbolList);
            //    int symbIndex = tempL.ToList().FindIndex(x => x.Name == cSymbol.Name);
            //    tempL[symbIndex] = cSymbol;
            //    DisplayArcaneSymbolList = new ObservableCollection<ArcaneSymbolCLS>(tempL);

            //}
            UpdateDisList();
            ResetInputFields();


            
        }

        public void InitVar()
        {
            //SymbolLvls = new List<int>();
            //for (int i = 1; i< GVar.MaxArcaneSymbolLevel; i++)
            //{
            //    SymbolLvls.Add(i);
            //}
 
            SymbolList = new List<ArcaneSymbolCLS>(SymbolM.ArcaneList);

            DaysLeft = String.Empty;
            CurrentAF = 0;
            TotalAF = SymbolM.MaxArcaneForce;
            TotalStat = 0;
        }

        private void toggleInput(ArcaneSymbolCLS arcaneSymbol)
        {

            if (arcaneSymbol.PQSymbolsGain != 0)
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


        private void UpdateDisList()
        {
            foreach (ArcaneSymbolCLS symbol in DisplayArcaneSymbolList)
            {
                Dictionary<string, int> dictRec = CalForm.CalArcaneStatsForce(symbol.CurrentLevel, ExtraStatType[ExtraStatIndex]);
                symbol.CurrentAF = dictRec["ArcaneForce"];
                symbol.CurrentAFStat = dictRec["Stat"];

            }

            DateTime today = DateTime.Now;
            int maxDate = DisplayArcaneSymbolList.Select(symbol => symbol.DaysLeft).ToList().Max();
            DaysLeft = String.Format("{0:MM/dd/yyyy} ({1} days)", today.AddDays(maxDate), maxDate);
            CurrentAF = DisplayArcaneSymbolList.Select(symbol => symbol.CurrentAF).ToList().Sum();
            TotalStat = DisplayArcaneSymbolList.Select(symbol => symbol.CurrentAFStat).ToList().Sum();
            DisplayArcaneSymbolList = new ObservableCollection<ArcaneSymbolCLS>(DisplayArcaneSymbolList);
            EndGoal.RaiseCanExecuteChanged();

        }

        private void ReEvalSymbolArcaneCatalyst()
        {
            Dictionary<string, int> dictRec = null;
            List<ArcaneSymbolCLS> tempList = new List<ArcaneSymbolCLS>(DisplayArcaneSymbolList);
            if (DisplayArcaneSymbolList.Count > 0 && DisplayArcaneSymbolList != null)
            {
                if (ArcaneCatT)
                {
                    foreach (ArcaneSymbolCLS symbol in tempList)
                    {
                        if (symbol.CurrentLevel > 1)
                        {
                            symbol.BeforeCatalyst = symbol.AccumulatedExp;
                            int newAccExp = (int)Math.Floor(symbol.AccumulatedExp * 0.8);
                            dictRec = CalForm.CalNewLvlExp(newAccExp);

                            symbol.CurrentLevel = dictRec["NewLevel"];
                            symbol.CurrentLimit = dictRec["NewLimit"];
                            symbol.AccumulatedExp = dictRec["CurrentTotalExp"];
                            symbol.CurrentExp = dictRec["RemainingExp"];
                            symbol.DaysLeft = CalForm.CalDaysLeft(symbol.AccumulatedExp, symbol.SymbolGainRate, ReEvalExpCap(symbol));


                        }
                    }

                }
                else
                {
                    foreach (ArcaneSymbolCLS symbol in tempList)
                    {
                        if (symbol.CurrentLevel > 1)
                        {
                            symbol.AccumulatedExp = symbol.BeforeCatalyst;
                            dictRec = CalForm.CalNewLvlExp(symbol.AccumulatedExp);

                            symbol.CurrentLevel = dictRec["NewLevel"];
                            symbol.CurrentLimit = dictRec["NewLimit"];
                            symbol.AccumulatedExp = dictRec["CurrentTotalExp"];
                            symbol.CurrentExp = dictRec["RemainingExp"];

                        }
                        symbol.DaysLeft = CalForm.CalDaysLeft(symbol.AccumulatedExp, symbol.SymbolGainRate, ReEvalExpCap(symbol));

                    }
                }

            }

            
            UpdateDisList();
        }

        public int ReEvalExpCap(ArcaneSymbolCLS symbol)
        {
            int CCap = 0;
            switch (EndType)
            {
                case "Max":
                    CCap = symbol.MaxExp;
                    break;
                case "Transfer":
                    CCap = symbol.Description == "Arcane" ? GFlex.ArcaneTransferSymbolExp : GFlex.AuthenticTransferSymbolExp;
                    break;
                case "Custom":
                    break;
            }

            return CCap;
        }

        
        private void ResetInputFields()
        {
            CSymbol = null;
            CLvlIndex = 0;
            CExp = String.Empty;
            ResetDailyS = ResetPQS = false;
            isSubMap = false;
            PQGains = String.Empty;
            DelSymbolCMD.RaiseCanExecuteChanged();
            EndGoal.RaiseCanExecuteChanged();
        }
    }

}