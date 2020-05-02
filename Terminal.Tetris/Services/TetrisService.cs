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
        private readonly GameScreen _gameScreen;
        private readonly LeaderBoardScreen _leaderBoardScreen;
        private readonly SplashScreen _splashScreen;

        public TetrisService(TerminalIO io,
            SplashScreen splashScreen,
            GameScreen gameScreen,
            LeaderBoardScreen leaderBoardScreen)
            : base(io)
        {
            _splashScreen = splashScreen;
            _gameScreen = gameScreen;
            _leaderBoardScreen = leaderBoardScreen;
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
                    var scores = await _gameScreen.PlayGameAsync(playerLevel, cancellationToken);
                    playAgain = await _leaderBoardScreen.ShowAsync(scores, cancellationToken);
                }

                await IO.TerminateAsync(cancellationToken);
            }, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, Constants.ScreenHeight, Strings.GameCopyright, cancellationToken);
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