using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;


namespace WizardWarz
{
    class SoundManager
    {
        
        MediaPlayer jukeBox = new MediaPlayer();
        
        public string trackLocation;
        bool isLooping = false;
               
        // All sound plays have a 'Stop Track', this is to prevent stacking sounds. 


        public SoundManager()
        {
            
        }

        public void playMainMusic()
        {

            
            isLooping = true;
            trackLocation = "wizard2.wav";
            PlayTrack();
        }

        public void playBombExplode()
        {
            StopTrack();
            trackLocation = "bomb_explode.wav";
            PlayTrack();
        }

        public void playBombTick()
        {
            StopTrack();
            trackLocation = "timer_fuse.wav";
            PlayTrack();
        }

        public void playPickupLife()
        {
            StopTrack();
            trackLocation = "pickup.wav";
            PlayTrack();
        }

        public void playPickupBomb()
        {
            StopTrack();
            trackLocation = "pickup.wav";
            PlayTrack();
        }

        public void playEnemyAttack()
        {
            StopTrack();
            trackLocation = "break_wall.wav";
            PlayTrack();
        }

        public void playPlayerDeath()
        {
            StopTrack();
            trackLocation = "defeat.wav";
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

                jukeBox.Volume = 0.25;
                jukeBox.Open(uriStreaming);
                //jukeBox.MediaEnded += new EventHandler(mediaElement_MediaEnded);
                jukeBox.Play();
                
                //isLooping = false;

            }
            else
            {
                Uri uriStreaming = new Uri(@"./Resources/" + trackLocation, UriKind.Relative);

                jukeBox.Volume = 1;
                jukeBox.Open(uriStreaming);

                jukeBox.Play();
            }

            
            

        }

        void mediaElement_MediaEnded(object sender, EventArgs e)
        {            
            jukeBox.Position = TimeSpan.FromSeconds(0);
            playMainMusic();
        }

    }
}
