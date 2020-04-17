using System;
using System.Threading;
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

        public override async Task ClearAsync(CancellationToken cancellationToken = default)
        {
            Terminal.ClearScreen();

            await Task.CompletedTask;
        }

        public override Task<string> ReadLineAsync(CancellationToken cancellationToken = default)
        {
            Terminal.SetRawMode(false, false);
            Terminal.IsCursorVisible = true;
            var result = Terminal.ReadLine();
            Terminal.SetRawMode(true, true);
            Terminal.IsCursorVisible = true;
            return Task.FromResult(result);
        }

        public override Task<(int Width, int Height)> GetWidthHeightAsync(CancellationToken cancellationToken = default)
        {
            var output = (Terminal.Size.Width, Terminal.Size.Height);

            return Task.FromResult(output);
        }

        public override async Task OutAsync(string output, CancellationToken cancellationToken = default)
        {
            Terminal.Out(output);

            await Task.CompletedTask;
        }

        public override async Task OutAsync(int x, int y, string output, CancellationToken cancellationToken = default)
        {
            Terminal.MoveCursorTo(x, y);
            Terminal.Out(output);

            await Task.CompletedTask;
        }
    }
}