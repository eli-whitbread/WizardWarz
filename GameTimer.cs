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
        public DispatcherTimer gameLoopTimer;

        Point curMousePos = new Point(0, 0);

        public Canvas GameCanRef = null;
        public Powerup puRef = null;
        public PlayerController playerReference = null;

        /// <summary>
        /// Event Flag for the games Render Tick Event. <para> Anything using this tick will be added to the Rendering tick thread (not an extra thread). </para>
        /// </summary> 
        public event EventHandler renderFrameEvent_TICK;

        /// <summary>
        /// Event Flag for the games Process Tick Event. <para> Anything using this tick will be added to the Processing tick thread (not an extra thread). </para>
        /// </summary>
        public event EventHandler processFrameEvent_TICK;



        // ---------------------------------------------------------------------
        // ----------------------INITIALISE DispatchTimer-----------------------
        // ---------------------------------------------------------------------
        public GameTimer()
        {
            gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(FramesPerSecond());
            gameLoopTimer.Tick += new EventHandler(timer_Tick);
            gameLoopTimer.Start();
            GameCanRef = GameWindow.ReturnMainCanvas();
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



        // ---------------------------------------------------------------------
        // ---------------------- TICK EVENT -----------------------------------
        // ---------------------------------------------------------------------
        public void timer_Tick(object sender, EventArgs e)
        {
            curTdelta += deltaTime;
            if (curTdelta % 5 == 0)
            {                
                Debug.WriteLine("Elapsed = {0}", curTdelta);
            }

            // ---------------------------------------------------------------------
            // ----------------------TICK EVENT FOR PROCESSING ---------------------
            // ---------------------------------------------------------------------             
            if (processFrameEvent_TICK != null)
            {
                processFrameEvent_TICK.Invoke(this, e);

            }

            // ---------------------------------------------------------------------
            // ----------------------TICK EVENT FOR RENDERING-----------------------
            // ---------------------------------------------------------------------            
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

            //MessageBox.Show(string.Format("Single tick = {0}", deltaTime));

            return frameTimeSpan;
        }
    }
}
