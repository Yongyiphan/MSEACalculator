using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using MSEACalculator.OtherRes.Database;

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

        public event EventHandler<CharTStore> RaiseChangeChar;

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
        private ObservableCollection<CharacterCLS> _charTrackList = new ObservableCollection<CharacterCLS>();
        public ObservableCollection<CharacterCLS> CharTrackList
        {
            get { return _charTrackList; }
            set { _charTrackList = value; OnPropertyChanged(nameof(CharTrackList)); }
        }


        //VARAIBLES
        private CharacterCLS _SelectedAllChar;
        public CharacterCLS SelectedAllChar
        {
            get { return _SelectedAllChar; }
            set
            {
                if (value != null)
                {
                    _SelectedAllChar = value;
                    OnPropertyChanged(nameof(SelectedAllChar));
                    OnChangedCharacter(new CharTStore(SelectedAllChar));
                    addCharTrackCMD.RaiseCanExecuteChanged();
                }
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


        private CharacterCLS _CharTSelect;
        public CharacterCLS CharTSelect
        {
            get { return _CharTSelect; }
            set { _CharTSelect = value;
                OnPropertyChanged(nameof(CharTSelect));
                removeCharTrackCMD.RaiseCanExecuteChanged();
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
        }

        

        private void initFields()
        {
            AllCharTrackM.AllCharList.ForEach(x => _AllCharList.Add(x));
            AllCharTrackM.AllCharTrackDB.Values.ToList().ForEach(x => _charTrackList.Add(x));
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
            string URank = ComFunc.returnUnionRank(SelectedAllChar.ClassName, charLvl);

            CharacterCLS tempChar = new CharacterCLS()
            {
                ClassName = SelectedAllChar.ClassName,
                UnionRank = URank,
                Level = charLvl,
                Starforce = sf
            };

            CharTrackList.Add(tempChar);
            AllCharList.Remove(SelectedAllChar);
            bool insertResult = DBAccess.insertCharTrack(tempChar);
            if (insertResult == false)
            {
                ComFunc.ErrorDia("This character has been added before.");
            }
            SelectedAllChar = null;
                
            LvlInput = "1";
            StarF = "0";

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

        protected virtual void OnChangedCharacter(CharTStore CC)
        {
            //EventHandler<CharTStore> raiseEvent = RaiseChangeChar;
            //if (raiseEvent != null)
            //{
            //    raiseEvent(this, CC);
            //}
            RaiseChangeChar?.Invoke(this, CC);
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
