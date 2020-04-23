using System;
using System.Collections.Generic;
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

        
        private static readonly IList<short[,]> _masks = new List<short[,]>(new[]
        {
            new short[,] {{1, 1, 1, 1}},
            new short[,] {{2, 2}, {2, 2}},
            new short[,] {{0, 0, 3}, {3, 3, 3}},
            new short[,] {{4, 0, 0}, {4, 4, 4}},
            new short[,] {{0, 5, 5}, {5, 5, 0}},
            new short[,] {{6, 6, 0}, {0, 6, 6}},
            new short[,] {{7, 7, 7}, {0, 7, 0}}
        });

        private readonly Random _random = new Random();

        public async Task<Block> GeNextBlockAsync(CancellationToken cancellationToken)
        {
            var index = _random.Next(_masks.Count - 1);
            var result = new Block(_masks[index]);
            return await Task.FromResult(result);
        }

        public Glass(TerminalIO io, int x, int y) : base(io)
        {
            _x = x;
            _y = y;
        }
        
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
            await Task.CompletedTask;
        }
    }
}