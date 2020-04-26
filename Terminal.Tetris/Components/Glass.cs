using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Enums;
using Terminal.Tetris.Extensions;
using Terminal.Tetris.IO;
using Strings = Terminal.Tetris.Resources.Strings;

namespace Terminal.Tetris.Components
{
    public class Glass : BaseComponent
    {
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

        private readonly short[,] _glassArray = new short[10, 20];

        private readonly Random _random = new Random();
        private readonly int _deltaX = 27;
        private readonly int _deltaY  = 1;
        private Block _block;
        private Block _nextBlock;
        private bool _nextFigureVisible;

        public Glass(TerminalIO io) : base(io)
        {
        }

        private async Task<Block> GetNextBlockAsync(CancellationToken cancellationToken = default)
        {
            var index = _random.Next(_masks.Count - 1);
            var result = new Block(_masks[index]);
            return await Task.FromResult(result);
        }

        private async Task GenerateNewBlockPairAsync(CancellationToken cancellationToken = default)
        {
            if (_nextBlock == null)
                _nextBlock = await GetNextBlockAsync(cancellationToken);
            _block = _nextBlock;
            _nextBlock = await GetNextBlockAsync(cancellationToken);
        }

        public async Task DisplayNextBlockAsync(CancellationToken cancellationToken = default)
        {
            _nextFigureVisible = !_nextFigureVisible;
            if (_nextFigureVisible)
            {
                // _nextBlock.Width
                // await IO.OutAsync(0, 12, )
            }
            else
            {
                var cleanLine = new string(' ', 8);
                for (var i = 0; i < 2; i++) await IO.OutAsync(16, 10 + i, cleanLine, cancellationToken);
            }
        }
        
        public async Task InitAsync(CancellationToken cancellationToken = default)
        {
            // draw borders
            int i;
            for (i = _deltaY; i < (_glassArray.GetUpperBound(1) + 1) + _deltaY; i++)
                await IO.OutAsync(_deltaX - 2, i, Strings.GlassItem, cancellationToken);
            await IO.OutAsync(_deltaX - 2, i, Strings.GlassBottom1, cancellationToken);
            await IO.OutAsync(_deltaX - 2, i + 1, Strings.GlassBottom2, cancellationToken);
            
            // generate first pair
            await GenerateNewBlockPairAsync(cancellationToken);
            //await DrawBlockAsync(cancellationToken);
            await TickAsync(PlayerActionEnum.None, cancellationToken);
        }


        public async Task DrawGlassAsync(CancellationToken cancellationToken)
        {
            for (var y = 0; y <= _glassArray.GetUpperBound(1); y++)
            {
                var line = _glassArray.ToGlassLine(y);
                await IO.OutAsync(_deltaX, y + _deltaY, line, cancellationToken);
            }
        }

        // glass 20x20
        public async Task TickAsync(PlayerActionEnum action, CancellationToken cancellationToken = default)
        {
            await DrawGlassAsync(cancellationToken);
            if (action == PlayerActionEnum.None || action == PlayerActionEnum.SoftDrop)
            {
                _block.Y++;
                await DrawBlockAsync(cancellationToken);
                if (_block.Y == _glassArray.GetUpperBound(1) + 1 - _block.Height)
                    await GenerateNewBlockPairAsync(cancellationToken);
            }
            
            await IO.OutAsync(0, 10, $"{action}       ", cancellationToken);
            await Task.CompletedTask;
        }

        private async Task DrawBlockAsync(CancellationToken cancellationToken)
        {
            for (var x = 0; x < _block.Width; x++)
            {
                for (var y = 0; y < _block.Height; y++)
                {
                    if (_block[x, y] != 0)
                        await IO.OutAsync(_deltaX + (_block.X + x)*2, 
                            _deltaY + _block.Y + y, 
                            Strings.BlockBox, 
                            cancellationToken);
                }
            }
        }

        private async Task<Block> GetBlockNewPositionAsync(Block block, PlayerActionEnum action, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}