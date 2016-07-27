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
using System.Diagnostics;

namespace WizardWarz
{

    public class PlayerController
    {
        public Rectangle playerTile;
        public Point currentPOS;
        public Point lastClickPOS;
        public Point relativePosition;
        //public Grid localGameGrid;
        public Int32 tileSize = 64;

        public GameBoardManager mangerRef = null;

        public int playerX;
        public int playerY;

        public bool playerClicked;
        public string playerName;

        public static Grid localGameGrid { get; internal set; }

        // OnClick event handler for the players' rectangles
        private void Rectangle_MouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(string.Format("Player rectangle last pos: {0}", lastClickPOS));
            playerClicked = true;
        }

        public PlayerController(Grid gameGrid, string pName)
        {
            localGameGrid = gameGrid;

            InitialisePlayerController(pName);

            relativePosition.Offset(tileSize, tileSize);

            testPlayerMove();

            gameGrid.MouseDown += new MouseButtonEventHandler(controller_MouseLeftButtonDown);
        }

        public void InitialisePlayerController(string playerName)
        {
            playerTile = new Rectangle();

            // Initialise onclick event handler
            playerTile.MouseRightButtonDown += Rectangle_MouseDown;

            int playerX = (int)currentPOS.X;
            int playerY = (int)currentPOS.Y;

            playerTile.Height = tileSize;
            playerTile.Width = tileSize;

            switch (playerName)
            {
                case ("playerOne"):
                    playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD1.png", UriKind.Relative)));
                    // Player one starts in the default position
                    break;
                case ("playerTwo"):
                    playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD2.png", UriKind.Relative)));
                    relativePosition.X = (7 * 64);      // Start this player in the middle of the top row
                    relativePosition.Y = (0 * 64);
                    break;
                case ("playerThree"):
                    playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD3.png", UriKind.Relative)));
                    relativePosition.X = (14 * 64);      // Start this player in the top right corner
                    relativePosition.Y = (0 * 64);
                    break;
                case ("playerFour"):
                    playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD4.png", UriKind.Relative)));
                    relativePosition.X = (0 * 64);      // Start this player in the bottom left corner
                    relativePosition.Y = (10 * 64);
                    break;
                case ("playerFive"):
                    playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD5.png", UriKind.Relative)));
                    relativePosition.X = (7 * 64);      // Start this player in the middle of the bottom row
                    relativePosition.Y = (10 * 64);
                    break;
                case ("playerSix"):
                    playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD6.png", UriKind.Relative)));
                    relativePosition.X = (14 * 64);      // Start this player in the bottom right corner
                    relativePosition.Y = (10 * 64);
                    break;
            }


            Grid.SetColumn(playerTile, playerX);
            Grid.SetRow(playerTile, playerY);

            localGameGrid.Children.Add(playerTile);


        }


        public void InitialiseRefs()
        {


        }

        public void controller_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousewasdownOn = e.Source as FrameworkElement;

            foreach (object child in localGameGrid.Children)
            {
                Console.WriteLine(child);
            }

            if (mousewasdownOn != null && e.RightButton == MouseButtonState.Pressed)
            {
                int elementNameC = (int)mousewasdownOn.GetValue(Grid.ColumnProperty);
                int elementNameR = (int)mousewasdownOn.GetValue(Grid.RowProperty);

                relativePosition = mousewasdownOn.TransformToAncestor(localGameGrid).Transform(new Point(0, 0));

                lastClickPOS = new Point(elementNameC, elementNameR);
             
                


                //MessageBox.Show(string.Format("Grid clicked at column {0}, row {1}", elementNameC, elementNameR));
                if (GameBoardManager._curTileState[elementNameC, elementNameR] == TileStates.Floor)
                {
                    if (playerClicked)
                    {
                        testPlayerMove();
                    }

                }

            }

        }


        public void testPlayerMove()
        {
            TranslateTransform translateTransform1 = new TranslateTransform(relativePosition.X, relativePosition.Y);

            playerTile.RenderTransform = translateTransform1;

            localGameGrid.Children.Remove(playerTile);
            localGameGrid.Children.Add(playerTile);

            playerClicked = false;
            //MessageBox.Show(string.Format("I should be moving to {0}", relativePosition));

        }

    }
}
