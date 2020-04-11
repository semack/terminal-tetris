using GameFramework.IO;

namespace GameFramework.Components
{
    public abstract class GameComponent
    {
        public Game Game { get; }

        public Keyboard Keyboard => Game?.Keyboard;

        public GameComponent(Game game)
        {
            Game = game;
        }
    }
}
