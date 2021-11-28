using MSEACalculator.OtherRes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSEACalculator.MainAppRes;
using MSEACalculator.CharacterRes.MesoRes;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace MSEACalculator.MainAppRes.Settings
{
    public class SettingsViewModel : INPCObject
    {

        public CustomCommand resetDBCMD { get; private set; }

        private string timeElapsed;

        public string TimeElapsed
        {
            get { return timeElapsed; }
            set { timeElapsed = value; OnPropertyChanged("TimeElapsed"); }
        }



        public SettingsViewModel()
        {
            resetDBCMD = new CustomCommand(resetDB, () => { return true; });

        }
        

        private void resetDB()
        {
            var timer = new Stopwatch();
            timer.Start();
            var resetDB = DatabaseAccess.databaseInit();
            //var testDB = DatabaseAccess.testDBCon();
            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;

            //timelapsed.Text = "Time Taken: " + timeTaken.ToString(@"m\:ss\.fff");

            TimeElapsed = String.Format("{0}", timeTaken.ToString(@"m\:ss\.fff"));

            //TimeElapsed = testDB.ToString();

        }




    }
}
