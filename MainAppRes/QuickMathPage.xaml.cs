using MSEACalculator.CalculationRes.View;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MSEACalculator.MainAppRes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuickMathPage : Page
    {
        public QuickMathPage()
        {
            this.InitializeComponent();
            this.DataContext = new QuickMathViewModel();
        }

        private void QMNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem navItem = args.SelectedItem as NavigationViewItem;


            switch (navItem.Tag.ToString())
            {
                case "ArcaneSym_Page":
                    QMContent.Navigate(typeof(ArcaneQMPage));
                    break;
                case "Conversion_Page":
                    break;


            }
        }
    }
}
