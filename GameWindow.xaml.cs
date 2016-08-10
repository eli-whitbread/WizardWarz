using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;

namespace WizardWarz
{
    /// <summary>
    /// Interaction logic for MainGameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl
    {
        protected static Int32 tileSize = 64;
        double varRotTransform = 90;
        protected static GameTimer gameTimerInstance;
        protected static GameBoardManager gameBoardManager;
        protected static Canvas GameCanvasInstance;
        protected static GameWindow gameWindowInstance;
        protected static List<PlayerController> PlayerListRef;
        public Grid MainGameGrid;
        public RotateTransform trRot = null;
        protected static int noOfPlayers = 6;
        public PlayerController[] playerControllers = null;
        public PlayerLivesAndScore[] playerLives;
        public Powerup powerupRef = null;
        public Int32 playersOnBoard;
        //public Int32[,] playerPositions;
        public List<PlayerController> ListOfPlayers = new List<PlayerController>();
        protected static Int16 randomNo4PowerUps;
        private DispatcherTimer endTimer;
        public int gameTimeSeconds = 00;
        public int gameTimeMinutes = 4;
        public double currentTick;

        //Hello
        //Random number generator instance
        public RandomNumberGenerator RNG = new RandomNumberGenerator();


        /// <summary>
        /// Returns reference to MainWindow Canvas. <para> The Canvas exists above the game Grid, so will be rendered first.</para>
        /// </summary>
        public static Canvas ReturnMainCanvas()
        {
            return GameCanvasInstance;
        }

        /// <summary>
        /// Returns a references to the Game Timer. <para> This provides a way to easily reference the game timer (once, instead of several instances), within any class. </para>
        /// </summary>
        public static GameTimer ReturnTimerInstance()
        {
            return gameTimerInstance;
        }

        /// <summary>
        /// Returns a reference to the GameBoardManager. <para> The GameBoard Manager holds tile/floor state information. </para>
        /// </summary>
        public static GameBoardManager ReturnGameBoardInstance()
        {
            return gameBoardManager;
        }

        /// <summary>
        /// Returns the number of players in the current game instance. <para> The game board size is different for both 4 or 6 players, wherein the latter is larger. If you use this reference, make sure what is placed on the board corresponds. </para>
        /// </summary>
        public static int ReturnNumberOfPlayer()
        {
            return noOfPlayers;
        }

        /// <summary>
        /// Returns a reference to the Main Window. 
        /// </summary>
        public static GameWindow ReturnGameWindowInstance()
        {
            return gameWindowInstance;
        }

        /// <summary>
        /// Returns the games set tile size. <para> Wizard Warz as default uses 64x64. </para>
        /// </summary>
        public static Int32 ReturnTileSize()
        {
            return tileSize;
        }

        /// <summary>
        /// Returns the globally callable player list - 1-4/ 1-6. Provides access to all players. <para> This list provides access to all players within the game - meaning other classes can interact with them as needed. </para>
        /// </summary>
        public static List<PlayerController> ReturnPlayerList()
        {
            return PlayerListRef;
        }

        /// <summary>
        /// Returns the globally callable random Number - between 0 and 3. <para> This number is generated within GameWindow, each tick, thus providing some randomness. </para>
        /// </summary>
        public static Int16 ReturnRandomPowerUpNo()
        {
            return randomNo4PowerUps;
        }

        public GameWindow()
        {
            InitializeComponent();

            GameCanvasInstance = GameCanvas;
            MainGameGrid = GameBoardGrid;
            gameWindowInstance = this;


            if (gameTimerInstance == null)
            {
                gameTimerInstance = new GameTimer();

            }

            gameTimerInstance.Initialise();

            // The number of players needs to be set before the game board is initialized
            if (MainMenu.GlobalPlayerMainMenu)
            { noOfPlayers = 6; playersOnBoard = 6; }
            else
            { noOfPlayers = 4; playersOnBoard = 4; }


            initialiseGameBoardSize();
            
            gameBoardManager = new GameBoardManager();
            gameBoardManager.gameGrid = MainGameGrid;
            gameBoardManager.InitializeGameBoard();

            playerControllers = new PlayerController[noOfPlayers];
            playerLives = new PlayerLivesAndScore[noOfPlayers];
            //playerPositions = new int[noOfPlayers];
            initialisePlayerReferences();            

            Debug.WriteLine(GameCanvasInstance.Name);

            StaticCollections _staticColections = new StaticCollections();

            powerupRef = new Powerup();
            powerupRef.curGameGrid = MainGameGrid;
            powerupRef.InitialisePowerups();
            gameTimerInstance.puRef = powerupRef;
      

            // End timer
            endTimer = new DispatcherTimer(DispatcherPriority.Render);
            endTimer.Interval = TimeSpan.FromSeconds(0.5);
            endTimer.Tick += new EventHandler(timer_Tick);
            endTimer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            currentTick += 0.5;

            //if (currentTick % 5 == 0)
            //{
            //    foreach(PlayerController player in ListOfPlayers)
            //    {
            //        MessageBox.Show(string.Format("{0} position: {1}", player.playerName, player.playerPosition));
            //    }
            //}

            if (currentTick % 1 == 0) {

                // Decrement the timer
                gameTimeSeconds -= 1;

                if (gameTimeSeconds <= -1)
                {
                    gameTimeSeconds = 59;
                    gameTimeMinutes -= 1;

                    if (gameTimeMinutes <= -1)
                    {
                        //MessageBox.Show("Four minutes passed. End of game reached.");
                        gameTimerInstance.gameLoopTimer.Stop();

                        MainWindow mwRef = MainWindow.ReturnMainWindowInstance();
                        mwRef.GameEnd();
                    }
                }

                //// Use the Generate method to return the first number of a set of randomly generated numbers.
                //if (currentTick % 1 == 0)
                //{
                //    int number = RNG.GenerateRandomNumber();

                //    // Simple method of returning the first digit of the 'number' variable
                //    //result = Math.Floor(number / Math.Pow(10, Math.Floor(Math.Log10(number))));

                //    // Testing random funtion.
                //    double result;
                //    if (number <= 85) { result = 0; }
                //    else if (number >= 86 && number <= 190) { result = 1; }
                //    else { result = 2; }

                //    Console.WriteLine("Random Number: {0} {1}", result, Environment.NewLine);
                //    //Console.WriteLine();
                //}
            }

            //provideAllPlayerPositions();
            CheckPlayersOnBoard();

            // "D2" = Standard Numeric Formatting. Ensures that the seconds will always be displayed in double digits.
            gameTimeText1.Content = gameTimeMinutes + ":" + gameTimeSeconds.ToString("D2");
            gameTimeText2.Content = gameTimeMinutes + ":" + gameTimeSeconds.ToString("D2");

            Random tempRandom = new Random();
            randomNo4PowerUps = (short)tempRandom.Next(0, 3);
        }

        private void initialiseGameBoardSize()
        {
            if (noOfPlayers == 4)
            {
                gameTimeText1.Margin = new Thickness(900, 0, -10, 0);
                gameTimeText2.Margin = new Thickness(-252, 0, -10, 0);
                TopPanel.HorizontalAlignment = HorizontalAlignment.Center;
                BottomPanel.HorizontalAlignment = HorizontalAlignment.Center;
                BottomPanel.Margin = new Thickness(60, 0, 0, 0);
            }
            else
            {
                gameTimeText1.Margin = new Thickness(400, 0, -10, 0);
                gameTimeText2.Margin = new Thickness(-252, 0, -10, 0);

            }
        }

        public void initialisePlayerReferences()
        {
            // Set the reference
            PlayerListRef = ListOfPlayers;


            // ------------------------------- Initialise Player References ------------------------------------------------
            for (int i = 0; i <= noOfPlayers - 1; i++)
            {
                playerControllers[i] = new PlayerController();
                playerLives[i] = new PlayerLivesAndScore();
                playerControllers[i].playerStartPos = i;
                //playerControllers[i].playerPosition = new Point(0, 0);
                playerControllers[i].localGameGrid = MainGameGrid;                               
                playerControllers[i].highlightLocalGrid = MainGameGrid;
                playerControllers[i].managerRef = gameBoardManager;
                playerControllers[i].gridCellsArray = gameBoardManager.flrTiles;
                playerControllers[i].myLivesAndScore = playerLives[i];
                playerControllers[i].initialisePlayerGridRef();
                playerControllers[i].myPowerupRef = new Powerup();
                playerControllers[i].playerName = "Player " + (i + 1).ToString();

                // Add the player to the list
                ListOfPlayers.Add(playerControllers[i]);

                // --------------------------- Initialise All Players Lives and Score Controls -----------------------------
                initialisePlayerLivesAndScore(i);

            }

            // I had to hard code the player's starting positions, but they'll update properly whenever the player moves.
            for (int x = 0; x < noOfPlayers; x++)
            {
                playerControllers[0].playerPosition = new Point(64, 64);

                if (noOfPlayers == 4)
                {
                    playerControllers[1].playerPosition = new Point(1344, 64);
                    playerControllers[2].playerPosition = new Point(1344, 704);
                    playerControllers[3].playerPosition = new Point(64, 704);
                }
                else if (noOfPlayers == 6)
                {
                    playerControllers[1].playerPosition = new Point(1344, 384);
                    playerControllers[2].playerPosition = new Point(64, 704);
                    playerControllers[3].playerPosition = new Point(64, 384);
                    playerControllers[4].playerPosition = new Point(1344, 64);
                    playerControllers[5].playerPosition = new Point(1344, 704);
                }
            }
        }

        public void initialisePlayerLivesAndScore(int currentPlayer)
        {
            switch(currentPlayer + 1)
            {
                case 1:

                    varRotTransform = 180;
                    trRot = new RotateTransform(varRotTransform);
                    playerLives[currentPlayer].LayoutTransform = trRot;
                    TopPanel.Children.Add(playerLives[currentPlayer]);
                    break;

                case 2:
                    varRotTransform = -90;
                    trRot = new RotateTransform(varRotTransform);
                    playerLives[currentPlayer].LayoutTransform = trRot;
                    RightPanel.Children.Add(playerLives[currentPlayer]);
                    break;

                case 3:
                    BottomPanel.Children.Add(playerLives[currentPlayer]);
                    break;

                case 4:
                    varRotTransform = 90;
                    trRot = new RotateTransform(varRotTransform);
                    playerLives[currentPlayer].LayoutTransform = trRot;
                    LeftPanel.Children.Add(playerLives[currentPlayer]);
                    break;

                case 5:                    
                    varRotTransform = 180;
                    trRot = new RotateTransform(varRotTransform);
                    playerLives[currentPlayer].LayoutTransform = trRot;
                    TopPanel2.Children.Add(playerLives[currentPlayer]);
                    break;

                case 6:
                    BottomPanel2.Children.Add(playerLives[currentPlayer]);
                    break;

                default:
                    Debug.WriteLine("Nothing Happened!!");
                    break;
            }
        }

        //public void provideAllPlayerPositions()
        //{
        //    for(int i = 0; i < playerControllers.Length; i++)
        //    {
        //        for(int j = 0; j < playerControllers.Length; j++)
        //        {
        //            //Debug.WriteLine("Player " + i + ", is located at point: " + playerControllers[i].relativePosition.X + "x , " + playerControllers[i].relativePosition.Y + "y");
        //            // Array of Arrays? Need to store two player ints, in each array, and store that array in another array which can be called globally - and checked in bombs.
        //        }     
        //    }
        //}

        private void CheckPlayersOnBoard()
        {
            for (int i = 0; i < noOfPlayers; i++)
            {
                if (playerControllers[i].myLivesAndScore.playerLivesNumber == 0 && playerControllers[i] != null)
                {
                    MainGameGrid.Children.Remove(playerControllers[i].playerTile);
                    MainGameGrid.Children.Remove(playerControllers[i].playerTileAnimOverlay);

                    playerControllers[i].myLivesAndScore.playerLivesNumber = -1;
                    playersOnBoard--;

                    if (playersOnBoard <= 1)
                    {
                        gameTimeMinutes = 0;
                        gameTimeSeconds = 0;
                    }
                }
            }
        }
    }
}
