using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using TerminalTetris.Components;
using TerminalTetris.Definitions;
using TerminalTetris.IO;

namespace TerminalTetris
{
    public class Tetris : Game
    {
        public Tetris(TerminalDisplay display,
            TerminalKeyboard keyboard,
            TimeSpan targetElapsedTime)
            : base(display, keyboard, targetElapsedTime)
        {
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var screenSize = await Display.GetWidthHeightAsync(cancellationToken);
            if (screenSize.Width < Constants.ScreenWidth || screenSize.Height < Constants.ScreenHeight)
                throw new ArgumentException(
                    $"The game has been designed for screen {Constants.ScreenWidth} x {Constants.ScreenHeight} symbols. Please adjust terminal window size.");

            // Register components
            Components.Add(new SplashScreen(this, true));

            await base.InitializeAsync(cancellationToken);
        }

        public override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            // checking screen size                         
            await base.RunAsync(cancellationToken);
        }

        public void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Display.ClearAsync().Wait();
            Display.OutAsync(e.ExceptionObject.ToString()).Wait();
            Environment.Exit(1);
        }
    }
}