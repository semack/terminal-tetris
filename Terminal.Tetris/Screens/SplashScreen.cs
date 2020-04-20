using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.Components;
using Terminal.Game.Framework.IO;
using Terminal.Tetris.Common;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Screens
{
    public class SplashScreen : Screen
    {
        public SplashScreen(GameIO io) : base(io)
        {
        }

        public async Task<int> GetPlayerLevelAsync(CancellationToken cancellationToken = default)
        {
            int? playerLevel = null;
            while (playerLevel == null)
            {
                await DrawAsync(cancellationToken);
                playerLevel = await InputLevelAsync(cancellationToken);
            }

            return await Task.FromResult((int) playerLevel);
        }

        private async Task DrawAsync(CancellationToken cancellationToken = default)
        {
            await IO.ClearAsync(cancellationToken);
            await IO.OutAsync(33, 6, Strings.SplashSymbol, cancellationToken);
            await IO.OutAsync(33, 7, Strings.SplashLogo, cancellationToken);
            await IO.OutAsync(41, 8, Strings.SplashSymbol, cancellationToken);
            await IO.OutAsync(19, 20, Strings.YourLevel, cancellationToken);
        }

        private async Task<int?> InputLevelAsync(CancellationToken cancellationToken = default)
        {
            int? result = null;

            var input = await IO.ReadLineAsync(cancellationToken);

            if (!int.TryParse(input, out var level)) return await Task.FromResult((int?) null);

            if (level >= 0 || level <= 9) result = level;

            return await Task.FromResult(result);
        }
    }
}