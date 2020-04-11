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
        public event Func<GameUpdateEventArgs,CancellationToken, Task> OnUpdate;

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
                    await OnUpdate.Invoke(args, cancellationToken);
                }
                await Task.Delay(10, cancellationToken);
            }
        }

        public Game(Display display, Keyboard keyboard, TimeSpan targetElapsedTime)
        {
            Components = new GameComponentsCollection(this);
            Display = display;
            Keyboard = keyboard;
            TargetElapsedTime = targetElapsedTime;
            OnUpdate += Update;
        }

        public virtual async Task Update(GameUpdateEventArgs args, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
