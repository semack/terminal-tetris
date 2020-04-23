using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.IO;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Components
{
    public class ScoreBoard: BaseComponent
    {
        private int _lines;
        private int _score;

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
        
        public ScoreBoard(TerminalIO io) : base(io)
        {
        }

        private async Task InvalidateAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, 1, $"{Strings.LinesCount}:", cancellationToken);
            await IO.OutAsync(13, 1, 3, _lines.ToString(), cancellationToken);
            await IO.OutAsync(0, 2, $"{Strings.Level}:", cancellationToken);
            await IO.OutAsync(13, 2, 3, Level.ToString(), cancellationToken);
            await IO.OutAsync(2, 3, $"{Strings.Score}:", cancellationToken);
            await IO.OutAsync(8, 3, 5, _score.ToString(), cancellationToken);
        }
        
        public async Task ResetAsync(short playerLevel, CancellationToken cancellationToken = default)
        {
            Level = playerLevel;
            _lines = 0;
            _score = 0;
            await InvalidateAsync(cancellationToken);
        }
        
        public async Task SpeedUpAsync(CancellationToken cancellationToken)
        {
            if (Level < 9)
                Level++;
            await InvalidateAsync(cancellationToken);
        }
    }
}