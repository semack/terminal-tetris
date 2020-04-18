using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.Components;
using GameFramework.EventArgs;
using GameFramework.IO;

namespace GameFramework
{
    public abstract class Game: BaseGameComponent
    {
        private bool _initialized;

        protected Game(Display display, Keyboard keyboard, TimeSpan targetElapsedTime)
        {
            Display = display;
            Keyboard = keyboard;
            TargetElapsedTime = targetElapsedTime;
        }

        public readonly  GameComponentsCollection Components = new GameComponentsCollection();

        public Display Display { get; }
        public Keyboard Keyboard { get; }

        public TimeSpan TargetElapsedTime { get; }

        public virtual async Task RunAsync(CancellationToken cancellationToken = default)
        {
            if (!_initialized)
            {
                await base.InitializeAsync(cancellationToken);
                foreach (var item in Components) await item.InitializeAsync(cancellationToken);
                _initialized = true;
            }

            ThreadPool.QueueUserWorkItem(async state =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await TickAsync(cancellationToken);
                    await Task.Delay(50, cancellationToken);
                }
            }, cancellationToken);
        }

        protected async Task TickAsync(CancellationToken cancellationToken = default)
        {
            var args = new GameUpdateEventArgs(new TimeSpan(), new TimeSpan());
            await UpdateAsync(this, args, cancellationToken);
        }

        public override async Task UpdateAsync(object sender, GameUpdateEventArgs args,
            CancellationToken cancellationToken = default)
        {
            foreach (var item in Components.Where(x => x.Enabled).OrderBy(x => x.UpdateOrder))
            {
                await item.UpdateAsync(sender, args, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;
            }
        }
    }
}