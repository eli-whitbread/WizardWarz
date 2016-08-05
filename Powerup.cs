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
        Shield,
        Lifeup
    }


    public class Powerup
    {
        public static string pName;
        public int powerupCount;
        public int powerupType;
        public int gameTime;
        public static PowerupTypes[,] _powerupTile = null;
        protected static Powerup powerupRef = null;

        GameBoardManager _localGameBoard = new GameBoardManager();
        public Grid curGameGrid = null;

        public int xPos;
        public int yPos;

        // Used to prevent the powerups from spawning on wall tiles
        bool xFlag = true;
        bool yFlag = true;

        public TileStates[,] curTileStates = GameBoardManager.curTileState;
        public PowerupStates[,] powerupTileStates = GameBoardManager.powerupTileState;


        public void InitialisePowerups()
        {
            //powerupRef = this;
        }

        //public static Powerup ReturnPowerupReference()
        //{
        //    return powerupRef;
        //}
        

        public void Count()
        {
            powerupCount += 1;
            //Debug.WriteLine("Power up count: {0}", powerupCount);
            if (powerupCount >= 1)
            {
                powerupCount = 0;

                Random rand = new Random();
                xPos = rand.Next(1, 12);
                yPos = rand.Next(1, 12);

                if (_localGameBoard.innerWallPos != null && _localGameBoard.destructibleWallPos != null)
                {
                    TileCheck();
                }
            }
        }


        public void TileCheck()
        {
            xFlag = true;
            yFlag = true;

            //Console.WriteLine("xPos: {0}", xPos);
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
                //Random randType = new Random();
                // The right-most number should be equal to the amount of powerups we've created 
                //powerupType = randType.Next(0, 3);
                powerupType = GameWindow.ReturnRandomPowerUpNo();

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
                else if (powerupType == 2)
                {
                    SpawnPowerup("Lifeup");
                    Console.WriteLine("Lfieup spawned");
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

            powerupTile.Height = GameWindow.ReturnTileSize(); 
            powerupTile.Width = GameWindow.ReturnTileSize();
            Grid.SetColumn(powerupTile, xPos);
            Grid.SetRow(powerupTile, yPos);

            GameBoardManager.curTileState[xPos, yPos] = TileStates.Powerup;
            // Set properties unique to each powerup
            switch (powerupName)
            {
                case ("SuperBomb"):
                    pName = "Superbomb";
                    powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\superbomb.png", UriKind.Relative)));
                    _localGameBoard.ChangeTileState(xPos, yPos, "Superbomb");
                    break;
                case ("Shield"):
                    pName = "Shield";
                    powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\shield.png", UriKind.Relative)));
                    _localGameBoard.ChangeTileState(xPos, yPos, "Shield");
                    break;
                case ("Lifeup"):
                    pName = "Lifeup";
                    powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\heart.png", UriKind.Relative)));
                    _localGameBoard.ChangeTileState(xPos, yPos, "Lifeup");
                    break;
            }

            curGameGrid.Children.Add(powerupTile);
        }


        // Spawn a powerup at a wall position
        public void WallSpawn(int PosX, int PosY, Grid GameGrid)
        {
            Rectangle powerupTile = new Rectangle();

            //Random r = new Random();
            //int rand = r.Next(0, 3);

            int rand = GameWindow.ReturnRandomPowerUpNo();
            //MessageBox.Show(string.Format("Random number: {0}", rand));
            if (rand == 0)
            {
                pName = "Superbomb";
                powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\superbomb.png", UriKind.Relative)));
                _localGameBoard.ChangeTileState(PosX, PosY, "Superbomb");
                //MessageBox.Show("Superbomb made");    
            }
            else if (rand == 1)
            {
                pName = "Shield";
                powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\shield.png", UriKind.Relative)));
                _localGameBoard.ChangeTileState(PosX, PosY, "Shield");
                //MessageBox.Show("Shield made");
            }
            else if (rand == 2)
            {
                pName = "Lifeup";
                powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\heart.png", UriKind.Relative)));
                _localGameBoard.ChangeTileState(PosX, PosY, "Lifeup");
                //MessageBox.Show("Lifeup made");
            }

            powerupTile.Height = GameWindow.ReturnTileSize();
            powerupTile.Width = GameWindow.ReturnTileSize();
            Grid.SetColumn(powerupTile, PosX);
            Grid.SetRow(powerupTile, PosY);
            GameBoardManager.curTileState[PosX, PosY] = TileStates.Powerup;
            GameGrid.Children.Add(powerupTile);
        }


        // Delete a single powerup, then return the name of it.
        // Used to empower players.
        public string ReturnPowerup(int col, int row, Grid GameGrid)
        {
            for (int i = 300; i < GameGrid.Children.Count; i++)
            {
                UIElement elem = GameGrid.Children[i];

                if (Grid.GetRow(elem) == row && Grid.GetColumn(elem) == col)
                {
                    //MessageBox.Show(string.Format("Child number: {0}. Total children: {1}", i, GameGrid.Children.Count));

                    if (GameBoardManager.curTileState[col, row] == TileStates.Powerup)
                    {
                        // Remove the powerup, then return its name.
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
                        else if (GameBoardManager.powerupTileState[col, row] == PowerupStates.Lifeup)
                        {
                            GameBoardManager.powerupTileState[col, row] = PowerupStates.Empty;
                            //MessageBox.Show("Lifeup!");
                            return "Lifeup";
                        }
                    }
                }
            }

            return null;
        }
    }
}