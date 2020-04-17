using GameFramework.IO;

namespace GameFramework.Components
{
    public abstract class Screen
    {
        private readonly Game _game;

        protected Screen(Game game)
        {
            _game = game;
        }

        protected Display Display => _game.Display;
        protected Keyboard Keyboard => _game.Keyboard;
    }
}