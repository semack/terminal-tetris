using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Terminal.Tetris.Common;
using Terminal.Tetris.Definitions;
using Terminal.Tetris.IO;
using Terminal.Tetris.Resources;
using Terminal.Tetris.Screens;

namespace Terminal.Tetris.Services
{
    public class TetrisService : BaseComponent, IHostedService
    {
        private readonly MainScreen _mainScreen;
        private readonly LetterBoardScreen _scoresScreen;
        private readonly SplashScreen _splashScreen;

        public TetrisService(TerminalIO io,
            SplashScreen splashScreen,
            MainScreen mainScreen,
            LetterBoardScreen scoresScreen)
            : base(io)
        {
            _splashScreen = splashScreen;
            _mainScreen = mainScreen;
            _scoresScreen = scoresScreen;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await InitializeAsync(cancellationToken);

            ThreadPool.QueueUserWorkItem(async state =>
            {
                var playAgain = true;
                while (playAgain && !cancellationToken.IsCancellationRequested)
                {
                    var playerLevel = await _splashScreen.GetPlayerLevelAsync(cancellationToken);
                    var scores = await _mainScreen.PlayGameAsync(playerLevel, cancellationToken);
                    playAgain = await _scoresScreen.ShowLetterBoardAsync(scores, cancellationToken);
                }

                System.Terminal.GenerateBreakSignal(TerminalBreakSignal.Quit);
            }, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await IO.OutAsync(0, 24, Strings.GameCopyright, cancellationToken);
        }

        private async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var (width, height) = await IO.GetWidthHeightAsync(cancellationToken);
            if (width < Constants.ScreenWidth || height < Constants.ScreenHeight)
                throw new ArgumentException(string.Format(Strings.ScreenResolutionError,
                    Constants.ScreenWidth, Constants.ScreenHeight));
        }
    }
}