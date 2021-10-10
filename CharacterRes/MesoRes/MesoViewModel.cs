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

namespace MSEACalculator.CharacterRes.MesoRes
{
    public class MesoViewModel
    {

        Dictionary<int, Boss> bossDict = DatabaseAccess.GetBossDB();

        Dictionary<string, Character> charDict = DatabaseAccess.GetAllCharDB();


        //Blank List
        public List<string> bossNameList { get; set; }
        public List<string> difficultyList { get; set; }
        public List<string> charNameList { get; set; }



        public MesoViewModel()
        {
            initloadFields();

           
            
        }

        private void initloadFields()
        {
            bossNameList = new List<string>();
            difficultyList = new List<string>();
            charNameList = new List<string>();

            foreach (Boss bossItem in bossDict.Values)
            {
                if (!difficultyList.Contains(bossItem.difficulty))
                {
                    difficultyList.Add(bossItem.difficulty);
                };

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
            bossNameList.Add("Custom");
            charNameList.Add("Custom");
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //CustomCommand AddBoss_Click = new CustomCommand();

    }

}
