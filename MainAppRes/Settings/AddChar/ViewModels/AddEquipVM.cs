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
using System.Reflection;
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

        public ObservableCollection<int> StarforceLevels { get; set; } = new ObservableCollection<int>();

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

        public ObservableCollection<string> EquipSlots { get; set; } = new ObservableCollection<string>();
      
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



        public ObservableCollection<string> CharacterWeapon { get; set; } = new ObservableCollection<string>();       

        private string _SEquipSlot; 
        public string SEquipSlot  // => EquipSlot's KEY
        {
            get { return _SEquipSlot; }
            set
            {
                _SEquipSlot = value;

                if (SEquipSlot != null)
                {
                    ItemDisType = ShowSet.Contains(AEM.EquipSlot[SEquipSlot]) ? "Set: " : "Name: ";

                    ShowWeapon = AEM.EquipSlot[SEquipSlot] == "Weapon" ? Visibility.Visible : Visibility.Collapsed;

                    
                    ShowEquipSet(SEquipSlot);
                    
                       
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
                if (CurrentSEquip != null && string.IsNullOrEmpty(value) == false)
                {
                    if(SEquipSlot == "Secondary")
                    {
                        SCharacter.CurrentSecondaryWeapon = SSetItem;
                    }
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
                if (SCharacter != null)
                {
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

        public List<PotentialStatsCLS> FirstPotL { get; set; } = new List<PotentialStatsCLS>();
        public List<PotentialStatsCLS > SecondPotL { get; set; } = new List<PotentialStatsCLS>();
        public List<PotentialStatsCLS> ThirdPotL { get; set; } = new List<PotentialStatsCLS>();
        
        private bool _isAddPot = false;
        public bool IsAddPot
        {
            get { return _isAddPot; }
            set
            {
                _isAddPot = value;

                if (CurrentSEquip != null)
                {
                    FirstLine = SecondLine = ThirdLine = null;
                    
                    SPotentialG = IsAddPot ? CurrentSEquip.APotGrade : CurrentSEquip.MPotGrade;

                    
                    if(CItemSelect != null)
                    {
                        DisplayPotential("Fields");
                    }
                }

                OnPropertyChanged(nameof(IsAddPot));
            }
        }

        private int _SelectedPotentialGrade = 0;
        public int SPotentialG
        {
            get { return _SelectedPotentialGrade; }
            set
            {
                _SelectedPotentialGrade = value;
                if(SPotentialG == -1)
                {
                    FirstLine = SecondLine = ThirdLine = null;
                    FirstPotL.Clear();
                    SecondPotL.Clear();
                    ThirdPotL.Clear();
                }
                if (SPotentialG != -1 && CurrentSEquip != null)
                {
                    FirstLine = SecondLine = ThirdLine = null;
                    
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

                if (FirstLine != null && SPotentialG != -1)
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


        private Visibility _ShowStarforce = Visibility.Collapsed;
        public Visibility ShowStarforce
        {
            get { return _ShowStarforce; }
            set { _ShowStarforce = value;
                OnPropertyChanged(nameof(ShowStarforce));
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
                    SEquipSlot = CItemSelect?.EquipListSlot;
                    SSetItem = ComFunc.ReturnSetCat(SEquipSlot) == "Accessory" || SEquipSlot == "Secondary" ? CItemSelect?.EquipName : CItemSelect?.EquipSet;
                    if(CItemSelect.EquipSlot == "Weapon")
                    {
                        SelectedWeapon = SCharacter.CurrentMainWeapon;
                    }
                    CurrentSEquip.UpdateFromDB(CItemSelect);
                    IsSpellTrace = CurrentSEquip.IsSpellTraced;
                    StarforceI = CurrentSEquip.StarForce;



                    if (CurrentSEquip.IsSpellTraced)
                    {
                        NoSlot = CurrentSEquip.SlotCount;
                        SelectedScrollStat = string.Format("{0}%", CurrentSEquip.SpellTracePerc);
                    }

                    ScrollRecord = CurrentSEquip.ScrollStats.ToRecord();
                    FlameRecord = CurrentSEquip.FlameStats.ToRecord();
                    CurrentSEquip.StarforceStats  = CalForm.CalStarforceStats(SCharacter, CurrentSEquip, AEM.StarforceStore[CurrentStarforceList].Values.ToList().AsReadOnly(), CurrentStarforceList);
                    UpdateDisplay();
                }
                DelEquipmentCMD.RaiseCanExecuteChanged();

                OnPropertyChanged(nameof(CItemSelect));
            }
        }


        //DISPLAY FINAL ITEM STATS
        #region
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

        public ObservableCollection<string> DisplayMainPotL { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> DisplayAddPotL { get; set; } = new ObservableCollection<string>();
        
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

        void HandleCharChange(object sender, CharacterCLS CC)
        {

            if (CC !=  SCharacter)
            {
                SCharacter = CC;

                initFields();
            }            
        }

        //TOGGLE DIFFERENT INPUT FRAMES
        public ObservableCollection<string> EnhancementType { get; set; } = new ObservableCollection<string>();

        private string _FrameSelection;
        public string FrameSelection
        {
            get { return _FrameSelection; }
            set
            {
                _FrameSelection = value;
                RedirectDisplayFrame(FrameSelection);
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
                AEM.EquipSlot.Keys.ToList().ForEach(x => EquipSlots.Add(x));

            }
            else
            {
                
                ArmorSet.Clear();
                CharacterWeapon.Clear();
                SCharacter?.MainWeapon.ForEach(x => CharacterWeapon.Add(x));
                SEquipSlot = null;
                CItemDictT.Clear();
                SCharacter?.EquipmentList.Values.ToList().ForEach(x =>  CItemDictT.Add(x));
                List<string> CurrentEquips = CItemDictT.Select(x => x.EquipListSlot).ToList();
                EquipSlots.Clear();
                foreach(string Eslot in AEM.EquipSlot.Keys)
                {
                    if (!CurrentEquips.Contains(Eslot))
                    {
                        EquipSlots.Add(Eslot);
                    }
                }
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
            if (EquipSlots.Contains(CurrentSEquip.EquipListSlot))
            {
                EquipSlots.Remove(CurrentSEquip.EquipListSlot);
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
            if (!EquipSlots.Contains(CurrentSEquip.EquipListSlot))
            {
                EquipSlots.Add(CurrentSEquip.EquipListSlot);
            }
            
            ResetInput();
        }

        private void ShowEquipSet(string selectedESlot)
        {
            
            if (SCharacter != null)
            {
                

                selectedESlot = ComFunc.ReturnRingPend(selectedESlot);
                string slotCat = ComFunc.ReturnSetCat(selectedESlot);
                ArmorSet.Clear();
                
                ComFunc.FilterBy(slotCat, SCharacter, selectedESlot, AEM.AllEquipStore[slotCat].AsReadOnly(), XenonEquipType).ForEach(x => ArmorSet.Add(x));
                
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
        
        private void DisplayEnhancementType()
        {

            EnhancementType.Clear();
            if (CurrentSEquip != null)
            {
                StarforceI = 0;
                ShowStarforce = GVar.EnhanceRestriction["Starforce"].Contains(CurrentSEquip.EquipSlot)  ? Visibility.Collapsed : Visibility.Visible;

                if (CurrentSEquip.EquipSlot == "Ring")
                {
                    if (CurrentSEquip.EquipName.Contains("Onyx") || CurrentSEquip.EquipName.Contains("Critical Ring") || CurrentSEquip.EquipSet == "Oz")
                    {
                        EnhancementType.Clear();
                    }
                    else if (CurrentSEquip.EquipSet == "Event")
                    {
                        EnhancementType.Add("Potential");  
                    }
                    else
                    {
                        EnhancementType.Add("Scroll");
                        EnhancementType.Add("Potential");
                    }

                }
                else
                {
                    foreach(string ET in GVar.EnhanceRestriction.Keys)
                    {
                        if (ET == "Starforce")
                        {
                            continue;
                        }
                        if (!GVar.EnhanceRestriction[ET].Contains(CurrentSEquip.EquipSlot))
                        {
                            EnhancementType.Add(ET);
                        }
                    }
                }

            }
        }
        public void GetCurrentEquipment()
        {
            
            if (SCharacter != null && SSetItem != null)
            {
                
                IsSpellTrace = false;
                 
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

                StarforceLevels.Clear();
                StarforceLevels.Add(0);
                AEM.StarforceStore[CurrentStarforceList].Select(x => x.SFLevel).ToList().ForEach(x => StarforceLevels.Add(x));


                CurrentSEquip = NewEquip;
                SPotentialG = -1;

                DisplayEnhancementType();
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
            Dictionary<string, Dictionary<string,List<PotentialStatsCLS>>> BasePotList = IsAddPot ? AEM.AllBonusPotDict : AEM.AllPotDict;

            FirstPotL = new List<PotentialStatsCLS>();
            foreach (PotentialStatsCLS pot in BasePotList[CurrentEquip.EquipSlot][GVar.PotentialGrade[PotType]])
            {
                if (CurrentEquip.EquipLevel > pot.MinLvl && CurrentEquip.EquipLevel < pot.MaxLvl)
                {
                    FirstPotL.Add(pot);
                }
            }
            OnPropertyChanged(nameof(FirstPotL));
        }

        private void ShowSecondPot()
        {
            SecondPotL =  new List<PotentialStatsCLS>(FirstPotL);

            foreach(string sC in GVar.RepeatOneMPot)
            {
                if (FirstLine.StatIncrease.Contains(sC))
                {
                    SecondPotL.Remove(FirstLine);
                    
                }
            }

            OnPropertyChanged(nameof(SecondPotL));
            
        }

        private void ShowThirdPot()
        {

            List<string> RepeatCondition = IsAddPot ? GVar.RepeatTwoAPot : GVar.RepeatTwoMPot;
            ThirdPotL = new List<PotentialStatsCLS>(FirstPotL);
            foreach (string sC in RepeatCondition)
            {
                if (SecondLine.StatIncrease.Contains(sC) && FirstLine.StatIncrease.Contains(sC))
                {
                    foreach (var pot in ThirdPotL.Where(x => x.StatIncrease.Contains(sC)).ToList())
                    {
                        ThirdPotL.Remove(pot);
                    }
                }
            }
            ThirdPotL.Add(new PotentialStatsCLS());
            OnPropertyChanged(nameof(ThirdPotL));
            
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
            selectedEquip.StarforceStats  = CalForm.CalStarforceStats(SCharacter, selectedEquip, AEM.StarforceStore[CurrentStarforceList], CurrentStarforceList);


            return selectedEquip;
        }

    
        private  void GatherDisplay()
        {

            //Dictionary<string, List<string>> ToDisplay = new Dictionary<string, List<string>>();
            
            Dictionary<string, DisplayStatValue> ToDisplay = new Dictionary<string, DisplayStatValue>();
            Dictionary<string, int> BaseStat = CurrentSEquip.BaseStats.ToRecord();
            Dictionary<string, int> ScrollStat = CurrentSEquip.ScrollStats.ToRecord();
            Dictionary<string, int> FlameStat = CurrentSEquip.FlameStats.ToRecord();
            Dictionary<string, int> SFStat = CurrentSEquip.StarforceStats.ToRecord();             
            
            foreach(string key in BaseStat.Keys)
            {
                DisplayStatValue temp  = new DisplayStatValue();
                temp.DBaseStat   = BaseStat[key].ToString();
                temp.DScrollStat = ScrollStat[key].ToString();
                temp.DFlameStat  = FlameStat[key].ToString();
                temp.DStarforceStat = SFStat[key].ToString();
                temp.Key = key;


                if(temp.ReturnTotal() > 0)
                {
                    temp.ReturnTotal();
                    ToDisplay.Add(key, temp);
                }
            }
            TotalRecordDisplay = ToDisplay;

        }
        private void DisplayPotential(string mode)
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

                                FirstLine  = FirstPotL.First(x => x.Equals(CurrentSEquip.AddPot["First"]));
                                SecondLine = SecondPotL.First(x => x.Equals(CurrentSEquip.AddPot["Second"]));
                                ThirdLine  = ThirdPotL.First(x => x.Equals(CurrentSEquip.AddPot["Third"]));

                            }
                        }
                        else
                        {

                            if (CurrentSEquip.MainPot.Values.ToList().Select(x => x.PotID).ToList().Sum() != 0)
                            {
                                RetrievePot(CurrentSEquip);

                                
                                //FirstLine = FirstPotL.Single(x => x.PotID == CurrentSEquip.MainPot["First"].PotID);
                                FirstLine = FirstPotL.First(x => x.Equals(CurrentSEquip.MainPot["First"]));
                                SecondLine = SecondPotL.First(x => x.Equals(CurrentSEquip.MainPot["Second"]));
                                ThirdLine = ThirdPotL.First(x => x.Equals(CurrentSEquip.MainPot["Third"]));
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
                            DisplayMainPotL.Clear();
                            DMP.ForEach(x => DisplayMainPotL.Add(x));

                            List<string> DAP = new List<string>();
                            foreach (string pot in CurrentSEquip.AddPot.Keys)
                            {
                                if (!CurrentSEquip.AddPot[pot].Equals(new PotentialStatsCLS()))
                                {
                                    DAP.Add(string.Format("{0} Line: {1}", pot, CurrentSEquip.AddPot[pot].DisplayStat));
                                }
                            }
                            DisplayAddPotL.Clear();
                            DAP.ForEach(x => DisplayAddPotL.Add(x));
                        }
                        break;
                }
                

                
            }
            
        }

        private void UpdateDisplay()
        {

            GatherDisplay();
            //ShowEnteredRecords();

            IsAddPot = true;
            IsAddPot = false;
            DisplayPotential("Display");

        }

        public void RedirectDisplayFrame(string targetStr)
        {
            FrameDis.DataContext = this;
            switch (targetStr)
            {
                case "Scroll":
                    FrameDis.Navigate(typeof(AddEquipScrollPage));
                    break;
                case "Flame":
                    FrameDis.Navigate(typeof(AddEquipFlamePage));
                    break;
                case "Potential":
                    FrameDis.Navigate(typeof(AddEquipPotentialPage));
                    break;
                case null:
                    FrameDis.Navigate(typeof(BlankPage));
                    break;
                default:
                    break;
            }
        }

        private void ResetInput()
        {
            SEquipSlot = null;
            SSetItem = null;
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
