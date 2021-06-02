using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;

namespace SpireHL.Base
{
    public class SlimRibbonApplicationMenu : RibbonApplicationMenu
    {
        private const double DefaultPopupWidth = 180;

        public double PopupWidth
        {
            get { return (double)GetValue(PopupWidthProperty); }
            set { SetValue(PopupWidthProperty, value); }
        }

        public static readonly DependencyProperty PopupWidthProperty =
            DependencyProperty.Register("PopupWidth", typeof(double),
                typeof(SlimRibbonApplicationMenu), new UIPropertyMetadata(DefaultPopupWidth));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.DropDownOpened +=
                new System.EventHandler(SlimRibbonApplicationMenu_DropDownOpened);
        }

        void SlimRibbonApplicationMenu_DropDownOpened(object sender, System.EventArgs e)
        {
            DependencyObject popupObj = base.GetTemplateChild("PART_Popup");
            Popup popupPanel = (Popup)popupObj;
            popupPanel.Width = (double)GetValue(PopupWidthProperty);
        }
    }
}
