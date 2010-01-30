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
using System.Windows.Resources;
using Media;

namespace Arbaureal.Arcle
{
    public partial class App : Application
    {
        public Grid m_rootGrid = new Grid();
        private MediaElement m_MainMenuLoop;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = m_rootGrid;
            m_MainMenuLoop = new MediaElement();
            m_rootGrid.Children.Add(new GameFrame());
            m_rootGrid.Children.Add(m_MainMenuLoop);
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // a ChildWindow control.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                ChildWindow errorWin = new Views.ErrorWindow(e.ExceptionObject);
                errorWin.Show();
            }
        }

        public void StartMainMenuMusic()
        {
            if (m_MainMenuLoop.CurrentState != MediaElementState.Playing)
            {
                Uri soundUri = new Uri("/Arcle;component/Sounds/main_menu_loop.mp3", UriKind.Relative);
                StreamResourceInfo sri = Application.GetResourceStream(soundUri);

                if (sri != null && sri.Stream != null)
                {
                    Mp3MediaStreamSource mediaSource = new Mp3MediaStreamSource(sri.Stream);
                    mediaSource.Loop = true;
                    m_MainMenuLoop.SetSource(mediaSource);
                }
            }
        }

        public void StopMainMenuMusic()
        {
            m_MainMenuLoop.Stop();
        }
    }
}