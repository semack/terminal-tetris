using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.Components;
using Terminal.Game.Framework.IO;
using Terminal.Tetris.Common;
using Terminal.Tetris.Models;

namespace Terminal.Tetris.Screens
{
    public class MainScreen : Screen
    {
        public MainScreen(GameIO io) : base(io)
        {
        }

        public async Task<PlayerScoreItem> PlayGameAsync(int userLevel, CancellationToken cancellationToken)
        {
            var result = new PlayerScoreItem
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