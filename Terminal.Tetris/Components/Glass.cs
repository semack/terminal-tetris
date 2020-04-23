using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Enums;
using Terminal.Tetris.IO;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Components
{
    public class Glass : BaseComponent
    {
        private Block _block;
        private readonly short[,] _glassArray = new short[20, 20];
        private readonly int _x;
        private readonly int _y;


        public Glass(TerminalIO io, int x, int y) : base(io)
        {
            _x = x;
            _y = y;
        }

        public Block NextBlock { get; }

        private async Task DrawGlassAsync(CancellationToken cancellationToken = default)
        {
            int i;
            for (i = _y; i < _glassArray.GetUpperBound(0) + 1 + _y; i++)
                await IO.OutAsync(_x - 2, i, Strings.GlassItem, cancellationToken);
            await IO.OutAsync(_x - 2, i, Strings.GlassBottom1, cancellationToken);
            await IO.OutAsync(_x - 2, i + 1, Strings.GlassBottom2, cancellationToken);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await DrawGlassAsync(cancellationToken);
        }

        public async Task TickAsync(PlayerActionEnum action, CancellationToken cancellationToken)
        {
        }
    }
}