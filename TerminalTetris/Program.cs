using System;
using System.Threading;
using System.Threading.Tasks;
using TerminalTetris.IO;

namespace TerminalTetris
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // initializaton
            var cancellationTokenSource = new CancellationTokenSource();
            var tetris = new Tetris(new TerminalDisplay(), new TerminalKeyboard(), new TimeSpan(100));

            // define global exception handler using terminal output      
            AppDomain.CurrentDomain.UnhandledException += tetris.UnhandledExceptionTrapper;            

            // run the game
            await tetris.RunAsync(cancellationTokenSource.Token);
        }
    }
}
