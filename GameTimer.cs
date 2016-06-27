using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
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

        public GameTimer()
        {
            DispatcherTimer gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(FramesPerSecond());
            gameLoopTimer.Tick += new EventHandler(timer_Tick);
            gameLoopTimer.Start();
            Debug.WriteLine("gameLoopTimer Initialised");
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            curTdelta += deltaTime;
            if (curTdelta % 10 == 0)
            {
                Debug.WriteLine("Elapsed = {0}", curTdelta);
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
