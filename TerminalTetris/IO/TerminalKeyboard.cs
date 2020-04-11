using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.IO;

namespace TerminalTetris.IO
{
    public class TerminalKeyboard : Keyboard
    {
        public TerminalKeyboard()
        {
            Terminal.SetRawMode(true, true);
        }

        public override async Task<byte?> GetKeyAsync(CancellationToken cancellationToken = default)
        {
            var key = Terminal.ReadRaw();

            return await Task.FromResult(key);
        }
    }
}
