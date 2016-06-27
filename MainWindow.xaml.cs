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

namespace WizardWarz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {
        public Int32 tileSize = 64;

        public MainWindow()
        {
            InitializeComponent();

            GameBoardManager _gameBoardManager = new GameBoardManager();
            _gameBoardManager.InitializeGameBoard(GameBoardGrid);

            PlayerController _playerController = new PlayerController();
            _playerController.InitialisePlayerController(GameBoardGrid);
            _playerController.InitialisePlayerMovement(GameBoardGrid);

            GameTimer gT = new GameTimer();
            
            //this.GameBoardGrid.MouseDown += new MouseButtonEventHandler(controller_MouseLeftButtonDown);

        }

       
        //public void controller_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{

        //    var mousewasdownOn = e.Source as FrameworkElement;

        //    if (mousewasdownOn != null)
        //    {
        //        int elementNameC = (int)mousewasdownOn.GetValue(Grid.ColumnProperty);
        //        int elementNameR = (int)mousewasdownOn.GetValue(Grid.RowProperty);

        //        MessageBox.Show(string.Format("Grid clicked at column {0}, row {1}", elementNameC, elementNameR));

        //    }


        //}

    }
}
