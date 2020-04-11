using System;
using System.Threading.Tasks;
using TerminalTetris.IO;

namespace TerminalTetris
{
    class Program
    {
        static async Task Main(string[] args)
        {            
            var tetris = new Tetris(new TerminalDisplay(), new TerminalKeyboard());
            await tetris.RunAsync();
        }
    }
}
