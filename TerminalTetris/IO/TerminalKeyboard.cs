using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.IO;

namespace TerminalTetris.IO
{
    public class TerminalKeyboard : Keyboard
    {
        public override async Task<byte?> GetKeyAsync(CancellationToken cancellationToken = default)
        {
            if (!Terminal.IsRawMode)
            {
                Terminal.SetRawMode(true, true);

                Terminal.IsCursorVisible = false;
                Terminal.IsCursorBlinking = false;
            }

            var key = Terminal.ReadRaw();

            if (key == 3) // Ctrl + C
                Terminal.GenerateBreakSignal(TerminalBreakSignal.Interrupt);

            return await Task.FromResult(key);
        }

        public override async Task<string> ReadLineAsync(CancellationToken cancellationToken = default)
        {
            if (Terminal.IsRawMode)
            {
                Terminal.SetRawMode(false, false);

                Terminal.IsCursorVisible = true;
                Terminal.IsCursorBlinking = true;
            }

            var result = Terminal.ReadLine();

            return await Task.FromResult(result);
        }
    }
}