using Services.SystemsFactoryServices;

namespace Game.Cursor
{
    public sealed class CursorFeature : Feature
    {
        public CursorFeature(ISystemFactory systems)
        {
            Add(systems.Create<UpdateCursorPositionSystem>());
        }
    }
}