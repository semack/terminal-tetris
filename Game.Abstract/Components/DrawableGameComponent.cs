using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.EventArgs;

namespace GameFramework.Components
{
    public abstract class DrawableGameComponent : GameComponent
    {
        private bool _visible;
        private int _drawOrder;

        public bool Visible
        {
            get {
                return _visible;
            }
            set {
                if (value != _visible)
                {
                    _visible = value;
                    Task.Run(async () =>
                    {
                        await VisibleChangedAsync(this, new System.EventArgs());
                    }).Start();                    
                }
            }
        }

        public virtual int DrawOrder
        {
            get
            {
                return _drawOrder;
            }
            set
            {
                if (value != _drawOrder)
                {
                    _drawOrder = value;
                    Task.Run(async () =>
                    {
                        await DrawOrderChangedAsync(this, new System.EventArgs());
                    }).Start();
                }
            }
        }

        public virtual async Task VisibleChangedAsync(object sender, System.EventArgs eventArgs, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public virtual async Task DrawOrderChangedAsync(object sender, System.EventArgs eventArgs, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public DrawableGameComponent(Game game, bool enabled = false) : base(game, enabled)
        {
            _visible = false;
        }

        public abstract Task DrawAsync(object sender, GameUpdateEventArgs args, CancellationToken cancellationToken = default);
    }
}
