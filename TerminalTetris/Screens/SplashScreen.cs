using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Components;
using TerminalTetris.Resources;

namespace TerminalTetris.Screens
{
    public class SplashScreen : Screen
    {
        public SplashScreen(Game game) : base(game)
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
            await Display.ClearAsync(cancellationToken);
            await Display.OutAsync(34, 9, Strings.SplashSymbol, cancellationToken);
            await Display.OutAsync(34, 10, Strings.SplashLogo, cancellationToken);
            await Display.OutAsync(42, 11, Strings.SplashSymbol, cancellationToken);
            await Display.OutAsync(18, 20, Strings.YourLevel, cancellationToken);
        }

        private async Task<int?> InputLevelAsync(CancellationToken cancellationToken = default)
        {
            int? result = null;

            var input = await Keyboard.ReadLineAsync(cancellationToken);

            if (!int.TryParse(input, out var level)) return await Task.FromResult((int?) null);

            if (level >= 0 || level <= 9) result = level;

            return await Task.FromResult(result);
        }
    }
}