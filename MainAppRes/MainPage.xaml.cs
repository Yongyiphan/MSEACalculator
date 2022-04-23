using MSEACalculator.CharacterRes;
using MSEACalculator.CharacterRes.MesoRes;
using MSEACalculator.MainAppRes.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MSEACalculator.MainAppRes
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

        private void navBar_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(HomePage));

        }

        private void navBar_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem navItem = args.SelectedItem as NavigationViewItem;

            if (args.IsSettingsSelected)
            {
                
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                switch (navItem.Tag.ToString())
                {
                    case "Home_Page":
                        ContentFrame.Navigate(typeof(HomePage));
                        break;
                    case "Meso_Page":
                        ContentFrame.Navigate(typeof(MesoPage));
                        break;
                    case "Character_Page":
                        ContentFrame.Navigate(typeof(CharacterPage));
                        break;
                    case "Quick_Math_Page":
                        ContentFrame.Navigate(typeof(QuickMathPage));
                        break;

                }
            }

        }

        
    }
}



