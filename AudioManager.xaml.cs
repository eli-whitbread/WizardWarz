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

namespace WizardWarz
{
    /// <summary>
    /// Interaction logic for AudioManager.xaml
    /// </summary>
    public partial class AudioManager : UserControl
    {
        MediaPlayer jukeBox = new MediaPlayer();
        public string trackLocation;
        public double newVolume;
        bool isLooping = false;
        public bool audioOn = true;

        // All sound plays have a 'Stop Track', this is to prevent stacking sounds. 


        public AudioManager()
        {
            InitializeComponent();            
            audioOn = MainWindow.GlobalAudio1;
        }

        

        public void playMainMusic()
        {
            isLooping = true;
            trackLocation = "8_bit_wizard.mp3";
            newVolume = 0.15;
            PlayTrack();
        }

        

        public void CalculateAudioVolume()
        {
            if (!audioOn)
            {
                jukeBox.Volume = 0;
            }
            else if(audioOn)
            {
                jukeBox.Volume = newVolume;
                
            }
        }

        public void StopTrack()
        {
            jukeBox.Stop();

        }

        public void PlayTrack()
        {
            // Check whether song should loop - if it is meant to, the mediaended event should fire, and set the jukebox.position back to 0, and starting the song again. NOT SURE WHETHER THIS IS ACTUALLY WORKING!!! :(
            if (isLooping)
            {

                Uri uriStreaming = new Uri(@"./Resources/" + trackLocation, UriKind.Relative);

                CalculateAudioVolume();
                jukeBox.Open(uriStreaming);
                jukeBox.MediaEnded += new EventHandler(mediaElement_MediaEnded);
                jukeBox.Play();

            }
            else
            {
                Uri uriStreaming = new Uri(@"./Resources/" + trackLocation, UriKind.Relative);
                CalculateAudioVolume();
                jukeBox.Open(uriStreaming);
                jukeBox.Play();
            }
        }

        // This event fires when the main audio track stops - thus looping the music
        void mediaElement_MediaEnded(object sender, EventArgs e)
        {
            // Loops a particular track
            jukeBox.Position = TimeSpan.Zero;
            playMainMusic();
        }

        public void playBombExplode()
        {
            StopTrack();
            trackLocation = "bomb_explode.wav";
            newVolume = 0.8;
            PlayTrack();
        }

        public void playBombTick()
        {
            StopTrack();
            trackLocation = "timer_fuse.wav";
            newVolume = 1.2;
            PlayTrack();
        }

        public void playPickupLife()
        {
            StopTrack();
            trackLocation = "pickup.wav";
            newVolume = 0.8;
            PlayTrack();
        }

        public void playPickupBomb()
        {
            StopTrack();
            trackLocation = "pickup.wav";
            newVolume = 0.8;
            PlayTrack();
        }

        public void playEnemyAttack()
        {
            StopTrack();
            trackLocation = "break_wall.wav";
            newVolume = 2;
            PlayTrack();
        }

        public void playPlayerDeath()
        {
            StopTrack();
            trackLocation = "defeat.wav";
            newVolume = 0.5;
            PlayTrack();
        }        
    }
}
