using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WizardWarz
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        GameWindow newGame;

        public MainMenu()
        {
            InitializeComponent();
        }

        public static bool GlobalPlayerMainMenu
        {
            get; set;
        }

        private void four_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {            
            GlobalPlayerMainMenu = false;
            RunWizardWarz();
        }

        private void six_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GlobalPlayerMainMenu = true;
            RunWizardWarz();
        }

        private void RunWizardWarz()
        {
            newGame = new GameWindow();

        }

        
    }
}
