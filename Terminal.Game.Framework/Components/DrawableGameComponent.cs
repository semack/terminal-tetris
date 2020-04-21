using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.Time;

namespace Terminal.Game.Framework.Components
{
    public abstract class DrawableGameComponent : GameComponent
    {
        private int _drawOrder;
        private bool _visible;

        protected DrawableGameComponent(Game game) : base(game)
        {
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                if (value == _visible) return;
                _visible = value;
                VisibleChanged(this, new System.EventArgs()); 
            }
        }

        public virtual int DrawOrder
        {
            get => _drawOrder;
            set
            {
                if (value != _drawOrder)
                {
                    _drawOrder = value;
                    DrawOrderChanged(this, new System.EventArgs());
                }
            }
        }

        protected virtual void VisibleChanged(object sender, System.EventArgs eventArgs)
        {
        }

        protected virtual void DrawOrderChanged(object sender, System.EventArgs eventArgs)
        {
        }

        public abstract Task DrawAsync(object sender, GameTime args,
            CancellationToken cancellationToken = default);
    }
}