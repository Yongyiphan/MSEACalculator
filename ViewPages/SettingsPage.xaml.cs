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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MSEACalculator.ViewPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }


        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {

            var timer = new Stopwatch();
            timer.Start();
            var resetDB =  DatabaseAccess.databaseInit();
            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;

            timelapsed.Text = "Time Taken: " + timeTaken.ToString(@"m\:ss\.fff");
        }

        private void uploadDb_Click(object sender, RoutedEventArgs e)
        {
            var timer = new Stopwatch();
            timer.Start();
            DatabaseAccess.insertDB(DatabaseAccess.GetAllCharDB());
            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;

            timelapsed.Text = "Time Taken: " + timeTaken.ToString(@"m\:ss\.fff");
        }
    }
}
