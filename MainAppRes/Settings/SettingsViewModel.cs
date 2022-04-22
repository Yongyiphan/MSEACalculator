using MSEACalculator.OtherRes;
using MSEACalculator.OtherRes.Database;
using System;
using System.Diagnostics;

namespace MSEACalculator.MainAppRes.Settings
{
    public class SettingsViewModel : INPCObject
    {

        public CustomCommand resetDBTableCMD { get; private set; }
        public CustomCommand resetDBBlankCMD { get; private set; }

        private string timeElapsed;

        public string TimeElapsed
        {
            get { return timeElapsed; }
            set { timeElapsed = value; OnPropertyChanged("TimeElapsed"); }
        }



        public SettingsViewModel()
        {
            resetDBTableCMD = new CustomCommand(resetDBTable, () => { return true; });
            resetDBBlankCMD = new CustomCommand(resetDBBlank, () => { return true; });

        }


        private void resetDBTable()
        {
            var timer = new Stopwatch();
            timer.Start();
            var resetDB = DatabaseINIT.DBInit();
            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;

            //timelapsed.Text = "Time Taken: " + timeTaken.ToString(@"m\:ss\.fff");

            TimeElapsed = String.Format("Completed at {0}", timeTaken.ToString(@"m\:ss\.fff"));

            //TimeElapsed = test.ToString();

            //TimeElapsed = testDB.ToString();

        }
        private void resetDBBlank()
        {
            var timer = new Stopwatch();
            timer.Start();
            var resetDB = DatabaseINIT.BlankTablesInit();
            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;

            //timelapsed.Text = "Time Taken: " + timeTaken.ToString(@"m\:ss\.fff");

            TimeElapsed = String.Format("Completed at {0}", timeTaken.ToString(@"m\:ss\.fff"));

            //TimeElapsed = test.ToString();

            //TimeElapsed = testDB.ToString();

        }




    }
}
