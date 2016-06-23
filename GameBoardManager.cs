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
        
        public static void InitializeGameBoard2(Grid gameGrid)
        {
            Int32 tileSize = 64;
            //set the grid size
            Int32 rows = 12;
            Int32 cols = 12;

            //setup an array of grid positions (int[,]) for inner walls
            Int16[,] innerWallPos = { { 3, 5 }, { 5, 5 }  };

            GridLengthConverter myGridLengthConverter = new GridLengthConverter();
            GridLength side = (GridLength)myGridLengthConverter.ConvertFromString("Auto");

            //Setup the grid Rows and Columns
            for (int i = 0; i < cols; i++)
            {
                gameGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
                gameGrid.ColumnDefinitions[i].Width = side;
            }
            for (int x = 0; x < rows; x++)
            {
                gameGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                gameGrid.RowDefinitions[x].Height = side;
            }

            //create an empty Rectangle array
            Rectangle[,] flrTiles = new Rectangle[cols, rows];

            //fill each element in the Rectangle array with an image "tile".
            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    flrTiles[c, r] = new Rectangle();
                    //add a wall tile if along the grid extremes
                    if (r == 0 || r == rows - 1 || c == 0 || c == cols - 1)
                    {
                        flrTiles[c, r].Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WallTile_64x64.png", UriKind.Relative)));
                    }
                    //otherwise add a floor tile
                    else
                    {
                        flrTiles[c, r].Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\FloorTile_64x64.png", UriKind.Relative)));
                    }
                    //
                    //inner solid and destrutable walls still required!!
                    //
                    flrTiles[c, r].Height = tileSize;
                    flrTiles[c, r].Width = tileSize;
                    Grid.SetColumn(flrTiles[c, r], c);
                    Grid.SetRow(flrTiles[c, r], r);

                    gameGrid.Children.Add(flrTiles[c, r]);
                }
            }
            
        }


    }
}
