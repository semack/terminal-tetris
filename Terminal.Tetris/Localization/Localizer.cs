using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Tetris.Definitions;

namespace Terminal.Tetris.Localization
{
    public class Localizer
    {
        private const string DefaultCultureName = "en";

        private readonly Dictionary<string, Dictionary<string, string>> _resources;

        private CultureInfo _culture = string.IsNullOrEmpty(CultureInfo.CurrentCulture.Name)
            ? new CultureInfo(DefaultCultureName)
            : CultureInfo.CurrentCulture;

        public Localizer()
        {
            var en = new Dictionary<string, string>
            {
                {nameof(SplashLogo), "T E T R I S"},
                {nameof(SplashSymbol), $"{Constants.Box} {Constants.Box}"},
                {nameof(YourLevel), "ENTER YOUR LEVEL? (0-9) - "},
                {nameof(Yes), "YES"},
                {nameof(No), "NO"},
                {nameof(PlayAgain), "PLAY AGAIN? (YES/NO) - "},
                {nameof(LinesCount), "FULL LINES"},
                {nameof(Name), "NAME"},
                {nameof(Level), "LEVEL"},
                {nameof(Score), "SCORE"},
                {nameof(ReadPlayerName), "YOUR NAME?"},
                {nameof(MoveLeft), "7: LEFT"},
                {nameof(MoveRight), "9: RIGHT"},
                {nameof(Rotate), "8: ROTATE"},
                {nameof(NextLevel), "4: SPEEDUP"},
                {nameof(SoftDrop), "5: DROP"},
                {nameof(ShowNext), "1: SHOW NEXT"},
                {nameof(ClearHelp), "0: CLEAR THIS TEXT"},
                {nameof(Drop), "  SPACE - DROP"},
                {
                    nameof(ScreenResolutionError),
                    "The game has been designed for screen {0} x {1} symbols. Please adjust terminal window size."
                },
                {
                    nameof(GameCopyright), "\r\nTETRIS © 1984 by Alexey Pajitnov.\r\n\r\n" +
                                           "The game has been ported to .NET Core platform by Andriy S\'omak, 2020.\r\n" +
                                           "Homepage url: https://github.com/semack/terminal-tetris/\r\n\r\n"
                },
                {nameof(SelectLanguage), "SELECT LANGUAGE (1: ENGLISH / 2: RUSSIAN) - "}
            };

            var ru = new Dictionary<string, string>
            {
                {nameof(SplashLogo), "Т Е Т Р И С"},
                {nameof(SplashSymbol), $"{Constants.Box} {Constants.Box}"},
                {nameof(YourLevel), "ВАШ УРОВЕНЬ? (0-9) - "},
                {nameof(Yes), "ДА"},
                {nameof(No), "НЕТ"},
                {nameof(PlayAgain), "ЕЩЕ ПАРТИЮ? (ДА/НЕТ) - "},
                {nameof(LinesCount), "ПOЛНЫX CТPOК"},
                {nameof(Name), "ИМЯ"},
                {nameof(Level), "УРОВЕНЬ"},
                {nameof(Score), "СЧЕТ"},
                {nameof(ReadPlayerName), "BAШE ИMЯ?"},
                {nameof(MoveLeft), "7: НAЛEBO"},
                {nameof(MoveRight), "9: НAПPABO"},
                {nameof(Rotate), "8:ПOBOPOТ"},
                {nameof(NextLevel), "4:УCКOPИТЬ"},
                {nameof(SoftDrop), "5:CБPOCИТЬ"},
                {nameof(ShowNext), "1: ПOКAЗAТЬ  CЛEДУЮЩУЮ"},
                {nameof(ClearHelp), "0:  CТEPEТЬ ЭТOТ ТEКCТ"},
                {nameof(Drop), "ПPOБEЛ - CБPOCИТЬ"},
                {
                    nameof(ScreenResolutionError),
                    "Игра разработана для дисплеев {0} x {1} символов. Поджалуйста отрегулируйте размер дисплея."
                },
                {
                    nameof(GameCopyright), "\r\nТЕТРИС © 1984 Алексей Пажитнов.\r\n\r\n" +
                                           "Игра портирована на платформу .NET Core Андреем Семаком в 2020 году.\r\n" +
                                           "Домашняя страница проекта: https://github.com/semack/terminal-tetris/\r\n\r\n"
                },
                {nameof(SelectLanguage), "ВЫБЕРИТЕ ЯЗЫК (1: АНГЛИЙСКИЙ / 2: РУССКИЙ) - "}
            };

            _resources = new Dictionary<string, Dictionary<string, string>>
            {
                {DefaultCultureName, en},
                {nameof(ru), ru}
            };
        }

        public string SplashLogo => this[nameof(SplashLogo)];
        public string SplashSymbol => this[nameof(SplashSymbol)];
        public string YourLevel => this[nameof(YourLevel)];
        public string Yes => this[nameof(Yes)];
        public string No => this[nameof(No)];
        public string PlayAgain => this[nameof(PlayAgain)];
        public string LinesCount => this[nameof(LinesCount)];
        public string Name => this[nameof(Name)];
        public string Level => this[nameof(Level)];
        public string Score => this[nameof(Score)];
        public string ReadPlayerName => this[nameof(ReadPlayerName)];
        public string MoveLeft => this[nameof(MoveLeft)];
        public string MoveRight => this[nameof(MoveRight)];
        public string Rotate => this[nameof(Rotate)];
        public string NextLevel => this[nameof(NextLevel)];
        public string SoftDrop => this[nameof(SoftDrop)];
        public string ShowNext => this[nameof(ShowNext)];
        public string ClearHelp => this[nameof(ClearHelp)];
        public string Drop => this[nameof(Drop)];
        public string ScreenResolutionError => this[nameof(ScreenResolutionError)];
        public string GameCopyright => this[nameof(GameCopyright)];
        public string SelectLanguage => this[nameof(SelectLanguage)];

        private string this[string name]
        {
            get
            {
                var val = string.Empty;
                if (!_resources.ContainsKey(_culture.Name)) return val;
                if (_resources[_culture.Name].ContainsKey(name))
                    val = _resources[_culture.Name][name];
                return val;
            }
        }

        public async Task SetCultureAsync(CultureInfo culture, CancellationToken cancellationToken = default)
        {
            _culture = culture;
            await Task.CompletedTask;
        }
    }
}