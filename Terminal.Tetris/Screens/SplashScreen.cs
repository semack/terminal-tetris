﻿using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Common;
using Terminal.Tetris.IO;
using Terminal.Tetris.Localization;

namespace Terminal.Tetris.Screens
{
    public class SplashScreen : BaseComponent
    {
        public SplashScreen(TerminalIO io,
            Localizer localizer) : base(io, localizer)
        {
        }

        public async Task<short> GetPlayerLevelAsync(CancellationToken cancellationToken = default)
        {
            int? playerLevel = null;
            while (playerLevel == null)
            {
                await DrawAsync(cancellationToken);
                playerLevel = await InputLevelAsync(cancellationToken);
            }

            return await Task.FromResult((short) playerLevel);
        }

        private async Task DrawAsync(CancellationToken cancellationToken = default)
        {
            await IO.ClearAsync(cancellationToken);
            await IO.OutAsync(33, 6, Text.SplashSymbol, cancellationToken);
            await IO.OutAsync(33, 7, Text.SplashLogo, cancellationToken);
            await IO.OutAsync(41, 8, Text.SplashSymbol, cancellationToken);
            await IO.OutAsync(19, 20, Text.YourLevel, cancellationToken);
        }

        private async Task<short?> InputLevelAsync(CancellationToken cancellationToken = default)
        {
            short? result = null;
            var input = await IO.ReadLineAsync(cancellationToken);
            if (!short.TryParse(input, out var level)) return await Task.FromResult((short?) null);
            if (level >= 0 || level <= 9) result = level;
            return await Task.FromResult(result);
        }
    }
}