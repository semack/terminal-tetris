using System;
using System.Threading.Tasks;
using GameFramework.IO;

namespace TerminalTetris.IO
{
    public class TerminalDisplay : Display
    {
        public TerminalDisplay()
        {
            Terminal.Screen.IsCursorVisible = false;
        }

        public override async Task ClearAsync()
        {
            Terminal.ClearScreen();

            await Task.CompletedTask;
        }

        public override Task<(int, int)> GetDimensionsXYAsync()
        {
            var output = (width: Terminal.Size.Width, height: Terminal.Size.Height);

            return Task.FromResult(output);
        }

        public override async Task OutAsync(string line)
        {
            Terminal.Out(line);

            await Task.CompletedTask;
        }

        public override async Task OutAsync(int x, int y, string line)
        {
            Terminal.MoveCursorTo(x, y);
            Terminal.Out(line);

            await Task.CompletedTask;

        }
    }
}