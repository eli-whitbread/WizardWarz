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

        protected static GameTimer gameTimerInstance;

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

            GameBoardManager _gameBoardManager = new GameBoardManager();
            _gameBoardManager.InitializeGameBoard(GameBoardGrid);

            

            PlayerLivesAndScore _player_1_Lives = new PlayerLivesAndScore();
            TopPanel.Children.Add(_player_1_Lives);

            PlayerController _playerController1 = new PlayerController(GameBoardGrid);
            _playerController1.managerRef = _gameBoardManager;
            _playerController1.gameCanRef = mainCanvas;
            _playerController1.gridCellsArray = _gameBoardManager.flrTiles;
            _playerController1.myLivesAndScore = _player_1_Lives;
            _playerController1.InitialiseRefs();

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

        
    }
}
