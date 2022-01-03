using MSEACalculator.BossRes;
using MSEACalculator.OtherRes;
using MSEACalculator.OtherRes.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSEACalculator.CharacterRes.MesoRes
{
    public class MesoViewModel : INPCObject
    {
        readonly BossMeso bossMeso;


        //Init Variables
        public List<int> daysList { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        public List<string> displayType { get; set; } = new List<string>() { "Daily", "Weekly", "Monthly" };

        public List<string> bossNameList { get; private set; }
        public List<string> charNameList { get; private set; }

        private string selectedMule;
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

        private List<string> difficultyList;
        public List<string> DifficultyList
        {
            get { return difficultyList; }
            set
            {
                difficultyList = value;
                OnPropertyChanged("DifficultyList");
            }
        }

        private List<Boss> bossList;
        public List<Boss> BossList
        {
            get { return bossList; }
            set
            {
                bossList = value;
                OnPropertyChanged("BossList");
            }
        }

        private string totalMeso;
        public string TotalMeso
        {
            get { return totalMeso; }
            set
            {
                totalMeso = value;
                OnPropertyChanged("TotalMeso");
            }
        }

        private int daysMultiplier;
        public int DaysMultiplier
        {
            get
            {
                return daysMultiplier;
            }
            set
            {
                daysMultiplier = value;
                OnPropertyChanged("DaysMultiplier");
                displayBossResult();
            }
        }

        private string mesoViewBy;
        public string MesoViewBy
        {
            get { return mesoViewBy; }
            set
            {
                mesoViewBy = value;
                OnPropertyChanged("MesoViewBy");
                displayBossResult();
            }
        }

        private string selectedBoss;
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

        private string selectedDifficulty;
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

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }

        private Boss selectedBossItem;
        public Boss SelectedBossItem
        {
            get
            {
                return selectedBossItem;
            }
            set
            {
                selectedBossItem = value;
                OnPropertyChanged("SelectedBossItem");
                deleteBossCMD.RaiseCanExecuteChanged();
            }

        }

        public CustomCommand addBossCMD { get; private set; }
        public CustomCommand deleteBossCMD { get; private set; }

        public MesoViewModel()
        {
            bossMeso = new BossMeso();
            bossNameList = bossMeso.bossNameList;
            charNameList = bossMeso.charNameList;
            daysMultiplier = bossMeso.DayMultiplier;
            mesoViewBy = bossMeso.MesoViewBy;


            addBossCMD = new CustomCommand(new Action(addBoss), canAddBoss);
            deleteBossCMD = new CustomCommand(new Action(deleteBoss), canDeleteBoss);
            
        }



        public void displayBossResult()
        {
            if (SelectedMule != null)
            {
                BossList = DBAccess.getCharBossList(SelectedMule);

                int dailyMeso = BossList.Where(boss => boss.EntryType == "Daily").Sum(boss => boss.Meso);
                int weeklyMeso = BossList.Where(boss => boss.EntryType == "Weekly").Sum(boss => boss.Meso);
                int monthlyMeso = BossList.Where(boss => boss.EntryType == "Monthly").Sum(boss => boss.Meso); ;
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
                    if (boss.BossName == SelectedBoss)
                    {
                        temp.Add(boss.Difficulty);
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
                if (boss.BossName == SelectedBoss && boss.Difficulty == SelectedDifficulty)
                {
                    bool insertResult = DBAccess.insertCharTBossList(SelectedMule, SelectedBoss, boss.BossID);

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

        public bool canDeleteBoss()
        {
            if(SelectedBossItem != null)
            {
                return true;
            }
            return false;
        }

        public void deleteBoss() 
        {

            bool deleteResult = DBAccess.deleteCharBossList(SelectedMule, SelectedBossItem.BossID);
            
            
            if(deleteResult == true)
            {
                displayBossResult();
            }
            else
            {
                ErrorMessage = "Unable to delete boss.";
            }
            SelectedBossItem = null;
            deleteBossCMD.RaiseCanExecuteChanged();
        }

        private string testvar;

        public string TestVar
        {
            get { return testvar; }
            set { testvar = value; OnPropertyChanged("TestVar"); }
        }




            







    }
}
