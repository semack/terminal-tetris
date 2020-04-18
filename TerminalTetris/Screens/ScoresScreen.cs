using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Components;
using TerminalTetris.Common;
using TerminalTetris.Definitions;
using TerminalTetris.Resources;

namespace TerminalTetris.Screens
{
    public class ScoresScreen : Screen
    {
        private IList<PlayerScoreItem> _letterBoard;

        public ScoresScreen(Game game) : base(game)
        {
            _letterBoard = new List<PlayerScoreItem>();
        }

        public async Task<bool> ShowLetterBoardAsync(PlayerScoreItem scores, CancellationToken cancellationToken)
        {
            await LoadScoresAsync(scores, cancellationToken);
            bool? playAgain = null;
            while (playAgain == null)
            {
                await DrawAsync(cancellationToken);
                playAgain = await PlayerInputAsync(cancellationToken);
            }

            return await Task.FromResult((bool) playAgain);
        }

        private async Task LoadScoresAsync(PlayerScoreItem scoreItem, CancellationToken cancellationToken)
        {
            // init serializer
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            //load data
            string jsonString;
            var fileName = $"{nameof(TerminalTetris)}.json";
            if (File.Exists(fileName))
            {
                jsonString = await File.ReadAllTextAsync(fileName, cancellationToken);
                _letterBoard = JsonSerializer.Deserialize<IList<PlayerScoreItem>>(jsonString, options);
            }

            // join actual scores
            var item = _letterBoard.FirstOrDefault(x =>
                x.Player.Equals(scoreItem.Player, StringComparison.OrdinalIgnoreCase));
            _letterBoard.Remove(item);

            _letterBoard.Add(scoreItem);

            // taking tops
            _letterBoard = _letterBoard
                .OrderBy(x => x.Level)
                .ThenBy(x => x.Score)
                .Take(Constants.MaxTopPlayers)
                .ToList();

            // storing back to file
            jsonString = JsonSerializer.Serialize(_letterBoard, options);
            await File.WriteAllTextAsync(fileName, jsonString, cancellationToken);
        }

        private async Task DrawAsync(CancellationToken cancellationToken = default)
        {
            await Display.ClearAsync(cancellationToken);
            await Display.OutAsync(21, 2, Strings.Name, cancellationToken);
            await Display.OutAsync(32, 2, Strings.Level, cancellationToken);
            await Display.OutAsync(41, 2, Strings.Score, cancellationToken);
            var i = 1;
            foreach (var item in _letterBoard)
            {
                await Display.OutAsync(21, 2 + i, item.Player, cancellationToken);
                await Display.OutAsync(32, 2 + i, 7, item.Level.ToString(), cancellationToken);
                await Display.OutAsync(41, 2 + i, 4, item.Score.ToString(), cancellationToken);
                if (item.IsCurrentPlayer)
                    await Display.OutAsync(45, 2 + i, 3, Strings.CurrentPlayer, cancellationToken);
                i++;
            }

            await Display.OutAsync(10, 24, Strings.PlayAgain, cancellationToken);
        }

        private async Task<bool?> PlayerInputAsync(CancellationToken cancellationToken = default)
        {
            var input = await Keyboard.ReadLineAsync(cancellationToken);
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