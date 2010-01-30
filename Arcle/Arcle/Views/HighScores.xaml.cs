using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace Arbaureal.Arcle.Views
{
    public partial class HighScores : Page
    {
        public HighScores()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App currentApp = (App)Application.Current;
            currentApp.RootVisual.GotFocus += new RoutedEventHandler(Page_GotFocus);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Remove all event handlers assigned at the Frame level
            App.Current.RootVisual.GotFocus -= new RoutedEventHandler(Page_GotFocus);
            base.OnNavigatedFrom(e);
        }

        void Page_GotFocus(object sender, RoutedEventArgs e)
        {
            ButtonMainMenu.Focus();
        }
    }
}
