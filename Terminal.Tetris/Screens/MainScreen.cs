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
        private readonly HelpBoard _helpBoard;
        private readonly ScoreBoard _scoreBoard;
        private bool _isGameActive;

        public MainScreen(TerminalIO io,
            HelpBoard helpBoard,
            ScoreBoard scoreBoard,
            Glass glass) : base(io)
        {
            _scoreBoard = scoreBoard;
            _helpBoard = helpBoard;
            _glass = glass;
        }

        private int LoopDelay => Math.Abs(Constants.LevelSpeedMultiplier / (_scoreBoard.Level + 1));

        private async Task<string> ReadPlayerNameAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(0, 15, Strings.ReadPlayerName, cancellationToken);
            var result = await IO.ReadLineAsync(cancellationToken);
            return await Task.FromResult(result);
        }

        private async Task InitKeyHandlerAsync(CancellationToken cancellationToken)
        {
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
                        await _helpBoard.DisplayAsync(cancellationToken);
                    else if (key == 49) // 1 - show/hide next figure
                        await _glass.DisplayNextBlockAsync(cancellationToken);
                    else if (key == 52) // 4 - next level
                        await _scoreBoard.NextLevelAsync(cancellationToken);
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
            
            await Task.CompletedTask;
        }

        public async Task<LetterBoardItem> PlayGameAsync(short playerLevel,
            CancellationToken cancellationToken = default)
        {
            await IO.ClearAsync(cancellationToken);
            
            await _helpBoard.DisplayAsync(cancellationToken);
            await _scoreBoard.ResetAsync(playerLevel, cancellationToken);
            await _glass.RunAsync(cancellationToken);
            
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