﻿using System;
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
using System.Windows;

namespace WizardWarz
{
    public class Bomb
    {
        public Grid curGameGrid;
        public float effectLifeTime, timeToExplode;
        float myTime, myTickIncrement;
        bool fillDir_up, fillDir_down, fillDir_left, fillDir_right;
        Int32 explosionStep;
        Int32 count = 1, distCount = 0;
        Int32 countUp = 1, countDown = 1, countLeft = 1, countRight = 1;
        Int32[,] explosionMatrix;
        bool iCanDestroy;
        Rectangle bombImage;
        List<Rectangle> explosionTiles;
        List<FrameworkElement> bombedCells = new List<FrameworkElement>();
        AudioManager playBombSndFX = new AudioManager();

        public GameBoardManager managerRef = null;
        public PlayerController myOwner = null;

        public Bomb(Grid localGameGrid)
        {
            curGameGrid = localGameGrid;
        }

        //called by GameTimer
        public void BombTickUpdate()
        {
            myTime += myTickIncrement;

            Debug.WriteLine("Bomb alive {0} seconds", myTime); //debug the timer
            if(myTime >= (effectLifeTime + timeToExplode) && iCanDestroy == true)
            {
                DestroyBomb();
            }
            else if(myTime >= timeToExplode)
            {
                ProcessExplosion();
                DrawExplosion();
            }

        }

        //remove the bomb from the level
        private void DestroyBomb()
        {
           
            foreach(Rectangle exp in explosionTiles)
            {
                curGameGrid.Children.Remove(exp);
            }

            StaticCollections.RemoveBomb(this);
            
        }

        public void InitialiseBomb(Int32 startX, Int32 startY, Int32 explosionDist)
        {
            explosionTiles = new List<Rectangle>();
            myTime = 0.0f;
            explosionStep = 0;
            myTickIncrement = 0.1f;
            effectLifeTime = 5.0f;
            timeToExplode = 6.0f;

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
            explosionMatrix = new Int32[(explosionDist * 4) + 1, 2];

            //set the initial bomb position (player's position)
            explosionMatrix[0, 0] = startX;
            explosionMatrix[0, 1] = startY;

            bombedCells.Add(myOwner.gridCellsArray[startX,startY]);
            Debug.WriteLine("x = {0}, y = {1} ", explosionMatrix[0, 0], explosionMatrix[0, 1]);

            //fill the explosion matrix with valid tile positions
            while ((fillDir_up == true || fillDir_down == true || fillDir_left == true || fillDir_right == true) && distCount < explosionDist)
            {
                
                if(fillDir_up == true)
                {
                    fillDir_up = AddTileToExplosionArea(startX, startY - countUp, countUp, out countUp);
                }
                if(fillDir_down == true)
                {
                    fillDir_down = AddTileToExplosionArea(startX, startY + countDown, countDown, out countDown);
                }
                if(fillDir_left == true)
                {
                    fillDir_left = AddTileToExplosionArea(startX - countLeft, startY, countLeft, out countLeft);
                }
                if(fillDir_right == true)
                {
                    fillDir_right = AddTileToExplosionArea(startX + countRight, startY, countRight, out countRight);
                }

                distCount++;

            }


            //output explosionMatrix for testing
            for (int p = 0; p < explosionMatrix.GetLength(0); p++)
            {
                Console.Write(string.Format("{0}{1}\n", explosionMatrix[p, 0], explosionMatrix[p, 1]));

            }

            //draw unexploded bomb image
            DrawBomb();
           

        }

        //check the current state (enum) of the tile at gameGrid position (xPos,yPos)
        //note: x = Columns y = Rows
        TileStates ReturnCellTileState(Int32 xPos, Int32 yPos)
        {
            return GameBoardManager.curTileState[xPos, yPos];
        }

        bool AddTileToExplosionArea(Int32 x, Int32 y, Int32 countDir, out Int32 countDirOut)
        {
            
            if (ReturnCellTileState(x, y) == TileStates.Floor)
            {
                explosionMatrix[count, 0] = x;
                explosionMatrix[count, 1] = y;
                countDir++;
                count++;
                countDirOut = countDir;

                bombedCells.Add(myOwner.gridCellsArray[x,y]);
                return true;
            }
            else if (ReturnCellTileState(x, y) == TileStates.DestructibleWall)
            {
                explosionMatrix[count, 0] = x;
                explosionMatrix[count, 1] = y;
                countDir++;
                count++;
                countDirOut = countDir;

                bombedCells.Add(myOwner.gridCellsArray[x, y]);
                return false;
            }
            else
            {
                countDirOut = count;
                return false;
            }

        }

        void DrawBomb()
        {
            Int32 colPos = explosionMatrix[0, 0];
            Int32 rowPos = explosionMatrix[0, 1];


            bombImage = new Rectangle();
            bombImage.Height = 64;
            bombImage.Width = 64;

            bombImage.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\Bomb.png", UriKind.Relative)));
            bombImage.IsHitTestVisible = false;

            Grid.SetColumn(bombImage, colPos);
            Grid.SetRow(bombImage, rowPos);
            curGameGrid.Children.Add(bombImage);
            playBombSndFX.playBombTick();


        }

        void ProcessExplosion()
        {
            if (explosionStep < explosionMatrix.GetLength(0))
            {
                Int32 colPos = explosionMatrix[explosionStep, 0];
                Int32 rowPos = explosionMatrix[explosionStep, 1];

                if (explosionStep == 0)
                {
                    foreach (Rectangle curCellInterrogated in bombedCells)
                    {
                        int curCellC = (int)curCellInterrogated.GetValue(Grid.ColumnProperty);
                        int curCellR = (int)curCellInterrogated.GetValue(Grid.RowProperty);
                        //TileStates curCellTS = new TileStates();
                        //curCellTS = GameBoardManager.curTileState[curCellC, curCellR];

                        //if (curCellTS == TileStates.Floor)
                        //{

                        //}
                        //else if (curCellTS == TileStates.DestructibleWall)
                        //{
                        //    GameBoardManager.curTileState[curCellC, curCellR] = TileStates.Floor;
                        //    managerRef.ChangeTileImage(curCellC, curCellR);
                        //}
                        if(ReturnCellTileState(curCellC, curCellR) == TileStates.DestructibleWall )
                        {
                            GameBoardManager.curTileState[curCellC, curCellR] = TileStates.Floor;
                            managerRef.ChangeTileImage(curCellC, curCellR);
                        }
                    }
                }

                if (colPos != 0 || rowPos != 0)
                {
                    checkEffectedPlayers();
                }
            }
        }

        void DrawExplosion()
        {

            if (explosionStep < explosionMatrix.GetLength(0))
            {
                Int32 colPos = explosionMatrix[explosionStep, 0];
                Int32 rowPos = explosionMatrix[explosionStep, 1];

                if (explosionStep == 0)
                {
                    curGameGrid.Children.Remove(bombImage);
                    playBombSndFX.playBombExplode();
                }

                if (colPos != 0 || rowPos != 0)
                {
                    Rectangle explosion = new Rectangle();
                    explosion.Height = 64;
                    explosion.Width = 64;

                    explosion.Fill = new SolidColorBrush(Colors.Red);
                    explosion.Fill.Opacity = 0.7f;
                    explosion.IsHitTestVisible = false;
                    explosionTiles.Add(explosion);

                    Grid.SetColumn(explosion, colPos);
                    Grid.SetRow(explosion, rowPos);
                    curGameGrid.Children.Add(explosion);

                }
                explosionStep++;
            }
            else
            {
                iCanDestroy = true;
            }


        }

        private Tuple<int> checkEffectedPlayers()
        {
            int colPos = explosionMatrix[explosionStep, 0];
            int rowPos = explosionMatrix[explosionStep, 1];

            if (colPos == myOwner.playerX && rowPos == myOwner.playerY)
            {
                myOwner.myLivesAndScore.ReduceLives(1);
            }

            return new Tuple<int>(0);
        }

    }
}

