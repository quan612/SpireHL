using System.Windows;

namespace SpireHL.Views
{
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for HullWindow.xaml
    /// </summary>
    public partial class HullWindow : Window
    {
        public HullWindow()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
                Source = e.Source
            };

            ScrollViewer scv = (ScrollViewer)sender;
            scv.RaiseEvent(eventArg);
            e.Handled = true;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //throw new System.NotImplementedException();
        }
    }
}
