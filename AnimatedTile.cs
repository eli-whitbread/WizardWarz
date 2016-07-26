using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WizardWarz
{
    /// <summary>
    /// AnimatedTile: new AnimatedTile(){int frameMaxX, int frameMaxY, int tileWidth, int tileHeight, BitmapImage imageSource};
    /// Use instance.AnimStart() to display tile - eg: gameGrid.Children.Add(animatedTileInstance.AnimStart())
    /// Call instance.AnimStop() before destroying an instance of the Class.
    /// </summary>
    class AnimatedTile : Control
    {
        public Int32 offsetX, offsetY, curFrameX, curFrameY, frameMaxX, frameMaxY;
        public Int32 tileWidth, tileHeight;
        public BitmapImage imageSource;
        public CroppedBitmap imageCropped;
        public Rectangle imageRect;
        public Rectangle imageRectOut;
        GameTimer myGameTimerInstance = null;
        Int32 myTick, tickDelay;
        bool playAnimation;

        //initialise the AnimatedTile
        public AnimatedTile()
        {
            myTick = 0;
            tickDelay = 1;
            curFrameX = 0;
            curFrameY = 0;
            imageRect = new Rectangle();
            imageRect.Width = tileWidth;
            imageRect.Height = tileHeight;
            imageRect.Fill = new ImageBrush(imageSource);
            myGameTimerInstance = GameWindow.ReturnTimerInstance();
            myGameTimerInstance.tickEvent += MyGameTimerInstance_tickEvent;
            playAnimation = false;
        }

        //start playing the animation
        public Rectangle AnimStart()
        {
            playAnimation = true;
            return imageRect;
        }

        //must be called before AnimatedTile class instance is destroyed in the instantiating Class in order to unsubscribe from the tick event
        public void AnimStop()
        {
            playAnimation = false;
            myGameTimerInstance.tickEvent -= MyGameTimerInstance_tickEvent;
        }

        //function subscribed to GameTimer "tickEvent" - executes every "timerTick" that instance is active
        private void MyGameTimerInstance_tickEvent(object sender, EventArgs e)
        {
            myTick++;
            if (playAnimation == true && myTick >= tickDelay)
            {
                myTick = 0;
                DrawNextFrame();
                
            }
        }

        private void DrawNextFrame()
        {
                        
            imageSource.SourceRect = new Int32Rect(tileWidth, tileHeight, (tileWidth * curFrameX), (tileHeight * curFrameY));
            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = imageSource;
            imageRect.Fill = imgBrush;
            
            
            Debug.WriteLine("curFrameX = {0}, curFrameY = {1}", curFrameX, curFrameY);

            if (curFrameX < frameMaxX)
            {
                curFrameX++;
                return;
            }
            else
            {
                curFrameX = 0;
            }
            if (curFrameY < frameMaxY)
            {
                curFrameY++;
            }
            else
            {
                curFrameY = 0;
            }

        }

        //cycle through "imageSource" (sprite sheet) drawing each cropped sprite image in turn.
        //private void DrawNextFrame()
        //{
        //    TranslateTransform imgPos = new TranslateTransform();
        //    imgPos.X = 0 - (tileWidth * curFrameX);
        //    imgPos.Y = 0 - (tileHeight * curFrameY);
        //    imageRect.RenderTransform = imgPos;

        //    if(curFrameX != frameMaxX)
        //    {
        //        curFrameX++;
        //    }
        //    else
        //    {
        //        curFrameX = 0;
        //    }
        //    if (curFrameY != frameMaxY)
        //    {
        //        curFrameY++;
        //    }
        //    else
        //    {
        //        curFrameY = 0;
        //    }
            
        //}
    }
}
