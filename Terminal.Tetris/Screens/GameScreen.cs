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
    public class GameScreen : BaseComponent
    {
        private readonly Glass _glass;
        private readonly HelpBoard _helpBoard;
        private readonly ScoreBoard _scoreBoard;
        private bool _isGameActive;

        public GameScreen(TerminalIO io,
            HelpBoard helpBoard,
            ScoreBoard scoreBoard,
            Glass glass) : base(io)
        {
            _scoreBoard = scoreBoard;
            _helpBoard = helpBoard;
            _glass = glass;
            _glass.OnFullLine += (sender, args) =>
            {
                _scoreBoard.Lines++;
            };
            _glass.OnGameFinished += (sender, args) =>
            {
                _isGameActive = false;
            };
            _glass.OnNewBlock += (sender, block) =>
            {
                _scoreBoard.Score += 10;
            };
        }

        private int LoopDelay => Constants.LevelSpeedMultiplier * (10 - _scoreBoard.Level);

        private async Task<string> ReadPlayerNameAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, 15, Strings.ReadPlayerName, cancellationToken);
            var result = await IO.ReadLineAsync(cancellationToken);
            return await Task.FromResult(result);
        }

        private async Task InitKeyHandlerAsync(CancellationToken cancellationToken = default)
        {
            ThreadPool.QueueUserWorkItem(async state =>
            {
                while (_isGameActive && !cancellationToken.IsCancellationRequested)
                {
                    var playerAction = PlayerActionEnum.None;

                    var key = await IO.GetKeyAsync(cancellationToken);
                    if (key == null) continue;

                    if (key == 3) // Ctrl+C - terminate program
                        await IO.TerminateAsync(cancellationToken);
                    else if (key == 48) // 0 - show/hide help screen
                        await _helpBoard.ShowHideAsync(cancellationToken);
                    else if (key == 49) // 1 - show/hide next figure
                        await _glass.ShowHideNextAsync(cancellationToken);
                    else if (key == 52) // 4 - next level
                        await _scoreBoard.NextLevelAsync(cancellationToken);
                    else if (key == 55 || key == 68) // 7 - left 
                        playerAction = PlayerActionEnum.Left;
                    else if (key == 57 || key == 67) // 9 - right 
                        playerAction = PlayerActionEnum.Right;
                    else if (key == 56 || key == 65) // 8 - rotate 
                        playerAction = PlayerActionEnum.Rotate;
                    else if (key == 53 || key == 66) // 5 - soft drop 
                        playerAction = PlayerActionEnum.SoftDrop;
                    else if (key == 32) // SPACE - drop
                        playerAction = PlayerActionEnum.Drop;

                    if (playerAction != PlayerActionEnum.None)
                        await _glass.TickAsync(playerAction, cancellationToken);
                }
            }, cancellationToken);

            await Task.CompletedTask;
        }

        public async Task<LetterBoardItem> PlayGameAsync(short playerLevel,
            CancellationToken cancellationToken = default)
        {
            await IO.ClearAsync(cancellationToken);

            await _helpBoard.ShowHideAsync(cancellationToken);
            await _scoreBoard.ResetAsync(playerLevel, cancellationToken);
            await _glass.InitAsync(cancellationToken);

            await InitKeyHandlerAsync(cancellationToken);

            _isGameActive = true;

            // main loop
            while (_isGameActive && !cancellationToken.IsCancellationRequested)
            {
                await _glass.TickAsync(PlayerActionEnum.None, cancellationToken);
                await Task.Delay(LoopDelay, cancellationToken);
            }

            var playerName = await ReadPlayerNameAsync(cancellationToken);
            var result = await _scoreBoard.ToLetterBoardItemAsync(playerName, cancellationToken);
            return await Task.FromResult(result);
        }
    }
}