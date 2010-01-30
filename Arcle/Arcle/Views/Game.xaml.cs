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

using Arbaureal.Arcle;

namespace Arbaureal.Arcle.Views
{
    public partial class Game : Page
    {
        private Engine.Game m_Game;

        public Game()
        {
            InitializeComponent();

            m_Game = new Engine.Game(gameSurface, LayoutRoot);
            LayoutRoot.SizeChanged += new SizeChangedEventHandler(LayoutRoot_SizeChanged);
            m_Game.ScoreUpdateEvent += new Engine.Game.ScoreUpdateHandler(ScoreUpdateHandler);
            ScoreText.Text = "0";
        }

        void ScoreUpdateHandler(object sender, int nScore)
        {
            ScoreText.Text = nScore.ToString();
        }

        void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // The unscaled game size is 600 x 600 pixels so always scale relative to this
            double unscaledSize = 600.0;

            double scaleX = LayoutRoot.ActualHeight / unscaledSize;
            double scaleY = LayoutRoot.ActualWidth / unscaledSize;

            scaleTransform.ScaleX = Math.Min(scaleX, scaleY);
            scaleTransform.ScaleY = -Math.Min(scaleX, scaleY);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App currentApp = (App)Application.Current;
            currentApp.StopMainMenuMusic();

            m_Game.NavigationService = NavigationService;

            // Attach event handlers at the frame level
            currentApp.RootVisual.GotFocus += new RoutedEventHandler(Page_GotFocus);
            currentApp.RootVisual.LostFocus += new RoutedEventHandler(Page_LostFocus);
            currentApp.RootVisual.MouseLeftButtonDown += new MouseButtonEventHandler(Page_MouseLeftButtonDown);
            currentApp.RootVisual.KeyDown += new KeyEventHandler(m_Game.KeyDownEventHandler);

            m_Game.StartGame();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            m_Game.StopGame();

            // Remove all event handlers assigned at the Frame level
            App.Current.RootVisual.GotFocus -= new RoutedEventHandler(Page_GotFocus);
            App.Current.RootVisual.LostFocus -= new RoutedEventHandler(Page_LostFocus);
            App.Current.RootVisual.MouseLeftButtonDown -= new MouseButtonEventHandler(Page_MouseLeftButtonDown);
            App.Current.RootVisual.KeyDown -= new KeyEventHandler(m_Game.KeyDownEventHandler);
            
            base.OnNavigatedFrom(e);
        }

        void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_Game.ResumeGame();
        }

        void Page_LostFocus(object sender, RoutedEventArgs e)
        {
            m_Game.PauseGame();
        }

        void Page_GotFocus(object sender, RoutedEventArgs e)
        {
            m_Game.ResumeGame();
        }
    }
}
