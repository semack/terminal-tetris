using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Components;

namespace Terminal.Tetris.Factories
{
    public class BlockFactory
    {
        private static readonly IList<short[,]> _masks = new List<short[,]>(new[]
        {
            new short[,] {{1, 1, 1, 1}},
            new short[,] {{2, 2}, {2, 2}},
            new short[,] {{0, 0, 3}, {3, 3, 3}},
            new short[,] {{4, 0, 0}, {4, 4, 4}},
            new short[,] {{0, 5, 5}, {5, 5, 0}},
            new short[,] {{6, 6, 0}, {0, 6, 6}},
            new short[,] {{7, 7, 7}, {0, 7, 0}}
        });

        private static readonly Random _random = new Random();

        public static async Task<Block> GeNextBlockAsync(CancellationToken cancellationToken)
        {
            var index = _random.Next(_masks.Count - 1);
            var result = new Block(_masks[index]);
            return await Task.FromResult(result);
        }
    }
}