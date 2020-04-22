using System;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Tetris.IO
{
    public class TerminalIO
    {
        public async Task ClearAsync(CancellationToken cancellationToken = default)
        {
            System.Terminal.ClearScreen();

            await Task.CompletedTask;
        }

        public async Task OutAsync(int x, int y, int width, string output,
            CancellationToken cancellationToken = default)
        {
            var xx = x;
            //if (width > output.Length)
            xx = x + width - output.Length;
            await OutAsync(xx, y, output, cancellationToken);
        }

        public Task<(int Width, int Height)> GetWidthHeightAsync(CancellationToken cancellationToken = default)
        {
            var output = (System.Terminal.Size.Width, System.Terminal.Size.Height);

            return Task.FromResult(output);
        }

        public async Task OutAsync(string output, CancellationToken cancellationToken = default)
        {
            System.Terminal.Out(output);

            await Task.CompletedTask;
        }

        public async Task OutAsync(int x, int y, string output, CancellationToken cancellationToken = default)
        {
            System.Terminal.MoveCursorTo(x, y);
            System.Terminal.Out(output);

            await Task.CompletedTask;
        }

        public async Task<byte?> GetKeyAsync(CancellationToken cancellationToken = default)
        {
            if (!System.Terminal.IsRawMode)
            {
                System.Terminal.SetRawMode(true, false);

                System.Terminal.IsCursorVisible = false;
                System.Terminal.IsCursorBlinking = false;
            }

            var key = System.Terminal.ReadRaw();

            return await Task.FromResult(key);
        }

        public async Task<string> ReadLineAsync(CancellationToken cancellationToken = default)
        {
            if (System.Terminal.IsRawMode)
            {
                System.Terminal.SetRawMode(false, false);

                System.Terminal.IsCursorVisible = true;
                System.Terminal.IsCursorBlinking = true;
            }

            var result = System.Terminal.ReadLine();

            return await Task.FromResult(result);
        }

        public async Task Terminate(CancellationToken cancellationToken = default)
        {
            System.Terminal.GenerateBreakSignal(TerminalBreakSignal.Interrupt);
            await Task.CompletedTask;
        }
    }
}