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
    public class LetterBoardScreen : BaseComponent
    {
        private IList<LetterBoardItem> _letterBoard;

        public LetterBoardScreen(TerminalIO io) : base(io)
        {
            _letterBoard = new List<LetterBoardItem>();
        }

        public async Task<bool> ShowLetterBoardAsync(LetterBoardItem scoresItem,
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

        private async Task UpdateScoresAsync(LetterBoardItem scoresItem, CancellationToken cancellationToken = default)
        {
            _letterBoard.Clear();
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
                _letterBoard = JsonSerializer.Deserialize<IList<LetterBoardItem>>(jsonString, options);
            }

            // normalize
            _letterBoard = _letterBoard.Where(x => x.Player != null).ToList();

            // join actual scores
            var item = _letterBoard.FirstOrDefault(x => x.Player.Equals(scoresItem.Player,
                StringComparison.OrdinalIgnoreCase));

            if (item != null)
                _letterBoard.Remove(item);

            _letterBoard.Add(scoresItem);

            // taking tops
            _letterBoard = _letterBoard
                .OrderByDescending(x => x.Level)
                .ThenByDescending(x => x.Score)
                .Take(Constants.MaxTopPlayers)
                .ToList();

            // storing back to file
            jsonString = JsonSerializer.Serialize(_letterBoard, options);
            await File.WriteAllTextAsync(fileName, jsonString, cancellationToken);
        }

        private async Task DrawAsync(CancellationToken cancellationToken = default)
        {
            await IO.ClearAsync(cancellationToken);
            await IO.OutAsync(16, 1, Strings.Name, cancellationToken);
            await IO.OutAsync(27, 1, Strings.Level, cancellationToken);
            await IO.OutAsync(36, 1, Strings.Score, cancellationToken);
            var i = 1;
            foreach (var item in _letterBoard)
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