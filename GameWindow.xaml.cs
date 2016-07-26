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
        public int noOfPlayers;
        GameBoardManager _gameBoardManager = null;
        PlayerController _playerController1 = null;
        PlayerLivesAndScore _player_1_Lives = null;
        PlayerLivesAndScore _player_2_Lives = null;
        PlayerLivesAndScore _player_3_Lives = null;
        PlayerLivesAndScore _player_4_Lives = null;


        public Canvas mainCanvas
        {
            get { return GameCanvas; }
        }        

        public static GameTimer ReturnTimerInstance()
        {
            return gameTimerInstance;
        }

        public GameWindow()
        {
            InitializeComponent();            

            _gameBoardManager = new GameBoardManager();
            _gameBoardManager.InitializeGameBoard(GameBoardGrid);

            initialisePlayerReferences();            

            Debug.WriteLine(mainCanvas.Name);

            StaticCollections _staticColections = new StaticCollections();

            //GameTimer gT = new GameTimer();
            //gT.GameCanRef = mainCanvas;
            //gT.p1Ref = _playerController1;
            //gT.Initialise();

            if (gameTimerInstance == null)
            {
                gameTimerInstance = new GameTimer();
            }
            gameTimerInstance.GameCanRef = mainCanvas;
            gameTimerInstance.p1Ref = _playerController1;
            gameTimerInstance.Initialise();

            _playerController1.timerRef = gameTimerInstance;
            //_playerController1.timerRef = gT;

           
        }

        public void initialisePlayerReferences()
        {
            // --------------------------- Initialise All Players Lives and Score Controls -----------------------------
            initialisePlayerLivesAndScore();

            // --------------------------- Initialise Player References ------------------------------------------------
            _playerController1 = new PlayerController(GameBoardGrid);
            _playerController1.managerRef = _gameBoardManager;
            _playerController1.gameCanRef = mainCanvas;
            _playerController1.gridCellsArray = _gameBoardManager.flrTiles;
            _playerController1.myLivesAndScore = _player_1_Lives;
            _playerController1.InitialiseRefs();
        }

        public void initialisePlayerLivesAndScore()
        {
            _player_1_Lives = new PlayerLivesAndScore();
            _player_2_Lives = new PlayerLivesAndScore();
            _player_3_Lives = new PlayerLivesAndScore();
            _player_4_Lives = new PlayerLivesAndScore();

            varRotTransform = 180;
            RotateTransform trRot = new RotateTransform(varRotTransform);
            _player_1_Lives.LayoutTransform = trRot;
            TopPanel.Children.Add(_player_1_Lives);
            trRot = null;

            varRotTransform = -90;
            trRot = new RotateTransform(varRotTransform);
            _player_2_Lives.LayoutTransform = trRot;
            RightPanel.Children.Add(_player_2_Lives);
            trRot = null;

            BottomPanel.Children.Add(_player_3_Lives);

            varRotTransform = 90;
            trRot = new RotateTransform(varRotTransform);
            _player_4_Lives.LayoutTransform = trRot;
            LeftPanel.Children.Add(_player_4_Lives);

        }

        public void initialiseEachPlayer()
        {
            for(int i = 0; i < noOfPlayers; i++)
            {


            }

        }

        
    }
}
