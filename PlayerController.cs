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
    class PlayerController
    {
        public Rectangle playerTile;
        public Point currentPOS;
        public Point lastClickPOS;
        public Point relativePosition;
        public Grid localGameGrid;

        public PlayerController(Grid gameGrid)
        {
            localGameGrid = gameGrid;

            InitialisePlayerController();

            gameGrid.MouseDown += new MouseButtonEventHandler(controller_MouseLeftButtonDown);
        }

        public void InitialisePlayerController()
        {
            Int32 tileSize = 64;

            playerTile = new Rectangle();            

            currentPOS = new Point(0, 0);

            playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD1.png", UriKind.Relative)));

            int playerX = (int)currentPOS.X;
            int playerY = (int)currentPOS.Y;

            playerTile.Height = tileSize;
            playerTile.Width = tileSize;
            Grid.SetColumn(playerTile, playerX);
            Grid.SetRow(playerTile, playerY);
            localGameGrid.Children.Add(playerTile);
        }


        public void controller_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousewasdownOn = e.Source as FrameworkElement;

            if (mousewasdownOn != null)
            {
                int elementNameC = (int)mousewasdownOn.GetValue(Grid.ColumnProperty);
                int elementNameR = (int)mousewasdownOn.GetValue(Grid.RowProperty);

                relativePosition = mousewasdownOn.TransformToAncestor(localGameGrid).Transform(new Point(0, 0));




                lastClickPOS = new Point(elementNameC, elementNameR);

                //MessageBox.Show(string.Format("Grid clicked at column {0}, row {1}", elementNameC, elementNameR));
                testPlayerMove(relativePosition);

            }


        }

        public void testPlayerMove(Point lastClickNEW)
        {
            TranslateTransform translateTransform1 = new TranslateTransform(relativePosition.X, relativePosition.Y);

            playerTile.RenderTransform = translateTransform1;

            localGameGrid.Children.Remove(playerTile);
            localGameGrid.Children.Add(playerTile);

            //MessageBox.Show(string.Format("I should be moving to {0}", relativePosition));


        }

    }
}
