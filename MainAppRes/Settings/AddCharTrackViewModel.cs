using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using MSEACalculator.CharacterRes.EquipmentRes;

namespace MSEACalculator.MainAppRes.Settings
{
    public class AddCharTrackViewModel :INPCObject
    {
        public Dictionary<string, Character> allCharDict { get; set; } = DatabaseAccess.GetAllCharDB();
        public Dictionary<string, Character> charTrackDict { get; set; } = DatabaseAccess.GetAllCharTrackDB();
        public List<EquipModel> ArmorList { get; set; } = DatabaseAccess.GetAllArmorDB();


        public List<Character> allCharList { get; set; }

        public List<string> armorSet { get; set; } = GlobalVars.ArmorSet;

        public Dictionary<string, string> equipSlots { get; set; } = GlobalVars.EquipmentDict;

        private ObservableCollection<Character> charTrackList;

        public ObservableCollection<Character> CharTrackList
        {
            get { return charTrackList; }
            set { charTrackList = value; OnPropertyChanged(nameof(CharTrackList)); }
        }


        private Character selectedAllChar;

        public Character SelectedAllChar
        {
            get { return selectedAllChar; }
            set 
            { 
                selectedAllChar = value; 
                OnPropertyChanged(nameof(SelectedAllChar)); 
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
                TestVar = value.ToString();
                IsVisible = chkboxSelected ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(ChkBoxSelected));
                

            }
        }
        private Visibility isVisible = Visibility.Collapsed;
        public Visibility IsVisible
        {
            get
            {
                return isVisible;
            }set
            {
                isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
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
        public CustomCommand validateInput { get; private set; }
        public AddCharTrackViewModel()
        {
            initFields();

            addCharTrackCMD = new CustomCommand(addChar, canAddChar);

        }

        private void initFields()
        {
            charTrackList = new ObservableCollection<Character>();
            allCharList = allCharDict.Values.ToList();
            charTrackDict.Values.ToList().ForEach(x => charTrackList.Add(x));

            
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


    }

}
