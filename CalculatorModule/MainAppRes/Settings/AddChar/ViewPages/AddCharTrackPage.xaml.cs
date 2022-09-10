using MSEACalculator.MainAppRes.Settings.AddChar.ViewModels;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MSEACalculator.MainAppRes.Settings.AddChar.ViewPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddCharTrackPage : Page
    {
        public AddCharTrackVM ACTViewModel;


        public AddCharTrackPage()
        {
            this.InitializeComponent();
            
            ACTViewModel = new AddCharTrackVM();
            this.DataContext = ACTViewModel;
        }

        private void AddEqupmentNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem navItem = args.SelectedItem as NavigationViewItem;


            switch (navItem.Tag.ToString())
            {
                case "AddEquip_Page":
                    SAddContentFrame.Navigate(typeof(AddEquipPage));
                    SAddContentFrame.DataContext = ACTViewModel.AEquipVM;
                    break;
                case "AddArcane_Page":
                    break;


            }
        }
    }
}
