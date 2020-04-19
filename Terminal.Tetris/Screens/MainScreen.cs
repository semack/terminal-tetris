using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.Components;
using Terminal.Tetris.Common;

namespace Terminal.Tetris.Screens
{
    public class MainScreen : Screen
    {
        public MainScreen(Game.Framework.Game game) : base(game)
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