using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Data;
using Microsoft.Surface;
using Microsoft.Surface.Core;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Input;
using System.Windows.Documents;
using System.Diagnostics;
using WizardWarz;


namespace WizardWarz
{
    public class GameTimer
    {
        float deltaTime = 0;
        float curTdelta = 0;
        float movementTimer = 0;
        int p1PathCellCount = 0;

        Point curMousePos = new Point(0, 0);

        bool isCanvasCap = false;
        bool isP1Influenced = false;
        bool p1HasPath = false;

        public Canvas GameCanRef = null;

        public PlayerController p1Ref = null;

        public GameTimer()
        {
            DispatcherTimer gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(FramesPerSecond());
            gameLoopTimer.Tick += new EventHandler(timer_Tick);
            gameLoopTimer.Start();
            Debug.WriteLine("gameLoopTimer Initialised");
        }

        public void Initialise()
        {
            Debug.WriteLine(GameCanRef.Name);
            if (p1Ref != null)
            {
                Debug.WriteLine("Controller ref passed successfully!");
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {

            curTdelta += deltaTime;
            if (curTdelta % 5 == 0)
            {
                Debug.WriteLine("Elapsed = {0}", curTdelta);
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed && isCanvasCap == false)
            {
                //Mouse.Capture(GameCanRef);
                isCanvasCap = true;
                Debug.WriteLine("Capturing...");
            }
            else if (Mouse.LeftButton == MouseButtonState.Released && isCanvasCap == true)
            {
                //GameCanRef.ReleaseMouseCapture();
                isCanvasCap = false;
                isP1Influenced = false;
                Debug.WriteLine("Finished capturing!");
            }

            if (isCanvasCap)
            {
                curMousePos =  Mouse.GetPosition(GameCanRef);
                //Debug.WriteLine(Mouse.DirectlyOver.ToString());
                if (Mouse.DirectlyOver == p1Ref.playerTile && isP1Influenced == false)
                {
                    isP1Influenced = true;

                    //Mouse.Capture(p1Ref.localGameGrid);
                    Debug.WriteLine("Player influenced...");
                }

                if (isP1Influenced == true)
                {
                    var mouseOver = Mouse.DirectlyOver as FrameworkElement;
                    int elementNameC = (int)mouseOver.GetValue(Grid.ColumnProperty);
                    int elementNameR = (int)mouseOver.GetValue(Grid.RowProperty);

                    if (Mouse.DirectlyOver != p1Ref.playerTile)
                    {
                        if (p1PathCellCount == 0)
                        {
                            if (cellStateCheck(mouseOver) && firstCellAxisCheck(mouseOver))
                            {
                                p1Ref.pathCells.Add(mouseOver);
                                p1PathCellCount++;
                                p1HasPath = true;

                                Debug.WriteLine("1st Element added!");
                            }
                            else
                            {
                                mouseOverFailure();
                            }
                        }
                        else if (p1PathCellCount > 0)
                        {
                            if (Mouse.DirectlyOver != p1Ref.pathCells[p1Ref.pathCells.Count - 1])
                            {
                                if (cellStateCheck(mouseOver) && genericCellAxisCheck(mouseOver))
                                {
                                    p1Ref.pathCells.Add(mouseOver);
                                    ++p1PathCellCount;

                                    Debug.WriteLine("Element added!");
                                }
                                else
                                {
                                    mouseOverFailure();
                                }
                            }
                        }
                    }
                }
            }

            if (!isP1Influenced && p1HasPath)
            {
                movementTimer += deltaTime;

                if (movementTimer % 10 == 0)
                {
                    if (p1Ref.pathCells.Count() != 0)
                    {
                        movementTimer = 0;
                        p1Ref.PlayerMoveToCell();
                        p1PathCellCount--;

                        Debug.WriteLine("Player moved!");
                    }
                }
            }
        }

        private double FramesPerSecond()
        {
            Int32 fps = 60;
            Double frameTimeSpan;

            frameTimeSpan = 1000 / fps;
            deltaTime = Convert.ToSingle(frameTimeSpan);

            return frameTimeSpan;
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
            int curPlayerC = p1Ref.playerX;
            int curPlayerR = p1Ref.playerY;
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
            int lastPathCellC = (int)p1Ref.pathCells[p1Ref.pathCells.Count - 1].GetValue(Grid.ColumnProperty);
            int lastPathCellR = (int)p1Ref.pathCells[p1Ref.pathCells.Count - 1].GetValue(Grid.RowProperty);
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

        private void mouseOverFailure()
        {
            Debug.WriteLine("Mouse over failure!");

            isCanvasCap = false;
            isP1Influenced = false;
            p1Ref.pathCells.Clear();
            p1PathCellCount = 0;

            Debug.WriteLine("Capturing aborted!");
        }
    }
}
