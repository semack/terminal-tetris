using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.IO;
using TerminalTetris.Components;
using TerminalTetris.Definitions;
using TerminalTetris.IO;

namespace TerminalTetris
{
    public class Tetris : Game
    {

        public SplashScreen SplashScreen;
        public GameScreen GameScreen;
        public ScoresScreen ScoreScreen;
        
        public Tetris(Display display,
            Keyboard keyboard,
            TimeSpan targetElapsedTime)
            : base(display, keyboard, targetElapsedTime)
        {
            SplashScreen = new SplashScreen(this);
            GameScreen = new GameScreen(this);
            ScoreScreen = new ScoresScreen(this);
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var (width, height) = await Display.GetWidthHeightAsync(cancellationToken);
            if (width < Constants.ScreenWidth || height < Constants.ScreenHeight)
                throw new ArgumentException(
                    $"The game has been designed for screen {Constants.ScreenWidth} x {Constants.ScreenHeight} symbols. Please adjust terminal window size.");

            // Register components
            Components.Add(SplashScreen);
            Components.Add(GameScreen);
            Components.Add(ScoreScreen);
        }

        public override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            await base.RunAsync(cancellationToken);
            var isGameFinished = false;
            while (!isGameFinished)
            {
                var userLevel = await SplashScreen.GetUserLevelAsync(cancellationToken);
                var scores = await  GameScreen.PlayGameAsync(userLevel, cancellationToken);
                isGameFinished = await ScoreScreen.ShowLetterBoardAsync(scores, cancellationToken);
            }
        }
    }
}