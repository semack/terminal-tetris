using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.EventArgs;

namespace GameFramework.Components
{
    public class GameComponentsCollection : ICollection<GameComponent>
    {
        private Game _game;
        private List<GameComponent> _list;

        private async Task UpdateAsync(GameUpdateEventArgs args, CancellationToken cancellationToken)
        {
            _list.ForEach(async x =>
            {
                if (x.IsEnabled)
                    await x.UpdateAsync(args, cancellationToken);
            });
            await Task.CompletedTask;
        }

        public GameComponentsCollection(Game game)
        {
            _game = game;
            _list = new List<GameComponent>();
            _game.OnUpdate += UpdateAsync;
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public void Add(GameComponent item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(GameComponent item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(GameComponent[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GameComponent> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool Remove(GameComponent item)
        {
            return _list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
