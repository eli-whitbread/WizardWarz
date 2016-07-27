using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public Grid localGameGrid = null;
        public Grid highlightLocalGrid = null;
        public Int32 tileSize = 64, bombRadius = 3;
        public Int32 playerPosition;
        public Int32[,] playerGridLocArray;
        Color tempColour = new Color();
        public GameBoardManager managerRef = null;
        public GameTimer playerTimerRef = null;
        AudioManager playMusic = new AudioManager();

        int p1PathCellCount = 0;
        public int playerX = 0;
        public int playerY = 0;

        float movementTimer = 0;

        bool p1HasPath = false;
        bool isP1Influenced = false;
        bool isTouched = false;

        Point curMousePos = new Point(0, 0);

        public List<FrameworkElement> pathCells = new List<FrameworkElement>();
        List<Ellipse> pathHighlightTile = new List<Ellipse>();
        public Rectangle[,] gridCellsArray = null;
        public Canvas gameCanRef = null;
        public PlayerLivesAndScore myLivesAndScore = null;

        // ------------------------- Player Controller Constructor ------------
        public PlayerController()
        {
            playerTimerRef = GameWindow.ReturnTimerInstance();
            
            //Debug.WriteLine("HELLO " + playerTimerRef);
            playerTimerRef.processFrameEvent_TICK += PlayerTimerRef_tickEventPROCESS;
        }

        // ---------------------------------------------------------------------
        // ----------------------PLAYER TICK PROCESS -----------------------------------
        // ---------------------------------------------------------------------
        private void PlayerTimerRef_tickEventPROCESS(object sender, EventArgs e)
        {
            //myTime += myTickIncrement;
            ProcessFrame();

            RenderFrame();
            
        }

        // ---------------------------------------------------------------------
        // ---------------------- END PLAYER TICK ------------------------------
        // ---------------------------------------------------------------------

        public void initialisePlayerGridRef()
        {
            //playerGridLocArray = new Int32[6, 2] { {0, 0 }, { 12, 0}, { 11, 11}, { 0, 11 }, { 0, 5}, { 11, 5} };

            setPlayerPos(playerPosition);

            testPlayerMove();

            localGameGrid.MouseDown += new MouseButtonEventHandler(controller_MouseLeftButtonDown);
        }
                

        private void setPlayerPos(int gridStartPos)
        {
            playerTile = new Rectangle();
            playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\PlayerRight1.png", UriKind.Relative)));
            playerTile.Height = tileSize;
            playerTile.Width = tileSize;
            Grid.SetColumn(playerTile, 0);
            Grid.SetRow(playerTile, 0);

            Grid.SetZIndex(playerTile, 10); //set the layering position of the playerTile - can use Grid.SetZIndex or Canvas.SetZIndex(object,int layer)
           
            if(gridStartPos == 0)
            {
                relativePosition = new Point(64, 64);
                playerX = 1;
                playerY = 1;
                tempColour = Colors.Blue;
                Debug.WriteLine("%%% Player {0}: player X: {1}, player Y: {2} /n", gridStartPos + 1, playerX, playerY);
            }
            else if(gridStartPos == 1)
            {
                relativePosition = new Point(704, 64);
                playerX = 11;
                playerY = 1;
                tempColour = Colors.Red;
                Debug.WriteLine("%%% Player {0}: player X: {1}, player Y: {2} /n", gridStartPos + 1, playerX, playerY);
            }
            else if (gridStartPos == 2)
            {
                relativePosition = new Point(704, 704);
                playerX = 10;
                playerY = 10;
                tempColour = Colors.Green;
                Debug.WriteLine("%%% Player {0}: player X: {1}, player Y: {2} /n", gridStartPos + 1, playerX, playerY);

            }
            else if (gridStartPos == 3)
            {
                relativePosition = new Point(64, 704);
                playerX = -1;
                playerY = 10;
                tempColour = Colors.Pink;
                Debug.WriteLine("%%% Player {0}: player X: {1}, player Y: {2} /n", gridStartPos + 1, playerX, playerY);
            }
            else if (gridStartPos == 4)
            {
                relativePosition = new Point(64, 256);
                playerX = 1;
                playerY = 4;
                tempColour = Colors.Yellow;
                Debug.WriteLine("%%% Player {0}: player X: {1}, player Y: {2} /n", gridStartPos + 1, playerX, playerY);
            }
            else if (gridStartPos == 5)
            {
                relativePosition = new Point(640, 256);
                playerX = 10;
                playerY = 4;
                tempColour = Colors.Silver;
                Debug.WriteLine("%%% Player {0}: player X: {1}, player Y: {2} /n", gridStartPos + 1, playerX, playerY);
            }            
            localGameGrid.Children.Add(playerTile);
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
                DeleteHighlight();
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

                            //HIGHLIGHT
                            HighlightPathCalc(elementNameC, elementNameR);
                            
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
                                //HIGHLIGHT
                                HighlightPathCalc(elementNameC, elementNameR);
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

        

        private void HighlightPathCalc(int elementC, int elementR)
        {
            
            Ellipse highlight = new Ellipse();
            
            //---------------------ADD HIGHLIGHT  
            highlight.Height = 64 * 0.2;
            highlight.Width = 64 * 0.2;
            
            
            highlight.Fill = new SolidColorBrush(tempColour);
            highlight.Fill.Opacity = 0.4f;
            highlight.IsHitTestVisible = false;
            pathHighlightTile.Add(highlight);

            Grid.SetColumn(highlight, elementC);
            Grid.SetRow(highlight, elementR);

            highlightLocalGrid.Children.Add(highlight);
            
            //----------------------------------            



        }

        private void DeleteHighlight()
        {

            foreach (Ellipse highlight in pathHighlightTile)
            {

                highlightLocalGrid.Children.Remove(highlight);
            }



        }



        public void RenderFrame()
        {
            if (!isP1Influenced && p1HasPath)
            {
                //movementTimer += timerRef.exposedDT;

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
                DeleteHighlight();
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
            playerDirection();

            relativePosition = pathCells[0].TransformToAncestor(localGameGrid).Transform(new Point(0, 0));
            //MessageBox.Show(String.Format("MY NEW POINTS ARE X: {0}, Y: {1} ", relativePosition.X, relativePosition.Y));
            TranslateTransform translateTransform1 = new TranslateTransform(relativePosition.X, relativePosition.Y);

            playerTile.RenderTransform = translateTransform1;

            localGameGrid.Children.Remove(playerTile);
            localGameGrid.Children.Add(playerTile);

            playerX = Convert.ToInt32(relativePosition.X) / tileSize;
            playerY = Convert.ToInt32(relativePosition.Y) / tileSize;
            Debug.WriteLine("New Player x = {0}, New Player y = {1}", playerX, playerY);

            
            pathCells.RemoveAt(0);
        }

        private void playerDirection()
        {
            if (relativePosition.X > pathCells[0].TransformToAncestor(localGameGrid).Transform(new Point(0, 0)).X)
            {
                playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\PlayerLeft1.png", UriKind.Relative)));
            }
            else if (relativePosition.X < pathCells[0].TransformToAncestor(localGameGrid).Transform(new Point(0, 0)).X)
            {
                playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\PlayerRight1.png", UriKind.Relative)));

            }
            if (relativePosition.Y > pathCells[0].TransformToAncestor(localGameGrid).Transform(new Point(0, 0)).Y)
            {
                playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\PlayerBack1.png", UriKind.Relative)));
            }
            else if (relativePosition.Y < pathCells[0].TransformToAncestor(localGameGrid).Transform(new Point(0, 0)).Y)
            {
                playerTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\PlayerFront1.png", UriKind.Relative)));
            }
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

            if (mousewasdownOn != null && e.ClickCount == 2 && mousewasdownOn == playerTile)
            {
                localBombRelative = mousewasdownOn.TransformToAncestor(localGameGrid).Transform(new Point(lastClickPOS.X, lastClickPOS.Y));
                double localCol = localBombRelative.X;
                double localRow = localBombRelative.Y;
                

                if (StaticCollections.CheckBombPosition((int)(localCol / tileSize), (int)(localRow / tileSize)) == true)
                {
                    Bomb fireBomb = new Bomb(localGameGrid);
                    fireBomb.managerRef = managerRef;
                    fireBomb.myOwner = this;

                    localGameGrid.Children.Remove(playerTile);

                    fireBomb.InitialiseBomb((int)(localCol / tileSize), (int)(localRow / tileSize), bombRadius);
                    localGameGrid.Children.Add(playerTile);


                    // Play Bomb Explode Sound (Should also play the tick sound here)
                    //playMusic.playBombExplode(); - move to Bomb.cs

                    //add bomb reference to bomb collection
                    StaticCollections.AddBomb(fireBomb, (int)(localCol / tileSize), (int)(localRow / tileSize));
                }
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
