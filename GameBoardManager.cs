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
using System.Diagnostics;


namespace WizardWarz
{
    public enum TileStates
    {
        Floor,
        SolidWall,
        DestructibleWall,
        Powerup
    }

    // Secondary enum for specific powerup types
    // Default state of each tile should be "Empty"
    public enum PowerupStates
    {
        Empty,
        Superbomb,
        Shield,
        Lifeup
    }

    public class GameBoardManager
    {
        public Grid gameGrid = null;
        public PlayerController p1Ref = null;
        public Rectangle[,] flrTiles = null;
        Int32 rows = 13;
        Int32 cols = 23;
        Int32 numberOfDestructibleWalls = 60;
        public static TileStates[,] curTileState = null;
        public static PowerupStates[,] powerupTileState = null;
        static Random randomNumber = new Random();

        //public int[,] innerWallPos = { { 2, 2 }, { 2, 4 }, { 2, 6 }, { 2, 8 }, { 2, 10 },
        //        { 4, 2 }, { 4, 4 }, { 4, 6 }, { 4, 8 }, { 4, 10 },
        //        { 6, 2 }, { 6, 4 }, { 6, 6 }, { 6, 8 }, { 6, 10 },
        //        { 8, 2 }, { 8, 4 }, { 8, 6 }, { 8, 8 }, { 8, 10 },
        //        { 10, 2 }, { 10, 4 }, { 10, 6 }, { 10, 8 }, { 10, 10 }
        //    };
        public int[,] innerWallPos;

        //public int[,] destructibleWallPos = { {9, 1 }, { 3, 2 }, { 3, 5 }, { 5, 5 }, { 5, 9 }, { 7, 1 }, { 7, 6 }, { 9, 6 }, { 5, 3 }, { 7, 9 } };
        public int[,] destructibleWallPos;


        //initialise the game board
        public void InitializeGameBoard()
        {
            
            Int32 tileSize = GameWindow.ReturnTileSize();
            //set the grid size
            //if (GameWindow.ReturnNumberOfPlayer() == 6)
            //{
            //    rows = 14;
            //    cols = 16;
            //}       

            curTileState = new TileStates[cols, rows];
            powerupTileState = new PowerupStates[cols, rows];


            GridLengthConverter myGridLengthConverter = new GridLengthConverter();
            GridLength side = (GridLength)myGridLengthConverter.ConvertFromString("Auto");

            //Setup the grid Rows and Columns
            for (int i = 0; i < cols; i++)
            {
                gameGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
                gameGrid.ColumnDefinitions[i].Width = side;
            }
            for (int x = 0; x < rows; x++)
            {
                gameGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                gameGrid.RowDefinitions[x].Height = side;
            }

            AssignDestructibleWallPositions();

            //create an empty Rectangle array
            flrTiles = new Rectangle[cols, rows];

            //fill each element in the Rectangle array with an image "tile".
            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    flrTiles[c, r] = new Rectangle();

                    powerupTileState[c, r] = PowerupStates.Empty;

                    //add a wall tile if along the grid extremes
                    if (InitialTilePlacementCheck(c,r, cols, rows) == true)
                    {
                        flrTiles[c, r].Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\Indesructable.png", UriKind.Relative)));
                        curTileState[c, r] = TileStates.SolidWall;
                    }
                    //add destructible walls within the game grid
                    else if (DestructableWallPlacementCheck(c, r) == true)
                    {
                        flrTiles[c, r].Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\Destructible.png", UriKind.Relative)));
                        curTileState[c, r] = TileStates.DestructibleWall;
                    }
                    //otherwise add a floor tile
                    else
                    {
                        flrTiles[c, r].Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\Floor.png", UriKind.Relative)));
                        curTileState[c, r] = TileStates.Floor;
                    }
                    //
                    //inner solid and destrutable walls still required!!
                    //
                    flrTiles[c, r].Height = tileSize;
                    flrTiles[c, r].Width = tileSize;
                    Grid.SetColumn(flrTiles[c, r], c);
                    Grid.SetRow(flrTiles[c, r], r);


                    gameGrid.Children.Add(flrTiles[c, r]);
                }
            }


            

                        
        }

        void AssignDestructibleWallPositions()
        {
            Int32 wallCount = 0;
            destructibleWallPos = new Int32[numberOfDestructibleWalls, 2];

            while (wallCount <= numberOfDestructibleWalls - 1)
            {
               
                Int32 rndColNum = randomNumber.Next(2, cols - 3);
                Int32 rndRowNum = randomNumber.Next(2, rows - 3);

                if ((rndColNum % 2 != 0) || (rndRowNum % 2 != 0))
                {

                    bool canPlaceTileHere = true;

                    for(int i = 0; i < destructibleWallPos.Length / 2; i++)
                    {
                        if (destructibleWallPos[i, 0] == rndColNum && destructibleWallPos[i, 1] == rndRowNum)
                        {
                            canPlaceTileHere = false;
                            break;
                        }
                       
                    }

                    if (canPlaceTileHere == true)
                    {
                        destructibleWallPos[wallCount, 0] = rndColNum;
                        destructibleWallPos[wallCount, 1] = rndRowNum;
                        Debug.WriteLine("D'Walls = {0} {1} ", destructibleWallPos[wallCount, 0], destructibleWallPos[wallCount, 1]);
                        wallCount++;
                    }


                }
                
                //if (DestructableWallPlacementCheck(rndColNum, rndRowNum) == true)
                //{
                //    //flrTiles[rndColNum, rndRowNum].Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\Destructible.png", UriKind.Relative)));
                //    //curTileState[rndColNum, rndRowNum] = TileStates.DestructibleWall;
                //    if ((rndColNum % 2 != 0) || (rndRowNum % 2 != 0))
                //    {
                //        destructibleWallPos[wallCount, 0] = rndColNum;
                //        destructibleWallPos[wallCount, 1] = rndRowNum;

                //        wallCount++;

                //    }
                    
                //}

            }
        }

        public bool InitialTilePlacementCheck(Int32 c, Int32 r, Int32 colsLength, Int32 rowsLength)
        {
            innerWallPos = new Int32[(rows * cols),2]; //this is waaay too long but don't have much choice

            Int32 innerWallCount = 0;

            if (r == 0 || r == rowsLength - 1 || c == 0 || c == colsLength - 1)
            {
                innerWallPos[innerWallCount, 0] = c;
                innerWallPos[innerWallCount, 1] = r;
                innerWallCount++;
                return true;
            }
            else if ((c % 2 == 0) && (r % 2 == 0) )
            {
                innerWallPos[innerWallCount, 0] = c;
                innerWallPos[innerWallCount, 1] = r;
                innerWallCount++;
                return true;
            }
            //setup an array of grid positions (int[,]) for inner walls  = {column,Row} - count starts from "outer wall". ie. 5 = 5 + wall.
            //Int32[,] innerWallPos = { { 2, 2 }, { 2, 4 }, { 2, 6 }, { 2, 8 }, { 2, 10 },
            //    { 4, 2 }, { 4, 4 }, { 4, 6 }, { 4, 8 }, { 4, 10 },
            //    { 6, 2 }, { 6, 4 }, { 6, 6 }, { 6, 8 }, { 6, 10 },
            //    { 8, 2 }, { 8, 4 }, { 8, 6 }, { 8, 8 }, { 8, 10 },
            //    { 10, 2 }, { 10, 4 }, { 10, 6 }, { 10, 8 }, { 10, 10 }

            //};

            //if both r and c are along the outer edge of the game board grid then return true - outer wall generation
            //if (r == 0 || r == rowsLength - 1 || c == 0 || c == colsLength - 1)
            //{
            //    return true;
            //}
            ////itterate through the innerWallPos 2D array and return true if both values match the passed in Row x Coll pos (r,c) - inner solid-wall generation
            ////we devide innerWallPos.Length by 2 because each "subarray" {x,y} is 2 elements long
            //for (int i = 0; i < innerWallPos.Length / 2; i++)
            //{
            //    if(innerWallPos[i, 0] == c && innerWallPos[i, 1] == r)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }

        public bool DestructableWallPlacementCheck(Int32 c, Int32 r)
        {
            //if (r == 1 || r == rowsLength - 2 || c == 1 || c == colsLength - 2 )
            //{
            //    return false;
            //}
            //if ((c % 2 != 0) || (r % 2 != 0))
            //{
            //    if (curTileState[c, r] != TileStates.DestructibleWall)
            //    {
            //        return true;
            //    }
                
            //}

            //setup an array of grid positions (int[,]) for destructable walls = {column,Row} - count starts from "outer wall". ie. 5 = 5 + wall.


            //itterate through the destructableWallPos 2D array and return true if both values match the passed in Row x Coll pos (r,c) - inner destructible-wall generation
            //we devide destructibleWallPos.Length by 2 because each "subarray" {x,y} is 2 elements long
            for (int i = 0; i < destructibleWallPos.Length / 2; i++)
            {
                if (destructibleWallPos[i, 0] == c && destructibleWallPos[i, 1] == r)
                {
                    if (curTileState[c, r] == TileStates.DestructibleWall)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void ChangeTileImage(int tileX, int tileY)
        {
            flrTiles[tileX, tileY].Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\Floor.png", UriKind.Relative)));
            Debug.WriteLine("Tile fill changed to floor!");
        }

        public void ChangeTileState(int PosX, int PosY, string tileState)
        {
            switch (tileState)
            {
                case ("Superbomb"):
                    curTileState[PosX, PosY] = TileStates.Powerup;
                    powerupTileState[PosX, PosY] = PowerupStates.Superbomb;
                    break;

                case ("Shield"):
                    curTileState[PosX, PosY] = TileStates.Powerup;
                    powerupTileState[PosX, PosY] = PowerupStates.Shield;
                    break;

                case ("Lifeup"):
                    curTileState[PosX, PosY] = TileStates.Powerup;
                    powerupTileState[PosX, PosY] = PowerupStates.Lifeup;
                    break;
            }
        }

    }
}
