using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Components;
using Terminal.Tetris.Models;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Screens
{
    public class MainScreen : Screen
    {
        private readonly Game.Framework.Game _game;

        private readonly Glass _glass;
        private readonly PlayerScoreItem _scores;
        
        public MainScreen(Game.Framework.Game game) : base(game.IO)
        {
            _scores = new PlayerScoreItem()
            {
                IsCurrentPlayer = true
            };
            _game = game;
            _glass = new Glass(game);
            // adding components
            _game.Components.Add(_glass);
        }

        private async Task ShowHelpScreenAsync(bool visible = true, CancellationToken cancellationToken = default)
        {
            if (visible)
            {
                await IO.OutAsync(52, 2, Strings.MoveLeft, cancellationToken);
                await IO.OutAsync(64, 2, Strings.MoveRight, cancellationToken);
                await IO.OutAsync(57, 3, Strings.Rotate, cancellationToken);
                await IO.OutAsync(52, 4, Strings.SpeedUp, cancellationToken);
                await IO.OutAsync(64, 4, Strings.Drop, cancellationToken);
                await IO.OutAsync(52, 5, Strings.ShowNext, cancellationToken);
                await IO.OutAsync(52, 6, Strings.ClearHelp, cancellationToken);
                await IO.OutAsync(54, 7, Strings.SpaceDrop, cancellationToken);
            }
            else
            {
                var cleanLine = new string(' ', 25);
                for (int i = 0; i < 6; i++)
                {
                    await IO.OutAsync(52, 2+i, cleanLine, cancellationToken);
                }
            }
        }

        private async Task InvalidateScoresAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, 1, $"{Strings.LinesCount}:", cancellationToken);
            await IO.OutAsync(13, 1, 3, _scores.Lines.ToString(), cancellationToken);
            await IO.OutAsync(0, 2, $"{Strings.Level}:", cancellationToken);
            await IO.OutAsync(13,2, 3, _scores.Level.ToString(), cancellationToken);
            await IO.OutAsync(2, 3, $"{Strings.Score}:", cancellationToken);
            await IO.OutAsync(8, 3, 5, _scores.Score.ToString(), cancellationToken);
        }

        private async Task DrawGlassAsync(CancellationToken cancellationToken = default)
        {
            for (var i = 1; i < 21; i++)
            {
                await IO.OutAsync(25, i, Strings.GlassItem, cancellationToken);
            }
            await IO.OutAsync(25, 21, Strings.GlassBottom1, cancellationToken);
            await IO.OutAsync(25, 22, Strings.GlassBottom2, cancellationToken);
        }

        private async Task<string> ReadPlayerNameAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0,15, Strings.ReadPlayerName, cancellationToken);
            var result = await IO.ReadLineAsync(cancellationToken);
            return await Task.FromResult(result);
        }

        public async Task<PlayerScoreItem> PlayGameAsync(short playerLevel, CancellationToken cancellationToken = default)
        {
            _scores.Level = playerLevel;
            
            await IO.ClearAsync(cancellationToken);
            await DrawGlassAsync(cancellationToken);
            await InvalidateScoresAsync(cancellationToken);
            await ShowHelpScreenAsync(true, cancellationToken);
            _scores.Player = await ReadPlayerNameAsync(cancellationToken);
            return await Task.FromResult(_scores);
        }
    }
}