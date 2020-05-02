using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.Definitions;
using Terminal.Tetris.IO;
using Terminal.Tetris.Models;
using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Screens
{
    public class LeaderBoardScreen : BaseComponent
    {
        private IList<LeaderBoardItem> _leaderBoard;

        public LeaderBoardScreen(TerminalIO io) : base(io)
        {
            _leaderBoard = new List<LeaderBoardItem>();
        }

        public async Task<bool> ShowAsync(LeaderBoardItem scoresItem,
            CancellationToken cancellationToken = default)
        {
            await UpdateScoresAsync(scoresItem, cancellationToken);
            bool? playAgain = null;
            while (playAgain == null)
            {
                await DrawAsync(cancellationToken);
                playAgain = await PlayerInputAsync(cancellationToken);
            }

            return await Task.FromResult((bool) playAgain);
        }

        private async Task UpdateScoresAsync(LeaderBoardItem scoresItem, CancellationToken cancellationToken = default)
        {
            _leaderBoard.Clear();
            // init serializer
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            //load data
            string jsonString;
            var fileName = $"{nameof(Tetris)}.letterBoard";
            if (File.Exists(fileName))
            {
                jsonString = await File.ReadAllTextAsync(fileName, cancellationToken);
                _leaderBoard = JsonSerializer.Deserialize<IList<LeaderBoardItem>>(jsonString, options);
            }

            // normalize
            _leaderBoard = _leaderBoard.Where(x => x.Player != null).ToList();

            // join actual scores
            var item = _leaderBoard.FirstOrDefault(x => x.Player.Equals(scoresItem.Player,
                StringComparison.OrdinalIgnoreCase));

            if (item != null)
                _leaderBoard.Remove(item);

            _leaderBoard.Add(scoresItem);

            // taking tops
            _leaderBoard = _leaderBoard
                .OrderByDescending(x => x.Level)
                .ThenByDescending(x => x.Score)
                .Take(Constants.MaxTopPlayers)
                .ToList();

            // storing back to file
            jsonString = JsonSerializer.Serialize(_leaderBoard, options);
            await File.WriteAllTextAsync(fileName, jsonString, cancellationToken);
        }

        private async Task DrawAsync(CancellationToken cancellationToken = default)
        {
            await IO.ClearAsync(cancellationToken);
            await IO.OutAsync(16, 1, Strings.Name, cancellationToken);
            await IO.OutAsync(27, 1, Strings.Level, cancellationToken);
            await IO.OutAsync(36, 1, Strings.Score, cancellationToken);
            var i = 1;
            foreach (var item in _leaderBoard)
            {
                await IO.OutAsync(16, 1 + i, item.Player, cancellationToken);
                await IO.OutAsync(27, 1 + i, 7, item.Level.ToString(), cancellationToken);
                await IO.OutAsync(36, 1 + i, 4, item.Score.ToString(), cancellationToken);
                if (item.IsCurrentPlayer)
                    await IO.OutAsync(40, 1 + i, 3, Strings.CurrentPlayer, cancellationToken);
                i++;
            }
        }

        private async Task<bool?> PlayerInputAsync(CancellationToken cancellationToken = default)
        {
            await IO.OutAsync(13, 23, Strings.PlayAgain, cancellationToken);
            var input = await IO.ReadLineAsync(cancellationToken);
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Equals(Strings.Yes, StringComparison.OrdinalIgnoreCase))
                    return await Task.FromResult(true);
                if (input.Equals(Strings.No, StringComparison.OrdinalIgnoreCase))
                    return await Task.FromResult(false);
            }

            return await Task.FromResult((bool?) null);
        }
    }
}