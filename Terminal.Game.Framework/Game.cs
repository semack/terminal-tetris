using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.Components;
using Terminal.Game.Framework.EventArgs;
using Terminal.Game.Framework.IO;

namespace Terminal.Game.Framework
{
    public abstract class Game : BaseGameComponent
    {
        protected Game(Display display, Keyboard keyboard, TimeSpan targetElapsedTime)
        {
            Components = new GameComponentsCollection();
            Display = display;
            Keyboard = keyboard;
            TargetElapsedTime = targetElapsedTime;
        }

        public GameComponentsCollection Components { get; }

        public Display Display { get; }
        public Keyboard Keyboard { get; }
        public TimeSpan TargetElapsedTime { get; }

        public override async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in Components) await item.InitializeAsync(cancellationToken);
            await base.InitializeAsync(cancellationToken);
        }

        public virtual async Task RunAsync(CancellationToken cancellationToken = default)
        {
            await InitializeAsync(cancellationToken);
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
            foreach (var item in Components.Where(x => x.Enabled).OrderBy(x => x.UpdateOrder))
            {
                await item.UpdateAsync(item, args, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;
            }
        }
    }
}