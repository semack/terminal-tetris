using System;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Tetris.Components
{
    public class Block
    {
        private readonly short[,] _mask;

        public Block(short[,] mask)
        {
            if (mask == null)
                throw new ArgumentNullException(nameof(mask));
            _mask = (short[,]) mask.Clone();
            X = Math.Abs(20 - Width / 2);
            Y = 1;
        }

        public int Height => _mask.GetUpperBound(0) + 1;
        public int Width => _mask.GetUpperBound(1) + 1;

        public int X { get; set; }
        public int Y { get; set; }

        public short this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    throw new IndexOutOfRangeException();
                return _mask[x, y];
            }
        }

        public async Task<Block> RotateAsync(CancellationToken cancellationToken)
        {
            var rotated = new short[Width, Height];
            for (var i = 0; i < Height; i++)
            for (var j = 0; j < Width; j++)
                rotated[j, Height - i - 1] = _mask[i, j];

            var result = new Block(rotated);
            return await Task.FromResult(result);
        }
    }
}