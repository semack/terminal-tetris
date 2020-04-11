using System;
using System.Threading.Tasks;
using GameFramework.IO;

namespace TerminalTetris.IO
{
    public class TerminalDisplay : Display
    {
        public override Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<(int, int)> GetDimensionXYAsync()
        {
            throw new NotImplementedException();
        }

        public override Task OutAsync(string line)
        {
            throw new NotImplementedException();
        }

        public override Task OutAsync(int X, int Y, string line)
        {
            throw new NotImplementedException();
        }
    }
}
