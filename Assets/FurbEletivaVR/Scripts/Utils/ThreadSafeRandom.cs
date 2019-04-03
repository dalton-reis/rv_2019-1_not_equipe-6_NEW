using System;
using System.Threading;

namespace Assets.FurbEletivaVR.Scripts.Utils
{
    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)); }
        }
    }
}
