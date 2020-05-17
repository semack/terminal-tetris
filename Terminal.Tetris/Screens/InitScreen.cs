using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Definitions;
using Terminal.Tetris.IO;
using Terminal.Tetris.Localization;

namespace Terminal.Tetris.Screens
{
    public class InitScreen : BaseComponent
    {
        public InitScreen(TerminalIO io,
            Localizer localizer) : base(io, localizer)
        {
        }

        private async Task DrawAsync(CancellationToken cancellationToken = default)
        {
            await IO.ClearAsync(cancellationToken);
            await IO.OutAsync(0, Constants.ScreenHeight, Text.GameCopyright, cancellationToken);
            await IO.OutAsync(0, Constants.ScreenHeight, Text.SelectLanguage, cancellationToken);
        }

        private async Task<CultureInfo> InputGameLanguageAsync(CancellationToken cancellationToken = default)
        {
            CultureInfo result = null;
            var input = await IO.ReadLineAsync(cancellationToken);
            if (!short.TryParse(input, out var level)) return await Task.FromResult((CultureInfo) null);
            if (level == 1)
                result = new CultureInfo("en");
            else if (level == 2)
                result = new CultureInfo("ru");
            return await Task.FromResult(result);
        }

        public async Task<CultureInfo> SelectCultureAsync(CancellationToken cancellationToken)
        {
            CultureInfo culture = null;
            while (culture == null)
            {
                await DrawAsync(cancellationToken);
                culture = await InputGameLanguageAsync(cancellationToken);
            }

            return culture;
        }
    }
}