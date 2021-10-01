using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using MSEACalculator.BossRes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MSEACalculator.CharacterRes.MesoRes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MesoProgress : Page
    {
        List<String> MulesNameList = new List<string>() { "Adele", "Hoyoung","Hayato" };

        Dictionary<int, Boss> bossDict = DatabaseAccess.GetBossDB();
        Dictionary<string, Character> charDict = DatabaseAccess.GetAllCharDB();
        List<string> bossNameList = new List<string>();
        List<string> difficultyList = new List<string>();
        List<string> charNameList = new List<string>();

        

        //Dictionary<int, Boss> bossList = CommonFunc.GetBossListAsync().Result;
        public MesoProgress()
        {
            this.InitializeComponent();

            initloadFields();
        }

        public void initloadFields()
        {
            
            
            foreach(Boss bossItem in bossDict.Values)
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
            foreach(Character charItem  in charDict.Values)
            {
                if (!charNameList.Contains(charItem.className))
                {
                    charNameList.Add(charItem.className);
                };
            }
            bossNameList.Add("Custom");
            charNameList.Add("Custom");

            BossSelectorCBox.ItemsSource = bossNameList;
            BossDifficulty.ItemsSource = difficultyList;
            MulesNameSelector.ItemsSource = charNameList;

        }

        private void toHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void toScrollBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void toMesoBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void toEventBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddMesoItem_Click(object sender, RoutedEventArgs e)
        {
            //if (BossSelector.SelectedValue.ToString() == "Custom")
            //{
            //    if (!AddCustomItemPopUp.IsOpen)
            //    {
            //        AddCustomItemPopUp.IsOpen = true;
            //    }
            //}



        }

        private void AddCustomItem_Click(object sender, RoutedEventArgs e)
        {
            
            if (AddCustomItemPopUp.IsOpen)
            {
                AddCustomItemPopUp.IsOpen = false;
            }
        }

    }


}
