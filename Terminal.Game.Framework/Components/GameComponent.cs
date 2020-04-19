using System;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Game.Framework.Components
{
    public abstract class GameComponent : BaseGameComponent
    {
        private bool _enabled;
        private int _updateOrder;

        protected GameComponent(Game game)
        {
            Game = game ?? throw new ArgumentException(nameof(game));
        }

        public Game Game { get; }

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
    }
}