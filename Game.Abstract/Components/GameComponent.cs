using System.Threading;
using System.Threading.Tasks;
using GameFramework.EventArgs;
using GameFramework.IO;

namespace GameFramework.Components
{
    public abstract class GameComponent
    {
        private bool _enabled;
        private int _updateOrder;

        protected GameComponent(Game game, bool enabled = false)
        {
            Game = game;
            _enabled = enabled;
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == _enabled) return;
                _enabled = value;
                Task.Run(async () => { await EnabledChangedAsync(this, new System.EventArgs()); }).Start();
            }
        }

        public virtual int UpdateOrder
        {
            get => _updateOrder;
            set
            {
                if (value == _updateOrder) return;
                _updateOrder = value;
                Task.Run(async () => { await UpdateOrderChangedAsync(this, new System.EventArgs()); }).Start();
            }
        }

        public Game Game { get; }

        public Keyboard Keyboard => Game.Keyboard;

        public virtual async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task EnabledChangedAsync(object sender, System.EventArgs eventArgs,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }


        protected virtual async Task UpdateOrderChangedAsync(object sender, System.EventArgs eventArgs,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }


        public virtual async Task UpdateAsync(object sender, GameUpdateEventArgs args,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}