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
        AudioManager newAudioManager;
        GameWindow newgame;
        TitleScreen title;

        public static bool GlobalAudio1
        {
            get; set;
        }

        public MainWindow()
        {
            InitializeComponent();
            // Initialise the Audio Manager
            newAudioManager = new AudioManager();

            // Load the TitleScreen, and a click event for it to use (to remove it after)
            title = new TitleScreen();
            title.MouseDown += Title_MouseDown;
            // Add Title screen to the Main Window
            MainCanvas.Children.Add(title);            

            // Initialising Audio (visual pushing to the far right), Play open track.
            GlobalAudio1 = true;
            newAudioManager.audioOn = GlobalAudio1;
            newAudioManager.playTitleSound();
            Canvas.SetLeft(audioTile, 0);
            MainCanvas.Children.Add(newAudioManager);
        }

        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //On any mouse click, load the Game Window, remove the title from the Main Window (along with it's tied event)
            newgame = new GameWindow();
            title.MouseDown -= Title_MouseDown;
            MainCanvas.Children.Remove(title);
            // Add Game to the Main Window
            newAudioManager.playMainMusic();
            MainCanvas.Children.Add(newgame);
        }

        private void volume_off_On()
        {
            // Event run when the audio button is pressed (either click or touch) - essentially turns audio on or off, and swaps the image to match. 
            // (On turning the audio on) Load a new instance of Audio Manager, and play the main music track
            // (On turning the audio off) unload the current instance of audio manager, and stock the music track
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
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            volume_off_On();
        }
        private void image_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            volume_off_On();
        }
    }
}
