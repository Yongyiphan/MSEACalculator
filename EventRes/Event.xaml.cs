using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
using MSEACalculator.EventRes;

using Windows.Storage;
using Windows.Storage.Pickers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MSEACalculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Event : Page
    {
        EventRecords eRec = new EventRecords();
        

        public Event()
        {
            this.InitializeComponent();
            //retreive from json database if have

            
            


        }


        private void toHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void toScrollBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void startDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

            var temp = (DateTimeOffset)startDatePicker.Date;
            eRec.startDate = temp.DateTime;

        }

        private void endDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

            //Get Today's Date
            DateTime tdy = DateTime.Today;
            //Retrieve Date from Calendar Picker
            var temp = (DateTimeOffset)endDatePicker.Date;
            double difference = Math.Floor((temp.DateTime - tdy).TotalDays);
            //Display difference in days
            DaysLeft.Text = String.Format("{0} ", difference);

        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {

            List<EventRecords> result = CommonFunc.retrieveEventJson().Result;

            

            TestBlock.Text = String.Format("{0}", result.ToString() );
        }

        private void updateEventRecords_Click(object sender, RoutedEventArgs e)
        {
            //Validate fields are filled

            


        }
    }
}
