using Terminal.Tetris.IO;

namespace Terminal.Tetris.Common
{
    public abstract class BaseComponent
    {
        protected BaseComponent(TerminalIO io)
        {
            IO = io;
        }

        protected TerminalIO IO { get; }
    }
}