using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using MSEACalculator.OtherRes.Database;
using MSEACalculator.CharacterRes.EquipmentRes;
using System.Collections.Specialized;

namespace MSEACalculator.MainAppRes.Settings.AddChar.ViewModels
{
    public class AddCharTrackVM : INPCObject
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
        public ACTModel AllCharTrackM { get; set; } = new ACTModel();


        private AddEquipVM _AEquipVM;
        public AddEquipVM AEquipVM 
        {
            get=> _AEquipVM;
            set 
            {
                _AEquipVM = value;
                OnPropertyChanged(nameof(AEquipVM));
            } 
        }

        private ObservableCollection<CharacterCLS> _AllCharList = new ObservableCollection<CharacterCLS>();
        public ObservableCollection<CharacterCLS> AllCharList 
        {
            get => _AllCharList; 
            set 
            { 
                _AllCharList = value;
                OnPropertyChanged(nameof(AllCharList));
            } 
        }

        //INIT Empty List
        


        //VARAIBLES
        public CharacterCLS CurrentChar { get; set; }

        private CharacterCLS _SelectedAllChar;
        public CharacterCLS SelectedAllChar
        {
            get { return _SelectedAllChar; }
            set
            {
                _SelectedAllChar = value;
                if (value != null)
                {
                    CurrentChar = SelectedAllChar;
                    CharTSelect = null;
                    OnChangedCharacter(CurrentChar);
                    addCharTrackCMD.RaiseCanExecuteChanged();
                }
                OnPropertyChanged(nameof(SelectedAllChar));
            }
        }

        private string lvlI = GVar.minLevel.ToString();
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

        private ObservableCollection<CharacterCLS> _charTrackList = new ObservableCollection<CharacterCLS>();
        public ObservableCollection<CharacterCLS> CharTrackList
        {
            get { return _charTrackList; }
            set 
            {
                _charTrackList = value; 
                OnPropertyChanged(nameof(CharTrackList)); 
            }
        }

        private CharacterCLS _CharTSelect;
        public CharacterCLS CharTSelect
        {
            get { return _CharTSelect; }
            set 
            {
                _CharTSelect = value;
                if(CharTSelect != null)
                {
                    CurrentChar = CharTSelect;
                    SelectedAllChar = null;
                    OnChangedCharacter(CurrentChar);
                    removeCharTrackCMD.RaiseCanExecuteChanged();
                }
                OnPropertyChanged(nameof(CharTSelect));
            }
        }

      

        public CustomCommand addCharTrackCMD { get; private set; }
        public CustomCommand removeCharTrackCMD { get; set; }   
        public CustomCommand UpdateDBCMD { get; set;} 
     
        public AddCharTrackVM()
        {
            initFields();
            AEquipVM = new AddEquipVM(this);
            addCharTrackCMD = new CustomCommand(addChar, canAddChar);
            removeCharTrackCMD = new CustomCommand(removeChar, canRemoveChar);
            UpdateDBCMD = new CustomCommand(AddCharE, canAddCharE);
            AEquipVM.CItemDictT.CollectionChanged += HandleDictChange;

        }

        void HandleDictChange(object sender, NotifyCollectionChangedEventArgs EC)
        {
            ObservableCollection<EquipCLS> EquipDict =  AEquipVM.CItemDictT;
            if(CurrentChar != null && EquipDict.Count > 0)
            {
                foreach(EquipCLS equip in EquipDict)
                {
                    if (CurrentChar.EquipmentList.ContainsKey(equip.EquipListSlot))
                    {
                        EquipCLS current = CurrentChar.EquipmentList[equip.EquipListSlot];
                        if (current.Equals(equip))
                        {
                            continue;
                        }
                        else
                        {
                            CurrentChar.EquipmentList[equip.EquipListSlot] = equip;
                        }
                    }
                    else
                    {
                        CurrentChar.EquipmentList.Add(equip.EquipListSlot, equip);
                    }
                }


                //CurrentChar.Starforce = CurrentChar.EquipmentList.Values.ToList().Select(x => x.StarForce).ToList().Sum();

            }
        }

        public event EventHandler<CharacterCLS> RaiseChangeChar;
        protected virtual void OnChangedCharacter(CharacterCLS CC)
        { 
            RaiseChangeChar?.Invoke(this, CC);
        }

       

        private void initFields()
        {
            AllCharTrackM.AllCharList.ForEach(x => _AllCharList.Add(x));
            AllCharTrackM.AllCharTList.ForEach(x => _charTrackList.Add(x));
        }
        private bool canAddChar()
        {

            //SIMPLE ADD CHAR
            if (SelectedAllChar != null && LvlInput != null && StarF != null)
            {
                int lvlOutput, sfOutput;
                if (int.TryParse(LvlInput, out lvlOutput) && int.TryParse(StarF, out sfOutput))
                {

                    if (lvlOutput <= GVar.maxLevel && lvlOutput >= GVar.minLevel)
                    {
                        return true;
                    }
                }
                ComFunc.ErrorDia("Invalid Level Value");
                return false;
            }           

            return false;
        }
        private void addChar()
        {

            //ObservableCollecion to List. => find if class already added before.
            
            int charLvl = Convert.ToInt32(LvlInput);
                
            int sf = Convert.ToInt32(StarF);
            CurrentChar.Level = charLvl;
            List<int> TotalStarforce = new List<int>() { sf};
            if(CurrentChar.EquipmentList.Count > 0)
            {
                TotalStarforce.Add(CurrentChar.EquipmentList.Values.ToList().Select(x => x.StarForce).ToList().Sum());
            }
            CurrentChar.Starforce = TotalStarforce.Max();
            CurrentChar.UnionRank = ComFunc.ReturnUnionRank(CurrentChar.ClassName, charLvl);

            bool InsertResult = DBAccess.InsertCharWithEquip(CurrentChar);
            if (InsertResult == false)
            {
                ComFunc.ErrorDia("This character has been added before.");
            }
 

            if (InsertResult)
            {

                CharTrackList.Add(CurrentChar);
                AllCharList.Remove(CurrentChar);
                SelectedAllChar = null;
                    
                LvlInput = "1";
                StarF = "0";
            }

            addCharTrackCMD.RaiseCanExecuteChanged();
        }
        private bool canRemoveChar()
        {
            if (CharTSelect != null)
            {
                return true;
            }
            return false;
        }
        private void removeChar()
        {

            if (DBAccess.deleteCharT(CharTSelect))
            {
                AllCharList.Add(AllCharTrackM.AllCharDB[CharTSelect.ClassName]);
                CharTrackList.Remove(CharTSelect);
            }

        }

        private bool canAddCharE()
        {
            if (AEquipVM.CItemDictT.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void AddCharE()
        {

            //CHECK FOR NULLS

            //ADD EQUIPMENT
            //CHECK IF CHARACTER IS IN CHARTRACK LIST
            // => ADD CHARACTER TO DB

            //ELSE CONTINUE TO ADDED EQUIPMENT
            //ADD EQUIPMENT TO RESPECTIVE DB


        }


    }

}
