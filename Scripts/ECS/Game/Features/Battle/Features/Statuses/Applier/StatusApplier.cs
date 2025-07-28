using System.Linq;
using Game.EntityIndices;
using Game.Extensions;

namespace Game.Battle
{
    public class StatusApplier : IStatusApplier
    {
        private readonly IStatusFactory _statusFactory;
        private readonly GameContext _game;

        public StatusApplier(IStatusFactory statusFactory, GameContext game)
        {
            _statusFactory = statusFactory;
            _game = game;
        }

        public GameEntity ApplyStatus(StatusSetup setup, int producerId, int targetId)
        {
            GameEntity status = _game.TargetStatusesOfType(setup.StatusEnum, targetId).FirstOrDefault();
            
            if (status != null)
                return status.ReplaceTimeLeft(setup.Duration);
            
            return _statusFactory.CreateStatus(setup, producerId, targetId)
                .With(x => x.isApplied = true);
        }
    }
}