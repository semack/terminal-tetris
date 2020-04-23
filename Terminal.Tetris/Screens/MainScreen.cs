using System;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Components;
using Terminal.Tetris.Definitions;
using Terminal.Tetris.Enums;
using Terminal.Tetris.IO;
using Terminal.Tetris.Models;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Screens
{
    public class MainScreen : BaseComponent
    {
        //private readonly PlayerScoresItem _scores;

        private readonly Glass _glass;
        private bool _isGameActive;
        private bool _nextFigureVisible;
        private readonly HelpMessage _helpMessage;
        private readonly ScoreBoard _scoreBoard;

        public MainScreen(TerminalIO io, HelpMessage helpMessage, ScoreBoard scoreBoard, Glass glass) : base(io)
        {
            _scoreBoard = scoreBoard;
            _helpMessage = helpMessage;
            _glass = glass;
        }

        private int LoopDelay => Math.Abs(Constants.LevelSpeedMultiplier / (_scoreBoard.Level + 1));

        private async Task<string> ReadPlayerNameAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, 15, Strings.ReadPlayerName, cancellationToken);
            var result = await IO.ReadLineAsync(cancellationToken);
            return await Task.FromResult(result);
        }

        private async Task DisplayNextFigureAsync(CancellationToken cancellationToken = default)
        {
            _nextFigureVisible = !_nextFigureVisible;
            if (_nextFigureVisible)
            {
            }
            else
            {
                var cleanLine = new string(' ', 8);
                for (var i = 0; i < 2; i++) await IO.OutAsync(16, 10 + i, cleanLine, cancellationToken);
            }
        }


        public async Task<LetterBoardItem> PlayGameAsync(short playerLevel,
            CancellationToken cancellationToken = default)
        {
            _isGameActive = true;


            await IO.ClearAsync(cancellationToken);
            await _helpMessage.DisplayAsync(cancellationToken);

            await _scoreBoard.ResetAsync(playerLevel, cancellationToken);
            await _glass.RunAsync(cancellationToken);

            // key polling background loop
            ThreadPool.QueueUserWorkItem(async state =>
            {
                while (_isGameActive && !cancellationToken.IsCancellationRequested)
                {
                    var playerAction = PlayerActionEnum.None;
                    var key = await IO.GetKeyAsync(cancellationToken);
                    if (key == null) continue;

                    if (key == 3) // Ctrl+C - terminate program
                        await IO.Terminate(cancellationToken);
                    else if (key == 48) // 0 - show/hide help screen
                        await _helpMessage.DisplayAsync(cancellationToken);
                    else if (key == 49) // 1 - show next figure
                        await DisplayNextFigureAsync(cancellationToken);
                    else if (key == 52) // 4 - speed up
                        await _scoreBoard.SpeedUpAsync(cancellationToken);
                    else if (key == 55) // 7 - left 
                        playerAction = PlayerActionEnum.Left;
                    else if (key == 57) // 9 - right 
                        playerAction = PlayerActionEnum.Right;
                    else if (key == 56) // 8 - rotate 
                        playerAction = PlayerActionEnum.Rotate;
                    else if (key == 53) // 5 - soft drop 
                        playerAction = PlayerActionEnum.SoftDrop;
                    else if (key == 32) // SPACE - drop
                        playerAction = PlayerActionEnum.Drop;

                    if (playerAction != PlayerActionEnum.None)
                        await _glass.TickAsync(playerAction, cancellationToken);
                }
            }, cancellationToken);

            // main loop
            while (_isGameActive && !cancellationToken.IsCancellationRequested)
            {
                await _glass.TickAsync(PlayerActionEnum.None, cancellationToken);
                await Task.Delay(LoopDelay, cancellationToken);
            }

            var result = new LetterBoardItem
            {
                IsCurrentPlayer = true,
                Score = _scoreBoard.Score,
                Level = _scoreBoard.Level,
                Player = await ReadPlayerNameAsync(cancellationToken)
            };
            return await Task.FromResult(result);
        }
    }
}