using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using MSEACalculator.CharacterRes.EquipmentRes;

namespace MSEACalculator.MainAppRes.Settings
{
    public class AddCharTrackViewModel : INPCObject
    {
        public ACTModel AllCharTModel { get; set; } = new ACTModel();
        public ScrollingModel ScrollModel { get; set; } = new ScrollingModel();

        //Retrieving List/Data/Dict
        public Dictionary<string, string> EquipSlots { get { return AllCharTModel.EquipSlot; } }
        public List<string> FlameStatsTypes { get { return AllCharTModel.FlameStatsTypes; }}
        public List<string> ScrollTypes { get { return ScrollModel.ScrollTypes; } } //= new Scrolling().ScrollTypes;
        public List<int> Slots { get { return ScrollModel.Slots; } } //= new Scrolling().Slots;
        public List<Character> AllCharList { get { return AllCharTModel.AllCharList; } }

        //INIT Empty List
        private ObservableCollection<Character> charTrackList = new ObservableCollection<Character>();
        public ObservableCollection<Character> CharTrackList
        {
            get { return charTrackList; }
            set { charTrackList = value; OnPropertyChanged(nameof(CharTrackList)); }
        }

        private List<string> _ArmorSet;
        public List<string> ArmorSet
        {
            get { return _ArmorSet; }
            set
            {
                _ArmorSet = value;
                OnPropertyChanged(nameof(ArmorSet));
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

        //Flame Data Retrieved
        public Dictionary<string, int> FlameRecord { get; set; } = new Dictionary<string, int>();

        //Scrolling Data Retrieved
        private Dictionary<string, int> _ScrollRecord  = new Dictionary<string, int>();
        public Dictionary<string, int> ScrollRecord
        {
            get { return _ScrollRecord; }
            set
            {
                _ScrollRecord = value;
                OnPropertyChanged(nameof(ScrollRecord));
            }
        }

        private List<string> dictKey;

        public List<string> DictKey
        {
            get { return dictKey; }
            set { dictKey = value;
                OnPropertyChanged(nameof(DictKey));
            }
        }

        private List<int> dictValue;

        public List<int> DictValue
        {
            get { return dictValue; }
            set { dictValue = value;
                OnPropertyChanged(nameof(DictValue));
            }
        }
        

        //VARAIBLES
        private Character _SelectedAllChar;
        public Character SelectedAllChar
        {
            get { return _SelectedAllChar; }
            set 
            { 
                _SelectedAllChar = value; 
                OnPropertyChanged(nameof(SelectedAllChar)); 
                addCharTrackCMD.RaiseCanExecuteChanged(); 
            }
        }

        private string lvlI;
        public string LvlInput
        {
            get
            {
                return lvlI;
            }
            set
            {
                lvlI = value;
                OnPropertyChanged(nameof(LvlInput));
                addCharTrackCMD.RaiseCanExecuteChanged();
            }
        }

        private bool chkboxSelected;
        public bool ChkBoxSelected
        {
            get
            {
                return chkboxSelected;
            }
            set
            {
                chkboxSelected = value;
                ShowEquipmentPanel = chkboxSelected ? Visibility.Visible : Visibility.Collapsed;
                ShowBonusStat = SelectedESlot != null && ChkBoxSelected ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(ChkBoxSelected));
            }
        }

        //Visibility Controls
        private Visibility _ShowEquipmentPanel = Visibility.Collapsed;
        public Visibility ShowEquipmentPanel
        {
            get
            {
                return _ShowEquipmentPanel;
            }set
            {
                _ShowEquipmentPanel = value;
                OnPropertyChanged(nameof(ShowEquipmentPanel));
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

        private Visibility _ShowBonusStat = Visibility.Collapsed;
        public Visibility ShowBonusStat
        {
            get { return _ShowBonusStat; }
            set
            {
                _ShowBonusStat = value;
                OnPropertyChanged(nameof(ShowBonusStat));
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

        private string _SelectedESlot;
        public string SelectedESlot
        {
            get {return _SelectedESlot;} 
            set 
            { 
                _SelectedESlot = value;
                ShowWeapon = EquipSlots[SelectedESlot] == "Weapon" ? Visibility.Visible : Visibility.Collapsed;
                //Code For each types of EquipSlot
                ArmorSet = EquipSlots[SelectedESlot] == "Armor" ? AllCharTModel.ArmorSets : null;
                ShowBonusStat = SelectedESlot != null && ChkBoxSelected ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(SelectedESlot));
            }
        }

        private int _NoSlot;
        public int NoSlot
        {
            get { return _NoSlot; }
            set
            {
                _NoSlot = value;
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
                StatTypes = IsSpellTrace ? ScrollModel.SpellTraceTypes : GlobalVars.BaseStatTypes;
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

       
        private string _SelectedScrollStat;
        public string SelectedScrollStat
        {
            get { return _SelectedScrollStat; }
            set
            {
                _SelectedScrollStat = value;
                ShowScrollValue = ScrollModel.SpellTraceTypes.Contains(SelectedScrollStat) ? Visibility.Collapsed : Visibility.Visible;

                if (SelectedScrollStat != null) 
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

                AddStatCMD.RaiseCanExecuteChanged();
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
            set { _FlameStat = value;
                AddFlameCMD.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(FlameStat));
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

        public CustomCommand addCharTrackCMD { get; private set; }
        public CustomCommand AddStatCMD { get; private set; }
        public CustomCommand AddFlameCMD { get; private set; }

        public AddCharTrackViewModel()
        {
            initFields();

            addCharTrackCMD = new CustomCommand(addChar, canAddChar);
            AddStatCMD = new CustomCommand(AddStat, canAddStat);
            AddFlameCMD = new CustomCommand(AddFlame, canAddFlame);
        }

        private void initFields()
        {
            AllCharTModel.AllCharTrackDict.Values.ToList().ForEach(x => charTrackList.Add(x));
            StatTypes = GlobalVars.BaseStatTypes;
        }

        private bool canAddChar()
        {
            if(SelectedAllChar != null && LvlInput != null)
            {
                int outValue;
                if(int.TryParse(LvlInput, out outValue)){
                    return true;
                }
                CommonFunc.errorDia("Invalid Level Value");
                LvlInput = null;
                return false;
            }

            return false;
        }
        private void addChar()
        {
            
            //ObservableCollecion to List. => find if class already added before.
            if(CharTrackList.ToList().Find(x => x.className == SelectedAllChar.className) == null)
            {
                int charLvl = Convert.ToInt32(LvlInput);
                string URank = CommonFunc.returnUnionRank(SelectedAllChar.className, charLvl);

                Character tempChar = new Character(SelectedAllChar.className, URank, charLvl);
                CharTrackList.Add(tempChar);
                bool insertResult = DatabaseAccess.insertCharTrack(tempChar);
                if(insertResult == false)
                {
                    CommonFunc.errorDia("This character has been added before.");
                }
                SelectedAllChar = null;
                LvlInput = null;
            }
            else
            {
                //ErrorMessage = "This character has been added before.";
                CommonFunc.errorDia("This character has been added before.");
                SelectedAllChar = null;
                LvlInput = null;
            }
            addCharTrackCMD.RaiseCanExecuteChanged();
        }


        private bool canAddStat()
        {
            bool canAdd = false;
            //Check if stat is selected
            //Check if int value is keyed into value textbox
            if(SelectedScrollStat != null && ScrollStatValue != null)
            {
                if (int.TryParse(ScrollStatValue.ToString(), out int value))
                {
                    canAdd = true;
                }
                else{   
                    if(ScrollStatValue != string.Empty)
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
            //DictKey = ScrollRecord.Keys.ToList();
            //DictValue = ScrollRecord.Values.ToList();
        }


        private bool canAddFlame()
        {
            bool canAdd = false;
            //Flame Stat selected
            if (SelectedFlame != null)
            {
                //Check Value is INT
                if(int.TryParse(FlameStat, out int value))
                {
                    canAdd = true;
                }
                else
                {
                    if(FlameStat != string.Empty)
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
            DictKey = FlameRecord.Keys.ToList();
            DictValue = FlameRecord.Values.ToList();
        }
    }

}
