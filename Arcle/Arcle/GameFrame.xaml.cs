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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arbaureal.Arcle
{
    public partial class GameFrame : UserControl
    {
        public GameFrame()
        {
            InitializeComponent();                        
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ChildWindow errorWin = new Views.ErrorWindow(e.Uri);
            errorWin.Closed += new EventHandler(errorWin_Closed);
            errorWin.Show();
        }

        void errorWin_Closed(object sender, EventArgs e)
        {
            ContentFrame.Source = new Uri("/Menu", UriKind.Relative);
        }
    }
}