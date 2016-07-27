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
//using Microsoft.Surface;
//using Microsoft.Surface.Core;
//using Microsoft.Surface.Presentation.Controls;
//using Microsoft.Surface.Presentation.Input;
using System.Diagnostics;

namespace WizardWarz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Int32 tileSize = 64;

        public Canvas mainCanvas
        {
            get { return GameCanvas; }
        }

        public MainWindow()
        {
            InitializeComponent();

            GameBoardManager _gameBoardManager = new GameBoardManager();
            _gameBoardManager.InitializeGameBoard(GameBoardGrid);

            PlayerController _playerController1 = new PlayerController(GameBoardGrid, "playerOne");
            _playerController1.mangerRef = _gameBoardManager;
            _playerController1.InitialiseRefs();

            PlayerController _playerController2 = new PlayerController(GameBoardGrid, "playerTwo");
            _playerController2.mangerRef = _gameBoardManager;
            _playerController2.InitialiseRefs();

            PlayerController _playerController3 = new PlayerController(GameBoardGrid, "playerThree");
            _playerController3.mangerRef = _gameBoardManager;
            _playerController3.InitialiseRefs();

            PlayerController _playerController4 = new PlayerController(GameBoardGrid, "playerFour");
            _playerController4.mangerRef = _gameBoardManager;
            _playerController4.InitialiseRefs();

            PlayerController _playerController5 = new PlayerController(GameBoardGrid, "playerFive");
            _playerController5.mangerRef = _gameBoardManager;
            _playerController5.InitialiseRefs();

            PlayerController _playerController6 = new PlayerController(GameBoardGrid, "playerSix");
            _playerController6.mangerRef = _gameBoardManager;
            _playerController6.InitialiseRefs();

            Debug.WriteLine(mainCanvas.Name);

            GameTimer gT = new GameTimer();
            gT.GameCanRef = mainCanvas;
            gT.p1Ref = _playerController1;
            gT.p2Ref = _playerController2;
            gT.p3Ref = _playerController3;
            gT.p4Ref = _playerController4;
            gT.p5Ref = _playerController5;
            gT.p6Ref = _playerController6;

            gT.Initialise();
            gT.localGameGrid = GameBoardGrid;

            Powerups powerupRef = new Powerups();
            powerupRef.InitialisePowerups();


            Bomb _masterBombClass = new Bomb();
            _masterBombClass.curGameGrid = GameBoardGrid;
            //test bomb
            //_masterBombClass.InitialiseBomb(7, 3, 3);
        }

    }
}
