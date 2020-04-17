using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.EventArgs;

namespace GameFramework.Components
{
    public class GameComponentsCollection : ICollection<GameComponent>
    {
        private readonly List<GameComponent> _list;

        public GameComponentsCollection(Game game)
        {
            if (game == null) 
                throw new ArgumentException(nameof(game));
            
            _list = new List<GameComponent>();
            game.OnUpdate += UpdateAsync;
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

        private async Task UpdateAsync(object sender, GameUpdateEventArgs args, CancellationToken cancellationToken)
        {
            foreach (var item in _list.Where(x=>x.Enabled).OrderBy(x=>x.UpdateOrder))
            {
                 await item.UpdateAsync(sender, args, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    break;
            }

            await Task.CompletedTask;
        }
    }
}