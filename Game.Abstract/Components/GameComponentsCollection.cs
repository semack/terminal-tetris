using System.Collections;
using System.Collections.Generic;

namespace GameFramework.Components
{
    public class GameComponentsCollection : ICollection<GameComponent>
    {
        private Game _game;
        private List<GameComponent> _list;

        public GameComponentsCollection(Game game)
        {
            _game = game;
            _list = new List<GameComponent>();
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public void Add(GameComponent item)
        {
            _list.Add(item);
            _game.OnUpdate += item.Update;
        }

        public void Clear()
        {
            _list.ForEach(x => _game.OnUpdate -= x.Update);
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
            _game.OnUpdate -= item.Update;
            return _list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
