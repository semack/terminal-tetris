using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.IO;

namespace Terminal.Tetris.IO
{
    public class TerminalDisplay : Display
    {
        public override async Task ClearAsync(CancellationToken cancellationToken = default)
        {
            System.Terminal.ClearScreen();

            await Task.CompletedTask;
        }

        public override async Task OutAsync(int x, int y, int width, string output,
            CancellationToken cancellationToken = default)
        {
            var xx = x;
            //if (width > output.Length)
            xx = x + width - output.Length;
            await OutAsync(xx, y, output, cancellationToken);
        }

        public override Task<(int Width, int Height)> GetWidthHeightAsync(CancellationToken cancellationToken = default)
        {
            var output = (System.Terminal.Size.Width, System.Terminal.Size.Height);

            return Task.FromResult(output);
        }

        public override async Task OutAsync(string output, CancellationToken cancellationToken = default)
        {
            System.Terminal.Out(output);

            await Task.CompletedTask;
        }

        public override async Task OutAsync(int x, int y, string output, CancellationToken cancellationToken = default)
        {
            System.Terminal.MoveCursorTo(x, y);
            System.Terminal.Out(output);

            await Task.CompletedTask;
        }
    }
}