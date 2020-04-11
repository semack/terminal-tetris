using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.EventArgs;
using GameFramework.IO;

namespace GameFramework.Components
{
    public abstract class GameComponent
    {
        public bool IsEnabled { get; set; }

        public virtual async Task UpdateAsync(GameUpdateEventArgs args, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public Game Game { get; }

        public Keyboard Keyboard => Game.Keyboard;

        public GameComponent(Game game, bool enabled = false)
        {
            Game = game;
            IsEnabled = enabled;
        }
    }
}
