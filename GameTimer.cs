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


namespace WizardWarz
{
    public class GameTimer
    {
        public void loopTimer()
        {
            DispatcherTimer gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(FramesPerSecond());
            gameLoopTimer.Tick += timer_Tick;
            gameLoopTimer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            
        }

        double FramesPerSecond()
        {
            Int32 fps = 30;
            Double frameTimeSpan;

            frameTimeSpan = fps / 1000;

            return frameTimeSpan;
        }
    }
}
