using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Data;
using Microsoft.Surface;
using Microsoft.Surface.Core;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Input;
using System.Diagnostics;


namespace WizardWarz
{
    public class GameTimer
    {
        float deltaTime = 0;
        float curTdelta = 0;

        Point curMousePos = new Point(0, 0);

        bool isCanvasCap = false;
        bool isP1Influenced = false;

        public Canvas GameCanRef = null;

        public PlayerController p1Ref = null;

        public GameTimer()
        {
            DispatcherTimer gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(FramesPerSecond());
            gameLoopTimer.Tick += new EventHandler(timer_Tick);
            gameLoopTimer.Start();
            Debug.WriteLine("gameLoopTimer Initialised");
        }

        public void Initialise()
        {
            Debug.WriteLine(GameCanRef.Name);
            if (p1Ref != null)
            {
                Debug.WriteLine("Controller ref passed successfully!");
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            curTdelta += deltaTime;
            if (curTdelta % 10 == 0)
            {
                Debug.WriteLine("Elapsed = {0}", curTdelta);
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed && isCanvasCap == false)
            {
                //Mouse.Capture(GameCanRef);
                isCanvasCap = true;
            }
            else if (Mouse.LeftButton == MouseButtonState.Released && isCanvasCap == true)
            {
                //GameCanRef.ReleaseMouseCapture();
                isCanvasCap = false;
            }

            if (isCanvasCap)
            {
                curMousePos =  Mouse.GetPosition(GameCanRef);

                if (Mouse.DirectlyOver == p1Ref)
                {
                    isP1Influenced = true;

                    Mouse.Capture(p1Ref.localGameGrid);
                }

            }
        }

        private double FramesPerSecond()
        {
            Int32 fps = 30;
            Double frameTimeSpan;

            frameTimeSpan = 1000 / fps;
            deltaTime = Convert.ToSingle(frameTimeSpan);

            return frameTimeSpan;
        }
    }
}
