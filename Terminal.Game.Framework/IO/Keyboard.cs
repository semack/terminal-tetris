using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Game.Framework.IO
{
    public abstract class Keyboard
    {
        public abstract Task<byte?> GetKeyAsync(CancellationToken cancellationToken = default);

        public abstract Task<string> ReadLineAsync(CancellationToken cancellationToken = default);
    }
}