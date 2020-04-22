using System;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Definitions;
using Terminal.Tetris.IO;
using Terminal.Tetris.Resources;
using Terminal.Tetris.Screens;

namespace Terminal.Tetris
{
    public class Tetris : BaseComponent
    {
        public Tetris(TerminalIO io)
            : base(io)
        {
        }

        private async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var (width, height) = await IO.GetWidthHeightAsync(cancellationToken);
            if (width < Constants.ScreenWidth || height < Constants.ScreenHeight)
                throw new ArgumentException(string.Format(Strings.ScreenResolutionError,
                    Constants.ScreenWidth, Constants.ScreenHeight));
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(Strings.GameCopyright, cancellationToken);

            await InitializeAsync(cancellationToken);

            var splashScreen = new SplashScreen(IO);
            var mainScreen = new MainScreen(IO);
            var scoresScreen = new ScoresScreen(IO);

            ThreadPool.QueueUserWorkItem(async state =>
            {
                var playAgain = true;
                while (playAgain && !cancellationToken.IsCancellationRequested)
                {
                    var playerLevel = await splashScreen.GetPlayerLevelAsync(cancellationToken);
                    var scores = await mainScreen.PlayGameAsync(playerLevel, cancellationToken);
                    playAgain = await scoresScreen.ShowLetterBoardAsync(scores, cancellationToken);
                }

                System.Terminal.GenerateBreakSignal(TerminalBreakSignal.Quit);
            }, cancellationToken);
        }
    }
}