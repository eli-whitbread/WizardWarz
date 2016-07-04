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
        public Point relativePosition, localBombRelative;
        public Grid localGameGrid;
        public Int32 tileSize = 64, bombRadius = 3;

        public GameBoardManager managerRef = null;

        public int playerX = 0;
        public int playerY = 0;

        public List<FrameworkElement> pathCells = new List<FrameworkElement>(); 

        public PlayerController(Grid gameGrid)
        {
            localGameGrid = gameGrid;

            InitialisePlayerController();

            testPlayerMove();

            gameGrid.MouseDown += new MouseButtonEventHandler(controller_MouseLeftButtonDown);
        }

        public void InitialisePlayerController()
        {

            playerTile = new Rectangle();            

            playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIZARD1.png", UriKind.Relative)));

            playerTile.Height = tileSize;
            playerTile.Width = tileSize;
            Grid.SetColumn(playerTile, playerX);
            Grid.SetRow(playerTile, playerY);
            localGameGrid.Children.Add(playerTile);

            relativePosition = new Point (tileSize, tileSize);
            playerX = Convert.ToInt32(relativePosition.X) / tileSize;
            playerY = Convert.ToInt32(relativePosition.Y) / tileSize;
        }


        public void InitialiseRefs()
        {

        }

        public void controller_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousewasdownOn = e.Source as FrameworkElement;

            int elementNameC = (int)mousewasdownOn.GetValue(Grid.ColumnProperty);
            int elementNameR = (int)mousewasdownOn.GetValue(Grid.RowProperty);

            if (mousewasdownOn != null && e.RightButton == MouseButtonState.Pressed)
            {
                relativePosition = mousewasdownOn.TransformToAncestor(localGameGrid).Transform(new Point(0, 0));
                
                lastClickPOS = new Point(elementNameC, elementNameR);

                MessageBox.Show(string.Format("Grid Tile State = {0}", GameBoardManager.curTileState[elementNameC, elementNameR]));
                //MessageBox.Show(string.Format("Grid clicked at column {0}, row {1}", elementNameC, elementNameR));
                testPlayerMove();

            }

            if (mousewasdownOn != null && e.ClickCount == 2 && mousewasdownOn == playerTile)
            {
                localBombRelative = mousewasdownOn.TransformToAncestor(localGameGrid).Transform(new Point(lastClickPOS.X, lastClickPOS.Y));
                double localCol = localBombRelative.X;
                double localRow = localBombRelative.Y;

                Bomb fireBomb = new Bomb(localGameGrid);

                localGameGrid.Children.Remove(playerTile);

                fireBomb.InitialiseBomb((int)(localCol / tileSize), (int)(localRow / tileSize), bombRadius);
                localGameGrid.Children.Add(playerTile);
            }

        }

        public void testPlayerMove()
        {
            TranslateTransform translateTransform1 = new TranslateTransform(relativePosition.X, relativePosition.Y);

            playerTile.RenderTransform = translateTransform1;

            localGameGrid.Children.Remove(playerTile);
            localGameGrid.Children.Add(playerTile);

            //MessageBox.Show(string.Format("I should be moving to {0}", relativePosition));
            

        }

        public void PlayerMoveToCell()
        {
            relativePosition = pathCells[0].TransformToAncestor(localGameGrid).Transform(new Point(0, 0));

            TranslateTransform translateTransform1 = new TranslateTransform(relativePosition.X, relativePosition.Y);

            playerTile.RenderTransform = translateTransform1;

            localGameGrid.Children.Remove(playerTile);
            localGameGrid.Children.Add(playerTile);

            playerX = Convert.ToInt32(relativePosition.X) / tileSize;
            playerY = Convert.ToInt32(relativePosition.Y) / tileSize;
            Debug.WriteLine("New Player x = {0}, New Player y = {1}", playerX, playerY);

            pathCells.RemoveAt(0);
        }

    }
}
