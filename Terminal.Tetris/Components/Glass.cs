using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Definitions;
using Terminal.Tetris.Enums;
using Terminal.Tetris.IO;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Components
{
    public class Glass : BaseComponent
    {
        private const int DeltaX = 27;
        private const int DeltaY = 1;

        private static readonly IList<short[,]> Masks = new List<short[,]>(new[]
        {
            new short[,] {{1, 1, 1, 1}},
            new short[,] {{2, 2}, {2, 2}},
            new short[,] {{0, 0, 3}, {3, 3, 3}},
            new short[,] {{4, 0, 0}, {4, 4, 4}},
            new short[,] {{0, 5, 5}, {5, 5, 0}},
            new short[,] {{6, 6, 0}, {0, 6, 6}},
            new short[,] {{7, 7, 7}, {0, 7, 0}},
            new short[,] {{0, 8, 0}, {8, 8, 8}}
        });

        private readonly Random _random = new Random();
        private Block _block;

        private short[,] _glassArray;
        private Block _nextBlock;
        private bool _showNext;

        public Glass(TerminalIO io) : base(io)
        {
        }

        public async Task ShowHideNextAsync(CancellationToken cancellationToken)
        {
            _showNext = !_showNext;
            await DisplayNextBlockAsync(cancellationToken);
        }

        private async Task<Block> GetNextBlockAsync()
        {
            var index = _random.Next(Masks.Count - 1);
            var result = new Block(Masks[index]);
            return await Task.FromResult(result);
        }

        private async Task GenerateNewBlockPairAsync()
        {
            _nextBlock ??= await GetNextBlockAsync();

            _block = _nextBlock;
            _block.X = (int) Math.Abs(Math.Ceiling((Constants.GlassWidth - _block.Width) / 2.0));
            _block.Y = 0;

            _nextBlock = await GetNextBlockAsync();
            _nextBlock.X = 0;
            _nextBlock.Y = (int) Math.Abs(Math.Ceiling((Constants.GlassHeight + DeltaY) / 2.0));
        }

        private async Task DisplayNextBlockAsync(CancellationToken cancellationToken = default)
        {
            var x = DeltaX - 11;
            var cleanLine = new string(' ', 8);
            for (var i = 0; i <= 3; i++) await IO.OutAsync(x, _nextBlock.Y + i, cleanLine, cancellationToken);

            if (_showNext)
                await DrawBlockAsync(_nextBlock, x, 0, cancellationToken);
        }


        public async Task InitAsync(CancellationToken cancellationToken = default)
        {
            _glassArray = new short[Constants.GlassWidth, Constants.GlassHeight];
            await Task.CompletedTask;
        }

        private async Task DrawGlassAsync(bool init, CancellationToken cancellationToken = default)
        {
            if (init)
            {
                int i;
                for (i = DeltaY; i < _glassArray.GetUpperBound(1) + 1 + DeltaY; i++)
                    await IO.OutAsync(DeltaX - 2, i, Strings.GlassItem, cancellationToken);
                await IO.OutAsync(DeltaX - 2, i, Strings.GlassBottom1, cancellationToken);
                await IO.OutAsync(DeltaX - 2, i + 1, Strings.GlassBottom2, cancellationToken);
            }
            else
            {
                for (var y = 0; y <= _glassArray.GetUpperBound(1); y++)
                {
                    string line = null;
                    for (var x = 0; x <= _glassArray.GetUpperBound(0); x++)
                        if (_glassArray[x, y] == 0)
                            line += Strings.EmptyBox;
                        else
                            line += Strings.BlockBox;
                    await IO.OutAsync(DeltaX, y + DeltaY, line, cancellationToken);
                }
            }
        }

        public async Task TickAsync(PlayerActionEnum action, CancellationToken cancellationToken = default)
        {
            Block block;

            if (_block == null) // first step
            {
                await GenerateNewBlockPairAsync();
                block = _block;
                await DrawGlassAsync(true, cancellationToken);
            }
            else
            {
                block = (Block) _block.Clone();
                switch (action)
                {
                    case PlayerActionEnum.None:
                    case PlayerActionEnum.SoftDrop:
                        block.Y++;
                        break;
                    case PlayerActionEnum.Left:
                    {
                        block.X--;
                        break;
                    }
                    case PlayerActionEnum.Right:
                    {
                        block.X++;
                        break;
                    }
                    case PlayerActionEnum.Rotate:
                    {
                        block = await _block.RotateAsync(cancellationToken);
                        //block.X = Math.Abs(Math.Ceiling());
                        break;
                    }
                    case PlayerActionEnum.Drop:
                        while (CanApply(block))
                            block.Y++;
                        block.Y--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(action), action, null);
                }
            }

            if (CanApply(block))
            {
                _block = block;

                await DrawGlassAsync(false, cancellationToken);
                await DrawBlockAsync(_block, DeltaX, DeltaY, cancellationToken);
                
                // check for matching
                if (BlockHasStuck(_block))
                {
                    await ApplyBlockAsync(_block); 
                    await RecalculateLinesAsync(cancellationToken);
                    await GenerateNewBlockPairAsync();
                    await DisplayNextBlockAsync(cancellationToken);
                }
            }

            await Task.CompletedTask;
        }

        private bool BlockHasStuck(Block block)
        {
            // test for next tick
            var testBlock = (Block) block.Clone();
            testBlock.Y++;
            return !CanApply(testBlock);
        }

        private async Task RecalculateLinesAsync(CancellationToken cancellationToken = default)
        {
            var newArray = new short[Constants.GlassWidth, Constants.GlassHeight];
            var yy = newArray.GetUpperBound(1);
            
            for (var y = _glassArray.GetUpperBound(1); y >= 0; y--)
            {
                var isFullLine = true;
                for (var x = 0; x <= _glassArray.GetUpperBound(0); x++)
                {
                    newArray[x, yy] = _glassArray[x, y];
                    if (isFullLine && _glassArray[x, y] == 0)
                        isFullLine = false;
                }

                if (isFullLine)
                {
                    for (var x = 0; x <= newArray.GetUpperBound(0); x++)
                         newArray[x, yy] = 0;
                }
                else
                    yy--;
            }

            _glassArray = newArray;

            await Task.CompletedTask;
        }

        private async Task ApplyBlockAsync(Block block)
        {
            for (var x = 0; x < block.Width; x++)
            for (var y = 0; y < block.Height; y++)
                if (block[x, y] != 0)
                    _glassArray[block.X + x, block.Y + y] = 1;
            await Task.CompletedTask;
        }

        private bool CanApply(Block block)
        {
            var result = block.X >= 0 &&
                         block.X + block.Width <= Constants.GlassWidth &&
                         block.Y + block.Height <= Constants.GlassHeight;

            if (result)
                for (var x = 0; x < block.Width; x++)
                for (var y = 0; y < block.Height; y++)
                    if (block[x, y] != 0 && _glassArray[block.X + x, block.Y + y] != 0)
                        return false;

            return result;
        }

        private async Task DrawBlockAsync(Block block, int deltaX, int deltaY,
            CancellationToken cancellationToken)
        {
            for (var x = 0; x < block.Width; x++)
            for (var y = 0; y < block.Height; y++)
                if (block[x, y] != 0)
                    await IO.OutAsync(deltaX + (block.X + x) * 2,
                        deltaY + block.Y + y,
                        Strings.BlockBox,
                        cancellationToken);
        }
    }
}