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
    public sealed partial class MinimizeWithVisibility : Page
    {
        public MinimizeWithVisibility()
        {
            this.InitializeComponent();
            Loaded += MinimizeWithVisibility_Loaded;
        }

        private void MinimizeWithVisibility_Loaded(object sender, RoutedEventArgs e)
        {
            BlinkIdeaAnim.Begin();
        }

       
        private void IdeaImageGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TryGrid.IsMinimized = !TryGrid.IsMinimized;
        }
    }
}
