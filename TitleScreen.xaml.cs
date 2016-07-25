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
    /// Interaction logic for TitleScreen.xaml
    /// </summary>
    public partial class TitleScreen : UserControl
    {
        AudioManager titleScreenSound = new AudioManager();
        System.Windows.Threading.DispatcherTimer dTimer1 = new System.Windows.Threading.DispatcherTimer();
        public bool dTimerBool = false;
        int picCount = 0;
        public TitleScreen()
        {
            InitializeComponent();
            dTimer1.Tick += DTimer1_Tick;
            dTimer1.Interval = new TimeSpan(0, 0, 0, 0, 150);
            dTimer1.Start();
            titleScreenSound.playTitleSound();
            
        }

        private void DTimer1_Tick(object sender, EventArgs e)
        {
            if(picCount <= 3)
            {
                flashText();

            }
            else
            {
                picCount = 0;
                flashText();
            }
            
        }

        private void flashText()
        {
            //if (!dTimerBool)
            //{
            //    wizardText.Source = new BitmapImage(new Uri("Resources/WizardWarzText2.png", UriKind.Relative));
            //    dTimerBool = true;
            //}
            //else
            //{

            //    wizardText.Source = new BitmapImage(new Uri("Resources/WizardWarzText1.png", UriKind.Relative));
            //    dTimerBool = false;
            //}

            switch (picCount)
            {
                case 0:
                    wizardText.Source = new BitmapImage(new Uri("Resources/WizardWarzText2.png", UriKind.Relative));
                    picCount++;
                    break;

                case 1:
                    wizardText.Source = new BitmapImage(new Uri("Resources/WizardWarzText4.png", UriKind.Relative));
                    picCount++;
                    break;

                case 2:
                    wizardText.Source = new BitmapImage(new Uri("Resources/WizardWarzText3.png", UriKind.Relative));
                    picCount++;
                    break;

                case 3:
                    wizardText.Source = new BitmapImage(new Uri("Resources/WizardWarzText1.png", UriKind.Relative));
                    picCount++;
                    break;

                default:
                    wizardText.Source = new BitmapImage(new Uri("Resources/WizardWarzText.png", UriKind.Relative));
                    picCount = 0;
                    break;
            }

        }
    }
}
