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
using Media;
using System.Windows.Resources;

namespace Arbaureal.Arcle.Views
{
    // The order in the enum defines the vertical order of the button
    // array from top to bottom
    public enum MenuButtonEnum
    {
        NewGame = 0,
        HowToPlay = 1,
        HighScores = 2,
        Credits = 3
    }

    public class MenuButtonNavigator
    {
        public static MenuButtonEnum GetNextEnumValue(MenuButtonEnum value)
        {
            if (Enum.IsDefined(typeof(MenuButtonEnum), value + 1))
            {
                return value + 1;
            }
            else
            {
                return MenuButtonEnum.NewGame;
            }
        }

        public static MenuButtonEnum GetPreviousEnumValue(MenuButtonEnum value)
        {
            if (Enum.IsDefined(typeof(MenuButtonEnum), value - 1))
            {
                return value - 1;
            }
            else
            {
                return MenuButtonEnum.Credits;
            }
        }
    }

    public partial class Menu : Page
    {
        private Dictionary<HyperlinkButton, MenuButtonEnum> m_ButtonPosition;
        private Nullable<MenuButtonEnum> m_FocusedButton = null;

        public Menu()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {          
            App currentApp = (App)Application.Current;
            currentApp.RootVisual.KeyDown += new KeyEventHandler(RootVisual_KeyDown);
            currentApp.StartMainMenuMusic();

            m_ButtonPosition = new Dictionary<HyperlinkButton, MenuButtonEnum>();
            m_ButtonPosition.Add(ButtonNewGame, MenuButtonEnum.NewGame);
            m_ButtonPosition.Add(ButtonHowToPlay, MenuButtonEnum.HowToPlay);
            m_ButtonPosition.Add(ButtonHighScores, MenuButtonEnum.HighScores);
            m_ButtonPosition.Add(ButtonCredits, MenuButtonEnum.Credits);

            SetUpButtonEventHandlers(ButtonNewGame);
            SetUpButtonEventHandlers(ButtonHowToPlay);
            SetUpButtonEventHandlers(ButtonHighScores);
            SetUpButtonEventHandlers(ButtonCredits);

            ButtonNewGame.Focus();

            ArcleTextAnimation.Begin();
        }

        private void SetUpButtonEventHandlers(HyperlinkButton hypBtn)
        {
            hypBtn.MouseEnter += new MouseEventHandler(MenuButton_MouseEnter);            
            hypBtn.GotFocus += new RoutedEventHandler(MenuButton_GotFocus);
            hypBtn.LostFocus += new RoutedEventHandler(MenuButton_LostFocus);
            hypBtn.MouseMove += new MouseEventHandler(MenuButton_MouseMove);
        }

        void MenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            HyperlinkButton hypBtn = sender as HyperlinkButton;
            m_FocusedButton = m_ButtonPosition[hypBtn];
        }

        void MenuButton_GotFocus(object sender, RoutedEventArgs e)
        {
            HyperlinkButton hypBtn = sender as HyperlinkButton;
            m_FocusedButton = m_ButtonPosition[hypBtn];
        }

        void MenuButton_LostFocus(object sender, RoutedEventArgs e)
        {
            m_FocusedButton = null;
        }

        void MenuButton_MouseMove(object sender, MouseEventArgs e)
        {
            HyperlinkButton hypBtn = sender as HyperlinkButton;
            if (!hypBtn.IsFocused)
            {
                hypBtn.Focus();
            }

            m_FocusedButton = m_ButtonPosition[hypBtn];
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App currentApp = (App)Application.Current;
            currentApp.RootVisual.KeyDown -= new KeyEventHandler(RootVisual_KeyDown);
            base.OnNavigatedFrom(e);
        }

        void RootVisual_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (!m_FocusedButton.HasValue)
                {
                    ButtonNewGame.Focus();
                    m_FocusedButton = MenuButtonEnum.NewGame;
                }
                else
                {
                    m_FocusedButton = MenuButtonNavigator.GetNextEnumValue(m_FocusedButton.Value);
                    ILookup<MenuButtonEnum, HyperlinkButton> lookup = m_ButtonPosition.ToLookup(x => x.Value, x => x.Key);
                    HyperlinkButton val = lookup[m_FocusedButton.Value].Single();
                    val.Focus();

                }
            }
            else if (e.Key == Key.Up)
            {
                if (m_FocusedButton == null)
                {
                    ButtonCredits.Focus();
                    m_FocusedButton = MenuButtonEnum.Credits;
                }
                else
                {
                    m_FocusedButton = MenuButtonNavigator.GetPreviousEnumValue(m_FocusedButton.Value);
                    ILookup<MenuButtonEnum, HyperlinkButton> lookup = m_ButtonPosition.ToLookup(x => x.Value, x => x.Key);
                    HyperlinkButton val = lookup[m_FocusedButton.Value].Single();
                    val.Focus();
                }
            }
        }
    }
}
