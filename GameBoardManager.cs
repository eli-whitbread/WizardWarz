using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace WizardWarz
{
    /// <summary>
    /// Not yet working - can't draw grid to window. Refer MainWindow.xaml.cs "InitializeGameBoard()"
    /// </summary>
    public partial class GameBoardManager : MainWindow
    {
        //public Int32 tileSize = 64;

        //public GameBoardManager()
        //{
        //    InitializeComponent();
        //    InitializeGameBoard();
        //}

        private void InitializeGameBoard()
        {
            //set the grid size
            Int32 rows = 12;
            Int32 cols = 12;

            GridLengthConverter myGridLengthConverter = new GridLengthConverter();
            GridLength side = (GridLength)myGridLengthConverter.ConvertFromString("Auto");

            for (int i = 0; i < cols; i++)
            {
                GameBoardGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
                GameBoardGrid.ColumnDefinitions[i].Width = side;
            }
            for (int x = 0; x < rows; x++)
            {
                GameBoardGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                GameBoardGrid.RowDefinitions[x].Height = side;
            }

            Rectangle[,] flrTiles = new Rectangle[cols, rows];

            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    flrTiles[c, r] = new Rectangle();
                    flrTiles[c, r].Fill = new ImageBrush(new BitmapImage(new Uri(@"I:\nsquared Visual Studio project\WizardWarz\Resources\FloorTile_64x64.png", UriKind.Relative)));
                    flrTiles[c, r].Height = tileSize;
                    flrTiles[c, r].Width = tileSize;
                    Grid.SetColumn(flrTiles[c, r], c);
                    Grid.SetRow(flrTiles[c, r], r);

                    GameBoardGrid.Children.Add(flrTiles[c, r]);
                }
            }
        }


    }
}
