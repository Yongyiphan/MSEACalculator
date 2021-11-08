using MSEACalculator.CharacterRes;
using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MSEACalculator.MainAppRes.Settings
{
    public class AddCharTrackViewModel :INPCObject
    {
        public Dictionary<string, Character> allCharDict { get; set; } = DatabaseAccess.GetAllCharDB();
        public Dictionary<string, Character> CharTrackDict { get; set; } = DatabaseAccess.GetAllCharTrackDB();

        public List<string> allCharList { get; } = new List<string>();

        private ObservableCollection<string> charTrackList;

        public ObservableCollection<string> CharTrackList
        {
            get { return charTrackList; }
            set { charTrackList = value; OnPropertyChanged(nameof(CharTrackList)); }
        }

        public List<string> equipSlots { get; set; } = GlobalVars.EquipmentSlots;

        private string selectedAllChar;

        public string SelectedAllChar
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

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public CustomCommand addCharTrackCMD { get; private set; }

        public AddCharTrackViewModel()
        {
            initFields();

            addCharTrackCMD = new CustomCommand(addChar, canAddChar);

        }

        private void initFields()
        {
            charTrackList = new ObservableCollection<string>();

            foreach(Character character in allCharDict.Values)
            {
                if (!allCharList.Contains(character.className))
                {
                    allCharList.Add(character.className);
                }
            }

            
            foreach(Character character in CharTrackDict.Values)
            {
                if (!CharTrackList.Contains(character.className))
                {
                    CharTrackList.Add(character.className);
                }
            }
        }

        private bool canAddChar()
        {
            if(SelectedAllChar != null)
            {
                return true;
            }

            return false;
        }

        private void addChar()
        {
            if (!CharTrackList.Contains(SelectedAllChar))
            {
                CharTrackList.Add(SelectedAllChar);
                OnPropertyChanged(nameof(CharTrackList));
                SelectedAllChar = null;
                ErrorMessage = "";
            }
            else
            {
                ErrorMessage = "This character has been added before.";
                Console.WriteLine(ErrorMessage);
            }
            addCharTrackCMD.RaiseCanExecuteChanged();
        }
    }

}
