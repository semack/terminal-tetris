using System;
using System.Threading.Tasks;
using GameFramework;
using TerminalTetris.Definitions;
using TerminalTetris.IO;

namespace TerminalTetris
{
    public class Tetris : Game
    {
        public Tetris(TerminalDisplay display, TerminalKeyboard keyboard) : base(display, keyboard)
        {
        }

        public override async Task RunAsync()
        {
            // checking screen size
            var screenSize = await Display.GetWidthHeightAsync();
            if (screenSize.Width != Constants.SceenWidth || screenSize.Height != Constants.ScreenHeight)
                throw new ArgumentException($"The game has been designed for screen {Constants.SceenWidth} x {Constants.ScreenHeight} symbols. Please adjust terminal window size.");

            await Display.OutAsync("Hello world");
            await base.RunAsync();
        }
    }
}
