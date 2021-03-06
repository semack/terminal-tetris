﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Terminal.Tetris.Components;
using Terminal.Tetris.IO;
using Terminal.Tetris.Localization;
using Terminal.Tetris.Screens;
using Terminal.Tetris.Services;

namespace Terminal.Tetris
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            await TerminalHost
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                    {
                        services.AddScoped<Localizer>();
                        services.AddSingleton<IHostedService, TetrisService>();
                        services.AddSingleton<TerminalIO>();
                        services.AddTransient<InitScreen>();
                        services.AddTransient<SplashScreen>();
                        services.AddTransient<LeaderBoardScreen>();
                        services.AddTransient<GameScreen>();
                        services.AddTransient<HelpBoard>();
                        services.AddTransient<ScoreBoard>();
                        services.AddTransient<Glass>();
                    }
                )
                .RunTerminalAsync(options =>
                {
                    options.Title = nameof(Tetris);
                    options.SuppressStatusMessages = true;
                });

            return 0;
        }
    }
}