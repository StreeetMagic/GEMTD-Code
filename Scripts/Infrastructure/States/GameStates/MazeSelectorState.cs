using Game.GameMainFeature;
using Infrastructure.States.StateInfrastructure;
using UserInterface.MazeSelectorMenu;

namespace Infrastructure.States.GameStates
{
    public class MazeSelectorState : IPayloadState<GameModeEnum>
    {
        private readonly MazeSelectorPresenter _mazeSelectorPresenter;

        public MazeSelectorState(MazeSelectorPresenter mazeSelectorPresenter)
        {
            _mazeSelectorPresenter = mazeSelectorPresenter;
        }

        public void Enter(GameModeEnum payload)
        {
            _mazeSelectorPresenter.Enable(payload);
        }

        public void Exit()
        {
            _mazeSelectorPresenter.Disable();
        }
    }
}