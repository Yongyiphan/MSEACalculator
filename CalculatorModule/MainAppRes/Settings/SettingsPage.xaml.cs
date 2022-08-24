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
using System.Diagnostics;
using MSEACalculator.MainAppRes.Settings.AddChar;
using MSEACalculator.MainAppRes.Settings.AddChar.ViewPages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MSEACalculator.MainAppRes.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {


        public SettingsPage()
        {
            this.InitializeComponent();

            SettingsGrid.DataContext = new SettingsViewModel();

        }

        private void SettingsNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem navItem = args.SelectedItem as NavigationViewItem;


            switch (navItem.Tag.ToString())
            {
                case "AddCharTrack_Page":
                    SettingsContentFrame.Navigate(typeof(AddCharTrackPage));
                    break;
                case "ModifyDB_Page":
                    break;


            }
            
        }
    }


}
