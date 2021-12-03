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
        public ACTModel AllCharTModel { get; set; } = new ACTModel();
        public ScrollingModel ScrollModel { get; set; } = new ScrollingModel();

        //Retrieving List/Data/Dict
        public Dictionary<string, string> EquipSlots { get { return AllCharTModel.EquipSlot; } }
        public List<string> FlameStatsTypes { get { return AllCharTModel.FlameStatsTypes; } }
        public List<string> ScrollTypes { get { return ScrollModel.ScrollTypes; } } //= new Scrolling().ScrollTypes;
        public List<int> Slots { get { return ScrollModel.Slots; } } //= new Scrolling().Slots;
        public List<Character> AllCharList { get { return AllCharTModel.AllCharList; } }

        public List<string> SlotSet { get; set; } = new List<string>
        {
            "Weapon", "Gloves", "Armor"
        };

        //INIT Empty List
        private ObservableCollection<Character> charTrackList = new ObservableCollection<Character>();
        public ObservableCollection<Character> CharTrackList
        {
            get { return charTrackList; }
            set { charTrackList = value; OnPropertyChanged(nameof(CharTrackList)); }
        }

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
        public ObservableCollection<EquipModel> CItemDictT { get; set; } = new ObservableCollection< EquipModel>();

        private bool _SyncCI = false;

        public bool SyncCI
        {
            get { return _SyncCI; }
            set { _SyncCI = value;
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
                OnPropertyChanged(nameof(CItemSelect));
                ShowSEquipStat = CItemSelect != null ? Visibility.Visible : Visibility.Collapsed;

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
                AddItemCMD.RaiseCanExecuteChanged();
            }
        }

        private string lvlI = GlobalVars.minLevel.ToString();
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

        private string _StarF = "0";
        public string StarF
        {
            get { return _StarF; }
            set 
            {
                _StarF = value;
                OnPropertyChanged(nameof(StarF));
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
                addCharTrackCMD.RaiseCanExecuteChanged();
            }
        }

        //Visibility Controls
        private Visibility _ShowEquipmentPanel = Visibility.Collapsed;
        public Visibility ShowEquipmentPanel
        {
            get
            {
                return _ShowEquipmentPanel;
            } set
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

        private Visibility _ShowSEquipStat = Visibility.Collapsed;

        public Visibility ShowSEquipStat
        {
            get { return _ShowSEquipStat; }
            set { _ShowSEquipStat = value;
                OnPropertyChanged(nameof(ShowSEquipStat));
            }
        }


        private string _SelectedESlot;
        public string SelectedESlot
        {
            get { return _SelectedESlot; }
            set
            {
                _SelectedESlot = value;
                ItemNameTypeTxt = SelectedESlot != null && SlotSet.Contains(EquipSlots[SelectedESlot]) ? "Set: " : "Item Name: ";
                ShowBonusStat = SelectedESlot != null && ChkBoxSelected ? Visibility.Visible : Visibility.Collapsed;
                 
                 
                ShowWeapon = ShowBonusStat == Visibility.Visible
                    && EquipSlots[SelectedESlot] == "Weapon" ? Visibility.Visible : Visibility.Collapsed;


                ShowEquipSet(SelectedESlot);
                IsSpellTrace = false;
                NoSlot = 0;
                SelectedScrollStat = null;

                OnPropertyChanged(nameof(SelectedESlot));
            }
        }

        private string _SelectedSetItem;
        public string SelectedSetItem
        {
            get { return _SelectedSetItem; }
            set
            {
                _SelectedSetItem = value;
                OnPropertyChanged(nameof(SelectedSetItem));
            }
        }

        private string _ItemNameTypeTxt = "Set: ";
        public string ItemNameTypeTxt
        {
            get { return _ItemNameTypeTxt; }
            set { 
                _ItemNameTypeTxt = value; 
                OnPropertyChanged(nameof(ItemNameTypeTxt)); }
        }

        private EquipModel _SelectedWeapon;
            
        public EquipModel SelectedWeapon
        {
            get { return _SelectedWeapon; }
            set { _SelectedWeapon = value;
                OnPropertyChanged(nameof(SelectedWeapon));
            }
        }


        private int _NoSlot;
        public int NoSlot
        {
            get { return _NoSlot; }
            set
            {
                _NoSlot = value;
                if(NoSlot == 0)
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

        private int _SelectedScrollIndex;

        public int SelectedScrollIndex
        {
            get { return _SelectedScrollIndex; }
            set { _SelectedScrollIndex = value;
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
                AddItemCMD.RaiseCanExecuteChanged();
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
                AddItemCMD.RaiseCanExecuteChanged();
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

        private Character _CharTSelect;
        public Character CharTSelect
        {
            get { return _CharTSelect; }
            set { _CharTSelect = value;
                OnPropertyChanged(nameof(CharTSelect));
                removeCharTrackCMD.RaiseCanExecuteChanged();
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
        public CustomCommand removeCharTrackCMD { get; set; }   
        public CustomCommand AddStatCMD { get; private set; }
        public CustomCommand AddFlameCMD { get; private set; }
        public CustomCommand AddItemCMD { get; private set; }

        public AddCharTrackViewModel()
        {
            initFields();

            addCharTrackCMD = new CustomCommand(addChar, canAddChar);
            removeCharTrackCMD = new CustomCommand(removeChar, canRemoveChar);
            AddStatCMD = new CustomCommand(AddStat, canAddStat);
            AddFlameCMD = new CustomCommand(AddFlame, canAddFlame);
            AddItemCMD = new CustomCommand(AddItem, canAddItem);
            ACTModel model = new ACTModel();
            
        }

        

        private void initFields()
        {
            AllCharTModel.AllCharTrackDict.Values.ToList().ForEach(x => charTrackList.Add(x));
            StatTypes = GlobalVars.BaseStatTypes;
        }

        private bool canAddChar()
        {
            if (!ChkBoxSelected)
            {
                //SIMPLE ADD CHAR
                if (SelectedAllChar != null && LvlInput != null && StarF != null)
                {
                    int lvlOutput, sfOutput;
                    if (int.TryParse(LvlInput, out lvlOutput) && int.TryParse(StarF, out sfOutput))
                    {

                        if (lvlOutput <= GlobalVars.maxLevel && lvlOutput >= GlobalVars.minLevel)
                        {
                            return true;
                        }
                    }
                    CommonFunc.errorDia("Invalid Level Value");
                    return false;
                }
            }
            else
            {
                //COMPLEX ADD CHAR WITH ITEMS
                if(CItemDictT.Count == 0)
                {
                    return false;
                }
                else
                {
                    if (SelectedAllChar != null && LvlInput != null && StarF != null)
                    {
                        int lvlOutput, sfOutput;
                        if (int.TryParse(LvlInput, out lvlOutput) && int.TryParse(StarF, out sfOutput))
                        {

                            if (lvlOutput <= GlobalVars.maxLevel && lvlOutput >= GlobalVars.minLevel)
                            {
                                return true;
                            }
                        }
                        CommonFunc.errorDia("Invalid Level Value");
                        return false;
                    }
                }
            }

           

            return false;
        }
        private void addChar()
        {

            //ObservableCollecion to List. => find if class already added before.
            //IF CHKBOX CHECKED == ADDING WITH ITEMS
            if (CharTrackList.ToList().Find(x => x.ClassName == SelectedAllChar.ClassName) == null)
            {
                int charLvl = Convert.ToInt32(LvlInput);
                
                int sf = Convert.ToInt32(StarF);
                string URank = CommonFunc.returnUnionRank(SelectedAllChar.ClassName, charLvl);

                Character tempChar = new Character(SelectedAllChar.ClassName, URank, charLvl, sf);
                CharTrackList.Add(tempChar);
                bool insertResult = DatabaseAccess.insertCharTrack(tempChar);
                if (insertResult == false)
                {
                    CommonFunc.errorDia("This character has been added before.");
                }
                SelectedAllChar = null;
                LvlInput = "1";
                StarF = "0";
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
        private void removeChar()
        {
            if(CharTSelect != null)
            {
                if (DatabaseAccess.deleteCharT(CharTSelect))
                {
                    CharTrackList.Remove(CharTSelect);
                }

            }
        }

        private bool canRemoveChar()
        {
            if(CharTSelect != null)
            {
                return true;
            }
            return false;
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
                else {
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
            AddItemCMD.RaiseCanExecuteChanged();
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
            AddItemCMD.RaiseCanExecuteChanged();
        }

        private void ShowEquipSet(string selectedESlot)
        {
            Func<string, ObservableCollection<string>, List<EquipModel>, ObservableCollection<string>> 
                FilterSet = (eSlot, displayList, itemList) =>
            {
                

                foreach(var item in itemList)
                {
                    if(item.EquipSlot == eSlot)
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
                foreach(var item in itemList)
                {
                    if(item.EquipSlot == eSlot)
                    {
                        if (!displayList.Contains(item.EquipName))
                        {
                            displayList.Add(item.EquipName);
                        }
                    }
                }

                return displayList;
            };
            
            switch (EquipSlots[selectedESlot])
            {
                case "Weapon":
                    break;
                case "Gloves":
                    ArmorSet.Clear();
                    ArmorSet = FilterSet(selectedESlot, ArmorSet, AllCharTModel.AllArmorList);
                    break;
                case "Armor":
                    ArmorSet.Clear();
                    ArmorSet = FilterSet(selectedESlot, ArmorSet, AllCharTModel.AllArmorList);
                    break;
                case "Accessory":
                    ArmorSet.Clear();
                    ArmorSet = DisplayItemNameL(selectedESlot, ArmorSet, AllCharTModel.AllAccList);
                    break;
                case "Ring":
                    ArmorSet.Clear();
                    ArmorSet = DisplayItemNameL(EquipSlots[selectedESlot], ArmorSet, AllCharTModel.AllAccList);
                    break;
                case "Pendant":
                    ArmorSet.Clear();
                    ArmorSet = DisplayItemNameL(EquipSlots[selectedESlot], ArmorSet, AllCharTModel.AllAccList);
                    break;
                default:
                    ArmorSet.Clear();
                    break;
            }

        }


        private bool canAddItem()
        {
            Func<bool,string,Dictionary<string, int>, Dictionary<string, int>, bool> 
                checkAddStatAdded = (isSpellTraced, scrollStat, scrollRecord, flameRecord) =>
            {
                //Spell trace, check slots, spell trace perc
                //check scrollrecord, flamerecords
                if (isSpellTraced)
                {
                    //scrollstat null == slotcount = 0
                    if(scrollStat != null && flameRecord.Count >= 0)
                    {
                        return true;
                    }
                }
                else
                {
                    //if flameRecord = 0 && scrollRecord = 0 == clean equip, no flame, no scroll
                    if(flameRecord.Count >= 0 && scrollRecord.Count >= 0)
                    {
                        return true;
                    }
                }

                return false;
            };
            if (SelectedAllChar != null)
            {
                if (SelectedESlot != null && SelectedSetItem != null)
                {
                    if (ShowWeapon == Visibility.Visible && SelectedWeapon != null)
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
            Func<Character, Dictionary<string, int>, Dictionary<string, int>, EquipModel>
                ConstructEquip = (character, scrollR, flameR) =>
            {
                EquipModel equipModel = new EquipModel();

                


                return equipModel;
            };

           //if scroll perc selected, slot >0
            //Information to gather
            //Selected Char
            //If armor -> EquipSet and Slot and ClassType <- Armor Data
            //If weapon -> EquipSet and Type
            //Else -> Set and Name. 
            switch (EquipSlots[SelectedESlot])
            {

                case "Armor":
                    Character selectedChar = AllCharTModel.AllCharDict[SelectedAllChar.ClassName];
                    EquipModel selectedEquip = new EquipModel();
                    selectedEquip = SelectedSetItem == "Fensalir" || SelectedSetItem == "Empress" ?
                        AllCharTModel.AllArmorList.Find(i => i.EquipSet == SelectedSetItem && i.EquipSlot == SelectedESlot && i.ClassType == selectedChar.ClassType ) :
                        AllCharTModel.AllArmorList.Find(i => i.EquipSet == SelectedSetItem && i.EquipSlot == SelectedESlot && i.ClassType == "All");
                    selectedEquip.EquipName = String.Format("{0} {1}", SelectedSetItem, SelectedESlot);
                    selectedEquip.ClassType = selectedChar.ClassType;
                    selectedEquip.SlotCount = NoSlot;
                    //Get Base Stats
                    selectedEquip = CommonFunc.retrieveConstructE(selectedChar, AllCharTModel.AllArmorList, selectedEquip);

                    if (IsSpellTrace)
                    {
                        string MainStat = selectedChar.MainStat, SecStat = selectedChar.SecStat;
                        int STTier = CommonFunc.SpellTraceTier(selectedEquip);
                        int perc = Convert.ToInt32(SelectedScrollStat.Remove(SelectedScrollStat.Length - 1));
                        selectedEquip.ScrollStats.HP = AllCharTModel.SpellTraceDict["Armor"][STTier][perc].HP * NoSlot;
                        selectedEquip.ScrollStats.DEF = AllCharTModel.SpellTraceDict["Armor"][STTier][perc].DEF *NoSlot;

                        if(MainStat == "HP")
                        {
                            selectedEquip.ScrollStats.HP += AllCharTModel.SpellTraceDict["Armor"][STTier][perc].MainStat * NoSlot * 50;
                        }
                        else
                        {
                            selectedEquip.ScrollStats.GetType().GetProperty(MainStat).SetValue(selectedEquip.ScrollStats, AllCharTModel.SpellTraceDict["Armor"][STTier][perc].MainStat * NoSlot, null);
                        }
                    }
                    

                    if(CItemDictT.ToList().Any(equip => equip.EquipSlot == selectedEquip.EquipSlot))
                    {
                        CommonFunc.errorDia("Equip added before");
                    }
                    else {
                        CItemDictT.Add(selectedEquip);
                    }
                    NoSlot = 0;

                    AddItemCMD.RaiseCanExecuteChanged();
                    break;
                default:
                    break;
            }
        }

        public void ShowEquipToAdd()
        {
            if(CItemDictT.Count != 0)
            {

            }
        }
    }

}
