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
        AudioManager newAudioManager; 

        public Canvas mainCanvas
        {
            get { return GameCanvas; }
        }

        public static bool GlobalAudio1
        {
            get; set;
        }

        public MainWindow()
        {
            InitializeComponent();   

            GameBoardManager _gameBoardManager = new GameBoardManager();
            _gameBoardManager.InitializeGameBoard(GameBoardGrid);

            newAudioManager = new AudioManager();

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

            GameTimer gT = new GameTimer();
            gT.GameCanRef = mainCanvas;
            gT.p1Ref = _playerController1;
            gT.Initialise();

            _playerController1.timerRef = gT;

            Bomb _masterBombClass = new Bomb(GameBoardGrid);

            // Initialising Audio (visual pushing to the far right)
            GlobalAudio1 = true;
            newAudioManager.audioOn = GlobalAudio1;
            newAudioManager.playMainMusic();
            Canvas.SetLeft(audioTile, tileSize * 15);            
            GameCanvas.Children.Add(newAudioManager);   
    }
                

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            volume_off_On();
        }

        private void volume_off_On()
        {
            if (!GlobalAudio1)
            {
                GlobalAudio1 = true;
                volImage.Source = new BitmapImage(new Uri("Resources/audioOn.png", UriKind.Relative));
                newAudioManager = new AudioManager();
                newAudioManager.playMainMusic();
            }
            else
            {
                GlobalAudio1 = false;
                volImage.Source = new BitmapImage(new Uri("Resources/audio.png", UriKind.Relative));
                newAudioManager.StopTrack();
                newAudioManager = null;
            }

            // UPDATE AUDIO MANAGER AS TO WHETHER AUDIO SHOULD BE ON
            //newAudioManager.audioOn = GlobalAudio1;
        }

        private void image_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            volume_off_On();
        }
    }
}
