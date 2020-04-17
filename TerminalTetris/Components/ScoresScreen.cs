using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Components;
using GameFramework.EventArgs;
using TerminalTetris.Common;

namespace TerminalTetris.Components
{
    public class ScoresScreen : DrawableGameComponent
    {
        public ScoresScreen(Game game) : base(game)
        {
        }

        public override Task DrawAsync(object sender, GameUpdateEventArgs args, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ShowLetterBoardAsync(GameScore scores, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}