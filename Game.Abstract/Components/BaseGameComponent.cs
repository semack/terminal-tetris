using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.EventArgs;

namespace GameFramework.Components
{
    public abstract  class BaseGameComponent
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