using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WizardWarz
{
    public class Bomb
    {
        public Grid curGameGrid;
        bool fillDir_up, fillDir_down, fillDir_left, fillDir_right;

        public void InitialiseBomb(Int32 startX, Int32 startY, Int32 explosionDist)
        {
            //set all fill directions as true (ie: explosion can expand in direction)
            fillDir_up = true;
            fillDir_down = true;
            fillDir_left = true;
            fillDir_right = true;

            //check if we can spawn the bomb at the player's position (should always be true - player can't walk on walls!)
            if (ReturnCellTileState(startX, startY) != TileStates.Floor)
            {
                return;
            }


            //create a new 2D array of grid cell positions
            Int32[,] explosionMatrix = new Int32[(explosionDist * 4) + 1, 2];

            //set the initial bomb position (player's position)
            explosionMatrix[0, 0] = startX;
            explosionMatrix[0, 1] = startY;
            Debug.WriteLine("x = {0}, y = {1} ", explosionMatrix[0, 0], explosionMatrix[0, 1]);

            //fill the explosion matrix with valid floor tile positions

            Int32 count = 1, distCount = 0;
            Int32 countUp = 1, countDown = 1, countLeft = 1, countRight = 1;

            while ((fillDir_up == true || fillDir_down == true || fillDir_left == true || fillDir_right == true) && distCount < explosionDist)
            {
                if (fillDir_up == true)
                {
                    //if (CheckCellTileState(startX, startY - countUp) == true)
                    if (ReturnCellTileState(startX, startY - countUp) == TileStates.Floor)
                    {
                        explosionMatrix[count, 0] = startX;
                        explosionMatrix[count, 1] = startY - countUp;
                        countUp++;
                        count++;
                    }
                    else if (ReturnCellTileState(startX, startY - countUp) == TileStates.DestructibleWall)
                    {
                        explosionMatrix[count, 0] = startX;
                        explosionMatrix[count, 1] = startY - countUp;
                        countUp++;
                        count++;
                        fillDir_up = false;
                    }
                    else
                    {
                        fillDir_up = false;
                    }
                }
                if (fillDir_down == true)
                {
                    if (ReturnCellTileState(startX, startY + countDown) == TileStates.Floor)
                    {
                        explosionMatrix[count, 0] = startX;
                        explosionMatrix[count, 1] = startY + countDown;
                        countDown++;
                        count++;
                    }
                    else if (ReturnCellTileState(startX, startY + countDown) == TileStates.DestructibleWall)
                    {
                        explosionMatrix[count, 0] = startX;
                        explosionMatrix[count, 1] = startY + countDown;
                        countDown++;
                        count++;
                        fillDir_down = false;
                    }
                    else
                    {
                        fillDir_down = false;
                    }
                }
                if (fillDir_left == true)
                {
                    if (ReturnCellTileState(startX - countLeft, startY) == TileStates.Floor)
                    {
                        explosionMatrix[count, 0] = startX - countLeft;
                        explosionMatrix[count, 1] = startY;
                        countLeft++;
                        count++;
                    }
                    else if (ReturnCellTileState(startX - countLeft, startY) == TileStates.DestructibleWall)
                    {
                        explosionMatrix[count, 0] = startX - countLeft;
                        explosionMatrix[count, 1] = startY;
                        countLeft++;
                        count++;
                        fillDir_left = false;
                    }
                    else
                    {
                        fillDir_left = false;
                    }


                }
                if (fillDir_right == true)
                {
                    if (ReturnCellTileState(startX + countRight, startY) == TileStates.Floor)
                    {
                        explosionMatrix[count, 0] = startX + countRight;
                        explosionMatrix[count, 1] = startY;
                        countRight++;
                        count++;
                    }
                    else if (ReturnCellTileState(startX + countRight, startY) == TileStates.DestructibleWall)
                    {
                        explosionMatrix[count, 0] = startX + countRight;
                        explosionMatrix[count, 1] = startY;
                        countRight++;
                        count++;
                        fillDir_right = false;
                    }
                    else
                    {
                        fillDir_right = false;
                    }
                }

                distCount++;
            }


            //output explosionMatrix for testing
            for (int p = 0; p < explosionMatrix.GetLength(0); p++)
            {
                Console.Write(string.Format("{0}{1}\n", explosionMatrix[p, 0], explosionMatrix[p, 1]));

            }

            //draw explosion on the gameBoard

            for (int c = 0; c < explosionMatrix.GetLength(0); c++)
            {
                Int32 colPos = explosionMatrix[c, 0];
                Int32 rowPos = explosionMatrix[c, 1];

                if (colPos != 0 || rowPos != 0)
                {
                    Rectangle explosion = new Rectangle();
                    explosion.Height = 64;
                    explosion.Width = 64;

                    explosion.Fill = new SolidColorBrush(Colors.Red);



                    Grid.SetColumn(explosion, colPos);
                    Grid.SetRow(explosion, rowPos);
                    curGameGrid.Children.Add(explosion);
                }


            }

        }

        //note: x = Columns y = Rows
        //bool CheckCellTileState(Int32 xPos, Int32 yPos )     //*changed to ReturnCellTileState() 
        //{
        //    if (GameBoardManager._curTileState[xPos, yPos] == TileStates.Floor)
        //    {
        //        //can place bomb or "spread" explosion to cell
        //        return true;
        //    }

        //    return false;
        //}

        //check the current state (enum) of the tile at gameGrid position (xPos,yPos)
        //note: x = Columns y = Rows
        TileStates ReturnCellTileState(Int32 xPos, Int32 yPos)
        {
            return GameBoardManager._curTileState[xPos, yPos];
        }

    }
}

