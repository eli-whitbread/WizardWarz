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
    public enum PowerupTypes
    {
        Superbomb,
        Shield
    }


    public class Powerup
    {
        public static string pName;
        public int powerupCount;
        public int powerupType;
        public int gameTime;
        public static PowerupTypes[,] _powerupTile = null;

        GameBoardManager _localGameBoard = new GameBoardManager();
        public Grid curGameGrid = null;

        public static int xPos;
        public static int yPos;

        // Used to prevent the powerups from spawning on wall tiles
        bool xFlag = true;
        bool yFlag = true;

        public TileStates[,] curTileStates = GameBoardManager.curTileState;
        public PowerupStates[,] powerupTileStates = GameBoardManager.powerupTileState;

        //// Use to retrieve each individual powerup
        //public List<puRef> pList = new List<puRef>();
        //// used to index powerups
        //public int totalObjects;

        //public class Powerup
        //{
        //    public int pIndex { get; set; }
        //    public string pName { get; set; }
        //    public int xPos { get; set; }
        //    public int yPos { get; set; }

        //    public Powerup(int index, string name, int posX, int posY)
        //    {
        //        pIndex = index;
        //        pName = name;
        //        xPos = posX;
        //        yPos = posY;
        //    }
        //}


        public void InitialisePowerups()
        {

        }

        public void Count()
        {
            powerupCount += 1;
            //Debug.WriteLine("Power up count: {0}", powerupCount);
            if (powerupCount >= 2)
            {
                powerupCount = 0;

                TileCheck();
            }
        }


        public void TileCheck()
        {
            xFlag = true;
            yFlag = true;


            Random rand = new Random();
            xPos = rand.Next(1, 12);
            //Console.WriteLine("xPos: {0}", xPos);
            yPos = rand.Next(1, 12);
            //Console.WriteLine("yPos: {0}", yPos);

            // Compare x-coordinates
            for (int x = 0; x < _localGameBoard.innerWallPos.GetLength(0); x++)
            {
                if (xPos == _localGameBoard.innerWallPos[x, 0])
                    xFlag = false;
                if (yPos == _localGameBoard.innerWallPos[x, 1])
                    yFlag = false;
            }

            // Compare y-coordinates
            for (int y = 0; y < _localGameBoard.destructibleWallPos.GetLength(0); y++)
            {
                if (xPos == _localGameBoard.destructibleWallPos[y, 0])
                    xFlag = false;
                if (yPos == _localGameBoard.destructibleWallPos[y, 1])
                    yFlag = false;
            }

            // Check for existing powerups
            // Need to check for player positions as well
            {
                if (GameBoardManager.curTileState[xPos, yPos] == TileStates.Powerup)
                {
                    xFlag = false;
                    yFlag = false;
                }
            }

            if (xFlag || yFlag)
            {
                Random randType = new Random();
                // The right-most number should be equal to the amount of powerups we've created 
                powerupType = randType.Next(0, 2);

                if (powerupType == 0)
                {
                    SpawnPowerup("SuperBomb");
                    Console.WriteLine("Superbomb spawned");
                }
                else if (powerupType == 1)
                {
                    SpawnPowerup("Shield");
                    Console.WriteLine("Shield spawned");
                }
                else
                    Console.WriteLine("Powerup type error.");
            }

            else if (!xFlag && !yFlag)
                Console.WriteLine("Obstacle detected");

            // Just in case
            else
                Console.WriteLine("Unexpected outcome");
        }

        public void SpawnPowerup(string powerupName)
        {
            // Set general powerup properties
            Rectangle powerupTile = new Rectangle();

            powerupTile.Height = 64;
            powerupTile.Width = 64;
            Grid.SetColumn(powerupTile, xPos);
            Grid.SetRow(powerupTile, yPos);

            GameBoardManager.curTileState[xPos, yPos] = TileStates.Powerup;
            // Set properties unique to each powerup
            switch (powerupName)
            {
                case ("SuperBomb"):
                    powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\superbomb.png", UriKind.Relative)));
                    _localGameBoard.ChangeTileState(xPos, yPos, "Superbomb");
                    pName = "Superbomb";
                    break;
                case ("Shield"):
                    powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\shield.png", UriKind.Relative)));
                    _localGameBoard.ChangeTileState(xPos, yPos, "Shield");
                    pName = "Shield";
                    break;
            }

            curGameGrid.Children.Add(powerupTile);
            Console.WriteLine("Game Grid children: {0}", curGameGrid.Children.Count);
        }


        // Delete a single powerup, then return the name of it.
        // Used to empower players.
        public string ReturnPowerup(int col, int row, Grid GameGrid)
        {
            //Console.WriteLine("Game Grid children: {0}", GameGrid.Children.Count);

            for (int i = 220; i < GameGrid.Children.Count; i++)
            {
                UIElement elem = GameGrid.Children[i];

                if (Grid.GetRow(elem) == row && Grid.GetColumn(elem) == col)
                {
                    if (GameBoardManager.curTileState[col, row] == TileStates.Powerup)
                    {
                        // Remove the powerup, then return it's name.
                        GameBoardManager.curTileState[col, row] = TileStates.Floor;
                        GameGrid.Children.Remove(elem);

                        if (GameBoardManager.powerupTileState[col, row] == PowerupStates.Superbomb)
                        {
                            GameBoardManager.powerupTileState[col, row] = PowerupStates.Empty;
                            //MessageBox.Show("Superbomb!");
                            return "Superbomb";
                        }
                        else if (GameBoardManager.powerupTileState[col, row] == PowerupStates.Shield)
                        {
                            GameBoardManager.powerupTileState[col, row] = PowerupStates.Empty;
                            //MessageBox.Show("Shield!");
                            return "Shield";
                        }
                    }
                }
            }

            return null;
        }
    }
}