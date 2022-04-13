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

        public ObservableCollection<CharacterCLS> AllCharList { get; set; } = new ObservableCollection<CharacterCLS>();
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
                    AddUpdateBtnTxt = "Add";
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
                if (int.TryParse(LvlInput, out int lvl))
                {
                    CurrentChar.Level = lvl;
                }
                
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
                if (int.TryParse(StarF, out int starF))
                {
                    CurrentChar.Starforce = starF;
                }
                OnPropertyChanged(nameof(StarF));
            }
        }


        public ObservableCollection<CharacterCLS> CharTrackList { get; set; } = new ObservableCollection<CharacterCLS>();

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
                    AddUpdateBtnTxt = "Update";
                    LvlInput = CurrentChar.Level.ToString();
                    StarF = CurrentChar.Starforce.ToString();
                    OnChangedCharacter(CurrentChar);
                    addCharTrackCMD.RaiseCanExecuteChanged();
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
            UpdateDBCMD = new CustomCommand(UpdateDB, canUpdateDB);
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
            AllCharTrackM.AllCharList.ForEach(x => AllCharList.Add(x));
            AllCharTrackM.AllCharTList.ForEach(x => CharTrackList.Add(x));
        }
        private bool canAddChar()
        {

            //SIMPLE ADD CHAR
            if (CurrentChar != null && LvlInput != null && StarF != null)
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
                UpdateCharacterLists();
                                    
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
        //End off Write method to mass update equipment list.
        private string _AddUpdateBtnTxt = "Add";
        public string AddUpdateBtnTxt
        {
            get { return _AddUpdateBtnTxt; }
            set 
            {
                _AddUpdateBtnTxt = value;
                OnPropertyChanged(nameof(AddUpdateBtnTxt));
            }
        }

        private bool canUpdateDB()
        {
            if (AEquipVM.CItemDictT.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void UpdateDB()
        {

            //CHECK FOR NULLS

            //ADD EQUIPMENT
            //CHECK IF CHARACTER IS IN CHARTRACK LIST
            // => ADD CHARACTER TO DB

            //ELSE CONTINUE TO ADDED EQUIPMENT
            //ADD EQUIPMENT TO RESPECTIVE DB

            foreach(CharacterCLS Character in CharTrackList)
            {
                List<int> TotalStarforce = new List<int>() { Character.Starforce};
                if(CurrentChar.EquipmentList.Count > 0)
                {
                    TotalStarforce.Add(CurrentChar.EquipmentList.Values.ToList().Select(x => x.StarForce).ToList().Sum());
                }
                CurrentChar.Starforce = TotalStarforce.Max();
                CurrentChar.UnionRank = ComFunc.ReturnUnionRank(CurrentChar.ClassName, Character.Level);

                bool InsertResult = DBAccess.InsertCharWithEquip(CurrentChar);
                if (InsertResult == false)
                {
                    continue;    
                }
            }

        }
    
        private void UpdateCharacterLists()
        {
            if (CharTrackList.Contains(CurrentChar) == false)
            {
                CharTrackList.Add(CurrentChar);
            }

            if (AllCharList.Contains(CurrentChar))
            {
                AllCharList.Remove(CurrentChar);
            }
            SelectedAllChar = null;
            CharTSelect = null;

        }

    }

}
