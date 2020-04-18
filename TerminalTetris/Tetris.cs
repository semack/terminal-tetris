using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.IO;
using TerminalTetris.Definitions;
using TerminalTetris.Screens;

namespace TerminalTetris
{
    public class Tetris : Game
    {
        public Tetris(Display display,
            Keyboard keyboard,
            TimeSpan targetElapsedTime)
            : base(display, keyboard, targetElapsedTime)
        {
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var (width, height) = await Display.GetWidthHeightAsync(cancellationToken);
            if (width < Constants.ScreenWidth || height < Constants.ScreenHeight)
                throw new ArgumentException(
                    $"The game has been designed for screen {Constants.ScreenWidth} x {Constants.ScreenHeight} symbols. Please adjust terminal window size.");
        }

        public override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            ThreadPool.QueueUserWorkItem(async state =>
            {
                var splash = new SplashScreen(this);
                var main = new MainScreen(this);
                var score = new ScoresScreen(this);

                await base.RunAsync(cancellationToken);

                var isGameFinished = false;
                while (!isGameFinished)
                {
                    var userLevel = await splash.GetUserLevelAsync(cancellationToken);
                    var scores = await main.PlayGameAsync(userLevel, cancellationToken);
                    isGameFinished = await score.ShowLetterBoardAsync(scores, cancellationToken);
                }
            }, cancellationToken);
            
            await base.RunAsync(cancellationToken);
        }
    }
}