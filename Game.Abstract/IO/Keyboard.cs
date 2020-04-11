using System.Threading;
using System.Threading.Tasks;

namespace GameFramework.IO
{
    public abstract class Keyboard
    {
        public abstract Task<byte?> GetKeyAsync(CancellationToken cancellationToken = default);
    }
}
