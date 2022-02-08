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

        private List<string> _SymbolNameList;
        public List<string> SymbolNameList
        {
            get { return _SymbolNameList; }
            set { _SymbolNameList = value; }
        }


        private ObservableCollection<ArcaneSymbol> _DisplayArcaneSymbolList = new ObservableCollection<ArcaneSymbol>();

        public ObservableCollection<ArcaneSymbol> DisplayArcaneSymbolList
        {
            get { return _DisplayArcaneSymbolList; }
            set { _DisplayArcaneSymbolList = value; }
        }

        private ArcaneSymbol _CurrentSymbol;
        public ArcaneSymbol CSymbol
        {
            get => _CurrentSymbol;
            set
            {
                _CurrentSymbol = value;
                OnPropertyChanged(nameof(CSymbol));
            }
        }

        private string _SArcaneSymbol;
        public string SArcaneSymbol
        {
            get { return _SArcaneSymbol; }
            set { _SArcaneSymbol = value;

                if (ComFunc.notNULL(SArcaneSymbol)) {
                    CSymbol = FindSymbol(SArcaneSymbol);
                    toggleInput(CSymbol);
                    ResetDailyS = ResetPQS = false;
                    ShowSubMap = CSymbol.SubMap == null ? Visibility.Collapsed : Visibility.Visible;
                    CLvl = SymbolLvls.Single(x => x == CSymbol.CurrentLevel);
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
                OnPropertyChanged(nameof(isSubMap));
            }
        }

        public List<int> SymbolLvls { get; set; } = new List<int>();

        private int _CurrentLvl;
        public int CLvl
        {
            get { return _CurrentLvl; }
            set { _CurrentLvl = value;

                if (ComFunc.notNULL(SArcaneSymbol))
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


        private string _CurrentExp;
        public string CExp
        {
            get { return _CurrentExp; }
            set { _CurrentExp = value;

                if (int.TryParse(value, out int result) == false && value != String.Empty)
                {
                    ComFunc.errorDia("Enter valid number");
                    CExp = "1";
                }
                else
                {
                    if (CExp != String.Empty)
                    {
                        int tempExp = int.Parse(CExp);
                        if (tempExp > GVar.MaxSymbolExp || tempExp<1)
                        {
                            ComFunc.errorDia(String.Format("Enter number within 1 to {0}", GVar.MaxSymbolExp));
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

        public string TotalSymbolExp { get; set; } = GVar.MaxSymbolExp.ToString();


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

                if (ComFunc.IsInt(PQGains) == false)
                {
                    ComFunc.errorDia("Enter valid number");
                    PQGains = "0";
                }
                else
                {
                    if (PQGains !=  String.Empty)
                    {
                        if (int.Parse(PQGains) > CSymbol?.PQGainLimit)
                        {
                            ComFunc.errorDia(String.Format("Enter number less than {0}", CSymbol?.PQGainLimit));
                            PQGains = "0";
                        }
                    }
                }
                OnPropertyChanged(nameof(PQGains));
            }
        }


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

        private string _CurrentAF = "0";
        public string CurrentAF
        {
            get => _CurrentAF;
            set
            {
                _CurrentAF = value;
                OnPropertyChanged(nameof(CurrentAF));
            }
        }

        public string TotalAF { get; set; } = GVar.MaxArcaneForce.ToString();

        private bool _ArcaenCat;
        public bool ArcaneCatT
        {
            get => _ArcaenCat;
            set
            {
                _ArcaenCat = value;
                ReEvalSymbol();
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
                OnPropertyChanged(nameof(ExtraStatIndex));
            }
        }

        private string _TotalStat;
        public string TotalStat
        {
            get { return _TotalStat; }
            set { _TotalStat = value;
                OnPropertyChanged(nameof(TotalStat));
            }
        }

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


        public CustomCommand AddSymbolCMD { get; set; }
        public CustomCommand ResetCMD { get; set; }

        public ArcaneQMViewModel()
        {

            InitVar();
            AddSymbolCMD = new CustomCommand(AddSymbol, CanAddSymbol);
            ResetCMD = new CustomCommand(ResetBtn);
        }

        private void ResetBtn()
        {
            InitVar();
            DisplayArcaneSymbolList.Clear();
            ResetInputFields();
            CurrentAF = 0.ToString();
        }

        private bool CanAddSymbol()
        {
            if (ComFunc.notNULL(SArcaneSymbol) == ComFunc.notNULL(CExp) == ComFunc.notNULL(CLvl) == true)
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
            ArcaneSymbol cSymbol = FindSymbol(SArcaneSymbol);
            cSymbol.CurrentLevel =  CLvl;
            cSymbol.CurrentExp = int.Parse(CExp);
            int dailyGains = ResetDailyS ? cSymbol.BaseSymbolGain : 0;
            cSymbol.unlockSubMap = isSubMap;
            int mod = isSubMap == true ? 2 : 1;
            dailyGains *= mod;

            switch (GainsType)
            {
                case "PQ":
                    dailyGains += cSymbol.PartyQuestSymbols;
                    break;
                case "Flex":
                    if (PQGains != null)
                    {

                        int tempGain = int.Parse(PQGains);
                        if (tempGain > cSymbol.PQGainLimit)
                        {
                            tempGain = cSymbol.PQGainLimit;
                        }

                        if (tempGain%cSymbol.SymbolExchangeRate == 0)
                        {
                            tempGain/=cSymbol.SymbolExchangeRate;
                        }
                        else
                        {
                            tempGain /= cSymbol.SymbolExchangeRate-1;

                        }
                        dailyGains += tempGain;
                    }
                    break;
            }

            int accExp = CalForm.CalAccEXp(cSymbol.CurrentLevel, cSymbol.CurrentExp);
            cSymbol.SymbolGainRate = dailyGains;
            Dictionary<string, int> CalculatedValues = CalForm.CalNewLvlExp(accExp, dailyGains);

            cSymbol.DaysLeft = CalculatedValues["DaysLeft"];
            cSymbol.CurrentLevel = CalculatedValues["NewLevel"];
            cSymbol.CurrentLimit = CalculatedValues["NewLimit"];
            cSymbol.AccumulatedExp = CalculatedValues["CurrentTotalExp"];
            cSymbol.CurrentExp = CalculatedValues["RemainingExp"];

            //if (DisplayArcaneSymbolList == null)
            //{
            //    DisplayArcaneSymbolList.Add(SArcaneSymbol);
            //}

            bool added = DisplayArcaneSymbolList.ToList().Any(x => x.Name == cSymbol.Name);
            if (!added)
            {
                DisplayArcaneSymbolList.Add(cSymbol);
            }
            else
            {
                int symbIndex = DisplayArcaneSymbolList.ToList().FindIndex(x => x.Name == cSymbol.Name);
                DisplayArcaneSymbolList[symbIndex] = cSymbol;
            }

            UpdateDisList();
            ResetInputFields();
        }

        public void InitVar()
        {
            SymbolLvls = new List<int>();
            for (int i = 1; i< GVar.MaxSymbolLvl; i++)
            {
                SymbolLvls.Add(i);
            }

            SymbolNameList = new List<string>();
            foreach (ArcaneSymbol symbol in GVar.Symbols)
            {
                SymbolNameList.Add(symbol.Name);
            }

        }

        private void toggleInput(ArcaneSymbol arcaneSymbol)
        {

            if (arcaneSymbol.PartyQuestSymbols != 0)
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
            foreach (ArcaneSymbol symbol in DisplayArcaneSymbolList)
            {
                Dictionary<string, int> dictRec = CalForm.CalArcaneStatsForce(symbol.CurrentLevel, ExtraStatType[ExtraStatIndex]);
                symbol.CurrentAF = dictRec["ArcaneForce"];
                symbol.CurrentAFStat = dictRec["Stat"];

            }

            DateTime today = DateTime.Now;
            int maxDate = DisplayArcaneSymbolList.Select(symbol => symbol.DaysLeft).ToList().Max();
            DaysLeft = String.Format("{0:MM/dd/yyyy} ({1} days)", today.AddDays(maxDate), maxDate);
            CurrentAF = DisplayArcaneSymbolList.Select(symbol => symbol.CurrentAF).ToList().Sum().ToString();
            TotalStat = DisplayArcaneSymbolList.Select(symbol => symbol.CurrentAFStat).ToList().Sum().ToString();

        }

        private void ReEvalSymbol()
        {
            Dictionary<string, int> dictRec = null;
            if (ArcaneCatT == true)
            {
                if (DisplayArcaneSymbolList.Count > 0)
                {
                    List<ArcaneSymbol> tempList = new List<ArcaneSymbol>(DisplayArcaneSymbolList);
                    foreach (ArcaneSymbol symbol in tempList)
                    {
                        if (symbol.CurrentLevel > 1)
                        {
                            symbol.BeforeCatalyst = symbol.AccumulatedExp;
                            int newAccExp = (int)Math.Floor(symbol.AccumulatedExp * 0.8);
                            dictRec = CalForm.CalNewLvlExp(newAccExp, symbol.SymbolGainRate);
                            symbol.DaysLeft = dictRec["DaysLeft"];
                            symbol.CurrentLevel = dictRec["NewLevel"];
                            symbol.CurrentLimit = dictRec["NewLimit"];
                            symbol.AccumulatedExp = dictRec["CurrentTotalExp"];
                            symbol.CurrentExp = dictRec["RemainingExp"];

                            int fIndex = tempList.FindIndex(x => x.Name == symbol.Name);
                            DisplayArcaneSymbolList[fIndex] = symbol;
                        }       
                    }
                }
            }
            else if (ArcaneCatT == false)
            {
                if (DisplayArcaneSymbolList.Count > 0)
                {
                    List<ArcaneSymbol> tempList = new List<ArcaneSymbol>(DisplayArcaneSymbolList);
                    foreach (ArcaneSymbol symbol in tempList)
                    {
                        symbol.AccumulatedExp = symbol.BeforeCatalyst;
                        dictRec = CalForm.CalNewLvlExp(symbol.AccumulatedExp, symbol.SymbolGainRate);

                        symbol.DaysLeft = dictRec["DaysLeft"];
                        symbol.CurrentLevel = dictRec["NewLevel"];
                        symbol.CurrentLimit = dictRec["NewLimit"];
                        symbol.AccumulatedExp = dictRec["CurrentTotalExp"];
                        symbol.CurrentExp = dictRec["RemainingExp"];

                        int fIndex = tempList.FindIndex(x => x.Name == symbol.Name);
                        DisplayArcaneSymbolList[fIndex] = symbol;
                    }
                }


            }
        }

        private ArcaneSymbol FindSymbol(string name)
        {
            ArcaneSymbol Asymbol = null;
            foreach (ArcaneSymbol symbol in new List<ArcaneSymbol>(GVar.Symbols))
            {
                if (symbol.Name == name)
                {

                    return Asymbol = symbol.ShallowCopy();
                }
            }

            return Asymbol;
        }

        private void ResetInputFields()
        {
            SArcaneSymbol = null;
            CLvlIndex = -1;
            CExp = String.Empty;
            ResetDailyS = ResetPQS = false;
            isSubMap = false;
        }
    }

}