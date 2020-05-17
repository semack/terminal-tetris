using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.IO;
using Terminal.Tetris.Localization;
using Terminal.Tetris.Models;

namespace Terminal.Tetris.Components
{
    public class ScoreBoard : BaseComponent
    {
        private int _lines;
        private int _score;

        public ScoreBoard(TerminalIO io, Localizer localizer) : base(io, localizer)
        {
        }

        public int Lines
        {
            get => _lines;
            set
            {
                _lines = value;
                InvalidateAsync().Wait();
            }
        }

        public short Level { get; private set; }

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                InvalidateAsync().Wait();
            }
        }

        private async Task InvalidateAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, 1, $"{Text.LinesCount}:", cancellationToken);
            await IO.OutAsync(13, 1, 3, _lines.ToString(), cancellationToken);
            await IO.OutAsync(0, 2, $"{Text.Level}:", cancellationToken);
            await IO.OutAsync(13, 2, 3, Level.ToString(), cancellationToken);
            await IO.OutAsync(2, 3, $"{Text.Score}:", cancellationToken);
            await IO.OutAsync(8, 3, 5, _score.ToString(), cancellationToken);
        }

        public async Task ResetAsync(short playerLevel, CancellationToken cancellationToken = default)
        {
            Level = playerLevel;
            _lines = 0;
            _score = 0;
            await InvalidateAsync(cancellationToken);
        }

        public async Task NextLevelAsync(CancellationToken cancellationToken = default)
        {
            if (Level < 9)
            {
                Level++;
                await InvalidateAsync(cancellationToken);
            }
        }

        public async Task<LeaderBoardItem> ToLeaderBoardItemAsync(string playerName,
            CancellationToken cancellationToken = default)
        {
            var result = new LeaderBoardItem
            {
                IsCurrentPlayer = true,
                Score = Score,
                Level = Level,
                Player = playerName
            };
            return await Task.FromResult(result);
        }
    }
}