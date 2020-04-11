using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Components;
using GameFramework.EventArgs;

namespace TerminalTetris.Components
{
    public class SplashScreen : GameComponent
    {
        public SplashScreen(Game game, bool enabled) : base(game, enabled)
        {
        }

        public override async Task UpdateAsync(GameUpdateEventArgs args, CancellationToken cancellationToken)
        {
            var key = (char)await Game.Keyboard.GetKeyAsync(cancellationToken);
            await Game.Display.OutAsync(key.ToString(), cancellationToken);

            await base.UpdateAsync(args, cancellationToken);
        }

    }
}
