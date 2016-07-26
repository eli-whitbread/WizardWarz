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
        protected static GameTimer gameTimerInstance;
        protected static GameBoardManager gameBoardManager;
        protected static Canvas GameCanvasInstance;
        public Grid MainGameGrid;
        public RotateTransform trRot = null;
        protected static int noOfPlayers = 4;
        public PlayerController[] playerControllers = null;
        public PlayerLivesAndScore[] playerLives;
        //PlayerLivesAndScore _player_1_Lives = null;
        //PlayerLivesAndScore _player_2_Lives = null;
        //PlayerLivesAndScore _player_3_Lives = null;
        //PlayerLivesAndScore _player_4_Lives = null;


        //public Canvas mainCanvas
        //{
        //    get { return GameCanvas; }
        //}        

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

            

            gameBoardManager = new GameBoardManager();
            gameBoardManager.gameGrid = MainGameGrid;
            gameBoardManager.InitializeGameBoard();

            playerControllers = new PlayerController[noOfPlayers];
            playerLives = new PlayerLivesAndScore[noOfPlayers];
           
            initialisePlayerReferences();            

            Debug.WriteLine(GameCanvasInstance.Name);

            StaticCollections _staticColections = new StaticCollections();

            //GameTimer gT = new GameTimer();
            //gT.GameCanRef = mainCanvas;
            //gT.p1Ref = _playerController1;
            //gT.Initialise();

            
            //gameTimerInstance.GameCanRef = mainCanvas;
             
            //gameTimerInstance.p1Ref = _playerController1; (REMEMBER TO RECONNECT THIS with EVENT IN PLAYERCONTROLLER)
            

            //_playerController1.timerRef = gameTimerInstance;
            

           
        }

        public void initialisePlayerReferences()
        {
            // --------------------------- Initialise Player References ------------------------------------------------
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
                //Debug.WriteLine("Player Controler {0} initialised /n", playerControllers[i]);
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
                    Debug.WriteLine("Not enough for five players!");
                    break;

                case 6:
                    Debug.WriteLine("Not enough for six players!");
                    break;

                default:
                    Debug.WriteLine("Nothing Happened!!");
                    break;

            }
        }
    }
}
