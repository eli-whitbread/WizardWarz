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
using Microsoft.Surface;
using Microsoft.Surface.Core;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Diagnostics;

namespace WizardWarz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
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

            PlayerController _playerController1 = new PlayerController(GameBoardGrid);
            _playerController1.managerRef = _gameBoardManager;
            _playerController1.gameCanRef = mainCanvas;
            _playerController1.InitialiseRefs();

            Debug.WriteLine(mainCanvas.Name);

            StaticCollections _staticColections = new StaticCollections();

            GameTimer gT = new GameTimer();
            gT.GameCanRef = mainCanvas;
            gT.p1Ref = _playerController1;
            gT.Initialise();

            _playerController1.timerRef = gT;

            Bomb _masterBombClass = new Bomb(GameBoardGrid);
            
        }

    }
}
