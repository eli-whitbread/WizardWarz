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
        public GameTimer timerRef = null;
        SoundManager playMusic = new SoundManager();

        int p1PathCellCount = 0;
        public int playerX = 0;
        public int playerY = 0;

        float movementTimer = 0;

        bool p1HasPath = false;
        bool isP1Influenced = false;
        bool isTouched = false;

        Point curMousePos = new Point(0, 0);

        public List<FrameworkElement> pathCells = new List<FrameworkElement>();
        public Canvas gameCanRef = null;

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

        public void ProcessFrame()
        {
            curMousePos = Mouse.GetPosition(gameCanRef);
            //Debug.WriteLine(Mouse.DirectlyOver.ToString());

            if (Mouse.LeftButton == MouseButtonState.Pressed && isTouched == false)
            {
                //Mouse.Capture(gameCanRef);
                isTouched = true;
                Debug.WriteLine("Capturing...");
            }
            else if (Mouse.LeftButton == MouseButtonState.Released && isTouched == true)
            {
                //gameCanRef.ReleaseMouseCapture();
                isTouched = false;
                isP1Influenced = false;
                Debug.WriteLine("Finished capturing!");
            }


            if (isTouched)
            {
                if (Mouse.DirectlyOver == playerTile && isP1Influenced == false)
                {
                    isP1Influenced = true;

                    //Mouse.Capture(localGameGrid);

                    Debug.WriteLine("Player influenced...");
                }
            }

            if (isP1Influenced == true)
            {
                var mouseOver = Mouse.DirectlyOver as FrameworkElement;
                int elementNameC = (int)mouseOver.GetValue(Grid.ColumnProperty);
                int elementNameR = (int)mouseOver.GetValue(Grid.RowProperty);

                if (Mouse.DirectlyOver != playerTile)
                {
                    if (p1PathCellCount == 0)
                    {
                        if (cellStateCheck(mouseOver) && firstCellAxisCheck(mouseOver))
                        {
                            pathCells.Add(mouseOver);
                            p1PathCellCount++;
                            p1HasPath = true;

                            Debug.WriteLine("1st Element added!");
                        }
                        else if (Mouse.DirectlyOver == playerTile)
                        {
                            mouseOverFailure();
                        }
                        else
                        {
                            mouseOverFailure();
                        }
                    }
                    else if (p1PathCellCount > 0)
                    {
                        if (Mouse.DirectlyOver != pathCells[pathCells.Count - 1])
                        {
                            if (cellStateCheck(mouseOver) && genericCellAxisCheck(mouseOver))
                            {
                                pathCells.Add(mouseOver);
                                ++p1PathCellCount;

                                Debug.WriteLine("Element added!");
                            }
                            else
                            {
                                mouseOverFailure();
                            }
                        }
                        else if (Mouse.DirectlyOver == playerTile)
                        {
                            mouseOverFailure();
                        }
                    }
                    //else if (Mouse.DirectlyOver == playerTile && Mouse.DirectlyOver != pathCells[pathCells.Count - 1])
                    //{
                    //    pathCells.Add(managerRef.flrTiles[playerX, playerY]);
                    //    ++p1PathCellCount;

                    //    Debug.WriteLine("Element added!");
                    //}
                }
            }
        }

        public void RenderFrame()
        {
            if (!isP1Influenced && p1HasPath)
            {
                movementTimer += timerRef.exposedDT;

                if (movementTimer % 10 == 0)
                {
                    if (pathCells.Count() != 0)
                    {
                        movementTimer = 0;
                        PlayerMoveToCell();
                        p1PathCellCount--;

                        Debug.WriteLine("Player moved!");
                    }
                    else
                    {
                        p1HasPath = false;

                        Debug.WriteLine("Player finished moving!");
                    }
                }
            }
        }

        private bool cellStateCheck(FrameworkElement curCellInterrogated)
        {
            int curCellC = (int)curCellInterrogated.GetValue(Grid.ColumnProperty);
            int curCellR = (int)curCellInterrogated.GetValue(Grid.RowProperty);

            TileStates curCellTS = new TileStates();
            curCellTS = GameBoardManager.curTileState[curCellC, curCellR];

            if (curCellTS == TileStates.Floor)
            {
                Debug.WriteLine("Cell state confirmed as floor!");
                return true;
            }
            else
            {
                Debug.WriteLine("Cell state confirmed as not floor!");
                return false;
            }
        }

        private bool firstCellAxisCheck(FrameworkElement curCellInterrogated)
        {
            int curPlayerC = playerX;
            int curPlayerR = playerY;
            int curCellC = (int)curCellInterrogated.GetValue(Grid.ColumnProperty);
            int curCellR = (int)curCellInterrogated.GetValue(Grid.RowProperty);

            Debug.WriteLine("Player x = {0}, Player y = {1}", curPlayerC, curPlayerR);
            Debug.WriteLine("Cell x = {0}, Cell y = {1}", curCellC, curCellR);

            if (curPlayerC == curCellC || curPlayerR == curCellR)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool genericCellAxisCheck(FrameworkElement curCellInterrogated)
        {
            int lastPathCellC = (int)pathCells[pathCells.Count - 1].GetValue(Grid.ColumnProperty);
            int lastPathCellR = (int)pathCells[pathCells.Count - 1].GetValue(Grid.RowProperty);
            int curCellC = (int)curCellInterrogated.GetValue(Grid.ColumnProperty);
            int curCellR = (int)curCellInterrogated.GetValue(Grid.RowProperty);

            Debug.WriteLine("Player x = {0}, Player y = {1}", lastPathCellC, lastPathCellR);
            Debug.WriteLine("Cell x = {0}, Cell y = {1}", curCellC, curCellR);

            if (lastPathCellC == curCellC || lastPathCellR == curCellR)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private void mouseOverFailure()
        {
            Debug.WriteLine("Mouse over failure!");

            isTouched = false;
            isP1Influenced = false;
            pathCells.Clear();
            p1PathCellCount = 0;

            Debug.WriteLine("Capturing aborted!");
        }

        public void controller_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousewasdownOn = e.Source as FrameworkElement;

            int elementNameC = (int)mousewasdownOn.GetValue(Grid.ColumnProperty);
            int elementNameR = (int)mousewasdownOn.GetValue(Grid.RowProperty);

            //if (mousewasdownOn != null && e.RightButton == MouseButtonState.Pressed)
            //{
            //    relativePosition = mousewasdownOn.TransformToAncestor(localGameGrid).Transform(new Point(0, 0));

            //    lastClickPOS = new Point(elementNameC, elementNameR);

            //    MessageBox.Show(string.Format("Grid Tile State = {0}", GameBoardManager.curTileState[elementNameC, elementNameR]));
            //    //MessageBox.Show(string.Format("Grid clicked at column {0}, row {1}", elementNameC, elementNameR));
            //    testPlayerMove();

            //}

            if (mousewasdownOn != null && e.ClickCount == 2 && mousewasdownOn == playerTile)
            {
                localBombRelative = mousewasdownOn.TransformToAncestor(localGameGrid).Transform(new Point(lastClickPOS.X, lastClickPOS.Y));
                double localCol = localBombRelative.X;
                double localRow = localBombRelative.Y;

                Bomb fireBomb = new Bomb(localGameGrid);

                localGameGrid.Children.Remove(playerTile);

                fireBomb.InitialiseBomb((int)(localCol / tileSize), (int)(localRow / tileSize), bombRadius);
                
                localGameGrid.Children.Add(playerTile);

                
                // Play Bomb Explode Sound (Should also play the tick sound here)
                playMusic.playBombExplode();

                //add bomb reference to bomb collection
                StaticCollections.AddBomb(fireBomb);
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
    }
}
