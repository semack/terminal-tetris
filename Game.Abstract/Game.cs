using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.Components;
using GameFramework.EventArgs;
using GameFramework.IO;

namespace GameFramework
{
    public abstract class Game
    {
        public event Func<object, GameUpdateEventArgs,CancellationToken, Task> OnUpdate;

        public GameComponentsCollection Components { get; }
        public Display Display { get; }
        public Keyboard Keyboard { get; }

        public TimeSpan TargetElapsedTime { get; private set; }

        public virtual async Task RunAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {                
                if (OnUpdate != null)
                {
                    var args = new GameUpdateEventArgs(new TimeSpan(), new TimeSpan());
                    await OnUpdate.Invoke(this, args, cancellationToken);
                }
                await Task.Delay(5, cancellationToken);
            }
        }

        public virtual async Task TickAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public virtual async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public Game(Display display, Keyboard keyboard, TimeSpan targetElapsedTime)
        {
            Components = new GameComponentsCollection(this);
            Display = display;
            Keyboard = keyboard;
            TargetElapsedTime = targetElapsedTime;
            OnUpdate += UpdateAsync;
        }

        protected virtual async Task UpdateAsync(object sender, GameUpdateEventArgs args, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
