using System;

namespace GameFramework.EventArgs
{
    public class GameUpdateEventArgs : System.EventArgs
    {
        public GameUpdateEventArgs(TimeSpan elapsedGameTime, TimeSpan elapsedRealTime)
        {
            ElapsedGameTime = elapsedGameTime;
            ElapsedRealTime = elapsedRealTime;
        }

        public TimeSpan ElapsedGameTime { get; }
        public TimeSpan ElapsedRealTime { get; }
    }
}