using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Util
{
    internal class Timer
    {
        Stopwatch stopwatch = new Stopwatch();
        private int targetTime;
        public Timer(int ticksPerSecond)
        {
            targetTime = 1000 / ticksPerSecond;
            stopwatch.Start();
        }
        public Timer()
        {
            stopwatch.Start();
        }
        public bool Next(out int elapsedTimeMilli)
        {
            elapsedTimeMilli = GetElapsedMilliseconds();
            if (elapsedTimeMilli < targetTime) return false;
            stopwatch.Restart();
            return true;
        }
        public int Restart()
        {
            int elapsedTimeMilli = GetElapsedMilliseconds();
            stopwatch.Restart();
            return elapsedTimeMilli;
        }
        public int GetElapsedMilliseconds()
        {
            return (int)stopwatch.ElapsedMilliseconds;
        }
        public void SetTPS(int ticksPerSecond)
        {
            targetTime = 1000 / ticksPerSecond;
            stopwatch.Start();
        }
    }
}
