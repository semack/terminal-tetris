using Terminal.Game.Framework.IO;

namespace Terminal.Tetris.Common
{
    public abstract class Screen
    {
        protected Screen(GameIO io)
        {
            IO = io;
        }

        protected GameIO IO { get; }
    }
}