using System;
using System.Threading.Tasks;
using GameFramework.Components;
using GameFramework.EventArgs;
using GameFramework.IO;

namespace GameFramework
{
    public abstract class Game
    {
        public Func<Task, GameUpdateEventArgs> OnUpdate;

        public GameComponentsCollection Components { get; }
        public Display Display { get; }
        public Keyboard Keyboard { get; }

        public TimeSpan TargetElapsedTime {get; set;}

        public virtual async Task RunAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task ExitAsync()
        {
            await Task.CompletedTask;
        }

        public Game(Display display, Keyboard keyboard)
        {
            Components = new GameComponentsCollection();
            Display = display;
            Keyboard = keyboard;
        }
    }
}
