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
    /// Interaction logic for HelpScreen.xaml
    /// </summary>
    public partial class HelpScreen : UserControl
    {
        public Rectangle imageRect = new Rectangle();
        public Point screenCoords;
        public TranslateTransform imagePos;

        protected static HelpScreen helpRef;
        public MainWindow mwRef = null;
        public TitleScreen titleRef = null;

        public HelpScreen()
        {
            InitializeComponent();

            //helpRef = this;

        }

        //public static HelpScreen ReturnHelpScreen()
        //{
        //    return helpRef;
        //}

        public void InitializeHelpScreen()
        {
            // Remove everything from screem
            //mwRef = MainWindow.ReturnMainWindowInstance();
            //mwRef.BottomLeftButton.IsEnabled = false;
            //mwRef.TopRightButton.IsEnabled = false;
            //mwRef.MainCanvas.Children.Remove(mwRef.title);

            // Create image
            //screenCoords = new Point(256, 256);
            //imageRect.Width = 640;
            //imageRect.Height = 640;

            //imageRect.Fill = new ImageBrush(new BitmapImage(new Uri(@".\Resources\WIzardWarzText1.png", UriKind.Relative)));

            //imagePos = new TranslateTransform(screenCoords.X, screenCoords.Y);
            //imageRect.RenderTransform = imagePos;

            //MessageBox.Show("Image made");
        }

        private void CloseHelpScreen_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
