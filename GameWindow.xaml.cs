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
        public Grid MainGameGrid;
        public RotateTransform trRot = null;
        protected static int noOfPlayers = 6;
        public PlayerController[] playerControllers = null;
        public PlayerLivesAndScore[] playerLives;
        public Powerup powerupRef = null;

        private DispatcherTimer endTimer;
        public int gameTimeSeconds = 00;
        public int gameTimeMinutes = 4;


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

        public static GameWindow ReturnGameWindowInstance()
        {
            return gameWindowInstance;
        }

        public static Int32 ReturnTileSize()
        {
            return tileSize;
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
                noOfPlayers = 6;
            else
                noOfPlayers = 4;


            initialiseGameBoardSize();
            
            gameBoardManager = new GameBoardManager();
            gameBoardManager.gameGrid = MainGameGrid;
            gameBoardManager.InitializeGameBoard();

            playerControllers = new PlayerController[noOfPlayers];
            playerLives = new PlayerLivesAndScore[noOfPlayers];
           
            initialisePlayerReferences();            

            Debug.WriteLine(GameCanvasInstance.Name);

            StaticCollections _staticColections = new StaticCollections();

            powerupRef = new Powerup();
            powerupRef.curGameGrid = MainGameGrid;
            powerupRef.InitialisePowerups();
            gameTimerInstance.puRef = powerupRef;


            // End timer
            endTimer = new DispatcherTimer(DispatcherPriority.Render);
            endTimer.Interval = TimeSpan.FromSeconds(1);
            endTimer.Tick += new EventHandler(timer_Tick);
            endTimer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            gameTimeSeconds -= 1;

            if (gameTimeSeconds <= -1)
            {
                gameTimeSeconds = 59;
                gameTimeMinutes -= 1;

                if (gameTimeMinutes <= -1)
                {
                    MessageBox.Show("Three minutes passed. End of game reached.");
                    gameTimerInstance.gameLoopTimer.Stop();

                    MainWindow mwRef = MainWindow.ReturnMainWindowInstance();
                    mwRef.GameEnd();
                } 
            }

            // "D2" = Standard Numeric Formatting. Ensures that the seconds will always be displayed in double digits.
            gameTimeText.Content = gameTimeMinutes + ":" + gameTimeSeconds.ToString("D2");
        }

        private void initialiseGameBoardSize()
        {
            if (noOfPlayers == 4)
            {
                gameTimeText.Margin = new Thickness(384, 0, -10, 0);
                TopPanel.HorizontalAlignment = HorizontalAlignment.Center;
                BottomPanel.HorizontalAlignment = HorizontalAlignment.Center;
                BottomPanel.Margin = new Thickness(60, 0, 0, 0);
            }
        }

        public void initialisePlayerReferences()
        {
            // ------------------------------- Initialise Player References ------------------------------------------------
            for (int i = 0; i <= noOfPlayers - 1; i++)
            {
                playerControllers[i] = new PlayerController();
                playerLives[i] = new PlayerLivesAndScore();
                playerControllers[i].playerPosition = i;
                playerControllers[i].localGameGrid = MainGameGrid;                               
                playerControllers[i].highlightLocalGrid = MainGameGrid;
                playerControllers[i].managerRef = gameBoardManager;
                playerControllers[i].gridCellsArray = gameBoardManager.flrTiles;
                playerControllers[i].myLivesAndScore = playerLives[i];
                playerControllers[i].initialisePlayerGridRef();
                playerControllers[i].myPowerupRef = new Powerup();
                playerControllers[i].playerName = "Player " + (i + 1).ToString();


                // --------------------------- Initialise All Players Lives and Score Controls -----------------------------
                initialisePlayerLivesAndScore(i);
                //Console.WriteLine("New player: {0}", playerControllers[i].playerName);
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
    }
}
