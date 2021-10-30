using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using MSEACalculator.BossRes;
using MSEACalculator.OtherRes;
using System.Runtime.CompilerServices;

namespace MSEACalculator.CharacterRes.MesoRes
{
    public class MesoViewModel : INotifyPropertyChanged
    {


        //Init Variables
        public List<string> bossNameList { get; set; } = new List<string>();
        public List<string> charNameList { get; set; } = new List<string>();
        public List<int> daysList { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7};
        public List<string> displayType { get; set; } = new List<string>() { "Daily", "Weekly", "Monthly" };

        private string selectedMule;
        private List<string> difficultyList;
        private List<Boss> bossList;
        private string totalMeso;
        private int daysMultiplier;
        private string mesoViewBy;
        private string selectedBoss;
        private string selectedDifficulty;
        private string errorMessage;

        //private ICommand addBossCMD;


        public List<string> DifficultyList
        {
            get { return difficultyList; }
            set { difficultyList = value;
                OnPropertyChanged("DifficultyList");
            }
        }
            
        readonly BossMeso bossMeso = new BossMeso();

        public string SelectedMule
        {
            get
            {
                return selectedMule;
            }
            set
            {
                selectedMule = value;
                OnPropertyChanged("SelectedMule");
                displayBossResult();
                
            }
        }

        public List<Boss> BossList
        {
            get { return bossList; }
            set
            {
                bossList = value;
                OnPropertyChanged("BossList");
            }
        }

        public string TotalMeso
        {
            get { return totalMeso; }
            set
            {
                totalMeso = value;
                OnPropertyChanged("TotalMeso");
            }
        }

        public int DaysMultiplier
        {
            get { return daysMultiplier; }
            set { daysMultiplier = value;
                OnPropertyChanged("DaysMultiplier");
                displayBossResult();
            }
        }

        public string MesoViewBy
        {
            get { return mesoViewBy; }
            set {
                mesoViewBy = value;
                OnPropertyChanged("MesoViewBy");
                displayBossResult();
            }
        }

        public string SelectedBoss
        {
            get { return selectedBoss; }
            set 
            { 
                selectedBoss = value; 
                OnPropertyChanged("SelectedBoss"); 
                showDifficulty(); 
            }
        }

        public string SelectedDifficulty
        {
            get { return selectedDifficulty; }
            set
            {
                selectedDifficulty = value;
                OnPropertyChanged("SelectedDifficulty");
                if (selectedDifficulty != null)
                {
                    addBossCMD.RaiseCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CustomCommand addBossCMD { get; private set; }

        public MesoViewModel()
        {
            
            initloadFields(bossMeso.bossDict, bossMeso.charDict);

            addBossCMD = new CustomCommand(new Action(addBoss), canAddBoss);

            
        }

        private void initloadFields(Dictionary<int,Boss> bossDict, Dictionary<string, Character> charDict)
        {

            foreach (Boss bossItem in bossDict.Values)
            {

                if (!bossNameList.Contains(bossItem.name))
                {
                    bossNameList.Add(bossItem.name);
                };
            }
            foreach (Character charItem in charDict.Values)
            {
                if (!charNameList.Contains(charItem.className))
                {
                    charNameList.Add(charItem.className);
                };
            }
            
            DaysMultiplier = daysList[0];
            MesoViewBy = displayType[1];
        }


        public void displayBossResult()
        {
            if (SelectedMule != null)
            {
                BossList = DatabaseAccess.getCharBossList(SelectedMule);

                int dailyMeso = BossList.Where(boss => boss.entryType == "Daily").Sum(boss => boss.meso);
                int weeklyMeso = BossList.Where(boss => boss.entryType == "Weekly").Sum(boss => boss.meso);
                int monthlyMeso = BossList.Where(boss => boss.entryType == "Monthly").Sum(boss => boss.meso); ;
                int tMeso = 0;

                switch (MesoViewBy)
                {
                    case "Daily":

                        tMeso += dailyMeso;

                        break;
                    case "Weekly":
                        dailyMeso *= DaysMultiplier;
                        tMeso += dailyMeso + weeklyMeso;
                        break;
                    case "Monthly":
                        dailyMeso *= DaysMultiplier;
                        weeklyMeso *= 4;

                        tMeso = dailyMeso + weeklyMeso + monthlyMeso;
                        break;
                }
                

                TotalMeso = String.Format("{0:n0}", tMeso);
            }

            SelectedBoss = null;
            SelectedDifficulty = null;
        }


        public void showDifficulty()
        {
            var temp = new List<string>();
            if (SelectedBoss != null)
            {
                foreach (Boss boss in bossMeso.bossDict.Values)
                {
                    if (boss.name == SelectedBoss)
                    {
                        temp.Add(boss.difficulty);
                    }
                }
            }

            DifficultyList = temp;
        }

        public bool canAddBoss()
        {

            if (SelectedMule != null && SelectedBoss != null && selectedDifficulty != null)
            {
                return true;
            }

            return false;
        }
        
        public void addBoss()
        {
            ErrorMessage = "";

            foreach(Boss boss in bossMeso.bossDict.Values)
            {
                if (boss.name == SelectedBoss && boss.difficulty == SelectedDifficulty)
                {
                    bool insertResult = DatabaseAccess.insertCharBossList(SelectedMule, SelectedBoss, boss.BossID);

                    if (insertResult == true)
                    {
                        displayBossResult();
                    }
                    else
                    {
                        ErrorMessage = "Boss has been added before.";
                    }
                }
            }



            displayBossResult();
            addBossCMD.RaiseCanExecuteChanged();
        }

        private string testvar;

        public string TestVar
        {
            get { return testvar; }
            set { testvar = value; OnPropertyChanged("TestVar"); }
        }




            







    }
}
