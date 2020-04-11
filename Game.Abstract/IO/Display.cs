using System.Threading.Tasks;

namespace GameFramework.IO
{
    public abstract class Display
    {
        public abstract Task ClearAsync();
        public abstract Task OutAsync(string line);
        public abstract Task OutAsync(int X, int Y, string line);
        public abstract Task<(int, int)> GetDimensionXYAsync();
    }
}
