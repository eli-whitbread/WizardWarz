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

namespace WizardWarz
{
    /// <summary>
    /// Interaction logic for MainGameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl
    {
        public Int32 tileSize = 64;
        double varRotTransform = 90;
        double gameLevelTime = 120;
        protected static GameTimer gameTimerInstance;
        protected static GameBoardManager gameBoardManager;
        protected static Canvas GameCanvasInstance;
        public Grid MainGameGrid;
        public RotateTransform trRot = null;
        protected static int noOfPlayers = 6;
        public PlayerController[] playerControllers = null;
        public PlayerLivesAndScore[] playerLives;
        public Powerup powerupRef = null;
             

        public static Canvas ReturnnMainCanvas()
        {
            return GameCanvasInstance;

        }

        public static GameTimer ReturnTimerInstance()
        {
            return gameTimerInstance;
        }

        public static GameBoardManager ReturnGameBoardInstance()
        {
            return gameBoardManager;
        }


        public static int ReturnNumberOfPlayer()
        {
            return noOfPlayers;
        }

        public GameWindow()
        {
            InitializeComponent();

            GameCanvasInstance = GameCanvas;
            MainGameGrid = GameBoardGrid;

            if (gameTimerInstance == null)
            {
                gameTimerInstance = new GameTimer();

            }

            gameTimerInstance.Initialise();

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

                // --------------------------- Initialise All Players Lives and Score Controls -----------------------------
                initialisePlayerLivesAndScore(i);
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
