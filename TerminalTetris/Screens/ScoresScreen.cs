using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Components;
using TerminalTetris.Common;

namespace TerminalTetris.Screens
{
    public class ScoresScreen : Screen
    {
        public ScoresScreen(Game game) : base(game)
        {
        }

        public async Task<bool> ShowLetterBoardAsync(GameScore scores, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}