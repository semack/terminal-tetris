using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.IO;
using Terminal.Tetris.Models;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Screens
{
    public class MainScreen : BaseComponent
    {
        private bool _isGameActive = true;
        private readonly PlayerScoresItem _scores;
        private bool _helpVisible;

        public MainScreen(TerminalIO io) : base(io)
        {
            _scores = new PlayerScoresItem();
        }

        private async Task DrawGlassAsync(CancellationToken cancellationToken = default)
        {
            for (var i = 1; i < 21; i++) await IO.OutAsync(25, i, Strings.GlassItem, cancellationToken);
            await IO.OutAsync(25, 21, Strings.GlassBottom1, cancellationToken);
            await IO.OutAsync(25, 22, Strings.GlassBottom2, cancellationToken);
        }

        private async Task<string> ReadPlayerNameAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, 15, Strings.ReadPlayerName, cancellationToken);
            var result = await IO.ReadLineAsync(cancellationToken);
            return await Task.FromResult(result);
        }

        private async Task DrawHelpAsync(CancellationToken cancellationToken = default)
        {
            _helpVisible = !_helpVisible;
            if (_helpVisible)
            {
                await IO.OutAsync(52, 2, Strings.MoveLeft, cancellationToken);
                await IO.OutAsync(64, 2, Strings.MoveRight, cancellationToken);
                await IO.OutAsync(57, 3, Strings.Rotate, cancellationToken);
                await IO.OutAsync(52, 4, Strings.SpeedUp, cancellationToken);
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

        private async Task DrawScoresAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, 1, $"{Strings.LinesCount}:", cancellationToken);
            await IO.OutAsync(13, 1, 3, _scores.Lines.ToString(), cancellationToken);
            await IO.OutAsync(0, 2, $"{Strings.Level}:", cancellationToken);
            await IO.OutAsync(13, 2, 3, _scores.Level.ToString(), cancellationToken);
            await IO.OutAsync(2, 3, $"{Strings.Score}:", cancellationToken);
            await IO.OutAsync(8, 3, 5, _scores.Score.ToString(), cancellationToken);
        }

        public async Task<LetterBoardItem> PlayGameAsync(short playerLevel,
            CancellationToken cancellationToken = default)
        {
            _scores.Level = playerLevel;
            await IO.ClearAsync(cancellationToken);
            await DrawHelpAsync(cancellationToken);
            await DrawScoresAsync(cancellationToken);
            await DrawGlassAsync(cancellationToken);

            while (_isGameActive && !cancellationToken.IsCancellationRequested)
            {
                var key = await IO.GetKeyAsync(cancellationToken);
                if (key != null)
                {
                    if (key == 3)
                        await IO.Terminate(cancellationToken);
                    else
                    if (key == 27)
                        _isGameActive = false;
                    else
                    if (key == 48)
                        await DrawHelpAsync(cancellationToken);
                }

                await Task.Delay(10, cancellationToken);
            }

            var result = new LetterBoardItem
            {
                IsCurrentPlayer = true,
                Score = _scores.Score,
                Level = _scores.Level,
                Player = await ReadPlayerNameAsync(cancellationToken)
            };
            return await Task.FromResult(result);
        }
    }
}