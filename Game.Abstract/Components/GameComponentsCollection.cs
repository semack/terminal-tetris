using System.Collections.Generic;
using GameFramework.EventArgs;

namespace GameFramework.Components
{
    public class GameComponentsCollection : List<GameComponent>
    {
        public new void Add(GameComponent component)
        {
            base.Add(component);
            component.UpdateOrder = Count;
        }
    }
}