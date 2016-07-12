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
        public double volume;
        bool isLooping = false;

        // All sound plays have a 'Stop Track', this is to prevent stacking sounds. 


        public AudioManager()
        {
            InitializeComponent();
            volume = slider.Value;
        }

        public void playMainMusic()
        {
            isLooping = true;
            trackLocation = "8_bit_wizard.mp3";
            volume = 0.15;
            PlayTrack();
        }

        public void playBombExplode()
        {
            StopTrack();
            trackLocation = "bomb_explode.wav";
            volume = 0.8;
            PlayTrack();
        }

        public void playBombTick()
        {
            StopTrack();
            trackLocation = "timer_fuse.wav";
            volume = 1.2;
            PlayTrack();
        }

        public void playPickupLife()
        {
            StopTrack();
            trackLocation = "pickup.wav";
            volume = 0.8;
            PlayTrack();
        }

        public void playPickupBomb()
        {
            StopTrack();
            trackLocation = "pickup.wav";
            volume = 0.8;
            PlayTrack();
        }

        public void playEnemyAttack()
        {
            StopTrack();
            trackLocation = "break_wall.wav";
            volume = 2;
            PlayTrack();
        }

        public void playPlayerDeath()
        {
            StopTrack();
            trackLocation = "defeat.wav";
            volume = 1;
            PlayTrack();
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

                slider_ChangedCustom();
                jukeBox.Open(uriStreaming);
                jukeBox.MediaEnded += new EventHandler(mediaElement_MediaEnded);
                jukeBox.Play();

            }
            else
            {
                Uri uriStreaming = new Uri(@"./Resources/" + trackLocation, UriKind.Relative);

                slider_ChangedCustom();
                
                jukeBox.Open(uriStreaming);
                jukeBox.Play();
            }




        }

        void mediaElement_MediaEnded(object sender, EventArgs e)
        {
            // Loops a particular track
            jukeBox.Position = TimeSpan.Zero;
            playMainMusic();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (slider.Value > 0)
            {
                try
                {
                    volImage.Source = new BitmapImage(new Uri("Resources/audio.png", UriKind.Relative));
                }
                catch 
                {

                    
                }
                
            }
            else
            {
                try
                {
                    volImage.Source = new BitmapImage(new Uri("Resources/audioOn.png", UriKind.Relative));
                }
                catch
                {


                }
                

            }
            slider_ChangedCustom();
        }

        private void slider_ChangedCustom()
        {            
            jukeBox.Volume = volume * slider.Value;
        }

        private void image_TouchDown(object sender, TouchEventArgs e)
        {
            volume_off_On();
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            volume_off_On();
        }

        private void volume_off_On()
        {
            if (slider.Value == 0)
            {
                slider.Value = 0.5;
                slider_ChangedCustom();
                volImage.Source = new BitmapImage(new Uri("Resources/audio.png", UriKind.Relative));
            }
            else
            {
                slider.Value = 0;
                slider_ChangedCustom();
                volImage.Source = new BitmapImage(new Uri("Resources/audioOn.png", UriKind.Relative));
            }
            
            
        }

    }
}
