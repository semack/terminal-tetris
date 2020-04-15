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
        private bool _initialized;

        protected Game(Display display, Keyboard keyboard, TimeSpan targetElapsedTime)
        {
            Components = new GameComponentsCollection(this);
            Display = display;
            Keyboard = keyboard;
            TargetElapsedTime = targetElapsedTime;
            OnUpdate += UpdateAsync;
        }

        protected GameComponentsCollection Components { get; }
        public Display Display { get; }
        public Keyboard Keyboard { get; }

        public TimeSpan TargetElapsedTime { get; }
        public event Func<object, GameUpdateEventArgs, CancellationToken, Task> OnUpdate;

        private void GameLoop(object obj)
        {
            var cancellationToken = (CancellationToken) obj;
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (OnUpdate != null)
                    {
                        var args = new GameUpdateEventArgs(new TimeSpan(), new TimeSpan());
                        OnUpdate.Invoke(this, args, cancellationToken).Wait(cancellationToken);
                    }
                    Thread.Sleep(50);
                }
        }

        public virtual async Task RunAsync(CancellationToken cancellationToken = default)
        {
            if (!_initialized)
            {
                await InitializeAsync(cancellationToken);
                foreach (var item in Components) await item.InitializeAsync(cancellationToken);
                _initialized = true;
            }
            ThreadPool.QueueUserWorkItem(GameLoop, cancellationToken);
        }

        public virtual async Task TickAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task UpdateAsync(object sender, GameUpdateEventArgs args,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}