using Terminal.Tetris.Resources;

namespace Terminal.Tetris.Extensions
{
    public static class ArrayExtensions
    {
        public static string ToGlassLine(this short[,] array, int y)
        {
            string result = null;
            for (var x = 0; x <= array.GetUpperBound(0); x++)
            {
                if (array[x, y] == 0)
                    result += Strings.EmptyBox;
                else
                    result += Strings.BlockBox;
            }
            return result;
        }
    }
}