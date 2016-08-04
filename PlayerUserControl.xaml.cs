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
    /// Interaction logic for PlayerUserControl.xaml
    /// </summary>
    public partial class PlayerUserControl : UserControl
    {
        public bool faceingRight;
        public BitmapImage facingRightImage, facingLeftImage;
        SpritesheetImage playerTile;

        public PlayerUserControl()
        {
            InitializeComponent();
            facingRightImage = new BitmapImage(new Uri("pack://application:,,,/Resources/ZombieHunter_SpriteSheet.png", UriKind.Absolute));
            facingLeftImage = new BitmapImage(new Uri("pack://application:,,,/Resources/ZombieHunter_SpriteSheet_facingLeft.png", UriKind.Absolute));
            Loaded += PlayerUserControl_Loaded;
        }

        /// <summary>
        /// takes 1 argument - bool - is the player facing right. (bool isFacingRight)
        /// </summary>
        /// <param name="isFacingRight"></param>
        public void UpdateDirection(bool isFacingRight)
        {
            if (playerTile != null)
            {
                if (isFacingRight == true)
                {
                    playerTile.Source = facingRightImage;
                }
                else
                {
                    playerTile.Source = facingLeftImage;
                }
            }
            
        }

        private void PlayerUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            playerTile = new SpritesheetImage()
            {
                Source = facingRightImage,
                FrameMaxX = 5,
                FrameMaxY = 2,
                FrameRate = 30,
                Width = 64,
                Height = 64,
                PlaysRemaining = 10,
                LoopForever = true
            };
            playerTile.AnimationComplete += (o, s) =>
            {
                myCanvas.Children.Remove(playerTile);
            };

            myCanvas.Children.Add(playerTile);
        }
    }
}
