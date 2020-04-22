using System;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Definitions;
using Terminal.Tetris.Enums;
using Terminal.Tetris.IO;
using Terminal.Tetris.Models;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Screens
{
    public class MainScreen : BaseComponent
    {
        private bool _isGameActive;
        private readonly PlayerScoresItem _scores;
        private bool _helpVisible;
        private bool _nextFigureVisible;

        private byte[,] _glass = new byte[20, 20];

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

        private async Task DisplayHelpAsync(CancellationToken cancellationToken = default)
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

        private async Task DisplayNextFigureAsync(CancellationToken cancellationToken = default)
        {
            _nextFigureVisible = !_nextFigureVisible;
            if (_helpVisible)
            {
            }
            else
            {
                var cleanLine = new string(' ', 8);
                for (var i = 0; i < 2; i++) await IO.OutAsync(16, 10 + i, cleanLine, cancellationToken);
            }
        }

        private async Task DisplayScoresAsync(CancellationToken cancellationToken = default)
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
            _scores.Lines = 0;
            _scores.Score = 0;
            _isGameActive = true;

            await IO.ClearAsync(cancellationToken);
            await DisplayHelpAsync(cancellationToken);
            await DisplayScoresAsync(cancellationToken);
            await DrawGlassAsync(cancellationToken);
            
            // key polling background loop
            ThreadPool.QueueUserWorkItem(async state =>
            {
                while (_isGameActive && !cancellationToken.IsCancellationRequested) 
                {
                    var playerAction = PlayerActionEnum.None;
                    var key = await IO.GetKeyAsync(cancellationToken);
                    if (key != null)
                    {
                        if (key == 3) // Ctrl+C - terminate program
                            await IO.Terminate(cancellationToken);
                        else if (key == 27) // ESC - stop game
                            _isGameActive = false;
                        else if (key == 48) // 0 - show/hide help screen
                            await DisplayHelpAsync(cancellationToken);
                        else if (key == 49) // 1 - show next figure
                            await DisplayNextFigureAsync(cancellationToken);
                        else if (key == 52) // 4 - speed up
                            await SpeedUpAsync(cancellationToken);
                        else if (key == 55 || key == 37) // 7 - left || cursor left
                            playerAction = PlayerActionEnum.Left;
                        else if (key == 57 || key == 39) // 9 - right || cursor right
                            playerAction = PlayerActionEnum.Right;
                        else if (key == 56 || key == 38) // 8 - rotate || cursor up
                            playerAction = PlayerActionEnum.Rotate;
                        else if (key == 53 || key == 40) // 5 - soft drop || cursor down
                            playerAction = PlayerActionEnum.SoftDrop;
                        else if (key == 32) // SPACE - drop
                            playerAction = PlayerActionEnum.Drop;

                        if (playerAction != PlayerActionEnum.None)
                            await MoveFigureAsync(playerAction, cancellationToken);
                    }
                }
            }, cancellationToken);
            
            // main loop
            while (_isGameActive && !cancellationToken.IsCancellationRequested)
            {
                var loopLength = Math.Abs(Constants.LevelSpeedMultiplier / (_scores.Level+1));
                await MoveFigureAsync(PlayerActionEnum.None, cancellationToken);
                await Task.Delay(loopLength, cancellationToken);
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

        private async Task SpeedUpAsync(CancellationToken cancellationToken)
        {
            if (_scores.Level < 9)
            {
                _scores.Level++;
                await DisplayScoresAsync(cancellationToken);
            }
        }

        private async Task MoveFigureAsync(PlayerActionEnum action,
            CancellationToken cancellationToken)
        {
            await IO.OutAsync(2, 10, DateTime.Now.TimeOfDay.TotalMilliseconds.ToString(), CancellationToken.None);
            await Task.CompletedTask;
        }
    }
}