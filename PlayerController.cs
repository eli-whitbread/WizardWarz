using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;
using WizardWarz;

namespace WizardWarz
{
    class PlayerController : MainWindow
    {
        public static void InitialisePlayerController(Grid gameGrid)
        {
            Int32 tileSize = 64;

            Rectangle playerTile = new Rectangle();

            //playerTile = new Rectangle();

            playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD1.png", UriKind.Relative)));


            playerTile.Height = tileSize;
            playerTile.Width = tileSize;
            Grid.SetColumn(playerTile, 1);
            Grid.SetRow(playerTile, 1);

            gameGrid.Children.Add(playerTile);          
        }
    }
}
