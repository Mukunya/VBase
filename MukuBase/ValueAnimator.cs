using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MukuBase
{
    internal class ValueAnimator
    {
        private ValueAnimator() 
        {
            timer.Elapsed +=Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            i++;
            number = from + ((i*(to-from))/maxi);
            if (i == maxi)
            {
                number = to;
                timer.Stop();
                timer.Dispose();
                isDone= true;
            }
            Valuechanged?.Invoke(this,number);
        }
        public void End()
        {
            number = to;
            if (!isDone)
            {
                timer.Stop();
                timer.Dispose();
            }
            Valuechanged?.Invoke(this,number);
            Valuechanged-=Handler;
        }
        public void Abort()
        {
            timer.Stop();
            timer.Dispose();
            Valuechanged-=Handler;
        }

        private event EventHandler<float> Valuechanged;
        private float number = 0;
        private float from = 0;
        private float to = 0;
        private float i = 0;
        private int maxi = 0;
        private int msduration = 0;
        public bool isDone = false;
        private Timer timer = new Timer();
        private EventHandler<float> Handler;
        public static ValueAnimator Empty
        {
            get => new ValueAnimator();
        }
        public static ValueAnimator Animate(float from, float to, int msduration, EventHandler<float> changehandler, int mstimestep = 30)
        {
            ValueAnimator instance = new ValueAnimator();
            instance.from = from;
            instance.to = to;
            instance.number = from;
            instance.msduration = msduration;
            instance.timer.Interval = mstimestep;
            instance.maxi = (int)Math.Ceiling(((double)msduration)/((double)mstimestep));
            instance.Handler= changehandler;
            instance.Valuechanged += changehandler;
            instance.timer.Start();
            return instance;
        }
        
    }
}
