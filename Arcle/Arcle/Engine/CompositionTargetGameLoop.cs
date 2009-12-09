using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Arbaureal.Arcle.Engine
{
    public class CompositionTargetGameLoop
    {
        protected DateTime lastTick;
        public delegate void UpdateHandler(object sender, TimeSpan elapsed);
        public event UpdateHandler Update;
        public bool IsLoopRunning = false;

        public CompositionTargetGameLoop()
        {
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            TimeSpan elapsed = now - lastTick;
            lastTick = now;
            if (Update != null)
            {
                Update(this, elapsed);
            }
        }

        public void Start()
        {
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            lastTick = DateTime.Now;
            IsLoopRunning = true;
        }

        public void Stop()
        {
            IsLoopRunning = false;
            CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
        }
    }
}
