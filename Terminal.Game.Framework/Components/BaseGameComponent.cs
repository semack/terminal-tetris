using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.EventArgs;

namespace Terminal.Game.Framework.Components
{
    public abstract class BaseGameComponent
    {
        public virtual async Task UpdateAsync(object sender, GameUpdateEventArgs args,
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