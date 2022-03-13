using MSEACalculator.CalculationRes;
using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.EquipmentRes;
using MSEACalculator.MainAppRes.Settings.AddChar.ViewPages;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MSEACalculator.MainAppRes.Settings.AddChar.ViewModels
{
    public class AddEquipVM : INPCObject
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
        public readonly AddCharTrackVM ACharTrackVM;
        public AddEquipModel AEM { get; set; } = new AddEquipModel();
        public ScrollingModelCLS ScrollM { get; set; } = new ScrollingModelCLS();

        public CheckTypes CT = new CheckTypes();


        //INIT MODELS END

        //Record Search List
        private string CurrentEquipList = string.Empty;
        private string CurrentStarforceList = string.Empty;

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

        private List<int> _SFLvlList;
        public List<int> StarforceLevels 
        { get => _SFLvlList; set
            {
                _SFLvlList = value;
                OnPropertyChanged(nameof(StarforceLevels));
            }
        }

        private int _StarforceInput = 0;
        public int StarforceI
        {
            get { return _StarforceInput; }
            set { _StarforceInput = value;
                OnPropertyChanged(nameof(StarforceI));
            }
        }


        //BASE EQUIPMENT SELECTION
        #region

        public Dictionary<string, string> EquipSlots { get => AEM.EquipSlot; }

        public ObservableCollection<string> ArmorSet { get; set; } = new ObservableCollection<string>();

        private string _XenonEquipType = string.Empty;

        public string XenonEquipType
        {
            get { return _XenonEquipType; }
            set { _XenonEquipType = value;

                if (_XenonEquipType != string.Empty && SEquipSlot != null)
                {
                    ShowEquipSet(SEquipSlot);
                }
                OnPropertyChanged(nameof(XenonEquipType));
            }
        }



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
        public string SEquipSlot  // => EquipSlot's KEY
        {
            get { return _SEquipSlot; }
            set
            {
                _SEquipSlot = value;

                if (SEquipSlot != null)
                {
                    ItemDisType = ShowSet.Contains(EquipSlots[SEquipSlot]) ? "Set: " : "Name: ";

                    ShowWeapon = EquipSlots[SEquipSlot] == "Weapon" ? Visibility.Visible : Visibility.Collapsed;

                    
                    ShowEquipSet(SEquipSlot);
                    
                       
                    
                    if (CItemDictT.ToList().Find(item=> item?.EquipListSlot == SEquipSlot) == null)
                    {
                        ResetInput();
                    }
                }

                AddEquipmentCMD.RaiseCanExecuteChanged();
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

                if(string.IsNullOrEmpty(value) == false && SEquipSlot != "Weapon")
                {
                    GetCurrentEquipment();
                }

                if (CurrentSEquip != null)
                {
                    if(SEquipSlot == "Secondary")
                    {
                        SCharacter.CurrentSecondaryWeapon = SSetItem;
                    }

                    RecordPotential("Grade",null, SPotentialG);
                    FirstLine = SecondLine = ThirdLine = null;
                    RetrievePot(CurrentSEquip);
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
                if (SCharacter !=  null && value != String.Empty)
                {
                    SCharacter.CurrentMainWeapon = SelectedWeapon;
                    GetCurrentEquipment();

                    RecordPotential("Grade",null, SPotentialG);
                    FirstLine = SecondLine = ThirdLine = null;
                    RetrievePot(CurrentSEquip);

                }
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedWeapon));
            }
        }

        #endregion

        //STORE EQUPMENT IN FOCUS
        public EquipCLS CurrentSEquip { get; set; }


        //SCROLL SELECTION
        #region
        public Dictionary<string, int> ScrollRecord { get; set; } = new Dictionary<string, int>();
        public List<int> Slots { get => ScrollM.Slots; }

        public List<string> ShowSet { get; set; } = new List<string>
        {
            "Weapon", "Armor"
        };

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

        public List<string> SpellTraceStats { get => ScrollM.SpellTraceStat;}

        private string _STStat;

        public string STStat
        {
            get { return _STStat; }
            set { _STStat = value;
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(STStat));
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
                ShowXenonScroll = SCharacter.ClassName == "Xenon" ? Visibility.Visible : Visibility.Collapsed;
                StatTypes = IsSpellTrace ? ScrollM.SpellTracePercTypes : GVar.BaseStatTypes;
                if (!IsSpellTrace)
                {
                    NoSlot = 0;
                    ScrollRecord.Clear();
                }
                AddEquipmentCMD.RaiseCanExecuteChanged();
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
                            ComFunc.ErrorDia("Slot cannot be 0.");
                            SelectedScrollIndex = -1;
                        }   
                    }
                    else
                    {
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
                if(value != string.Empty)
                {
                    AddRecordValue("Scroll", value);
                }
                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ScrollStatValue));
            }
        }
        #endregion

        //FLAME SELECTION
        #region
        public List<string> FlameStatsTypes { get => AEM.FlameStatsTypes; }
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
                if(value != string.Empty)
                {
                    AddRecordValue("Flame", value);
                }

                AddEquipmentCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(FlameStatValue));
            }
        }

        #endregion

        //POTENTIAL SELECTION
        #region
        public List<string> PotentialGrade { get => GVar.PotentialGrade; }


        private ObservableCollection<PotentialStatsCLS> _FirstPotL = new ObservableCollection<PotentialStatsCLS>();
        public ObservableCollection<PotentialStatsCLS> FirstPotL
        {
            get { return _FirstPotL; }
            set { _FirstPotL = value;
                OnPropertyChanged(nameof(FirstPotL));
            }
        }

        private ObservableCollection<PotentialStatsCLS> _SecondPotL = new ObservableCollection<PotentialStatsCLS>();
        public ObservableCollection<PotentialStatsCLS> SecondPotL
        {
            get { return _SecondPotL; }
            set
            {
                _SecondPotL= value;
                OnPropertyChanged(nameof(SecondPotL));
            }
        }

        private ObservableCollection<PotentialStatsCLS> _ThirdPotL = new ObservableCollection<PotentialStatsCLS>();
        public ObservableCollection<PotentialStatsCLS> ThirdPotL
        {
            get { return _ThirdPotL; }
            set
            {
                _ThirdPotL= value;
                OnPropertyChanged(nameof(ThirdPotL));
            }
        }

        private bool _isAddPot = false;
        public bool IsAddPot
        {
            get => _isAddPot;
            set
            {
                _isAddPot = value;

                if (SPotentialG != -1 && CurrentSEquip != null)
                {
                    FirstLine = SecondLine = ThirdLine = null;
                    FirstPotL.Clear();
                    SecondPotL.Clear();
                    ThirdPotL.Clear();

                    SPotentialG = IsAddPot ? CurrentSEquip.APotGrade : CurrentSEquip.MPotGrade;

                    RecordPotential("Grade",new PotentialStatsCLS(), SPotentialG);

                    RetrievePot(CurrentSEquip);

                    if(CItemSelect != null)
                    {
                        ShowPotential("Fields");
                    }
                }

                OnPropertyChanged(nameof(IsAddPot));
            }
        }

        private int _SelectedPotentialGrade;
        public int SPotentialG
        {
            get { return _SelectedPotentialGrade; }
            set
            {
                _SelectedPotentialGrade = value;
                if (SPotentialG != -1 && CurrentSEquip != null)
                {
                    FirstLine = SecondLine = ThirdLine = null;
                    FirstPotL.Clear();
                    SecondPotL.Clear();
                    ThirdPotL.Clear();
                    RecordPotential("Grade", new PotentialStatsCLS(), value);

                    RetrievePot(CurrentSEquip);
                }


                OnPropertyChanged(nameof(SPotentialG));
            }
        }

        private PotentialStatsCLS _FirstLine;
        public PotentialStatsCLS FirstLine
        {
            get => _FirstLine;
            set
            {
                _FirstLine = value;

                if (FirstLine != null)
                {
                    ShowSecondPot();
                    RecordPotential("First", value);
                }
                OnPropertyChanged(nameof(FirstLine));

            }
        }

        private PotentialStatsCLS _SecondLine;
        public PotentialStatsCLS SecondLine
        {
            get => _SecondLine;
            set
            {
                _SecondLine = value;

                if (SecondLine != null)
                {
                    ShowThirdPot(); 
                    RecordPotential("Second", value);
                }
                OnPropertyChanged(nameof(SecondLine));
            }
        }

        private PotentialStatsCLS _ThirdLine;
        public PotentialStatsCLS ThirdLine
        {
            get => _ThirdLine;
            set
            {
                _ThirdLine = value;
                if(ThirdLine != null)
                {
                    RecordPotential("Third", value);
                }
                OnPropertyChanged(nameof(ThirdLine));
            }
        }

        #endregion



        /// VISIBILITY CONTROL
        #region
        private Visibility _ShowXenonClassType = Visibility.Collapsed;

        public Visibility ShowXenonClassType
        {
            get { return _ShowXenonClassType; }
            set { _ShowXenonClassType = value;
                OnPropertyChanged(nameof(ShowXenonClassType));
            }
        }

        private Visibility _showXenonScroll = Visibility.Collapsed;


        //ISSUES: NEEDS TO UPDATE WHEN CHAR CHANGE
        public Visibility ShowXenonScroll
        {
            get { return _showXenonScroll; }
            set { _showXenonScroll = value;
                OnPropertyChanged(nameof(ShowXenonScroll));
            }
        }


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
        #endregion



        public ObservableCollection<EquipCLS> CItemDictT { get; set; } = new ObservableCollection<EquipCLS>();
        




        private EquipCLS _CItemSelect;

        public EquipCLS CItemSelect
        {
            get { return _CItemSelect; }
            set { _CItemSelect = value;


                ShowSEquipStat = CItemSelect != null ? Visibility.Visible : Visibility.Collapsed;

                if (CItemSelect != null)
                {
                    CurrentSEquip = CItemSelect;
                    SEquipSlot = CItemSelect?.EquipListSlot;
                    SSetItem = ComFunc.ReturnSetCat(SEquipSlot) == "Accessory" ? CItemSelect?.EquipName : CItemSelect?.EquipSet;
                    SelectedWeapon = CItemSelect?.WeaponType;
                    IsSpellTrace = CItemSelect.IsSpellTraced;
                    StarforceI = CItemSelect.StarForce;

                    if (CItemSelect.IsSpellTraced)
                    {
                        NoSlot = CItemSelect.SlotCount;
                        string perc = string.Format("{0}%", CItemSelect.SpellTracePerc);
                        SelectedScrollIndex =  ScrollM.SpellTracePercTypes.IndexOf(perc);
                    }
                    //ScrollRecord = ComFunc.PropertyToRecord(CItemSelect.ScrollStats);
                    //FlameRecord = ComFunc.PropertyToRecord(CItemSelect.FlameStats);

                    ScrollRecord = CItemSelect.ScrollStats.ToRecord();
                    FlameRecord = CItemSelect.FlameStats.ToRecord();

                    UpdateDisplay();
                }
                DelEquipmentCMD.RaiseCanExecuteChanged();

                OnPropertyChanged(nameof(CItemSelect));
            }
        }


        //DISPLAY FINAL ITEM STATS
        #region
        //private Dictionary<string, string> _TotalRecordDisplay;
        //public Dictionary<string, string> TotalRecordDisplay
        //{
        //    get => _TotalRecordDisplay;
        //    set
        //    {
        //        _TotalRecordDisplay = value;
        //        OnPropertyChanged(nameof(TotalRecordDisplay));
        //    }
        //}
        private Dictionary<string, StatValue> _TotalRecordDisplay;
        public Dictionary<string, StatValue> TotalRecordDisplay
        {
            get => _TotalRecordDisplay;
            set
            {
                _TotalRecordDisplay = value;
                OnPropertyChanged(nameof(TotalRecordDisplay));
            }
        }

        private List<string> _MainPotL;
        public List<string> DisplayMainPotL
        {
            get => _MainPotL;
            set
            {
                _MainPotL = value;
                OnPropertyChanged(nameof(DisplayMainPotL));
            }
        }

        private List<string> _AddPotL;
        public List<string> DisplayAddPotL
        {
            get => _AddPotL;
            set
            {
                _AddPotL = value;
                OnPropertyChanged(nameof(DisplayAddPotL));
            }
        }
        #endregion
        

        /// <summary>
        /// MAIN CONSTRUCTOR
        /// </summary>
        /// <param name="aCharTrackVM"></param>
        public AddEquipVM(AddCharTrackVM aCharTrackVM)
        {
            
            ACharTrackVM = aCharTrackVM;
            
            ACharTrackVM.RaiseChangeChar += HandleCharChange;
            SCharacter = ACharTrackVM?.SelectedAllChar;
            initFields();


            AddEquipmentCMD = new CustomCommand(AddItem, canAddItem);
            DelEquipmentCMD = new CustomCommand(DelItem, canDelItem);
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
                RedirectDisplayFrame(FrameSelection.Content.ToString());
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


        


        public CustomCommand AddEquipmentCMD { get; private set; }
        public CustomCommand DelEquipmentCMD { get;private set; }   

        //FUNCTION
        #region

        private void initFields()
        {

            StatTypes = GVar.BaseStatTypes;
            if (SCharacter == null)
            {
                CharacterWeapon = new List<string>();
            }
            else
            {
                
                ArmorSet.Clear();
                CharacterWeapon = SCharacter?.MainWeapon;
                SEquipSlot = null;
                CItemDictT.Clear();
                SCharacter?.EquipmentList.ForEach(x =>  CItemDictT.Add(x));

                if(SCharacter.ClassName == "Xenon")
                {
                    ShowXenonClassType = Visibility.Visible;
                }
                else
                {
                    ShowXenonClassType = Visibility.Collapsed;
                    XenonEquipType = String.Empty;
                }
                
            }
        }


        private bool canAddItem()
        {
            if(SCharacter != null && CurrentSEquip != null)
            {
                if (IsSpellTrace)
                {
                    if(SCharacter.ClassName == "Xenon")
                    {
                        if (NoSlot > 0 && string.IsNullOrEmpty(SelectedScrollStat) == false  && string.IsNullOrEmpty(STStat) == false)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (NoSlot > 0 && string.IsNullOrEmpty(SelectedScrollStat) == false)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }                    
                }
                return true;
            }

            SyncCI = CItemDictT.Count == 0 ? true : false;

            return false;
        }

        private void AddItem()
        {

            string currentSSlot = ComFunc.ReturnRingPend(SEquipSlot);            

            //CurrentSEquip.EquipListSlot = SEquipSlot; //<- override value of Selected slot i.e ring1... pendant1...
            CurrentSEquip.SlotCount = NoSlot;
            CurrentSEquip.IsSpellTraced = IsSpellTrace;
            CurrentSEquip.StarForce = StarforceI;

            
            
            string slotType = ComFunc.ReturnScrollCat(currentSSlot);

            //Update Scroll/Flame/Starforce Effects
            CurrentSEquip = updateEquipModelStats(CurrentSEquip, slotType);
            

            //Check for new / update of item.
            List<EquipCLS> existEquip = CItemDictT.ToList().FindAll(equip => equip.EquipListSlot == SEquipSlot);
            //if slot added before
            if (existEquip.Count > 0)
            {
                if(existEquip.Count > 1)
                {
                    ComFunc.ErrorDia("Item Added before");
                }
                else
                {
                    int existitngIndex = CItemDictT.ToList().FindIndex(item => item.EquipListSlot == SEquipSlot);
                    CItemDictT[existitngIndex] = CurrentSEquip;
                    CItemSelect = CurrentSEquip;
                    UpdateDisplay();
                }
            }
            else if(CItemDictT.ToList().Where(item => item.EquipListSlot == CurrentSEquip.EquipListSlot && (item.EquipName ==  CurrentSEquip.EquipName || item.EquipSet == CurrentSEquip.EquipSet)).Any())
            {
                ComFunc.ErrorDia("Item Added before");
                ResetInput();
            }
            else
            {
                CItemDictT.Add(CurrentSEquip);
                ResetInput();
            }
            
            AddEquipmentCMD.RaiseCanExecuteChanged();
            ACharTrackVM.UpdateDBCMD.RaiseCanExecuteChanged();
        }
      
        
        private bool canDelItem()
        {
            if (CItemSelect != null)
            {
                return true;
            }
            return false;
        }
        private void DelItem()
        {
            CItemDictT.Remove(CItemSelect);
            UpdateDisplay();
        }

        private void ShowEquipSet(string selectedESlot)
        {
            
            if (SCharacter != null)
            {
                

                selectedESlot = ComFunc.ReturnRingPend(selectedESlot);
                string slotCat = ComFunc.ReturnSetCat(selectedESlot);
                ArmorSet.Clear();
                
                ComFunc.FilterBy(slotCat, SCharacter, selectedESlot, AEM.AllEquipStore[slotCat], XenonEquipType).ForEach(x => ArmorSet.Add(x));
                
                CurrentEquipList = slotCat;
                
                

                if (CItemDictT.Count > 0)
                {
                    foreach (EquipCLS AddedEquip in CItemDictT)
                    {
                        if (ArmorSet.Contains(AddedEquip.EquipName))
                        {
                            if(CItemSelect != null && AddedEquip.EquipName == CItemSelect.EquipName)
                            {
                                continue;
                            }
                            ArmorSet.Remove(AddedEquip.EquipName);
                        }
                        else if (ArmorSet.Contains(AddedEquip.EquipSet))
                        {
                            if (CItemSelect != null && AddedEquip.EquipSet == CItemSelect.EquipSet)
                            {
                                continue;
                            }
                            ArmorSet.Remove(AddedEquip.EquipSet);
                        }
                    }
                }
             
            }

        }
        
        public void GetCurrentEquipment()
        {
            

            if (SCharacter != null && SSetItem != null)
            {
                 
                string currentSSlot = ComFunc.ReturnRingPend(SEquipSlot);

                //Retreive base equip stats from list

                EquipCLS NewEquip = ComFunc.FindEquip(AEM.AllEquipStore[CurrentEquipList], SCharacter, currentSSlot, SSetItem, XenonEquipType);

                if(NewEquip.ClassType ==  null)
                {
                    NewEquip.ClassType =  SCharacter.ClassName == "Xenon" ? XenonEquipType : SCharacter.ClassType;
                }
                if((CurrentSEquip != null && !CurrentSEquip.Equals(NewEquip)) || CurrentSEquip == null)
                {
                    //NewEquip  = ComFunc.UpdateBaseStats(SCharacter, NewEquip, XenonEquipType);
                    NewEquip.InitBaseEquipStat();
                }
                NewEquip.EquipListSlot = SEquipSlot;

                CurrentStarforceList = NewEquip.EquipSet == "Tyrant" ? "Superior" : "Basic";

                StarforceLevels =  AEM.StarforceStore[CurrentStarforceList].Select(x => x.SFLevel).ToList();


                CurrentSEquip = NewEquip;
            }


        }

        private void AddRecordValue(string type, string value)
        {
            if (int.TryParse(value, out int result))
            {
                switch (type)
                {
                    case "Scroll":
                        ScrollRecord[SelectedScrollStat] = result;
                        break;

                    case "Flame":
                        FlameRecord[SelectedFlame] = result;
                        break;
                }
            }
            else
            {
                ComFunc.ErrorDia("Enter valid number.");
                ScrollStatValue = string.Empty;
            }
        }

        private void RecordPotential(string PotLine, PotentialStatsCLS value, int grade = -1)
        {
            value = value == null ? new PotentialStatsCLS() : value;

            if(CurrentSEquip != null)
            {
                if (IsAddPot)
                {
                    switch (PotLine)
                    {
                        case "Grade":
                            CurrentSEquip.APotGrade = grade;
                            break;
                        case "First":
                            CurrentSEquip.AddPot["First"] = value;
                            break;
                        case "Second":
                            CurrentSEquip.AddPot["Second"] = value;
                            break;
                        case "Third":
                            CurrentSEquip.AddPot["Third"] = value;
                            break;
                    }
                }
                else
                {
                    switch (PotLine)
                    {
                        case "Grade":
                            CurrentSEquip.MPotGrade = grade;
                            break;
                        case "First":
                            CurrentSEquip.MainPot["First"] = value;
                            break;
                        case "Second":
                            CurrentSEquip.MainPot["Second"] = value;
                            break;
                        case "Third":
                            CurrentSEquip.MainPot["Third"] = value;
                            break;
                    }
                }
            }
        }

        public void RetrievePot(EquipCLS CurrentEquip)
        {
            int PotType = IsAddPot ? CurrentEquip.APotGrade: CurrentEquip.MPotGrade;
            List<PotentialStatsCLS> BasePotList = IsAddPot ? AEM.AllBonusPotDict : AEM.AllPotDict;

            ObservableCollection<PotentialStatsCLS> potList = new ObservableCollection<PotentialStatsCLS>();
            if (CurrentEquip != null && PotType != -1)
            {
                foreach (PotentialStatsCLS lines in BasePotList)
                {

                    if (lines.MinLvl <= CurrentEquip.EquipLevel && CurrentEquip.EquipLevel <= lines.MaxLvl && CurrentEquip.EquipSlot == lines.EquipGrp)
                    {
                        string grade = PotentialGrade[PotType];
                        if (PotType == 0)
                        {
                            if (lines.Grade == PotentialGrade[PotType])
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
                                if (lines.Grade == grade || lines.Grade == PotentialGrade[PotType-1])
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
            FirstPotL = potList;
        }

        private void ShowSecondPot()
        {
            ObservableCollection<PotentialStatsCLS> tempList = new ObservableCollection<PotentialStatsCLS>(FirstPotL);

            bool checks = false;
            if (FirstLine != null && SPotentialG != -1)
            {
                checks = true;
            }

            if (checks == true)
            {
                foreach(string sC in GVar.RepeatOneMPot)
                {
                    if (FirstLine.StatIncrease.Contains(sC))
                    {
                        tempList.Remove(FirstLine);
                        
                    }
                }
                SecondPotL = tempList;
            }
        }

        private void ShowThirdPot()
        {
            List<string> RepeatCondition = IsAddPot ? GVar.RepeatTwoAPot : GVar.RepeatTwoMPot;
            ObservableCollection<PotentialStatsCLS> tempList = new ObservableCollection<PotentialStatsCLS>(FirstPotL);

            bool checks = false;

            if(SecondLine != null && FirstLine != null && SPotentialG != -1)
            {
                checks = true;
            }
            if (checks == true)
            {
                foreach(string sC in RepeatCondition)
                {
                    if (SecondLine.StatIncrease.Contains(sC) && FirstLine.StatIncrease.Contains(sC))
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

        private EquipCLS updateEquipModelStats(EquipCLS selectedEquip, string slotType)
        {
            
            if (selectedEquip.IsSpellTraced)
            {
                selectedEquip.SpellTracePerc = Convert.ToInt32(SelectedScrollStat.TrimEnd('%'));
                selectedEquip.ScrollStats = CalForm.CalSpellTrace(selectedEquip, SCharacter, slotType, STStat);
            }
            else
            {
                selectedEquip.ScrollStats = new EquipStatsCLS();
                //selectedEquip.ScrollStats = ComFunc.RecordToProperty(ScrollRecord);
                selectedEquip.ScrollStats.DictToProperty(ScrollRecord);
            }

            //selectedEquip.FlameStats = ComFunc.RecordToProperty(FlameRecord);
            selectedEquip.FlameStats.DictToProperty(FlameRecord);
            selectedEquip.StarforceStats  = CalForm.CalStarforceStats(SCharacter, selectedEquip, AEM.StarforceStore[CurrentStarforceList]);


            return selectedEquip;
        }


        //private void ShowEnteredRecords()
        //{
        //    //Dictionary<string, int> BaseStat = ComFunc.PropertyToRecord(CurrentSEquip.BaseStats);
        //    Dictionary<string, int> BaseStat = CurrentSEquip.BaseStats.ToRecord(); 
        //    Dictionary<string, string> DisplayDict = new Dictionary<string, string>();
        //    foreach(string dictKey in BaseStat.Keys)
        //    {
        //        if(BaseStat[dictKey] != 0)
        //        {
        //            int curretValue = BaseStat[dictKey];
        //            if (DisplayDict.ContainsKey(dictKey))
        //            {
        //                DisplayDict[dictKey] = String.Format("{0} + {1}", DisplayDict[dictKey], curretValue);
        //            }
        //            else
        //            {
        //                DisplayDict[dictKey] = String.Format("{0}: {1}",dictKey, curretValue);
        //            }
        //        }
        //    }

        //    foreach(string dictKey in ScrollRecord.Keys)
        //    {
        //        if(ScrollRecord[dictKey] != 0)
        //        {
        //            int curretValue = ScrollRecord[dictKey];
        //            if (DisplayDict.ContainsKey(dictKey))
        //            {
        //                DisplayDict[dictKey] = String.Format("{0} + {1}", DisplayDict[dictKey], curretValue);
        //            }
        //            else
        //            {

        //                DisplayDict[dictKey] = String.Format("{0}: 0 + {1}",dictKey, curretValue);
        //            }
        //        }
        //    }
        //    //Dictionary<string, int> StarforceRecord = ComFunc.PropertyToRecord(CurrentSEquip.StarforceStats);

        //    Dictionary<string, int> StarforceRecord = CurrentSEquip.StarforceStats.ToRecord(); 
        //    foreach (string dictKey in StarforceRecord.Keys)
        //    {
        //        if(StarforceRecord[dictKey] != 0)
        //        {
        //            int curretValue = StarforceRecord[dictKey];
        //            if (DisplayDict.ContainsKey(dictKey))
        //            {
        //                DisplayDict[dictKey] = String.Format("{0} + {1}", DisplayDict[dictKey], curretValue);
        //            }
        //            else
        //            {

        //                DisplayDict[dictKey] = String.Format("{0}: 0 + {1}",dictKey, curretValue);
        //            }
        //        }
        //    }


        //    foreach(string dictKey in FlameRecord.Keys)
        //    {
        //        if(FlameRecord[dictKey] != 0)
        //        {
        //            int curretValue = FlameRecord[dictKey];
        //            if (DisplayDict.ContainsKey(dictKey))
        //            {
        //                if (GVar.SpecialStatType.Contains(dictKey) || dictKey == "IED")
        //                {
        //                    DisplayDict[dictKey] = String.Format("{0} + {1}%", DisplayDict[dictKey], curretValue);
        //                }
        //                else
        //                {
        //                    DisplayDict[dictKey] = String.Format("{0} + {1}", DisplayDict[dictKey], curretValue);
        //                }
                        
        //            }
        //            else
        //            {
        //                if (GVar.SpecialStatType.Contains(dictKey) || dictKey == "IED")
        //                {
        //                    DisplayDict[dictKey] = String.Format("{0}: 0 + {1}%", DisplayDict[dictKey], curretValue);
        //                }
        //                else
        //                {
        //                    DisplayDict[dictKey] = String.Format("{0}: 0 + {1}", dictKey, curretValue);
        //                }
                        
        //            }
        //        }
        //    }

            

        //    TotalRecordDisplay = DisplayDict;
        //}
        
        public struct StatValue
        {
            public string BaseStat { get; set; }
            public string ScrollStat { get; set; }
            public string FlameStat { get; set; }
            public string StarforceStat { get; set; }
            
            public int CheckAllZero()
            {
                char[] toTrim = { '+', '%', ' ', ':' };
                int BS = Convert.ToInt32(BaseStat.Trim(toTrim));
                int SS = Convert.ToInt32(ScrollStat.Trim(toTrim));
                int FS = Convert.ToInt32(FlameStat.Trim(toTrim));
                int SfS = Convert.ToInt32(StarforceStat.Trim(toTrim));

                return BS + SS + FS + SfS; 
            }
        }

        private  void GatherDisplay()
        {

            DataTable dt = new DataTable();
            //Dictionary<string, List<string>> ToDisplay = new Dictionary<string, List<string>>();
            Dictionary<string, StatValue> ToDisplay = new Dictionary<string, StatValue>();
            Dictionary<string, int> BaseStat = CurrentSEquip.BaseStats.ToRecord();
            Dictionary<string, int> ScrollStat = CurrentSEquip.ScrollStats.ToRecord();
            Dictionary<string, int> FlameStat = CurrentSEquip.FlameStats.ToRecord();
            Dictionary<string, int> SFStat = CurrentSEquip.StarforceStats.ToRecord();             
            
            foreach(string key in BaseStat.Keys)
            {
                StatValue temp  = new StatValue();

                if(key == "AllStat")
                {
                    temp.BaseStat   = string.Format(": {0}%", BaseStat[key].ToString());
                    temp.ScrollStat = string.Format(" +{0}%", ScrollStat[key].ToString());
                    temp.FlameStat  = string.Format(" +{0}%", FlameStat[key].ToString());
                    temp.StarforceStat = string.Format(" +{0}%", SFStat[key].ToString());

                }
                else
                {
                    temp.BaseStat   = string.Format(": {0}", BaseStat[key].ToString());
                    temp.ScrollStat = string.Format(" +{0}", ScrollStat[key].ToString());
                    temp.FlameStat  = string.Format(" +{0}", FlameStat[key].ToString());
                    temp.StarforceStat = string.Format(" +{0}", SFStat[key].ToString());

                }


                if(temp.CheckAllZero() > 0)
                {

                    ToDisplay.Add(key, temp);
                }
            }
            TotalRecordDisplay = ToDisplay;

        }
        private void ShowPotential(string mode)
        {
            if (CurrentSEquip != null)
            {
                switch (mode)
                {
                    case "Fields":
                        if (IsAddPot)
                        {
                            if (CurrentSEquip.AddPot.Values.ToList().Select(x => x.PotID).ToList().Sum() != 0)
                            {
                                RetrievePot(CurrentSEquip);

                                FirstLine = FirstPotL.Single(x => x.PotID == CurrentSEquip.AddPot["First"].PotID);
                                SecondLine = SecondPotL.Single(x => x.PotID == CurrentSEquip.AddPot["Second"].PotID);
                                ThirdLine = ThirdPotL.Single(x => x.PotID == CurrentSEquip.AddPot["Third"].PotID);

                            }
                        }
                        else
                        {

                            if (CurrentSEquip.MainPot.Values.ToList().Select(x => x.PotID).ToList().Sum() != 0)
                            {
                                RetrievePot(CurrentSEquip);

                                FirstLine = FirstPotL.Single(x => x.PotID == CurrentSEquip.MainPot["First"].PotID);
                                SecondLine = SecondPotL.Single(x => x.PotID == CurrentSEquip.MainPot["Second"].PotID);
                                ThirdLine = ThirdPotL.Single(x => x.PotID == CurrentSEquip.MainPot["Third"].PotID);
                            }

                        }
                        break;
                    case "Display":
                        if (CItemSelect != null)
                        {
                            List<string> DMP = new List<string>();
                            foreach (string pot in CurrentSEquip.MainPot.Keys)
                            {
                                if (!CurrentSEquip.MainPot[pot].Equals(new PotentialStatsCLS()))
                                {
                                    DMP.Add(string.Format("{0} Line: {1}", pot, CurrentSEquip.MainPot[pot].DisplayStat));
                                }
                            }
                            DisplayMainPotL = DMP;

                            List<string> DAP = new List<string>();
                            foreach (string pot in CurrentSEquip.AddPot.Keys)
                            {
                                if (!CurrentSEquip.AddPot[pot].Equals(new PotentialStatsCLS()))
                                {
                                    DAP.Add(string.Format("{0} Line: {1}", pot, CurrentSEquip.AddPot[pot].DisplayStat));
                                }
                            }
                            DisplayAddPotL = DAP;
                        }
                        break;
                }
                

                
            }
            
        }

        private void UpdateDisplay()
        {

            GatherDisplay();
            //ShowEnteredRecords();
            ShowPotential("Fields");
            ShowPotential("Display");

        }

        public void RedirectDisplayFrame(string targetStr)
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

        private void ResetInput()
        {

            SSetItem = String.Empty;
            SelectedWeapon = String.Empty;
            IsSpellTrace = false;
            NoSlot = 0;
            SelectedScrollStat = null;
            ScrollRecord.Clear();
            FlameRecord.Clear();
            FirstLine = SecondLine = ThirdLine = null;
            CItemSelect = null;
        }

        #endregion
    }
}
