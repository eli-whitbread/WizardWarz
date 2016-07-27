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
    public enum PowerupType
    {
        Superbomb,
        Shield
    }


    public class Powerups
    {
        public int powerupCount;
        public int powerupType;
        public int gameTime;
        public static PowerupType[,] _powerupTile = null;

        GameBoardManager _localGameBoard = new GameBoardManager();
        public Grid curGameGrid = PlayerController.localGameGrid;
        public int[,] powerupPositions { get; set; }

        public static int xPos;
        public static int yPos;

        // Used to prevent the powerups from spawning on wall tiles
        bool xTrue = true;
        bool yTrue = true;


        public void InitialisePowerups()
        {
          
        }

        public void Count()
        {
            powerupCount += 1;
            Debug.WriteLine("Power up count: {0}", powerupCount);
            if (powerupCount >= 2)
            {
                powerupCount = 0;
                WallCheck();
            }
        }


        public void WallCheck()
        {
            xTrue = true;
            yTrue = true;

            Random rand = new Random();
            xPos = rand.Next(1, 16);
            Console.WriteLine("xPos: {0}", xPos);
            yPos = rand.Next(1, 11);
            Console.WriteLine("yPos: {0}", yPos);

            for (int x = 0; x < _localGameBoard.innerWallPos.GetLength(0); x++)
            {
                if (xPos == _localGameBoard.innerWallPos[x, 0])
                    xTrue = false;
                if (yPos == _localGameBoard.innerWallPos[x, 1])
                    yTrue = false;
            }

           for (int y = 0; y < _localGameBoard.destructibleWallPos.GetLength(0); y++)
            {
                if (xPos == _localGameBoard.destructibleWallPos[y, 0])
                    xTrue = false;
                if (yPos == _localGameBoard.destructibleWallPos[y, 1])
                    yTrue = false;
            }


            if (xTrue || yTrue)
            {
                Random randType = new Random();
                // The right-most number should be equal to the amount of powerups we've created minus 1
                powerupType = randType.Next(0, 2);

                if (powerupType == 0)
                {
                    SpawnPowerup("SuperBomb");
                    Console.WriteLine("Super bomb");
                }
                else if (powerupType == 1)
                {
                    SpawnPowerup("Shield");
                    Console.WriteLine("Shield");
                }
                else
                    Console.WriteLine("Powerup type error.");
            }

            else if (!xTrue && !yTrue)
                Console.WriteLine("Wall detected");

            // Just in case
            else
                Console.WriteLine("Unexpected outcome"); 
        }

        public void SpawnPowerup (string powerupName)
        {
            // Set general powerup properties
            Rectangle powerupTile = new Rectangle();
            powerupTile.Height = 64;
            powerupTile.Width = 64;
            Grid.SetColumn(powerupTile, xPos);
            Grid.SetRow(powerupTile, yPos);


            // Set properties unique to each powerup
            switch(powerupName)
            {
                case ("SuperBomb"):
                    powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\superbomb.png", UriKind.Relative)));
                    break;
                case ("Shield"):
                    powerupTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\shield.png", UriKind.Relative)));
                    break;
            }
                
            //
            curGameGrid.Children.Add(powerupTile);

        }
    }
}
