using System;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.IO;

namespace Terminal.Tetris.IO
{
    public class TerminalKeyboard : Keyboard
    {
        public override async Task<byte?> GetKeyAsync(CancellationToken cancellationToken = default)
        {
            if (!System.Terminal.IsRawMode)
            {
                System.Terminal.SetRawMode(true, true);

                System.Terminal.IsCursorVisible = false;
                System.Terminal.IsCursorBlinking = false;
            }

            var key = System.Terminal.ReadRaw();

            if (key == 3) // Ctrl + C
                System.Terminal.GenerateBreakSignal(TerminalBreakSignal.Interrupt);

            return await Task.FromResult(key);
        }

        public override async Task<string> ReadLineAsync(CancellationToken cancellationToken = default)
        {
            if (System.Terminal.IsRawMode)
            {
                System.Terminal.SetRawMode(false, false);

                System.Terminal.IsCursorVisible = true;
                System.Terminal.IsCursorBlinking = true;
            }

            var result = System.Terminal.ReadLine();

            return await Task.FromResult(result);
        }
    }
}