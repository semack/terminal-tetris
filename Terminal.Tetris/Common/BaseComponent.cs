using Terminal.Tetris.IO;
using Terminal.Tetris.Localization;

namespace Terminal.Tetris.Common
{
    public abstract class BaseComponent
    {
        protected BaseComponent(TerminalIO io, Localizer localizer)
        {
            IO = io;
            Text = localizer;
        }

        protected TerminalIO IO { get; }
        protected Localizer Text { get; }
    }
}