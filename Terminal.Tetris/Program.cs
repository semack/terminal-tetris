using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Terminal.Tetris.Components;
using Terminal.Tetris.IO;
using Terminal.Tetris.Screens;

namespace Terminal.Tetris
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            await TerminalHost
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                    {
                        services.AddSingleton<IHostedService, Tetris>();
                        services.AddSingleton<TerminalIO>();
                        services.AddSingleton<SplashScreen>();
                        services.AddSingleton<LetterBoardScreen>();
                        services.AddSingleton<MainScreen>();
                        services.AddTransient<HelpMessage>();
                        services.AddTransient<ScoreBoard>();
                        services.AddTransient(x => new Glass(x.GetRequiredService<TerminalIO>(), 27, 1));
                    }
                )
                .RunTerminalAsync(options =>
                {
                    options.Title = nameof(Terminal.Tetris);
                    options.SuppressStatusMessages = true;
                }, cancellationTokenSource.Token);

            return 0;
        }
    }
}