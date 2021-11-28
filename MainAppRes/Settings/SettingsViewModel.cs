using MSEACalculator.OtherRes;
using System;
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
            //var test = DatabaseAccess.TestDBCon();
            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;

            //timelapsed.Text = "Time Taken: " + timeTaken.ToString(@"m\:ss\.fff");

            TimeElapsed = String.Format("Completed at {0}", timeTaken.ToString(@"m\:ss\.fff"));

            //TimeElapsed = test.ToString();


        }




    }
}
