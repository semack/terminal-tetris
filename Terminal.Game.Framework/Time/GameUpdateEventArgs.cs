using System;

namespace Terminal.Game.Framework.Time
{
    public class GameTime 
    {
        public GameTime(TimeSpan elapsedGameTime, TimeSpan elapsedRealTime)
        {
            ElapsedGameTime = elapsedGameTime;
            ElapsedRealTime = elapsedRealTime;
        }

        public TimeSpan ElapsedGameTime { get; }
        public TimeSpan ElapsedRealTime { get; }
    }
}