using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Components;
using GameFramework.EventArgs;
using GameFramework.IO;
using TerminalTetris.Resources;

namespace TerminalTetris.Components
{
    public class SplashScreen : DrawableGameComponent
    {
        public SplashScreen(Game game) : base(game)
        {
        }

        public override Task DrawAsync(object sender, GameUpdateEventArgs args,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override async Task UpdateAsync(object sender, GameUpdateEventArgs args,
            CancellationToken cancellationToken = default)
        {
            var key = await Game.Keyboard.GetKeyAsync(cancellationToken);
            if (key != null)
            {
                await Display.OutAsync(((char)key).ToString(), cancellationToken);
            }
            await base.UpdateAsync(sender, args, cancellationToken);
        }

        public async Task<int> GetUserLevelAsync(CancellationToken cancellationToken = default)
        {
            int? userLevel = null;
            while (userLevel == null)
            {
                await DrawSplashAsync(cancellationToken);
                userLevel = await InputLevelAsync(cancellationToken);
            }
            return await Task.FromResult((int)userLevel);
        }

        private async Task DrawSplashAsync(CancellationToken cancellationToken = default)
        {
            await Display.ClearAsync(cancellationToken);
            await Display.OutAsync(10, 18, Strings.YourLevel, cancellationToken);
        }

        private async Task<int?> InputLevelAsync(CancellationToken cancellationToken = default)
        {
            int? result = null;
            var input = await Display.ReadLineAsync(cancellationToken);
            if (int.TryParse(input, out var level))
            {
                if (level >= 0 || level <= 9)
                {
                    result = level;
                }
            }
            return await Task.FromResult(result);
        }
    }
}