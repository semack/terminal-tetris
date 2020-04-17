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

        public async Task<GameScore> PlayGameAsync(int userLevel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}