using MSEACalculator.CalculationRes.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MSEACalculator.CalculationRes.ViewPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArcaneQMPage : Page
    {
        public ArcaneQMPage()
        {
            this.InitializeComponent();
            this.DataContext = new QMArcaneVM();
        }


    }
}
