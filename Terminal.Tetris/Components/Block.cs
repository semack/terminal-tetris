using System;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Tetris.Components
{
    public class Block : ICloneable
    {
        private readonly short[,] _mask;

        public Block(short[,] mask)
        {
            if (mask == null)
                throw new ArgumentNullException(nameof(mask));
            _mask = (short[,]) mask.Clone();
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
                return _mask[y, x];
            }
        }

        public object Clone()
        {
            return new Block(_mask)
            {
                X = X,
                Y = Y
            };
        }

        public async Task<Block> RotateAsync(CancellationToken cancellationToken = default)
        {
            var rotated = new short[Width, Height];
            for (var i = 0; i < Height; i++)
            for (var j = 0; j < Width; j++)
                rotated[j, Height - i - 1] = _mask[i, j];
            
            var result = new Block(rotated);
            
            result.X = X + (int)((Width - result.Width) / 2.0);
            result.Y = Y + (int)((Height - result.Height) / 2.0);
            
            return await Task.FromResult(result);
        }
    }
}