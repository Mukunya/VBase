using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace VBase
{
    public class Timer
    {
        /// <summary>
        /// Block current thread for an exact amount of time.
        /// Millisecond accuracy.
        /// </summary>
        /// <param name="miliseconds">Milliseconds to sleep.</param>
        /// <returns></returns>
        public static async Task Sleep(int miliseconds)
        {
            var start = DateTime.Now;
            while (true)
            {
                await Task.Delay(0);
                double span = (DateTime.Now - start).TotalMilliseconds;
                if (span>=miliseconds)
                {
                    return;
                }
            }

        }
        /// <summary>
        /// Run an action each <INT> milliseconds.
        /// </summary>
        /// <param name="milliseconds">What amount of time to wait between two runs.</param>
        /// <param name="action">Action to to.</param>
        /// <returns>A service, where you can stop and restart the actions.</returns>
        public static RunEachService RunEach(int milliseconds,Action action)
        {
            return new RunEachService(milliseconds,action);
        }

        public class RunEachService:IDisposable
        {
            bool IsAllowedToRun = false;
            int eachms;
            Action stufftodo;

            public void Stop()
            {
                IsAllowedToRun = false;
            }

            public void Start()
            {
                IsAllowedToRun = true;
                Task.Run(() =>
                {
                    RunTask();
                });
            }

            public RunEachService(int ms,Action a)
            {
                stufftodo = a;
                eachms = ms;
                IsAllowedToRun = true;
                Task.Run(() =>
                {
                    RunTask();
                });
            }

            private async void RunTask()
            {
                while (IsAllowedToRun)
                {
                    await Sleep(eachms);
                    if (IsAllowedToRun)
                    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        Task.Run(stufftodo);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    }
                }
            }

            #region IDisposable Support
            private bool disposedValue = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        IsAllowedToRun =false;
                        eachms = 0;
                        stufftodo = null;
                    }

                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }
            #endregion
        }
    }
}
