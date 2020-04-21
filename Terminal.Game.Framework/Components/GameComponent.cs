using System;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.IO;

namespace Terminal.Game.Framework.Components
{
    public abstract class GameComponent : BaseGameComponent
    {
        private bool _enabled;
        private int _updateOrder;
        private Game _game;

        protected GameComponent(Game game)
        {
            _game = game ?? throw new ArgumentException(nameof(game));
        }

        public GameIO IO => _game.IO;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == _enabled) return;
                _enabled = value;
                EnabledChanged(this, new System.EventArgs());
            }
        }

        public virtual int UpdateOrder
        {
            get => _updateOrder;
            set
            {
                if (value == _updateOrder) return;
                _updateOrder = value;
               UpdateOrderChanged(this, new System.EventArgs());
            }
        }


        protected virtual void EnabledChanged(object sender, System.EventArgs eventArgs)
        {
        }


        protected virtual void UpdateOrderChanged(object sender, System.EventArgs eventArgs)
        {
        }
    }
}