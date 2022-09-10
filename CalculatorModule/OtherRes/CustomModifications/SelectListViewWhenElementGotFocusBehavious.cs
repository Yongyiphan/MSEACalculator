using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MSEACalculator.OtherRes.Database.CustomModifications
{
    public class SelectListViewWhenElementGotFocusBehavious : DependencyObject, IBehavior
    {
        private UIElement _element;
        public DependencyObject AssociatedObject { get; set; }
        public ListView ListView
        {
            get => (ListView)GetValue(ListViewProperty);
            set => SetValue(ListViewProperty, value);
        }

        public static readonly DependencyProperty ListViewProperty = DependencyProperty.Register("ListView", typeof(ListView), typeof(SelectListViewWhenElementGotFocusBehavious), new PropertyMetadata(null));

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            _element = this.AssociatedObject as UIElement;

            if (_element != null)
                _element.GotFocus += OnElementGotFocus;

        }
        private void OnElementGotFocus(object sender, RoutedEventArgs e)
        {
            var item = ((TextBox)sender).DataContext;
            var container = (ListViewItem)ListView.ContainerFromItem(item);

            container.IsSelected = true;
        }

        public void Detach()
        {
            if (_element != null)
                _element.GotFocus -= OnElementGotFocus;
        }
    }
}
