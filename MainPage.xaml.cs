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
using MSEACalculator.BossRes;
using MSEACalculator.EventRes;
using MSEACalculator.StarforceRes;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MSEACalculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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
            this.Frame.Navigate(typeof(MesoProgress));
        }

        private void toEventBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Event));
        }
    }
}
