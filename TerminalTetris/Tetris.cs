using System;
using System.Threading.Tasks;
using GameFramework;
using TerminalTetris.IO;

namespace TerminalTetris
{
    public class Tetris : Game
    {
        public Tetris(TerminalDisplay display, TerminalKeyboard keyboard) : base(display, keyboard)
        {
        }

        public override async Task RunAsync()
        {
            await Display.OutAsync("Hello world");
            await base.RunAsync();
        }
    }
}
