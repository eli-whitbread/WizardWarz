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
    /// Interaction logic for PlayerLivesAndScore.xaml
    /// </summary>
    public partial class PlayerLivesAndScore : UserControl
    {
        public Int32 playerLivesNumber;
        public Rectangle playerHomeTile;
        public Rectangle playerHomeTile2;

        public Rectangle playerLivesTile;
        public Label playerScore;
        public int currentScore;
        public string finalScore;
        public Canvas mainCanvasLocalRef;
        public int tileSizeLocal;
        AudioManager playMusic = new AudioManager();

        // public PlayerLivesAndScore(Canvas mainCanvas, Int32 tileSize)

        public PlayerLivesAndScore()
        {
            InitializeComponent();

            //mainCanvasLocalRef = mainCanvas;
            tileSizeLocal = 64;

            //--------------- Player Lives (HAS TO BE CHANGED HERE)--------------------------
            playerLivesNumber = playerLivesNumber + 3;

            //--------------- Player Score (HAS TO BE CHANGED HERE)--------------------------
            currentScore = 0;

            // -------------- INITIALISATION of PLAYER STATS----------------------------------------
            initialiseHomeBases();
            initialiseLives();
            initialiseScore();


          
        }

        

        public void initialiseHomeBases()
        {
            //--------------------------------------------| Initialise Player Home Base |-------------------------------------------           
            playerHomeTile = new Rectangle();

            playerHomeTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\castleWizardBlue.png", UriKind.Relative)));

            playerHomeTile.Height = tileSizeLocal;
            playerHomeTile.Width = tileSizeLocal;

            // --------------- Set position, within the local grid (livesGrid) of this element ------------------------------
            Grid.SetRow(playerHomeTile, 0);
            Grid.SetColumn(playerHomeTile, 0);


            // ------------------ Add element to the grid, according to the above -------------------------------------
            livesGrid.Children.Add(playerHomeTile);
            
            

        }

        public void initialiseLives()
        {
            //-----------------------------------------------------| Initialise Lives|-------------------------------------------
            // --------------------- Add hearts to grid, depending on number of lives--------------
            for (int i = 1; i <= playerLivesNumber; i++)
            {
                playerLivesTile = new Rectangle();

                playerLivesTile.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\heart.png", UriKind.Relative)));

                playerLivesTile.Height = tileSizeLocal;
                playerLivesTile.Width = tileSizeLocal;

                // --------------- Set position, within the local grid (livesGrid) of this element ------------------------------
                Grid.SetRow(playerLivesTile, 0);
                Grid.SetColumn(playerLivesTile, i);

                livesGrid.Children.Add(playerLivesTile);

            }

        }

        public void ReduceLives(int count)
        {
            playerLivesNumber -= count;

            CalculateLives();
            Debug.WriteLine("Player lives reduced!");
        }

        public void CalculateLives()
        {
            // ---------------------- Remove ALL hearts from grid----------------------------
            try { livesGrid.Children.RemoveAt(3); }
            catch
            {//throw;
                //MessageBox.Show("Nothing at index: " + 3);
            }
            try { livesGrid.Children.RemoveAt(2); }
            catch
            {//throw;
                //MessageBox.Show("Nothing at index: " + 2);
            }
            try { livesGrid.Children.RemoveAt(1); }
            catch
            {//throw;
                //MessageBox.Show("Nothing at index: " + 1);
            }

            playMusic.playPickupBomb();
            initialiseLives();
        }

        public void initialiseScore()
        {
            //--------------------------------------------------------------------------------------------------------------
            //--------------------------------------------| Initialise Player Score |-------------------------------------------           
            //------------------------------------------------------------------------------------------------------------------

            playerScore = new Label();



            playerScore.Content = currentScore.ToString();
            playerScore.FontSize = 32;
            playerScore.Foreground = new SolidColorBrush(Colors.Black);
            playerScore.Width = 128;
            playerScore.Height = 64;


            // --------------- Set position, within the local grid (scoreGrid) of this element ------------------------------

            Grid.SetRow(playerScore, 0);
            Grid.SetColumn(playerScore, 1);

            

            scoreGrid.Children.Add(playerScore);


            //--------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------

        }

        public void ChangeScore(int scoreUpOrDown, bool Up)
        {
            string info;
            if (Up)
            { currentScore += scoreUpOrDown; info = " + " + scoreUpOrDown.ToString(); Debug.WriteLine("Player score changed by:" + info); }
            else if (!Up)
            { currentScore -= scoreUpOrDown; info = " - " + scoreUpOrDown.ToString(); Debug.WriteLine("Player score changed by:" + info); }            

            CalculateScore();            
        }

        public void CalculateScore()
        {
            try
            {
                // ------------------ Remove Score from Grid ----------------------------------
                scoreGrid.Children.RemoveAt(0);
            }
            catch
            {

                //throw;
            }

            // ----------------- Run initialise Score method again, to populate grid with new score information ------------------
            playMusic.playEnemyAttack();

            initialiseScore();

        }


    
    }
}
