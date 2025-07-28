using System;
using Game.Entity;
using Game.Extensions;
using Services.Identifiers;

namespace Game.Battle
{
    public class StatusFactory : IStatusFactory
    {
        private readonly IIdentifierService _identifiers;

        public StatusFactory(IIdentifierService identifiers)
        {
            _identifiers = identifiers;
        }

        public GameEntity CreateStatus(StatusSetup setup, int producerId, int targetId)
        {
            GameEntity status;

            switch (setup.StatusEnum)
            {
                case StatusEnum.PoisonStatus:
                    status = CreatePoisonStatus(setup, producerId, targetId);
                    break;

                case StatusEnum.SlowStatus:
                    status = CreateSlowStatus(setup, producerId, targetId);
                    break;

                case StatusEnum.DecreaseArmor:
                    status = CreateDecreaseArmorStatus(setup, producerId, targetId);
                    break;

                case StatusEnum.AdditionalProjectiles:
                    status = CreateAdditionalProjectilesStatus(setup, producerId, targetId);
                    break;

                case StatusEnum.AttackSpeedStatus:
                    status = CreateAttackSpeedStatus(setup, producerId, targetId);
                    break;

                case StatusEnum.EAura1:
                    status = CreateEAura1Status(setup, producerId, targetId);
                    break;

                case StatusEnum.EAura2:
                    status = CreateEAura2Status(setup, producerId, targetId);
                    break;

                case StatusEnum.EAura3:
                    status = CreateEAura3Status(setup, producerId, targetId);
                    break;

                case StatusEnum.EAura4:
                    status = CreateEAura4Status(setup, producerId, targetId);
                    break;

                case StatusEnum.EAura5:
                    status = CreateEAura5Status(setup, producerId, targetId);
                    break;

                case StatusEnum.EAura6:
                    status = CreateEAura6Status(setup, producerId, targetId);
                    break;

                case StatusEnum.Unknown:
                case StatusEnum.Cleave:
                default:
                    throw new Exception($"Status with type id {setup.StatusEnum} does not exist");
            }

            status
                .With(x => x.AddDuration(setup.Duration), when: setup.Duration > 0)
                .With(x => x.AddTimeLeft(setup.Duration), when: setup.Duration > 0)
                .With(x => x.AddPeriod(setup.Period), when: setup.Period > 0)
                .With(x => x.AddTimeSinceLastTick(0), when: setup.Period > 0)
                ;

            return status;
        }

        private GameEntity CreatePoisonStatus(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.PoisonStatus)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isPoison = true)
                ;
        }

        private GameEntity CreateSlowStatus(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.SlowStatus)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isFreeze = true)
                ;
        }

        private GameEntity CreateDecreaseArmorStatus(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.DecreaseArmor)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isDecreaseArmor = true)
                ;
        }

        private GameEntity CreateAdditionalProjectilesStatus(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.AdditionalProjectiles)
                    .AddTargetId(targetId)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isAdditionalProjectilesStatus = true)
                ;
        }

        private GameEntity CreateAttackSpeedStatus(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.AttackSpeedStatus)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isAttackSpeedStatus = true)
                ;
        }

        private GameEntity CreateEAura1Status(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.EAura1)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isEAura1Status = true)
                ;
        }

        private GameEntity CreateEAura2Status(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.EAura2)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isEAura2Status = true)
                ;
        }

        private GameEntity CreateEAura3Status(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.EAura3)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isEAura3Status = true)
                ;
        }

        private GameEntity CreateEAura4Status(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.EAura4)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isEAura4Status = true)
                ;
        }

        private GameEntity CreateEAura5Status(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.EAura5)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isEAura5Status = true)
                ;
        }

        private GameEntity CreateEAura6Status(StatusSetup setup, int producerId, int targetId)
        {
            return CreateGameEntity.Empty()
                    .AddId(_identifiers.Next())
                    .AddStatusTypeId(StatusEnum.EAura6)
                    .AddEffectValue(setup.Value)
                    .AddProducerId(producerId)
                    .AddTargetId(targetId)
                    .With(x => x.isStatus = true)
                    .With(x => x.isEAura6Status = true)
                ;
        }
    }
}