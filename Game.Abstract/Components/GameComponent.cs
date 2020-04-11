using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.EventArgs;
using GameFramework.IO;

namespace GameFramework.Components
{
    public abstract class GameComponent
    {
        public virtual async Task Update(GameUpdateEventArgs args, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public Game Game { get; }

        public Keyboard Keyboard => Game?.Keyboard;

        public GameComponent(Game game)
        {
            Game = game;
        }
    }
}
