using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.OtherRes;
using MSEACalculator.OtherRes.Database.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MSEACalculator.CalculationRes.ViewModels
{
    public class StatInputValue
    {
        public string Stat { get; set; }

        private string _Value;
        public string Value
        {
            get => _Value;
            set
            {
                _Value = value;
                if (int.TryParse(value, out int IOutput))
                {
                    if (IOutput != 0)
                    {
                        Debug.WriteLine("Value Updated");
                    }
                }
                else if(double.TryParse(value, out double DOutput))
                {
                    if(DOutput != 0.0)
                    {
                        Debug.WriteLine("Value Updated");

                    }
                }
                else
                {
                    ComFunc.ErrorDia("Enter a number");
                }
            }
        }
    }


    public class QMEquipStatVM : INPCObject
    {
        public EquipStatModel ESModel { get; set; } = new EquipStatModel();
        public ScrollingModelCLS ScrollM { get; set; } = new ScrollingModelCLS();

        public CustomCommand AddEquipmentCMD { get; private set; }
        public CustomCommand DelEquipmentCMD { get; private set; }
        public CustomCommand ClearEquipmentCMD { get; private set; }
        public CustomCommand CalScrollCMD { get; private set; }

        private string _AddUpdateBtnText = "Add Equip";
        public string AddUpdateBtnText
        {
            get => _AddUpdateBtnText;
            set
            {
                _AddUpdateBtnText = value;
                OnPropertyChanged(nameof(AddUpdateBtnText));
            }
        }


        public EquipCLS CurrentEquip { get; set; }

        public CustomCommand TestBtn { get; private set; }
        public QMEquipStatVM()
        {

            AddEquipmentCMD = new CustomCommand(AddEquip, CanAddEquip);
            DelEquipmentCMD = new CustomCommand(DelEquip, CanDelEquip);
            ClearEquipmentCMD = new CustomCommand(ClearEquip, CanClearEquip);
            CalScrollCMD = new CustomCommand(CalScroll, CanCalScroll);
            VMInit();

            TestBtn = new CustomCommand(ActivateTest);
        }

        private void ActivateTest()
        {
            SESlot = "Pendant";
            SortByClass = false;
            ClassFilter = "Any";
            SEquip = EquipList.Find(x => x.EquipName == "Dominator Pendant");
            SSF = 15;
            DisplayType = false;
            //List<int> TestValues = new List<int>() {40,65,40,40,255, 0, 0, 119};

            //for(int i = 0; i < TestValues.Count; i++)
            //{
            //    StatInput.ElementAt(i).Value = TestValues[i].ToString();
            //}
            if (CanAddEquip())
            {
                AddEquip();
            }
            
        }

        public List<string> EquipSlotList { get; set; } = new List<string>();

        private string _SEquipSlot;
        public string SESlot
        {
            get => _SEquipSlot;
            set
            {
                _SEquipSlot = value;
                EquipList.Clear();
                if (value != null)
                {
                    ClassFilter = "None";
                    SMainStat = "None";
                    FilterEquipName();
                    if (SESlot != "Weapon")
                    {
                        if (PercList.Contains("15%") && IsSpellTrace)
                        {
                            PercList.Remove("15%");
                        }
                        if (MainStatList.Contains("All Stat") == false)
                        {
                            MainStatList.Add("All Stat");
                        }
                    }
                    else
                    {
                        if (PercList.Contains("15%") == false)
                        {
                            PercList.Add("15%");
                        }
                        if (MainStatList.Contains("All Stat"))
                        {
                            MainStatList.Remove("All Stat");
                        }
                    }
                    

                    OnPropertyChanged(nameof(MainStatList));
                    OnPropertyChanged(nameof(PercList));

                }

                OnPropertyChanged(nameof(SESlot));
            }
        }

        private bool _SortByClass = false;
        public bool SortByClass
        {
            get => _SortByClass;
            set
            {
                _SortByClass = value;

                //If True, Sort By Class Name, i.e Wind Archer:
                if (SortByClass)
                {
                    ClassFilterText = "Class Name: ";
                    ClassFilterList = ESModel.CharacterStore.Keys.ToList();
                }
                //If False, Sort By Class Type, i.e Bowman:
                else
                {
                    ClassFilterText = "Class Type: ";
                    ClassFilterList = ESModel.ClassJob.Keys.ToList();
                    //MSIndex = -1;
                }
                ClassFilter = "None";
                SMainStat = "None";

                OnPropertyChanged(nameof(ClassFilterText));
                OnPropertyChanged(nameof(ClassFilterList));

                OnPropertyChanged(nameof(SortByClass));
            }
        }

        public string ClassFilterText { get; set; } = "Class Type: ";
        public List<string> ClassFilterList { get; set; } = new List<string>();

        private string _ClassFilter = "None";
        public string ClassFilter
        {
            get => _ClassFilter;
            set
            {
                _ClassFilter = value;
                if (value != null)
                {
                    //SortByClass ? Class Name : Class Type
                    if (SortByClass && value != "None")
                    {
                        SMainStat = value == "Xenon" ? "None" : ESModel.CharacterStore[ClassFilter].MainStat;
                    }
                    else
                    {
                        //Set MainStat As "None" When Xenon
                        SMainStat = ComFunc.ReturnMainStat(ClassFilter);
                    }
                    FilterEquipName();
                }

                OnPropertyChanged(nameof(ClassFilter));
            }
        }


        public List<string> MainStatList { get; set; }

        private int _MSIndex;
        public int MSIndex
        {
            get => _MSIndex;
            set
            {
                _MSIndex = value;
                OnPropertyChanged(nameof(MSIndex));
            }
        }

        private string _SMainStat;
        public string SMainStat
        {
            get => _SMainStat;
            set
            {
                _SMainStat = value;
                //if (value != null || string.IsNullOrEmpty(value))
                //{
                //    if (value == "None")
                //    {
                //        MSIndex = -1;
                //    }
                //    FilterEquipName();
                //}
                AddEquipmentCMD.RaiseCanExecuteChanged();
                CalScrollCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SMainStat));
            }
        }


        public List<EquipCLS> EquipList { get; set; } = new List<EquipCLS>();

        private EquipCLS _SEquip;

        public EquipCLS SEquip
        {
            get => _SEquip; 
            set
            {
                _SEquip = value;

                if (value != null)
                {
                    AddUpdateBtnText = CItemList.ToList().Exists(x => x.EquipName == SEquip.EquipName) ? "Update Equip" : "Add Equip";

                    GenerateStarforceRange();
                    UpgradeSlots.Clear();
                    if (SEquip.BaseStats.NoUpgrades >0)
                    {
                        Enumerable.Range(0, SEquip.BaseStats.NoUpgrades + 3).ToList().ForEach(x => UpgradeSlots.Add(x.ToString()));
                    }
                    else
                    {
                        UpgradeSlots.Add("0");
                    }

                                    }
                //END OFF HERE, CONTINUE WITH DISCARDED CITEMSELECT IF SEQUIP CHANGES

                AddEquipmentCMD.RaiseCanExecuteChanged();
                CalScrollCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SEquip));
            }
        }

        public List<int> StarforceRange { get; set; } = new List<int>();

        private int _SSF;
        public int SSF
        {
            get => _SSF;
            set
            {
                _SSF = value;
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SSF));
            }
        }

        public Dictionary<string, int> ScrollRecord { get; set; } = new Dictionary<string, int>();

        public List<string> UpgradeSlots { get; set; } = new List<string>();

        private bool _IsSpellTrace = true;
        public bool IsSpellTrace
        {
            get => _IsSpellTrace;
            set
            {
                _IsSpellTrace = value;

                if (DisplayType)
                {
                    SlotStatText = "Slot";
                    SlotOrStatList = UpgradeSlots;
                    ShowSlot = Visibility.Collapsed;
                    ShowStatValue = Visibility.Collapsed;

                    OnPropertyChanged(nameof(SlotStatText));
                    OnPropertyChanged(nameof(SlotOrStatList));
                }
                else
                {
                    SlotStatText = IsSpellTrace ? "Slot" : "Stat";
                    SSlotStat = null;
                    SlotOrStatList = IsSpellTrace ? UpgradeSlots : GVar.BaseStatTypes;

                    ShowSlot = IsSpellTrace ? Visibility.Visible : Visibility.Collapsed;
                    ShowStatValue = IsSpellTrace ? Visibility.Collapsed : Visibility.Visible;
                    OnPropertyChanged(nameof(SlotStatText));
                    OnPropertyChanged(nameof(SlotOrStatList));
                }
               

                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(IsSpellTrace));
            }
        }

        public string SlotStatText { get; set; } = "Slot";
        public List<string> SlotOrStatList { get; set; }

        private string _SSlotStat;
        public string SSlotStat
        {
            get => _SSlotStat;
            set
            {
                _SSlotStat = value;
                if (IsSpellTrace && (value == "0" || string.IsNullOrEmpty(value)))
                {
                    SPercIndex = -1;
                }
                AddEquipmentCMD.RaiseCanExecuteChanged();
                CalScrollCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SSlotStat));
            }
        }

        private int _SSSIndex;
        public int SSSIndex
        {
            get => _SSSIndex;
            set
            {
                _SSSIndex = value;
                OnPropertyChanged(nameof(SSSIndex));
            }
        }

        //Spell Trace Percentage Selection
        #region
        public List<string> PercList { get => ScrollM.SpellTracePercTypes; }

        private string _SPerc;
        public string SPerc
        {
            get => _SPerc;
            set
            {
                _SPerc = value;
                if (IsSpellTrace && (SSlotStat == "0" || string.IsNullOrEmpty(SSlotStat)))
                {
                    SPercIndex = -1;
                }
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SPerc));
            }
        }

        private int _SPercIndex;
        public int SPercIndex
        {
            get => _SPercIndex;
            set
            {
                _SPercIndex = value;
                OnPropertyChanged(nameof(SPercIndex));
            }
        }

        #endregion

        private string _ScrollValue;
        public string ScrollValue
        {
            get => _ScrollValue;
            set
            {
                _ScrollValue = value;
                if (int.TryParse(ScrollValue, out int intResult))
                {
                    AddScrollRecord();
                }
                else
                {
                    ComFunc.ErrorDia("Enter a valid number.");
                    _ScrollValue = "";
                }
                OnPropertyChanged(nameof(ScrollValue));
            }
        }

        private Visibility _ShowSlot;       
        public Visibility ShowSlot
        {
            get => _ShowSlot; set
            {
                _ShowSlot = value;
                OnPropertyChanged(nameof(ShowSlot));
            }
        }

        private Visibility _ShowStatValue;         
        public Visibility ShowStatValue
        {
            get => _ShowStatValue; set
            {
                _ShowStatValue = value;
                OnPropertyChanged(nameof(ShowStatValue));
            }
        }


        private bool _DisplayType;
        public bool DisplayType
        {
            get => _DisplayType;
            set
            {
                _DisplayType = value;
                //Find Scroll Perc
                IsSpellTrace = true;
                if (DisplayType)
                {
                    DisplayScrollCal = Visibility.Visible;
                    DisplaySimEquip = Visibility.Collapsed;
                }
                //Simulate Equip
                else
                {
                    DisplayScrollCal = Visibility.Collapsed;
                    DisplaySimEquip = Visibility.Visible;

                }
                OnPropertyChanged(nameof(DisplayType));
            }
        }



        //Display Fields for Calculating Scrolling
        private Visibility _DisplayScrolLCal;
        public Visibility DisplayScrollCal
        {
            get => _DisplayScrolLCal; set
            {
                _DisplayScrolLCal = value;
                OnPropertyChanged(nameof(DisplayScrollCal));
            }
        }

        public ObservableCollection<StatInputValue> StatInput { get; set; } = new ObservableCollection<StatInputValue>();
        private StatInputValue _ScrollStatInput;
        public StatInputValue ScrollStatInput
        {
            get { return _ScrollStatInput; }
            set
            {
                _ScrollStatInput = value;
                OnPropertyChanged(nameof(ScrollStatInput));
            }
        }


        //Display Simulated Equips with Starforce/Scroll
        private Visibility _DisplaySimEquip;
        public Visibility DisplaySimEquip
        {
            get => _DisplaySimEquip; set
            {
                _DisplaySimEquip = value;
                OnPropertyChanged(nameof(DisplaySimEquip));
            }
        }

        public ObservableCollection<EquipCLS> CItemList { get; set; } = new ObservableCollection<EquipCLS>();

        private EquipCLS _CItemSelect;
        public EquipCLS CItemSelect
        {
            get => _CItemSelect;
            set
            {
                _CItemSelect = value;

                if (CItemSelect != null)
                {
                    CurrentEquip = CItemSelect;
                    SESlot = value.EquipSlot;
                    SortByClass = false;
                    ClassFilter = value.ClassType;
                    if (value.MainStat != SMainStat)
                    {
                        SMainStat = value.MainStat;
                    }
                    SEquip = EquipList.Find(x => x.EquipName == value.EquipName);

                    //Starforce

                    //Spell Trace
                    if (value.IsSpellTraced && value.SlotCount > 0)
                    {
                        IsSpellTrace = value.IsSpellTraced;
                        SSlotStat = value.SlotCount.ToString();
                        SPerc = string.Format("{0}%", value.SpellTracePerc);
                    }
                    ScrollRecord = value.ScrollStats.ToRecord();
                    SSF = value.StarForce;
                    GatherDisplay();

                    //Display Stats

                }

                DelEquipmentCMD.RaiseCanExecuteChanged();

                OnPropertyChanged(nameof(CItemSelect));
            }
        }

        private Dictionary<string, DisplayStatValue> _TotalRecordDisplay;
        public Dictionary<string, DisplayStatValue> TotalRecordDisplay
        {
            get => _TotalRecordDisplay;
            set
            {
                _TotalRecordDisplay = value;
                OnPropertyChanged(nameof(TotalRecordDisplay));
            }
        }

        private bool _SyncCI = false;
        public bool SyncCI
        {
            get => _SyncCI;
            set
            {
                _SyncCI = value;
                OnPropertyChanged(nameof(SyncCI));
            }
        }


        public void VMInit()
        {
            EquipSlotList = ESModel.EquipmentStore.Keys.ToList();
            SortByClass = false;
            DisplayType = false;
            StatInput.Clear();
            GVar.MainStats.Concat(new List<string>() { "MaxHP", "ATK", "MATK", "DEF"}).ToList().ForEach(x => StatInput.Add(new StatInputValue() { Stat=x, Value="0" }));
            MainStatList = GVar.MainStats.Concat(new List<string>() { "HP", "All Stat", "None" }).ToList();
            IsSpellTrace = true;
        }

        public void FilterEquipName()
        {
            if (SESlot != null)
            {

                if (ClassFilter == null || ClassFilter == "None")
                {

                    EquipList = ESModel.EquipmentStore[SESlot].OrderBy(x => x.EquipName).ToList();

                }
                //Filter By Class
                else
                {
                    List<EquipCLS> FilterByClass = new List<EquipCLS>();
                    List<string> FilterList = new List<string>() { ClassFilter, "Any", "None" };

                    //SortByClass ? Class Name : ClassType
                    if (SortByClass)
                    {
                        FilterList.Add(ESModel.CharacterStore[ClassFilter].ClassType);
                        FilterList.Add(ESModel.CharacterStore[ClassFilter].Faction);
                        ESModel.CharacterStore[ClassFilter].MainWeapon.ForEach(x => FilterList.Add(x));
                        ESModel.CharacterStore[ClassFilter].SecondaryWeapon.ForEach(x => FilterList.Add(x));
                    }
                    else
                    {
                        ESModel.ClassJob[ClassFilter].ForEach(x => { if (FilterList.Contains(x) == false) FilterList.Add(x); });

                        foreach (CharacterCLS Char in ESModel.CharacterStore.Values)
                        {
                            if (FilterList.Contains(Char.Faction))
                            {
                                continue;
                            }
                            FilterList.Add(Char.Faction);
                        }

                    }
                    if (ClassFilter == "Xenon")
                    {
                        FilterList.Add("Pirate");
                        FilterList.Add("Thief");
                    }


                    if (SESlot == "Weapon" || SESlot == "Secondary")
                    {
                        foreach (EquipCLS equip in ESModel.EquipmentStore[SESlot])
                        {
                            if (FilterList.Contains(equip.WeaponType))
                            {
                                FilterByClass.Add(equip);
                            }
                        }
                    }
                    else
                    {
                        foreach (EquipCLS equip in ESModel.EquipmentStore[SESlot])
                        {
                            
                            if (FilterList.Contains(equip.ClassType)   || equip.ClassType == null)
                            {
                                FilterByClass.Add(equip);
                            }

                        }


                    }
                    EquipList = FilterByClass.OrderBy(x => x.EquipName).ToList();
                }
            }
            OnPropertyChanged(nameof(EquipList));
        }

        public void GenerateStarforceRange()
        {
            StarforceRange.Clear();
            string EquipType = ComFunc.ReturnNormalOrSuperiorTitle(SEquip.EquipName);
            (int, int) RangeKey = ComFunc.ReturnSFRange(ESModel.SFLimits[EquipType].Keys.ToList(), SEquip);

            StarforceRange = RangeKey != (0, 0) ? Enumerable.Range(0, ESModel.SFLimits[EquipType][RangeKey] + 1).ToList() : Enumerable.Range(0, 0).ToList();
            OnPropertyChanged(nameof(StarforceRange));
        }

        private void AddScrollRecord()
        {
            if (ScrollRecord.ContainsKey(SSlotStat))
            {
                ScrollRecord[SSlotStat] =  Convert.ToInt32(ScrollValue);
            }
            else
            {
                ScrollRecord.Add(SSlotStat, Convert.ToInt32(ScrollValue));
            }
        }

        private bool CanAddEquip()
        {
            List<bool> Conditions = new List<bool>();

            bool Check = false;
            Check = SEquip == null ? false : true;
            Conditions.Add(Check);
            bool MainStatEmpty = (SMainStat == "None"  || string.IsNullOrEmpty(SMainStat)) ? true : false;
            if (SSF > 0)
            {
                Check = MainStatEmpty ? false : true;
            }
            Conditions.Add(Check);
            //Equip !=  null
            //if Starforce == 0 dont need check
            //if spell traced
            // => if slot == 0 -> Reset Slot Perc, Dont check mainstat
            //if not spell traced -> no need check
            Check = false;
            if (IsSpellTrace)
            {
                int SlotCount = Convert.ToInt32(SSlotStat);
                if (SlotCount > 0 && SPercIndex != -1 && !MainStatEmpty)
                {
                    Check = true;
                }
                if (string.IsNullOrEmpty(SSlotStat) || SlotCount == 0)
                {
                    Check = true;
                }
            }
            Conditions.Add(Check);

            if (!DisplayType)
                SyncCI = CItemList.Count == 0 ? true : false;


            return Conditions.Contains(false) ? false : true;

        }
        private void AddEquip()
        {
            //Flow
            //Copy Current Selected equip
            CurrentEquip = SEquip.DeepCopy();
            CurrentEquip.MainStat = SMainStat;


            //Calculate Scroll Stats
            if (IsSpellTrace)
            {
                int SlotCount = Convert.ToInt32(SSlotStat);
                CurrentEquip.SlotCount = SlotCount;
                CurrentEquip.IsSpellTraced = IsSpellTrace;
                if (SlotCount > 0)
                {

                    CurrentEquip.SpellTracePerc = Convert.ToInt32(SPerc.Trim('%'));
                    string SlotType = ComFunc.ReturnScrollCat(SESlot);
                    string ClassType = SortByClass ? ESModel.CharacterStore[ClassFilter].ClassType : ClassFilter;
                    CurrentEquip.ScrollStats = CalForm.NewCalSpellTrace(CurrentEquip, SlotType, SMainStat, ClassType);
                }
            }
            else
            {
                CurrentEquip.ScrollStats.DictToProperty(ScrollRecord);
            }



            //Calculate Starforce Stats
            CurrentEquip.StarForce = SSF;

            string CT = ClassFilter == "Xenon" ? "Xenon" : SortByClass ? ESModel.CharacterStore[ClassFilter].ClassType : ClassFilter;
            string SFType = ComFunc.ReturnNormalOrSuperiorTitle(CurrentEquip.EquipName);

            CurrentEquip.StarforceStats = CalForm.NewCalStarforceStats(CurrentEquip, ESModel.StarforceStore[SFType].Values.ToList().AsReadOnly(), CT, SMainStat, SFType);
            //Add to Dict List
            //Record By EquipName
            List<EquipCLS> CurrentList = CItemList.ToList();

            if (CurrentList.Exists(x => x.EquipName == CurrentEquip.EquipName))
            {
                int FoundIndex = CurrentList.FindIndex(x => x.EquipName == CurrentEquip.EquipName);

                CItemList[FoundIndex] = CurrentEquip;
                CItemSelect = CurrentEquip;
            }
            else
            {
                CItemList.Add(CurrentEquip);
                ResetInputs();
            }



            //Discard Current equip
            CurrentEquip = null;

            AddEquipmentCMD.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(CItemList));
        }


        private bool CanDelEquip()
        {
            List<bool> Conditions = new List<bool>();
            bool Check = false;
            Check = CItemSelect == null ? false : true;
            Conditions.Add(Check);

            return Conditions.Contains(false) ? false : true;
        }
        private void DelEquip()
        {
            int TargetIndex = CItemList.ToList().FindIndex(x => x.EquipName == CItemSelect.EquipName);
            CItemList.RemoveAt(TargetIndex);
            CItemSelect = null;
            ResetInputs();
            TotalRecordDisplay.Clear();
        }


        private void ClearEquip()
        {
            CItemList.Clear();
            CItemSelect = null;
        }
        private bool CanClearEquip()
        {
            return CItemList.Count > 0 ? true : false;
        }


        private bool CanCalScroll()
        {
            List<bool> Conditions = new List<bool>();

            bool Check = false;
            Check = SEquip == null ? false : true;
            Conditions.Add(Check);

            Check = SMainStat == "None" || string.IsNullOrEmpty(SMainStat) ? false : true;
            Conditions.Add(Check);
            
            //Cannot be Empty, If Spell traced, slot cannot be 0
            Check = string.IsNullOrEmpty(SSlotStat) || (IsSpellTrace && Convert.ToInt32(SSlotStat) == 0) ? false : true;
            Conditions.Add(Check);

            return Conditions.Contains(false) ? false : true;

        }
        private void CalScroll()
        {
            //Base + Scroll + SF = Total
            //Base + Blue = Total

            CurrentEquip = SEquip.DeepCopy();
            CurrentEquip.IsSpellTraced = IsSpellTrace;
            CurrentEquip.SlotCount = Convert.ToInt32(SSlotStat);

            EquipStatsCLS CurrentTotal = CurrentEquip.BaseStats.ShallowCopy();
            EquipStatsCLS BlueStat = new EquipStatsCLS();
            BlueStat.DictToProperty(StatInput.ToDictionary(x => x.Stat, x => Convert.ToInt32(x.Value)));

            //
            CurrentTotal.ModifyEquipStat(BlueStat, "Add");

            if (SSF > 0)
            {

                CurrentEquip.StarForce = SSF;

                string CT = ClassFilter == "Xenon" ? "Xenon" : SortByClass ? ESModel.CharacterStore[ClassFilter].ClassType : ClassFilter;
                string SFType = ComFunc.ReturnNormalOrSuperiorTitle(CurrentEquip.EquipName);

                CurrentTotal = CalForm.ReverseStarforceStats(CurrentEquip, CurrentTotal, ESModel.StarforceStore[SFType].Values.ToList().AsReadOnly(), CT, SMainStat, SFType);
                
            }
            CurrentTotal.ModifyEquipStat(CurrentEquip.BaseStats, "Subtract");
            (List<StatInputValue>, string) FinalResult = CalForm.CalScrolling(CurrentEquip, CurrentTotal, StatInput.ToList().Select(x => x.Stat).ToList(),SMainStat);

            ScrollCalResult.Clear();
            FinalResult.Item1.ForEach(x => ScrollCalResult.Add(x));
            PercResult = FinalResult.Item2;

        }

        private string _PercResult;
        public string PercResult
        {
            get => _PercResult;
            set
            {
                _PercResult = value;
                OnPropertyChanged(nameof(PercResult));
            }
        }
        public ObservableCollection<StatInputValue> ScrollCalResult { get; set; } = new ObservableCollection<StatInputValue>();

        private void ResetInputs()
        {
            SESlot = null;
            ClassFilter = "None";
            MSIndex = -1;
            SMainStat = null;
            SEquip = null;

            SSlotStat = null;
            SPercIndex = -1;

        }
        private void GatherDisplay()
        {

            //Dictionary<string, List<string>> ToDisplay = new Dictionary<string, List<string>>();

            Dictionary<string, DisplayStatValue> ToDisplay = new Dictionary<string, DisplayStatValue>();
            Dictionary<string, int> BaseStat = CurrentEquip.BaseStats.ToRecord();
            Dictionary<string, int> ScrollStat = CurrentEquip.ScrollStats.ToRecord();
            Dictionary<string, int> SFStat = CurrentEquip.StarforceStats.ToRecord();

            foreach (string key in BaseStat.Keys)
            {
                DisplayStatValue temp = new DisplayStatValue();
                temp.DBaseStat   = BaseStat[key].ToString();
                temp.DScrollStat = ScrollStat[key].ToString();
                temp.DStarforceStat = SFStat[key].ToString();
                temp.Key = key;


                if (temp.ReturnTotal() > 0)
                {
                    temp.ReturnTotal();
                    ToDisplay.Add(key, temp);
                }
            }
            TotalRecordDisplay = ToDisplay;

        }





    }
}
