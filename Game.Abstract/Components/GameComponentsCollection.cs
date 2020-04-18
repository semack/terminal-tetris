using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Components
{
    public class GameComponentsCollection : List<GameComponent>
    {
        public new void Add(GameComponent component)
        {
            base.Add(component);

            component.UpdateOrder = Count;

            if (component is DrawableGameComponent gameComponent)
                gameComponent.DrawOrder = this.Count(x => x is DrawableGameComponent);
        }
    }
}