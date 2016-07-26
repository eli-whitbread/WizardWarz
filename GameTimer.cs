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
//using Microsoft.Surface;
//using Microsoft.Surface.Core;
//using Microsoft.Surface.Presentation.Controls;
//using Microsoft.Surface.Presentation.Input;
using System.Windows.Input;
using System.Windows.Documents;
using System.Diagnostics;
using WizardWarz;


namespace WizardWarz
{
    public class GameTimer /*GAME MANAGER*/
    {
        float deltaTime = 0;
        public float exposedDT = 0;
        float curTdelta = 0;

        Point curMousePos = new Point(0, 0);

        public Canvas GameCanRef = null;

        public PlayerController playerReference = null;

        public event EventHandler renderFrameEvent_TICK;
        public event EventHandler processFrameEvent_TICK;
        
        public GameTimer()
        {
            DispatcherTimer gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(FramesPerSecond());
            gameLoopTimer.Tick += new EventHandler(timer_Tick);
            gameLoopTimer.Start();
            GameCanRef = GameWindow.ReturnnMainCanvas();
            Debug.WriteLine("gameLoopTimer Initialised");
        }

        public void Initialise()
        {
            Debug.WriteLine(GameCanRef.Name);
            if (playerReference != null)
            {
                Debug.WriteLine("Controller ref passed successfully!");
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            curTdelta += deltaTime;
            if (curTdelta % 5 == 0)
            {                
                Debug.WriteLine("Elapsed = {0}", curTdelta);
            }

            // Send Public Tick Event For PROCESSING
            if (processFrameEvent_TICK != null)
            {
                processFrameEvent_TICK.Invoke(this, e);

            }

            //send public tick event for RENDERING
            if (renderFrameEvent_TICK != null)
            {
                renderFrameEvent_TICK.Invoke(this,e);
            }

            
            

            //for all bombs currently active in the level fire "BombUpdateTick"
            //StaticCollections.SendBombUpdate();

            //PROCESS FRAMES FOR ALL OBJECTS ON SCREEN
            //playerReference.ProcessFrame();

            //RENDER FRAMES FOR ALL OBJECTS ON SCREEN
            //playerReference.RenderFrame();
        }



        private double FramesPerSecond()
        {
            Int32 fps = 60;
            Double frameTimeSpan;

            frameTimeSpan = 1000 / fps;
            deltaTime = Convert.ToSingle(frameTimeSpan);
            exposedDT = deltaTime;

            return frameTimeSpan;
        }
    }
}
