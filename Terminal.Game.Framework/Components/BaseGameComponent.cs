using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.Time;

namespace Terminal.Game.Framework.Components
{
    public abstract class BaseGameComponent
    {
        public virtual async Task UpdateAsync(object sender, GameTime time,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public virtual async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}