using System.Threading.Tasks;

namespace GameFramework.IO
{
    public abstract class Keyboard
    {
        private char? _keyPressed;

        public virtual Task<char?> GetKeyAsync()
        {
            var value = _keyPressed;
            _keyPressed = null;
            return Task.FromResult(value);
        }

        protected virtual async Task SetKeyAsync(char key)
        {
            _keyPressed = key;
            await Task.CompletedTask;
        }
    }
}
