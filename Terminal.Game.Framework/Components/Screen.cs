using Terminal.Game.Framework.IO;

namespace Terminal.Game.Framework.Components
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