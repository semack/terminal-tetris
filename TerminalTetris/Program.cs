using System;
using System.Threading.Tasks;
using TerminalTetris.IO;

namespace TerminalTetris
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // init global exception handler 
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            // run the game
            var tetris = new Tetris(new TerminalDisplay(), new TerminalKeyboard());
            await tetris.RunAsync();
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Terminal.OutLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }
    }
}
