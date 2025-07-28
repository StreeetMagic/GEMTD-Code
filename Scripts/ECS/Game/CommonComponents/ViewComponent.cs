using Entitas;

namespace Game.CommonComponents
{
    [Game]
    public class ViewComponent : IComponent
    {
        public IGameEntityView Value;
    }
}