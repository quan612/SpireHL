using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;

namespace SpireHL.Base
{
    public class AlternateRibbonApplicationMenu : RibbonApplicationMenu
    {
        public bool ShowAuxilaryPanel
        {
            get { return (bool)GetValue(ShowAuxilaryPanelProperty); }
            set { SetValue(ShowAuxilaryPanelProperty, value); }
        }

        public static readonly DependencyProperty ShowAuxilaryPanelProperty =
            DependencyProperty.Register("ShowAuxilaryPanel", typeof(bool),
                typeof(SlimRibbonApplicationMenu), new UIPropertyMetadata(true));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.DropDownOpened += SlimRibbonApplicationMenu_DropDownOpened;
        }

        void SlimRibbonApplicationMenu_DropDownOpened(object sender, EventArgs e)
        {
            DependencyObject popupObj = base.GetTemplateChild("PART_Popup");
            Popup panel = (Popup)popupObj;
            var exp = panel.GetBindingExpression(Popup.WidthProperty);

            if (!this.ShowAuxilaryPanel && exp == null)
            {
                DependencyObject panelArea = base.GetTemplateChild("PART_SubMenuScrollViewer");

                var panelBinding = new Binding("ActualWidth")
                {
                    Source = panelArea,
                    Mode = BindingMode.OneWay
                };
                panel.SetBinding(Popup.WidthProperty, panelBinding);
            }
            else if (this.ShowAuxilaryPanel && exp != null)
            {
                BindingOperations.ClearBinding(panel, Popup.WidthProperty);
            }
        }
    }
}
