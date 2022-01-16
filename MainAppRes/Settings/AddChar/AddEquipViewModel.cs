using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.MainAppRes.Settings;
using MSEACalculator.OtherRes;
using MSEACalculator.OtherRes.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MSEACalculator.MainAppRes.Settings.AddChar
{
    public class AddEquipViewModel : INPCObject
    {
        /// <summary>
        /// CHECKLIST FOR THIS FUNCTION
        /// ADDING CHARACTER TO DB => DONE
        /// DELETING CHARACTER FROM DB => DONE
        /// 
        /// SELECT ITEM SLOT => DONE
        /// SELECT ITEM SET/ ITEM NAME => DONE
        /// INPUT ITEM SCROLL STATS => DONE
        /// INPUT FLAME STATS => DONE
        /// 
        /// INSERT ITEM TO ITEMlIST/DICT
        /// ADDING ITEMLIST/DICT TO DB
        /// 
        /// </summary>

        public readonly AddCharTrackViewModel ACharTrackVM;

        public AddEquipModel AEquipM { get; set; } = new AddEquipModel();
        public ScrollingModel ScrollModel { get; set; } = new ScrollingModel();


        /// <summary>
        /// RETRIEVING DATA FROM MODEL
        /// </summary>

        //KEY: EquipSlot | VALUE: Equip Category
        public Dictionary<string, string> EquipSlots { get => AEquipM.EquipSlot; }
        public List<string> FlameStatsTypes { get => AEquipM.FlameStatsTypes; }

        public List<string> PotentialGrade { get => GVar.PotentialGrade; }
        public List<int> Slots { get => ScrollModel.Slots; } //= new Scrolling().Slots;
        public List<string> SlotSet { get; set; } = new List<string>
        {
            "Weapon", "Gloves", "Armor"
        };
        public List<string> WSE { get; set; } = new List<string>
        {
            "Weapon", "Secondary", "Emblem"
        };

        /// <summary>
        /// INITIALISE EMPTY COLLECTIONS
        /// </summary>

        private ObservableCollection<string> _ArmorSet = new ObservableCollection<string>();
        public ObservableCollection<string> ArmorSet
        {
            get { return _ArmorSet; }
            set
            {
                _ArmorSet = value;
                OnPropertyChanged(nameof(ArmorSet));
            }
        }

        public List<EquipModel> CurrentEquipList { get; set; } = new List<EquipModel>();

        private List<string> _CharWeapon;
        public List<string> CharacterWeapon
        {
            get => _CharWeapon;
            set
            {
                _CharWeapon = value;
                OnPropertyChanged(nameof(CharacterWeapon));
            }
        }

        public List<string> _StatTypes;
        public List<string> StatTypes
        {
            get { return _StatTypes; }
            set
            {
                _StatTypes = value;
                OnPropertyChanged(nameof(StatTypes));
            }
        }
        public Dictionary<string, int> ScrollRecord { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FlameRecord { get; set; } = new Dictionary<string, int>();

        private ObservableCollection<PotentialStats> _FirstPotL  = new ObservableCollection<PotentialStats>();
        public ObservableCollection<PotentialStats> FirstPotL
        {
            get =>  _FirstPotL;
            set
            {
                _FirstPotL = value;
                OnPropertyChanged(nameof(FirstPotL));
            }

        }
        private ObservableCollection<PotentialStats> _SecondPotL = new ObservableCollection<PotentialStats>();
        public ObservableCollection<PotentialStats> SecondPotL
        {
            get => _SecondPotL;
            set
            {
                _SecondPotL =  value;
                OnPropertyChanged(nameof(SecondPotL));
            }
        }
        private ObservableCollection<PotentialStats> _ThirdPotL = new ObservableCollection<PotentialStats>();
        public ObservableCollection<PotentialStats> ThirdPotL
        {
            get { return _ThirdPotL; }
            set { _ThirdPotL = value;
                OnPropertyChanged(nameof(ThirdPotL));
            }
        }

        public ObservableCollection<EquipModel> CItemDictT { get; set; } = new ObservableCollection<EquipModel>();






        public AddEquipViewModel(AddCharTrackViewModel aCharTrackVM)
        {
            
            ACharTrackVM = aCharTrackVM;
            SCharacter = ACharTrackVM?.SelectedAllChar;
            initFields();
            ACharTrackVM.RaiseChangeChar += HandleCharChange;
            AddScrollCMD = new CustomCommand(AddStat, canAddStat);
            AddFlameCMD = new CustomCommand(AddFlame, canAddFlame);

            AddEquipmentCMD = new CustomCommand(AddItem, canAddItem);
        }

        void HandleCharChange(object sender, CharTStore CC)
        {

            if (CC.CurrentCharacter !=  SCharacter)
            {
                SCharacter = CC.CurrentCharacter;

                initFields();
            }
            
            
        }

        /// <summary>
        /// VISIBILITY CONTROLS
        /// </summary>

        private Visibility _ShowWeapon = Visibility.Collapsed;
        public Visibility ShowWeapon
        {
            get { return _ShowWeapon; }
            set
            {
                _ShowWeapon = value;
                OnPropertyChanged(nameof(ShowWeapon));
            }
        }

        private Visibility _ShowSlot = Visibility.Collapsed;
        public Visibility ShoWSlot
        {
            get
            {
                return _ShowSlot;
            }
            set
            {
                _ShowSlot = value;
                OnPropertyChanged(nameof(ShoWSlot));
            }
        }

        private Visibility _ShowScrollValue = Visibility.Collapsed;
        public Visibility ShowScrollValue
        {
            get { return _ShowScrollValue; }
            set
            {
                _ShowScrollValue = value;
                OnPropertyChanged(nameof(ShowScrollValue));
            }
        }

        private Visibility _ShowSEquipStat = Visibility.Collapsed;

        public Visibility ShowSEquipStat
        {
            get { return _ShowSEquipStat; }
            set
            {
                _ShowSEquipStat = value;
                OnPropertyChanged(nameof(ShowSEquipStat));
            }
        }

        private Visibility _ifWeapon = Visibility.Collapsed;

        public Visibility ifWeapon
        {
            get { return _ifWeapon; }
            set { _ifWeapon = value;
                OnPropertyChanged(nameof(ifWeapon));
            }
        }




        /// <summary>
        /// VARIABLES
        /// </summary>
        private Character _SCharacter;
        public Character SCharacter
        {
            get => _SCharacter;
            set
            {
                _SCharacter = value;
                OnPropertyChanged(nameof(SCharacter));
            }

        }



        private string _SEquipSlot;
        public string SEquipSlot
        {
            get { return _SEquipSlot; }
            set
            {
                _SEquipSlot = value;

                ItemDisType = SEquipSlot != null && SlotSet.Contains(EquipSlots[SEquipSlot]) ? "Set: " : "Name: ";
                


                ShowWeapon = SEquipSlot != null
                    && EquipSlots[SEquipSlot] == "Weapon" ? Visibility.Visible : Visibility.Collapsed;
                if (SEquipSlot != null)
                {
                    ShowEquipSet(SEquipSlot);
                    IsSpellTrace = false;
                    NoSlot = 0;
                    SelectedScrollStat = null;
                    ScrollRecord.Clear();
                    FlameRecord.Clear();
                }
                

                OnPropertyChanged(nameof(SEquipSlot));
            }
        }

        private string _SSetItem;
        public string SSetItem
        {
            get { return _SSetItem; }
            set
            {
                _SSetItem = value;
                AddEquipmentCMD.RaiseCanExecuteChanged();

                GetCurrentEquipment();
                FirstPotL = RetrievePot();
                
                OnPropertyChanged(nameof(SSetItem));
            }
        }

        private EquipModel _CurrentEquipment = new EquipModel();

        public EquipModel CurrentSEquip
        {
            get { return _CurrentEquipment; }
            set { _CurrentEquipment = value;
                OnPropertyChanged(nameof(CurrentSEquip));
            }
        }
            


        //EITHER "SET" OR "EQUIPNAME"
        private string _ItemDisType = "Set: ";
        public string ItemDisType
        {
            get { return _ItemDisType; }
            set
            {
                _ItemDisType = value;
                OnPropertyChanged(nameof(ItemDisType));
            }
        }

        private string _SelectedWeapon;
        public string SelectedWeapon
        {
            get { return _SelectedWeapon; }
            set
            {
                _SelectedWeapon = value;
                if (SCharacter !=  null)
                {
                    SCharacter.CurrentMainWeapon = SelectedWeapon;
                }
                OnPropertyChanged(nameof(SelectedWeapon));
            }
        }

        private ListViewItem _FrameSelection;
        public ListViewItem FrameSelection
        {
            get { return _FrameSelection; }
            set
            {
                _FrameSelection = value;
                toDisplayFrame(FrameSelection.Content.ToString());
                OnPropertyChanged(nameof(FrameSelection));

            }
        }
        public Frame FrameDis { get; set; } = new Frame();

        private int _NoSlot;
        public int NoSlot
        {
            get { return _NoSlot; }
            set
            {
                _NoSlot = value;
                if (NoSlot == 0)
                {
                    SelectedScrollStat = null;
                }
                OnPropertyChanged(nameof(NoSlot));
            }
        }

        private bool _IsSpellTrace;
        public bool IsSpellTrace
        {
            get
            {
                return _IsSpellTrace;
            }
            set
            {
                _IsSpellTrace = value;
                ShoWSlot = IsSpellTrace ? Visibility.Visible : Visibility.Collapsed;
                ShowScrollValue = IsSpellTrace ? Visibility.Collapsed : Visibility.Visible;
                ScrollTypeTxt = IsSpellTrace ? "Perc:" : "Stat:";
                StatTypes = IsSpellTrace ? ScrollModel.SpellTraceTypes : GVar.BaseStatTypes;
                OnPropertyChanged(nameof(IsSpellTrace));
            }
        }



        private string _ScrollTypeTxt = "Stat:";
        public string ScrollTypeTxt
        {
            get { return _ScrollTypeTxt; }
            set
            {
                _ScrollTypeTxt = value;
                OnPropertyChanged(nameof(ScrollTypeTxt));
            }
        }

        private int _SelectedScrollIndex;
        public int SelectedScrollIndex
        {
            get { return _SelectedScrollIndex; }
            set
            {
                _SelectedScrollIndex = value;
                OnPropertyChanged(nameof(SelectedScrollIndex));
            }
        }

        private string _SelectedScrollStat;
        public string SelectedScrollStat
        {
            get { return _SelectedScrollStat; }
            set
            {
                _SelectedScrollStat = value;

                if (IsSpellTrace)
                {
                    if (SelectedScrollStat != null)
                    {
                        if (NoSlot == 0)
                        {
                            CommonFunc.errorDia("Slot cannot be 0.");
                            SelectedScrollIndex = -1;

                        }
                    }
                }
                else
                {
                    if (SelectedScrollStat != null)
                    {

                        ShowScrollValue = ScrollModel.SpellTraceTypes.Contains(SelectedScrollStat) ? Visibility.Collapsed : Visibility.Visible;

                        if (ScrollRecord.ContainsKey(SelectedScrollStat))
                        {
                            ScrollStatValue = ScrollRecord[SelectedScrollStat].ToString();

                        }
                        else
                        {
                            ScrollStatValue = string.Empty;
                        }

                    }
                }
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedScrollStat));
            }
        }

        private string _ScrollStatvalue = string.Empty;
        public string ScrollStatValue
        {
            get { return _ScrollStatvalue; }
            set
            {
                _ScrollStatvalue = value;

                AddScrollCMD.RaiseCanExecuteChanged();
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ScrollStatValue));
            }
        }

        private string _SelectedFlame;
        public string SelectedFlame
        {
            get { return _SelectedFlame; }
            set
            {
                _SelectedFlame = value;
                if (SelectedFlame != null)
                {

                    if (FlameRecord.ContainsKey(SelectedFlame))
                    {
                        FlameStat = FlameRecord[SelectedFlame].ToString();

                    }
                    else
                    {
                        FlameStat = string.Empty;
                    }

                }
                OnPropertyChanged(nameof(SelectedFlame));
            }
        }

        private string _FlameStat = string.Empty;
        public string FlameStat
        {
            get { return _FlameStat; }
            set
            {
                _FlameStat = value;
                AddFlameCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(FlameStat));
            }
        }

        private int _SPotentialG;
        public int SPotentialG
        {
            get { return _SPotentialG; }
            set { _SPotentialG = value;
                if (SPotentialG != -1)
                {
                    FirstPot =  null;
                    SecondPot = null;
                    ThirdPot = null;
                    FirstPotL = RetrievePot();
                }


                OnPropertyChanged(nameof(SPotentialG));
            }
        }

        private PotentialStats _FirstPot;
        public PotentialStats FirstPot
        {
            get { return _FirstPot; }
            set { _FirstPot = value; }
        }

        private PotentialStats _SecondPot;
        public PotentialStats SecondPot
        {
            get { return _SecondPot; }
            set { _SecondPot = value; }
        }

        private PotentialStats _ThirdPot;
        public PotentialStats ThirdPot
        {
            get { return _ThirdPot; }
            set { _ThirdPot = value; }
        }




        private bool _SyncCI = false;
        public bool SyncCI
        {
            get { return _SyncCI; }
            set
            {
                _SyncCI = value;
                OnPropertyChanged(nameof(SyncCI));
            }
        }


        private EquipModel _CItemSelect;
        public EquipModel CItemSelect
        {
            get { return _CItemSelect; }
            set
            {
                _CItemSelect = value;
                ShowSEquipStat = CItemSelect != null ? Visibility.Visible : Visibility.Collapsed;
                if (CItemSelect != null)
                {
                    ifWeapon = CItemSelect.EquipSlot == "Weapon" ? Visibility.Visible : Visibility.Collapsed;
                    SEquipSlot = CItemSelect.EquipSlot;
                    SSetItem = AEquipM.AccGrp.Contains(EquipSlots[SEquipSlot]) ? CItemSelect.EquipName : CItemSelect.EquipSet;
                    IsSpellTrace = CItemSelect.SpellTraced;
                    if (CItemSelect.SpellTraced)
                    {
                        NoSlot = CItemSelect.SlotCount;
                        SelectedScrollIndex = CItemSelect.SpellTracePerc;
                    }
                    ScrollRecord = CommonFunc.propertyToRecord(CItemSelect.ScrollStats, ScrollRecord);
                    FlameRecord = CommonFunc.propertyToRecord(CItemSelect.FlameStats, FlameRecord);
                }
                OnPropertyChanged(nameof(CItemSelect));

            }
        }

        private string testVar = "";

        public string TestVar
        {
            get { return testVar; }
            set
            {
                testVar = value;
                OnPropertyChanged(nameof(TestVar));
            }
        }

        public CustomCommand AddScrollCMD { get; private set; }
        public CustomCommand AddFlameCMD { get; private set; }

        public CustomCommand AddEquipmentCMD { get; private set; }

        /// <summary>
        /// FUNCTIONS
        /// </summary>

        private void initFields()
        {

            StatTypes = GVar.BaseStatTypes;
            if (SCharacter == null)
            {
                CharacterWeapon = new List<string>();
            }
            else
            {
                //CharacterWeapon.Clear();
                //SCharacter.MainWeapon.ForEach(weapon => CharacterWeapon.Add(weapon));
                CharacterWeapon = new List<string>();
                ArmorSet = ArmorSet != null ? new ObservableCollection<string>() : ArmorSet;
                SEquipSlot = SEquipSlot!=null ? null : SEquipSlot;
                CharacterWeapon = SCharacter.MainWeapon;
            }
        }

        private bool canAddStat()
        {
            bool canAdd = false;
            //Check if stat is selected
            //Check if int value is keyed into value textbox
            if (SelectedScrollStat != null && ScrollStatValue != null)
            {
                if (int.TryParse(ScrollStatValue.ToString(), out int value))
                {
                    canAdd = true;
                }
                else
                {
                    if (ScrollStatValue != string.Empty)
                    {
                        CommonFunc.errorDia("Invalid");
                        canAdd = false;
                    }
                }
            }

            return canAdd;
        }
        private void AddStat()
        {
            //Stat selected, Int value inserted
            ScrollRecord[SelectedScrollStat] = int.Parse(ScrollStatValue);

            SelectedScrollStat = null;
            AddEquipmentCMD.RaiseCanExecuteChanged();
        }


        private bool canAddFlame()
        {
            bool canAdd = false;
            //Flame Stat selected
            if (SelectedFlame != null)
            {
                //Check Value is INT
                if (int.TryParse(FlameStat, out int value))
                {
                    canAdd = true;
                }
                else
                {
                    if (FlameStat != string.Empty)
                    {
                        canAdd = false;
                    }
                }
            }
            return canAdd;
        }
        private void AddFlame()
        {
            FlameRecord[SelectedFlame] = int.Parse(FlameStat);
            SelectedFlame = null;
            AddEquipmentCMD.RaiseCanExecuteChanged();
        }

        private void ShowEquipSet(string selectedESlot)
        {
            Func<string,string, ObservableCollection<string>, List<EquipModel>, ObservableCollection<string>>
                FilterSet = (classtype, eSlot, displayList, itemList) =>
                {

                    foreach (var item in itemList)
                    {
                        if (item.EquipSlot == eSlot && item.ClassType == classtype)
                        {
                            if (!displayList.Contains(item.EquipSet))
                            {
                                displayList.Add(item.EquipSet);
                            }
                        }
                    }


                    return displayList;
                };

            Func<string, ObservableCollection<string>, List<EquipModel>, ObservableCollection<string>>
                DisplayItemNameL = (eSlot, displayList, itemList) =>
                {

                    foreach (var item in itemList)
                    {
                        if (item.EquipSlot == eSlot)
                        {
                            if (!displayList.Contains(item.EquipName))
                            {
                                displayList.Add(item.EquipName);
                            }
                        }
                    }

                    return displayList;
                };
            Func<string,Character, ObservableCollection<string>, List<EquipModel>, ObservableCollection<string>>
                FilterWeapon = (disType, character, displayList, itemList) =>
                {
                    switch (disType)
                    {
                        case "Weapon":
                            foreach (var item in itemList)
                            {
                                foreach (string weap in character.MainWeapon)
                                {
                                    if (item.WeaponType == weap)
                                    {
                                        if (!displayList.Contains(item.EquipSet))
                                        {
                                            displayList.Add(item.EquipSet);
                                        }
                                    }
                                }
                            }
                            break;
                        case "Secondary":
                            foreach (var item in itemList)
                            {
                                foreach (string weap in character.SecondaryWeapon)
                                {
                                    if (weap == "Shield")
                                    {
                                        if (item.WeaponType == weap && item.ClassType == character.ClassType)
                                        {
                                            if (!displayList.Contains(item.EquipName))
                                            {
                                                displayList.Add(item.EquipName);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.WeaponType == weap)
                                        {
                                            if (!displayList.Contains(item.EquipName))
                                            {
                                                displayList.Add(item.EquipName);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    

                    return displayList;
                };
            if (SCharacter != null)
            {
                switch (EquipSlots[selectedESlot])
                {
                    case "Weapon":
                        ArmorSet.Clear();
                        ArmorSet = FilterWeapon("Weapon", SCharacter, ArmorSet, AEquipM.AllWeapList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllWeapList;
                        break;
                    case "Secondary":
                        ArmorSet.Clear();
                        ArmorSet = FilterWeapon("Secondary", SCharacter, ArmorSet, AEquipM.AllSecList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllSecList;
                        break;
                    case "Gloves":
                        ArmorSet.Clear();
                        ArmorSet = FilterSet(SCharacter.ClassType, selectedESlot, ArmorSet, AEquipM.AllArmorList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllArmorList;
                        break;
                    case "Armor":
                        ArmorSet.Clear();
                        ArmorSet = FilterSet(SCharacter.ClassType, selectedESlot, ArmorSet, AEquipM.AllArmorList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllArmorList;
                        break;
                    case "Accessory":
                        ArmorSet.Clear();
                        ArmorSet = DisplayItemNameL(selectedESlot, ArmorSet, AEquipM.AllAccList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllAccList;
                        break;
                    case "Ring":
                        ArmorSet.Clear();
                        ArmorSet = DisplayItemNameL(EquipSlots[selectedESlot], ArmorSet, AEquipM.AllAccList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllAccList;
                        break;
                    case "Pendant":
                        ArmorSet.Clear();
                        ArmorSet = DisplayItemNameL(EquipSlots[selectedESlot], ArmorSet, AEquipM.AllAccList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllAccList;
                        break;
                    case "Misc":
                        ArmorSet.Clear();
                        ArmorSet = DisplayItemNameL(selectedESlot, ArmorSet, AEquipM.AllAccList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllAccList;
                        break;
                    default:
                        ArmorSet.Clear();
                        CurrentEquipList.Clear();
                        break;
                }


            }

        }


        private bool canAddItem()
        {
            Func<bool, string, Dictionary<string, int>, Dictionary<string, int>, bool>
                checkAddStatAdded = (isSpellTraced, scrollStat, scrollRecord, flameRecord) =>
                {
                    //Spell trace, check slots, spell trace perc
                    //check scrollrecord, flamerecords
                    if (isSpellTraced)
                    {
                        //scrollstat null == slotcount = 0
                        if (scrollStat != null && flameRecord.Count >= 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //if flameRecord = 0 && scrollRecord = 0 == clean equip, no flame, no scroll
                        if (flameRecord.Count >= 0 && scrollRecord.Count >= 0)
                        {
                            return true;
                        }
                    }

                    return false;
                };

            if (SCharacter != null)
            {
                if (SEquipSlot != null && SSetItem != null)
                {
                    if (SEquipSlot == "Weapon" && SelectedWeapon != null)
                    {
                        return checkAddStatAdded(IsSpellTrace, SelectedScrollStat, ScrollRecord, FlameRecord);
                    }
                    return checkAddStatAdded(IsSpellTrace, SelectedScrollStat, ScrollRecord, FlameRecord);
                }
            }
            SyncCI = CItemDictT.Count == 0 ? true : false;

            return false;
        }

        private void AddItem()
        {

            Character selectedChar = SCharacter;
            List<string> cat = new List<string>() { "Ring", "Pendant" };
            string currentSSlot = cat.Contains(EquipSlots[SEquipSlot]) ? EquipSlots[SEquipSlot] : SEquipSlot;
           

            //Blank Equp
            EquipModel selectedEquip = new EquipModel();
            List<EquipModel> EList = CurrentEquipList;



            //Retreive base equip stats from list
            selectedEquip = CommonFunc.FindEquip(EList, selectedChar, currentSSlot, SSetItem);
            //if (namingType == 0) { selectedEquip.EquipName = string.Format("{0} {1}", SSetItem, SEquipSlot); }
            selectedEquip.EquipSlot = SEquipSlot; //<- override value of Selected slot i.e ring1... pendant1...
            selectedEquip.SlotCount = NoSlot;
            //Assign base stats to correct property
            selectedEquip = CommonFunc.updateBaseStats(selectedChar, selectedEquip);
            selectedEquip.SpellTraced = IsSpellTrace;
            string slotType = SEquipSlot == "Shoulder" ? "Armor" : EquipSlots[SEquipSlot];
            //Update Scroll/Flame Effects
            selectedEquip = updateEquipModelStats(selectedEquip, selectedChar, slotType);

            //Check for new / update of item.
            EquipModel existEquip = CItemDictT.ToList().Find(equip => equip.EquipSlot == SEquipSlot);
            //if slot added before
            if (existEquip != null)
            {
                if (existEquip.Equals(selectedEquip))
                {
                    CommonFunc.errorDia("Equip added before");
                    CItemSelect = selectedEquip;
                }
                //update
                else
                {
                    int existitngIndex = CItemDictT.ToList().FindIndex(item => item.EquipSlot == SEquipSlot);
                    CItemDictT[existitngIndex] = selectedEquip;
                    CItemSelect = CItemDictT[existitngIndex];
                }
            }
            else
            {
                CItemDictT.Add(selectedEquip);
                NoSlot = 0;
            }
            AddEquipmentCMD.RaiseCanExecuteChanged();
        }

        public void toDisplayFrame(string targetStr)
        {
            switch (targetStr)
            {
                case "Scroll":
                    FrameDis.Navigate(typeof(AddEquipScrollPage));
                    FrameDis.DataContext = this;
                    break;
                case "Flame":
                    FrameDis.Navigate(typeof(AddEquipFlamePage));
                    FrameDis.DataContext = this;
                    break;
                case "Potential":
                    FrameDis.Navigate(typeof(AddEquipPotentialPage));
                    FrameDis.DataContext = this;
                    break;
                default:
                    break;
            }
        }


        public void GetCurrentEquipment()
        {
            EquipModel selectedEquip = new EquipModel();

            if (SCharacter != null && SSetItem != null)
            {
                Character selectedChar = SCharacter;
                List<string> cat = new List<string>() { "Ring", "Pendant" };
                string currentSSlot = cat.Contains(EquipSlots[SEquipSlot]) ? EquipSlots[SEquipSlot] : SEquipSlot;


                //Blank Equp

                List<EquipModel> EList = CurrentEquipList;



                //Retreive base equip stats from list
                selectedEquip = CommonFunc.FindEquip(EList, selectedChar, currentSSlot, SSetItem);

                CurrentSEquip = selectedEquip;
            }
        }

        public ObservableCollection<PotentialStats> RetrievePot()
        {
            ObservableCollection<PotentialStats> potList = new ObservableCollection<PotentialStats>();
            if (CurrentSEquip != null && SPotentialG != -1)
            {
                foreach(PotentialStats lines in AEquipM.PotentialStats)
                {
                    if (lines.MinLvl <= CurrentSEquip.EquipLevel && CurrentSEquip.EquipLevel <= lines.MaxLvl && CurrentSEquip.EquipSlot == lines.EquipGrp)
                    {
                        string grade = PotentialGrade[SPotentialG];
                        if (SPotentialG == 0)
                        {
                            if (lines.Grade == PotentialGrade[SPotentialG])
                            {
                                if (lines.Prime == "Prime" || lines.Prime == "Non prime")
                                {
                                    var tempPot = lines;
                                    tempPot.DisplayStat = tempPot.StatValue == "0" ? tempPot.StatIncrease.ToString() : String.Format("{0} +{1}", tempPot.StatIncrease.TrimEnd('%'), tempPot.StatValue);
                                    potList.Add(tempPot);
                                }
                            }
                        }
                        else
                        {
                            if (lines.Prime == "Prime")
                            {
                                if (lines.Grade == grade || lines.Grade == PotentialGrade[SPotentialG-1])
                                {
                                    var tempPot = lines;
                                    tempPot.DisplayStat = tempPot.StatValue == "0" ? tempPot.StatIncrease.ToString() : String.Format("{0} +{1}", tempPot.StatIncrease.TrimEnd('%'), tempPot.StatValue);
                                    potList.Add(tempPot);
                                }
                            }
                        }
                    }
                }
            }
            return potList;
        }


        public void ShowSecondPotential()
        {
            SecondPotL = RetrievePot();
            bool checks = false;
            if (SecondPot !=  null && FirstPot != null && SPotentialG != -1)
            {
                checks = true;
            }

            if (checks)
            {
                foreach (PotentialStats potentialStats in SecondPotL)
                {

                }
            }
            

        }
        private EquipModel updateEquipModelStats(EquipModel selectedEquip, Character selectedChar, string slotType)
        {
            if (selectedEquip.SpellTraced)
            {
                string MainStat = selectedChar.MainStat;
                int STTier = CommonFunc.SpellTraceTier(selectedEquip);
                int perc = Convert.ToInt32(SelectedScrollStat.Remove(SelectedScrollStat.Length - 1));
                selectedEquip.ScrollStats.HP = CommonFunc.SpellTraceDict[slotType][STTier][perc].HP * NoSlot;
                selectedEquip.ScrollStats.DEF = CommonFunc.SpellTraceDict[slotType][STTier][perc].DEF * NoSlot;

                if (slotType == "Weapon" || slotType == "Heart" || slotType == "Gloves")
                {
                    selectedEquip.ScrollStats.ATK = CommonFunc.SpellTraceDict[slotType][STTier][perc].ATK * NoSlot;
                }

                if (MainStat == "HP")
                {
                    selectedEquip.ScrollStats.HP += CommonFunc.SpellTraceDict[slotType][STTier][perc].MainStat * NoSlot * 50;
                }
                else
                {
                    selectedEquip.ScrollStats.GetType().GetProperty(MainStat).SetValue(selectedEquip.ScrollStats, CommonFunc.SpellTraceDict[slotType][STTier][perc].MainStat * NoSlot, null);
                }
                selectedEquip.FlameStats = CommonFunc.recordToProperty(selectedEquip.FlameStats, FlameRecord);
                selectedEquip.SpellTracePerc = SelectedScrollIndex;
            }
            else
            {
                selectedEquip.ScrollStats = CommonFunc.recordToProperty(selectedEquip.ScrollStats, ScrollRecord);
                selectedEquip.FlameStats = CommonFunc.recordToProperty(selectedEquip.FlameStats, FlameRecord);
            }

            return selectedEquip;
        }



    }
}
