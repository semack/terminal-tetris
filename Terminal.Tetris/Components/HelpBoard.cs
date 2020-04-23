using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.IO;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Components
{
    public class HelpBoard : BaseComponent
    {
        private bool _visible;

        public HelpBoard(TerminalIO io) : base(io)
        {
        }

        public async Task DisplayAsync(CancellationToken cancellationToken = default)
        {
            _visible = !_visible;
            if (_visible)
            {
                await IO.OutAsync(52, 2, Strings.MoveLeft, cancellationToken);
                await IO.OutAsync(64, 2, Strings.MoveRight, cancellationToken);
                await IO.OutAsync(57, 3, Strings.Rotate, cancellationToken);
                await IO.OutAsync(52, 4, Strings.NextLevel, cancellationToken);
                await IO.OutAsync(64, 4, Strings.SoftDrop, cancellationToken);
                await IO.OutAsync(52, 5, Strings.ShowNext, cancellationToken);
                await IO.OutAsync(52, 6, Strings.ClearHelp, cancellationToken);
                await IO.OutAsync(54, 7, Strings.Drop, cancellationToken);
            }
            else
            {
                var cleanLine = new string(' ', 25);
                for (var i = 0; i < 6; i++) await IO.OutAsync(52, 2 + i, cleanLine, cancellationToken);
            }
        }
    }
}