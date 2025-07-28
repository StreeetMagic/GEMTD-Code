using Services.SystemsFactoryServices;

namespace Game.Enemies
{
    public sealed class EnemyFeature : Feature
    {
        public EnemyFeature(ISystemFactory systems)
        {
            Add(systems.Create<EnemyDeathSystem>());

            Add(systems.Create<SetNextPointMoveSystem>());
            Add(systems.Create<UpdateMazePathDirectionSystem>());
            Add(systems.Create<CountKilledEnemiesSystem>());

            Add(systems.Create<UpdateAgeSystem>());
            Add(systems.Create<EnemyKilledReactiveSystem>());
            Add(systems.Create<FinalizeEnemyDeathProcessingSystem>());
        }
    }
}