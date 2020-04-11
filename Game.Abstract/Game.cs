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

        public GameComponentCollection Components { get; }

        public Keyboard Keyboard { get; }

        public TimeSpan TargetElapsedTime {get; set;}

        public async Task RunAsync()
        {
            await Task.CompletedTask;
        }

        public async Task ExitAsync()
        {
            await Task.CompletedTask;
        }

        public Game(Display display, Keyboard keyboard)
        {
            Components = new GameComponentCollection();
            Keyboard = keyboard;
        }
    }
}
