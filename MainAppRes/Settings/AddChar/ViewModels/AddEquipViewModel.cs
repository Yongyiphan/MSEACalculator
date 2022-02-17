using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.MainAppRes.Settings.AddChar.ViewPages;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MSEACalculator.MainAppRes.Settings.AddChar.ViewModels
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


        
        //INIT MODELS BEGIN
        public readonly AddCharTrackViewModel ACharTrackVM;
        public AddEquipModel AEquipM { get; set; } = new AddEquipModel();
        public ScrollingModelCLS ScrollModel { get; set; } = new ScrollingModelCLS();
        //INIT MODELS END
        
        //CURRENT SELECTED CHARACTER
        private CharacterCLS _SCharacter;
        public CharacterCLS SCharacter
        {
            get => _SCharacter;
            set
            {
                _SCharacter = value;
                OnPropertyChanged(nameof(SCharacter));
            }

        }

        /// <summary>
        /// BASE EQUIPMENT SELECTION
        /// </summary>
        //KEY: EquipSlot | VALUE: Equip Category
        public Dictionary<string, string> EquipSlots { get => AEquipM.EquipSlot; }

        //private ObservableCollection<string> _ArmorSet = new ObservableCollection<string>();
        public ObservableCollection<string> ArmorSet { get; set; } = new ObservableCollection<string>();
        public List<EquipCLS> CurrentEquipList { get; set; } = new List<EquipCLS>();

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
        

        private string _SEquipSlot;
        public string SEquipSlot
        {
            get { return _SEquipSlot; }
            set
            {
                _SEquipSlot = value;

                if (SEquipSlot != null)
                {
                    ItemDisType = SlotSet.Contains(EquipSlots[SEquipSlot]) ? "Set: " : "Name: ";



                    ShowWeapon = EquipSlots[SEquipSlot] == "Weapon" ? Visibility.Visible : Visibility.Collapsed;

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

        private string _SSetItem;
        public string SSetItem
        {
            get { return _SSetItem; }
            set
            {
                _SSetItem = value;


                GetCurrentEquipment();
                if (CurrentSEquip != null)
                {
                    FirstPot = SecondPot = ThirdPot = null;
                    FirstPotL = RetrievePot();
                }
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SSetItem));
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
                    GetCurrentEquipment();
                    FirstPot = SecondPot = ThirdPot = null;
                    FirstPotL = RetrievePot();

                }
                OnPropertyChanged(nameof(SelectedWeapon));
            }
        }


        //EQUPMENT IN FOCUS
        public EquipCLS CurrentSEquip { get; set; }



        /// <summary>
        /// SCROLLING SELECTION
        /// </summary>
        public Dictionary<string, int> ScrollRecord { get; set; } = new Dictionary<string, int>();
        public List<int> Slots { get => ScrollModel.Slots; } //= new Scrolling().Slots;


        //Spell trace types OR Stats for special scrolls
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


        private bool _IsSpellTrace = false;
        public bool IsSpellTrace
        {
            get
            {
                return _IsSpellTrace;
            }
            set
            {
                _IsSpellTrace = value;
                ScrollTypeTxt = IsSpellTrace ? "Perc:" : "Stat:";
                ShoWSlot = IsSpellTrace ? Visibility.Visible : Visibility.Collapsed;
                ShowScrollValue = IsSpellTrace ? Visibility.Collapsed : Visibility.Visible;
                
                StatTypes = IsSpellTrace ? ScrollModel.SpellTracePercTypes : GVar.BaseStatTypes;
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

                if (SelectedScrollStat != null)
                {
                    if (IsSpellTrace)
                    {
                        if (NoSlot == 0)
                        {
                            ComFunc.errorDia("Slot cannot be 0.");
                            SelectedScrollIndex = -1;
                        }
                        
                    }
                    else
                    {
                        ShowScrollValue = ScrollModel.SpellTracePercTypes.Contains(SelectedScrollStat) ? Visibility.Collapsed : Visibility.Visible;

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
                AddRecordValue("Scroll", value);
                //AddScrollCMD.RaiseCanExecuteChanged();
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ScrollStatValue));
            }
        }


        /// <summary>
        /// FLAME SELECTION
        /// </summary>
        public List<string> FlameStatsTypes { get => AEquipM.FlameStatsTypes; }
        public Dictionary<string, int> FlameRecord { get; set; } = new Dictionary<string, int>();

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
                        FlameStatValue = FlameRecord[SelectedFlame].ToString();

                    }
                    else
                    {
                        FlameStatValue = string.Empty;
                    }

                }
                OnPropertyChanged(nameof(SelectedFlame));
            }
        }

        private string _FlameStatValue = string.Empty;
        public string FlameStatValue
        {
            get { return _FlameStatValue; }
            set
            {
                _FlameStatValue = value;
                //AddFlameCMD.RaiseCanExecuteChanged();
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(FlameStatValue));
            }
        }


        /// <summary>
        /// POTENTIAL SELECTION
        /// </summary>


        public List<string> PotentialGrade { get => GVar.PotentialGrade; }
        

        private ObservableCollection<PotentialStatsCLS> _FirstPotL = new ObservableCollection<PotentialStatsCLS>();
        public ObservableCollection<PotentialStatsCLS> FirstPotL
        {
            get => _FirstPotL;
            set
            {
                _FirstPotL = value;
                OnPropertyChanged(nameof(FirstPotL));
            }

        }

        private ObservableCollection<PotentialStatsCLS> _SecondPotL = new ObservableCollection<PotentialStatsCLS>();
        public ObservableCollection<PotentialStatsCLS> SecondPotL
        {
            get => _SecondPotL;
            set
            {
                _SecondPotL =  value;
                OnPropertyChanged(nameof(SecondPotL));
            }
        }

        private ObservableCollection<PotentialStatsCLS> _ThirdPotL = new ObservableCollection<PotentialStatsCLS>();
        public ObservableCollection<PotentialStatsCLS> ThirdPotL
        {
            get => _ThirdPotL;
            set
            {
                _ThirdPotL = value;
                OnPropertyChanged(nameof(ThirdPotL));
            }
        }


        private List<PotentialStatsCLS> _MainPotL;
        public List<PotentialStatsCLS> MainPotL
        {
            get => _MainPotL;
            set
            {
                _MainPotL = value;
                OnPropertyChanged(nameof(MainPotL));
            }
        }

        private List<PotentialStatsCLS> _AddPotL;
        public List<PotentialStatsCLS> AddPotL
        {
            get => _AddPotL;
            set
            {
                _AddPotL = value;
                OnPropertyChanged(nameof(AddPotL));
            }
        }

        private bool _isAddPot = false;
        public bool isAddPot
        {
            get => _isAddPot;
            set
            {
                _isAddPot = value;

                if (isAddPot == true && CItemSelect != null)
                {
                    if (AddPotL.Select(x => x.PotID).ToList().Sum() != 0)
                    {
                        FirstPot = AddPotL[0];
                        SecondPot = AddPotL[1];
                        ThirdPot = AddPotL[2];

                    }
                }

                OnPropertyChanged(nameof(isAddPot));
            }
        }

        private int _SPotentialG;
        public int SPotentialG
        {
            get { return _SPotentialG; }
            set
            {
                _SPotentialG = value;
                if (SPotentialG != -1)
                {
                    FirstPot = SecondPot = ThirdPot = null;
                    FirstPotL.Clear();
                    SecondPotL.Clear();
                    ThirdPotL.Clear();
                    FirstPotL = RetrievePot();
                }


                OnPropertyChanged(nameof(SPotentialG));
            }
        }

        private PotentialStatsCLS _FirstPot;
        public PotentialStatsCLS FirstPot
        {
            get => _FirstPot;
            set
            {
                _FirstPot = value;

                ShowSecondPot();
                AddPotCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(FirstPot));

            }
        }

        private PotentialStatsCLS _SecondPot;
        public PotentialStatsCLS SecondPot
        {
            get => _SecondPot;
            set
            {
                _SecondPot = value;
                ShowThirdPot();
                AddPotCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SecondPot));
            }
        }

        private PotentialStatsCLS _ThirdPot;
        public PotentialStatsCLS ThirdPot
        {
            get => _ThirdPot;
            set
            {
                _ThirdPot = value;
                AddPotCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ThirdPot));
            }
        }


        public List<string> SlotSet { get; set; } = new List<string>
        {
            "Weapon", "Gloves", "Armor"
        };
        public List<string> WSE { get; set; } = new List<string>
        {
            "Weapon", "Secondary", "Emblem"
        };

        /// <summary>
        /// VISIBILITY CONTROL
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
        

        private Visibility _ShowScrollValue = Visibility.Visible;
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
            set
            {
                _ifWeapon = value;
                OnPropertyChanged(nameof(ifWeapon));
            }
        }

        

        

        
        

        

        public ObservableCollection<EquipCLS> CItemDictT { get; set; }



        private Dictionary<string, string> _TotalRecordDisplay;
        public Dictionary<string, string> TotalRecordDisplay
        {
            get => _TotalRecordDisplay;
            set
            {
                _TotalRecordDisplay = value;
                OnPropertyChanged(nameof(TotalRecordDisplay));
            }
        }


        ///BUTTON CONTRSUCTOR
        
        /// <summary>
        /// Main Constructor
        /// </summary>
        /// <param name="aCharTrackVM"></param>
        public AddEquipViewModel(AddCharTrackViewModel aCharTrackVM)
        {
            
            ACharTrackVM = aCharTrackVM;
            
            
            ACharTrackVM.RaiseChangeChar += HandleCharChange;
            SCharacter = ACharTrackVM?.SelectedAllChar;
            initFields();
            //AddScrollCMD = new CustomCommand(AddScrollStat, canAddScrollStat);
            //AddFlameCMD = new CustomCommand(AddFlame, canAddFlame);
            AddPotCMD = new CustomCommand(AddPotential, canAddPot);


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

        //TOGGLE DIFFERENT INPUT FRAMES
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


        private EquipCLS _CItemSelect;
        public EquipCLS CItemSelect
        {
            get => _CItemSelect;
            set
            {
                _CItemSelect = value;
                ShowSEquipStat = CItemSelect != null ? Visibility.Visible : Visibility.Collapsed;
                if (CItemSelect != null)
                {
                    ifWeapon = CItemSelect.EquipSlot == "Weapon" ? Visibility.Visible : Visibility.Collapsed;
                    SEquipSlot = CItemSelect.EquipSlot;
                    //SSetItem = AEquipM.AccGrp.Contains(EquipSlots[SEquipSlot]) ? CItemSelect.EquipName : CItemSelect.EquipSet;
                    SSetItem = ComFunc.returnSetCat(SEquipSlot) == "Accessory" ? CItemSelect.EquipName : CItemSelect.EquipSet;
                    SelectedWeapon = CItemSelect?.WeaponType;
                    IsSpellTrace = CItemSelect.SpellTraced;
                    if (CItemSelect.SpellTraced)
                    {
                        NoSlot = CItemSelect.SlotCount;
                        SelectedScrollIndex = CItemSelect.SpellTracePerc;
                    }
                    ScrollRecord = ComFunc.propertyToRecord(CItemSelect.ScrollStats, ScrollRecord);
                    FlameRecord = ComFunc.propertyToRecord(CItemSelect.FlameStats, FlameRecord);
                    SPotentialG = CItemSelect.MPgrade;

                    MainPotL = CItemSelect.MainPot;
                    AddPotL = CItemSelect.AddPot;
                    
                    if (CItemSelect.MainPot.Select(x => x.PotID).ToList().Sum() != 0)
                    {
                        FirstPot = FirstPotL.Single(x => x.PotID == CItemSelect.MainPot[0].PotID);
                        SecondPot = FirstPotL.Single(x => x.PotID == CItemSelect.MainPot[1].PotID);
                        ThirdPot = FirstPotL.Single(x => x.PotID == CItemSelect.MainPot[2].PotID);
                        //FirstPot =  CItemSelect.MainPot[0];
                        //SecondPot = CItemSelect.MainPot[1];
                        //ThirdPot = CItemSelect.MainPot[2];
                    }
                    ShowEnteredRecords();
                }
                OnPropertyChanged(nameof(CItemSelect));

            }
        }



        public CustomCommand AddScrollCMD { get; private set; }
        public CustomCommand AddFlameCMD { get; private set; }
        public CustomCommand AddPotCMD { get; private set; }
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
                CItemDictT = new ObservableCollection<EquipCLS>(SCharacter?.EquipmentList);
            }
        }

        private bool canAddScrollStat()
        {
            //Check if stat is selected
            //Check if int value is keyed into value textbox
            if (SelectedScrollStat != null && ScrollStatValue != null && ScrollStatValue  != String.Empty)
            {
                if (int.TryParse(ScrollStatValue.ToString(), out int value))
                {
                    return true;
                }
                else
                {
                    if (ScrollStatValue != string.Empty)
                    {
                        ComFunc.errorDia("Invalid");
                        return false;
                    }
                }
            }
            return false;

        }
        private void AddScrollStat()
        {
            //Stat selected, Int value inserted
            ScrollRecord[SelectedScrollStat] = int.Parse(ScrollStatValue);

            SelectedScrollStat = String.Empty;
            AddEquipmentCMD.RaiseCanExecuteChanged();
        }


        private bool canAddFlame()
        {
            bool canAdd = false;
            //Flame Stat selected
            if (SelectedFlame != null)
            {
                //Check Value is INT
                if (int.TryParse(FlameStatValue, out int value))
                {
                    canAdd = true;
                }
                else
                {
                    if (FlameStatValue != string.Empty)
                    {
                        canAdd = false;
                    }
                }
            }
            return canAdd;
        }
        private void AddFlame()
        {
            FlameRecord[SelectedFlame] = int.Parse(FlameStatValue);
            SelectedFlame = String.Empty;
            AddEquipmentCMD.RaiseCanExecuteChanged();
        }

        private bool canAddPot()
        {
            if (FirstPot != null && SecondPot != null)
            {
                return true;
            }
            return false;
        }
        private void AddPotential()
        {
            PotentialStatsCLS thirdPot = ThirdPot == null ? new PotentialStatsCLS() : ThirdPot;
            if (isAddPot)
            {
                if (ComFunc.notNULL(CurrentSEquip))
                {
                    CurrentSEquip.APgrade = SPotentialG;
                    CurrentSEquip.AddPot = new List<PotentialStatsCLS> { FirstPot, SecondPot, thirdPot };
                }
            }
            else
            {
                if (ComFunc.notNULL(CurrentSEquip))
                {
                    CurrentSEquip.MPgrade = SPotentialG;
                    CurrentSEquip.MainPot = new List<PotentialStatsCLS> { FirstPot, SecondPot, thirdPot };
                }
            }


        }
        private bool canAddItem()
        {
            //Func<bool, string, Dictionary<string, int>, Dictionary<string, int>, bool>
            //    checkAddStatAdded = (isSpellTraced, scrollStat, scrollRecord, flameRecord) =>
            //    {
            //        //Spell trace, check slots, spell trace perc
            //        //check scrollrecord, flamerecords
            //        if (isSpellTraced)
            //        {
            //            //scrollstat null == slotcount = 0
            //            if (scrollStat != null && flameRecord.Count >= 0)
            //            {
            //                return true;
            //            }
            //        }
            //        else
            //        {
            //            //if flameRecord = 0 && scrollRecord = 0 == clean equip, no flame, no scroll
            //            if (flameRecord.Count >= 0 && scrollRecord.Count >= 0)
            //            {
            //                return true;
            //            }
            //        }

            //        return false;
            //    };

            //if (SCharacter != null)
            //{
            //    if (SEquipSlot != null && SSetItem != null)
            //    {
            //        if (SEquipSlot == "Weapon" && SelectedWeapon != null)
            //        {
            //            return checkAddStatAdded(IsSpellTrace, SelectedScrollStat, ScrollRecord, FlameRecord);
            //        }
            //        return checkAddStatAdded(IsSpellTrace, SelectedScrollStat, ScrollRecord, FlameRecord);
            //    }
            //}

            if(SCharacter != null && CurrentSEquip != null)
            {

            }

            SyncCI = CItemDictT.Count == 0 ? true : false;

            return false;
        }

        private void AddItem()
        {

            CharacterCLS selectedChar = SCharacter;
            string currentSSlot = ComFunc.returnRingPend(SEquipSlot);
           

            //Blank Equp
            EquipCLS selectedEquip = new EquipCLS();
            selectedEquip = CurrentSEquip;

            //if (namingType == 0) { selectedEquip.EquipName = string.Format("{0} {1}", SSetItem, SEquipSlot); }
            selectedEquip.EquipSlot = SEquipSlot; //<- override value of Selected slot i.e ring1... pendant1...
            selectedEquip.SlotCount = NoSlot;
            selectedEquip.SpellTraced = IsSpellTrace;
            //Assign base stats to correct property
            selectedEquip = ComFunc.updateBaseStats(selectedChar, selectedEquip);
            
            string slotType = ComFunc.returnScrollCat(currentSSlot);

            //Update Scroll/Flame Effects
            selectedEquip = updateEquipModelStats(selectedEquip, selectedChar, slotType);
            if (ComFunc.notNULL(MainPotL))
            {
                selectedEquip.MainPot =  MainPotL;
            }
            if (ComFunc.notNULL(AddPotL))
            {
                selectedEquip.AddPot =  AddPotL;
            }
            selectedEquip.MPgrade = SPotentialG;
            

            //Check for new / update of item.
            EquipCLS existEquip = CItemDictT.ToList().Find(equip => equip.EquipSlot == SEquipSlot);
            //if slot added before
            if (existEquip != null)
            {
                if (existEquip.Equals(selectedEquip))
                {
                    ComFunc.errorDia("Equip added before");
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
                FirstPot = SecondPot = ThirdPot = null;
            }
            
            AddEquipmentCMD.RaiseCanExecuteChanged();
            ACharTrackVM.UpdateDBCMD.RaiseCanExecuteChanged();
        }
        

        private void ShowEquipSet(string selectedESlot)
        {
            Func<string, string, ObservableCollection<string>, List<EquipCLS>, ObservableCollection<string>>
                FilterSet = (classtype, eSlot, displayList, itemList) =>
                {

                    foreach (var item in itemList)
                    {
                        if (item.EquipSlot == eSlot && (item.ClassType == classtype ||item.ClassType == "Any" ))
                        {
                            if(eSlot == "Shoulder")
                            {
                                if (!displayList.Contains(item.EquipName))
                                {
                                    displayList.Add(item.EquipName);
                                }
                            }
                            else
                            {
                                if (!displayList.Contains(item.EquipSet))
                                {
                                    displayList.Add(item.EquipSet);
                                }
                            }

                            
                        }
                    }


                    return displayList;
                };

            Func<string, ObservableCollection<string>, List<EquipCLS>, ObservableCollection<string>>
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
            Func<string, CharacterCLS, ObservableCollection<string>, List<EquipCLS>, ObservableCollection<string>>
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
                string slotCat = ComFunc.returnSetCat(selectedESlot);
                switch (slotCat)
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
                    case "Armor":
                        ArmorSet.Clear();
                        ArmorSet = FilterSet(SCharacter.ClassType, selectedESlot, ArmorSet, AEquipM.AllArmorList);
                        CurrentEquipList.Clear();
                        CurrentEquipList = AEquipM.AllArmorList;
                        break;
                    case "Accessory":
                        ArmorSet.Clear();
                        string eslot = ComFunc.returnRingPend(selectedESlot);
                        ArmorSet = selectedESlot == "Shoulder" ? FilterSet(SCharacter.ClassType, eslot, ArmorSet, AEquipM.AllAccList) :
                            DisplayItemNameL(eslot, ArmorSet, AEquipM.AllAccList);
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
            EquipCLS selectedEquip = new EquipCLS();

            if (SCharacter != null && SSetItem != null)
            {
                CharacterCLS selectedChar = SCharacter;
                string currentSSlot = ComFunc.returnRingPend(SEquipSlot);
                //Blank Equp

                List<EquipCLS> EList = CurrentEquipList;

                //Retreive base equip stats from list
                selectedEquip = ComFunc.FindEquip(EList, selectedChar, currentSSlot, SSetItem);

                CurrentSEquip = selectedEquip;
            }


        }

        private void AddRecordValue(string type, string value)
        {
            if(value != string.Empty && int.TryParse(value, out int result))
            {
                switch (type)
                {
                    case "Scroll":
                        ScrollRecord[SelectedScrollStat] = result;
                        SelectedScrollIndex = -1;
                        ScrollStatValue = String.Empty;
                        break;

                    case "Flame":
                        FlameRecord[SelectedFlame] = result;
                        FlameStatValue = String.Empty;
                        break;
                }
            }
            else
            {
                ComFunc.errorDia("Enter valid number.");
            }
            

        }

        public ObservableCollection<PotentialStatsCLS> RetrievePot()
        {
            ObservableCollection<PotentialStatsCLS> potList = new ObservableCollection<PotentialStatsCLS>();
            if (CurrentSEquip != null && SPotentialG != -1)
            {
                foreach(PotentialStatsCLS lines in AEquipM.AllPotDict)
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

        private void ShowSecondPot()
        {
            ObservableCollection<PotentialStatsCLS> tempList = new ObservableCollection<PotentialStatsCLS>(FirstPotL);

            bool checks = false;
            if (FirstPot != null && SPotentialG != -1)
            {
                checks = true;
            }

            if (checks == true)
            {
                foreach(string sC in GVar.RepeatOnePot)
                {
                    if (FirstPot.StatIncrease.Contains(sC))
                    {
                        tempList.Remove(FirstPot);
                        
                    }
                }
                SecondPotL = tempList;
            }
        }

        private void ShowThirdPot()
        {
            ObservableCollection<PotentialStatsCLS> tempList = new ObservableCollection<PotentialStatsCLS>(FirstPotL);

            bool checks = false;

            if(SecondPot != null && FirstPot != null && SPotentialG != -1)
            {
                checks = true;
            }
            if (checks == true)
            {
                foreach(string sC in GVar.RepeatTwoPot)
                {
                    if (SecondPot.StatIncrease.Contains(sC) && FirstPot.StatIncrease.Contains(sC))
                    {
                        foreach(var pot in tempList.Where(x => x.StatIncrease.Contains(sC)).ToList())
                        {
                            tempList.Remove(pot);
                        }
                    }
                }
                tempList.Add(new PotentialStatsCLS());
                ThirdPotL = tempList;
            }

        }

        private EquipCLS updateEquipModelStats(EquipCLS selectedEquip, CharacterCLS selectedChar, string slotType)
        {
            if (selectedEquip.SpellTraced)
            {
                string MainStat = selectedChar.MainStat;
                int STTier = ComFunc.SpellTraceTier(selectedEquip);
                int perc = Convert.ToInt32(SelectedScrollStat.Remove(SelectedScrollStat.Length - 1));
                selectedEquip.ScrollStats.HP = ComFunc.SpellTraceDict[slotType][STTier][perc].HP * selectedEquip.SlotCount;
                selectedEquip.ScrollStats.DEF = ComFunc.SpellTraceDict[slotType][STTier][perc].DEF * selectedEquip.SlotCount;

                if (slotType == "Weapon" || slotType == "Heart" || slotType == "Gloves")
                {
                    if (selectedChar.ClassType == "Magician")
                    {
                        selectedEquip.ScrollStats.MATK = ComFunc.SpellTraceDict[slotType][STTier][perc].ATK * selectedEquip.SlotCount;
                    }
                    else
                    {
                        selectedEquip.ScrollStats.ATK = ComFunc.SpellTraceDict[slotType][STTier][perc].ATK * selectedEquip.SlotCount;
                    }
                }

                if (MainStat == "HP")
                {
                    selectedEquip.ScrollStats.HP += ComFunc.SpellTraceDict[slotType][STTier][perc].MainStat * selectedEquip.SlotCount * 50;
                }
                else
                {
                    selectedEquip.ScrollStats.GetType().GetProperty(MainStat).SetValue(selectedEquip.ScrollStats, ComFunc.SpellTraceDict[slotType][STTier][perc].MainStat * selectedEquip.SlotCount, null);
                }
                selectedEquip.FlameStats = ComFunc.recordToProperty(selectedEquip.FlameStats, FlameRecord);
                selectedEquip.SpellTracePerc = SelectedScrollIndex;
            }
            else
            {
                selectedEquip.ScrollStats = ComFunc.recordToProperty(selectedEquip.ScrollStats, ScrollRecord);
                selectedEquip.FlameStats = ComFunc.recordToProperty(selectedEquip.FlameStats, FlameRecord);
            }

            return selectedEquip;
        }


        private void ShowEnteredRecords()
        {
            Dictionary<string, int> BaseStat = ComFunc.propertyToRecord(CItemSelect.BaseStats, new Dictionary<string, int>());
            Dictionary<string, string> DisplayDict = new Dictionary<string, string>();
            foreach(string dictKey in BaseStat.Keys)
            {
                if(BaseStat[dictKey] != 0)
                {
                    int curretValue = BaseStat[dictKey];
                    if (DisplayDict.ContainsKey(dictKey))
                    {
                        DisplayDict[dictKey] = String.Format("{0} +{1}", DisplayDict[dictKey], curretValue);
                    }
                    else
                    {
                        DisplayDict[dictKey] = String.Format("{0}: {1}",dictKey, curretValue);
                    }
                }
            }
            foreach(string dictKey in ScrollRecord.Keys)
            {
                if(ScrollRecord[dictKey] != 0)
                {
                    int curretValue = ScrollRecord[dictKey];
                    if (DisplayDict.ContainsKey(dictKey))
                    {
                        DisplayDict[dictKey] = String.Format("{0} +{1}", DisplayDict[dictKey], curretValue);
                    }
                    else
                    {
                        DisplayDict[dictKey] = String.Format("{0}: {1}",dictKey, curretValue);
                    }
                }
            }
            foreach(string dictKey in FlameRecord.Keys)
            {
                if(FlameRecord[dictKey] != 0)
                {
                    int curretValue = FlameRecord[dictKey];
                    if (DisplayDict.ContainsKey(dictKey))
                    {
                        if (GVar.SpecialStatType.Contains(dictKey) || dictKey == "IED")
                        {
                            DisplayDict[dictKey] = String.Format("{0} +{1}%", DisplayDict[dictKey], curretValue);
                        }
                        else
                        {
                            DisplayDict[dictKey] = String.Format("{0} +{1}", DisplayDict[dictKey], curretValue);
                        }
                        
                    }
                    else
                    {
                        if (GVar.SpecialStatType.Contains(dictKey) || dictKey == "IED")
                        {
                            DisplayDict[dictKey] = String.Format("{0}: {1}%", DisplayDict[dictKey], curretValue);
                        }
                        else
                        {
                            DisplayDict[dictKey] = String.Format("{0}: {1}", dictKey, curretValue);
                        }
                        
                    }
                }
            }

            


            TotalRecordDisplay = DisplayDict;
        }
    }
}
