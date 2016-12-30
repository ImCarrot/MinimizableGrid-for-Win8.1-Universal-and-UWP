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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MinimizableGrid.Scenarios
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HeaderChangeOnMinimize : Page
    {
        public HeaderChangeOnMinimize()
        {
            this.InitializeComponent();
        }

        private void MinimizableGrid_OperationCompleted(object sender, UIElement e)
        {
            var element = (CustomControls.MinimizableGridControl)e;
            if(element!=null)
            {
                if ((string)element.Name == "MyVerticalMinimizableGrid")
                {
                    //if(MyVerticalMinimizableGrid.IsMinimized)
                    //    MyVerticalMinimizableGrid.IsShowingMinimizedView = true;
                    //else
                    //    MyVerticalMinimizableGrid.IsShowingMinimizedView = false;
                }
                else if ((string)element.Name == "MyHorrizontalMinimizableGrid")
                {
                    //Do anything here for example push layout or even do nothing
                }
            }
        }

        private void MinimizeClicked(object sender, RoutedEventArgs e)
        {
            MyVerticalMinimizableGrid.IsMinimized = !MyVerticalMinimizableGrid.IsMinimized;
            if (MyVerticalMinimizableGrid.IsMinimized)
            {
                MinimizeIcon.Icon = new SymbolIcon(Symbol.Upload);
                MinimizeIcon.Label = "Maximize";
            }
            else
            {
                MinimizeIcon.Icon = new SymbolIcon(Symbol.Download);
                MinimizeIcon.Label = "Minimize";
            }
        }

        private void HorrizontalMinimizeClicked(object sender, RoutedEventArgs e)
        {

            MyHorrizontalMinimizableGrid.IsMinimized = !MyHorrizontalMinimizableGrid.IsMinimized;
            if (MyHorrizontalMinimizableGrid.IsMinimized)
            {
                HorrizontalMinimizeIcon.Icon = new SymbolIcon(Symbol.Back);
                HorrizontalMinimizeIcon.Label = "Maximize";
            }
            else
            {
                HorrizontalMinimizeIcon.Icon = new SymbolIcon(Symbol.Forward);
                HorrizontalMinimizeIcon.Label = "Minimize";
            }
        }
    }
}
