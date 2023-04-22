using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MukuBase
{
    public class ValueAnimator
    {
        private ValueAnimator() 
        {
            timer.Elapsed +=Timer_Elapsed;
        }
        private DateTime start;
        private DateTime end;
        private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            float ms = (float)(DateTime.Now-start).TotalMilliseconds;
            number = (from + (ms/msduration)*(to-from));
            if (number>=to)
            {
                End();
            }
            else
            {
                Valuechanged?.Invoke(this, number);
            }
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
            instance.Handler= changehandler;
            instance.Valuechanged += changehandler;
            instance.timer.Start();
            instance.start = DateTime.Now;
            instance.end = instance.start.AddMilliseconds(msduration);
            return instance;

        }
        
    }
}
