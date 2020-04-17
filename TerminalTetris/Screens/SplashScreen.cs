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

        public async Task<int> GetUserLevelAsync(CancellationToken cancellationToken = default)
        {
            int? userLevel = null;
            while (userLevel == null)
            {
                await DrawSplashAsync(cancellationToken);
                userLevel = await InputLevelAsync(cancellationToken);
            }

            return await Task.FromResult((int) userLevel);
        }

        private async Task DrawSplashAsync(CancellationToken cancellationToken = default)
        {
            await Display.ClearAsync(cancellationToken);
            await Display.OutAsync(10, 18, Strings.YourLevel, cancellationToken);
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