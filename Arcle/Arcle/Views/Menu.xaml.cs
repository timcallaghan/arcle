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
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.Current.RootVisual.KeyDown += new KeyEventHandler(RootVisual_KeyDown);

            ButtonNewGame.Focus();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Current.RootVisual.KeyDown -= new KeyEventHandler(RootVisual_KeyDown);

            base.OnNavigatedFrom(e);
        }

        void RootVisual_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                ButtonHowToPlay.Focus();
                //ButtonHowToPlay.S
            }
            else if (e.Key == Key.Up)
            {
                ButtonHighScores.Focus();
            }
        }

        private void MainMenuLoop_MediaEnded(object sender, RoutedEventArgs e)
        {
            MainMenuLoop.Position = new TimeSpan(0);
            MainMenuLoop.Play();
        }

    }
}
