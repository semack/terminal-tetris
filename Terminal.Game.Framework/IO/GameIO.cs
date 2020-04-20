using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Game.Framework.IO
{
    public abstract class GameIO
    {
        public abstract Task ClearAsync(CancellationToken cancellationToken = default);
        public abstract Task OutAsync(string output, CancellationToken cancellationToken = default);
        public abstract Task OutAsync(int x, int y, string output, CancellationToken cancellationToken = default);

        public abstract Task OutAsync(int x, int y, int width, string output,
            CancellationToken cancellationToken = default);

        public abstract Task<(int Width, int Height)>
            GetWidthHeightAsync(CancellationToken cancellationToken = default);
        
        public abstract Task<byte?> GetKeyAsync(CancellationToken cancellationToken = default);

        public abstract Task<string> ReadLineAsync(CancellationToken cancellationToken = default);
    }
}