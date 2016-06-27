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

        public static void InitialisePlayerMovement(Grid gameGrid)
        {
            gameGrid.MouseDown += new MouseButtonEventHandler(controller_MouseLeftButtonDown);
        }
        public static void controller_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var mousewasdownOn = e.Source as FrameworkElement;

            if (mousewasdownOn != null)
            {
                int elementNameC = (int)mousewasdownOn.GetValue(Grid.ColumnProperty);
                int elementNameR = (int)mousewasdownOn.GetValue(Grid.RowProperty);

                MessageBox.Show(string.Format("Grid clicked at column {0}, row {1}", elementNameC, elementNameR));

            }


        }


    }
}
