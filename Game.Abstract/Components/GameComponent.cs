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

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    Task.Run(async () =>
                    {
                        await EnabledChangedAsync(this, new System.EventArgs());
                    }).Start();
                }
            }
        }

        public virtual int UpdateOrder
        {
            get
            {
                return _updateOrder;
            }
            set
            {
                if (value != _updateOrder)
                {
                    _updateOrder = value;
                    Task.Run(async () =>
                    {
                        await UpdateOrderChangedAsync(this, new System.EventArgs());
                    }).Start();
                }
            }
        }

        public virtual async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public virtual async Task EnabledChangedAsync(object sender, System.EventArgs eventArgs, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }


        public virtual async Task UpdateOrderChangedAsync(object sender, System.EventArgs eventArgs, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }


        public virtual async Task UpdateAsync(object sender, GameUpdateEventArgs args, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public Game Game { get; }

        public Keyboard Keyboard => Game.Keyboard;

        public GameComponent(Game game, bool enabled = false)
        {
            Game = game;
            _enabled = enabled;
        }
    }
}
