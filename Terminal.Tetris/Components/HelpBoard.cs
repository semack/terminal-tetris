using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.IO;
using Terminal.Tetris.Localization;

namespace Terminal.Tetris.Components
{
    public class HelpBoard : BaseComponent
    {
        public HelpBoard(TerminalIO io, Localizer localizer) : base(io, localizer)
        {
        }

        public bool Visible { get; private set; }

        public async Task SetVisibleAsync(bool visible, CancellationToken cancellationToken = default)
        {
            Visible = visible;
            if (Visible)
            {
                await IO.OutAsync(52, 2, Text.MoveLeft, cancellationToken);
                await IO.OutAsync(64, 2, Text.MoveRight, cancellationToken);
                await IO.OutAsync(57, 3, Text.Rotate, cancellationToken);
                await IO.OutAsync(52, 4, Text.NextLevel, cancellationToken);
                await IO.OutAsync(64, 4, Text.SoftDrop, cancellationToken);
                await IO.OutAsync(52, 5, Text.ShowNext, cancellationToken);
                await IO.OutAsync(52, 6, Text.ClearHelp, cancellationToken);
                await IO.OutAsync(54, 7, Text.Drop, cancellationToken);
            }
            else
            {
                var cleanLine = new string(' ', 25);
                for (var i = 0; i < 6; i++) await IO.OutAsync(52, 2 + i, cleanLine, cancellationToken);
            }
        }

        public async Task ResetAsync(CancellationToken cancellationToken = default)
        {
            await SetVisibleAsync(true, cancellationToken);
        }
    }
}