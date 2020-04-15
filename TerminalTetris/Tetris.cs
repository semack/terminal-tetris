using System;
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

            // Register components
            Components.Add(new SplashScreen(this, true));
        }
    }
}