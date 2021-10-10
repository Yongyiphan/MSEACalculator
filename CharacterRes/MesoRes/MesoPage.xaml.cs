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



//The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MSEACalculator.CharacterRes.MesoRes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MesoPage : Page
    {

        //// Retrieve BossList from DB
        //Dictionary<int, Boss> bossDict = DatabaseAccess.GetBossDB();
        ////Retrieve Character from DB

        //Dictionary<string, Character> charDict = DatabaseAccess.GetAllCharDB();

        //Character getCharGains = new Character();

        ////Blank List
        //List<string> bossNameList = new List<string>();
        //List<string> difficultyList = new List<string>();
        //List<string> charNameList = new List<string>();



        //Dictionary<int, Boss> bossList = CommonFunc.GetBossListAsync().Result;
        public MesoPage()
        {
            this.InitializeComponent();
            //initloadFields();
            this.DataContext = new MesoViewModel();

        }

        //public void initloadFields()
        //{


        //    foreach (Boss bossItem in bossDict.Values)
        //    {
        //        if (!difficultyList.Contains(bossItem.difficulty))
        //        {
        //            difficultyList.Add(bossItem.difficulty);
        //        };

        //        if (!bossNameList.Contains(bossItem.name))
        //        {
        //            bossNameList.Add(bossItem.name);
        //        };
        //    }
        //    foreach (Character charItem in charDict.Values)
        //    {
        //        if (!charNameList.Contains(charItem.className))
        //        {
        //            charNameList.Add(charItem.className);
        //        };
        //    }
        //    bossNameList.Add("Custom");
        //    charNameList.Add("Custom");

        //    BossSelectorCBox.ItemsSource = bossNameList;
        //    BossDifficulty.ItemsSource = difficultyList;
        //    MulesNameSelector.ItemsSource = charNameList;



        //}



        //private void AddBoss_Click(object sender, RoutedEventArgs e)
        //{

        //    bool insertStatus = DatabaseAccess.insertCharBossList("Adele", "Zakum", 6);

        //    if(insertStatus == false)
        //    {
                
        //    }

        //}

        //private void AddCustomItem_Click(object sender, RoutedEventArgs e)
        //{

        //    if (AddCustomItemPopUp.IsOpen)
        //    {
        //        AddCustomItemPopUp.IsOpen = false;
        //    }
        //}



        //private void MulesNameSelector_DropDownClosed(object sender, object e)
        //{
        //    //displayTxt.Text = MulesNameSelector.SelectedValue.ToString();

        //    if (MulesNameSelector.SelectedValue != null)
        //    {
        //        getCharGains = DatabaseAccess.getCharBossList(MulesNameSelector.SelectedValue.ToString());

        //        int totalMeso = 0;


        //        int weekDayM = int.Parse(weekDayMultiplier.SelectedIndex.ToString());
        //        int weekEndM = int.Parse(weekEndsMultiplier.SelectedIndex.ToString());

        //        foreach (Boss item in getCharGains.bossList)
        //        {
        //            if (item.entryType == "Daily")
        //            {
        //                totalMeso += item.meso * weekDayM;
        //            }
        //            if (item.entryType == "Weekly")
        //            {
        //                totalMeso += item.meso * weekEndM;

        //            }
        //            totalMeso += item.meso;
        //        }

        //        bossDisplay.ItemsSource = getCharGains.bossList;
        //        TotalMeso.Text = String.Format("{0:n0}", totalMeso);
        //    }

        //}

        //private void multiplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    MulesNameSelector_DropDownClosed(MulesNameSelector.SelectedItem, e);
        //}




    }


}
