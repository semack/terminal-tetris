using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Components;
using TerminalTetris.Common;

namespace TerminalTetris.Screens
{
    public class MainScreen : Screen
    {
        public MainScreen(Game game) : base(game)
        {
        }

        public async Task<PlayerScoreItem> PlayGameAsync(int userLevel, CancellationToken cancellationToken)
        {
            var result = new PlayerScoreItem()
            {
                Player = "TEST",
                Level = 7,
                Score = 9876,
                IsCurrentPlayer = true
            };
            return await Task.FromResult(result);
        }
    }
}