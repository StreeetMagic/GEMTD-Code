using Services.SystemsFactoryServices;

namespace Game.Inputs
{
    public sealed class InputFeature : Feature
    {
        public InputFeature(ISystemFactory systems)
        {
            Add(systems.Create<InitializeInputSystem>());
            Add(systems.Create<EmitInputSystem>());
        }
    }
}