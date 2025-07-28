using Entitas;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace UserInterface.GameplayHeadsUpDisplay.ObjectInfoPanel
{
    public class ObjectInfoPanelPresenter : Presenter<ObjectInfoPanelView>, IInitializable, ITickable
    {
        private readonly GameContext _gameContext;
        private readonly IStaticDataService _staticDataService;

        private IGroup<GameEntity> _portraits;

        public ObjectInfoPanelPresenter(
            ObjectInfoPanelView view,
            GameContext gameContext,
            IStaticDataService staticDataService) : base(view)
        {
            _staticDataService = staticDataService;
            _gameContext = gameContext;
            View.Hide();
        }

        public void Initialize()
        {
            _portraits = _gameContext.GetGroup(GameMatcher.PortraitTarget);
        }

        public void Enable()
        {
            View.Show();
        }

        public void Disable()
        {
            View.Hide();
        }

        public void Tick()
        {
            foreach (GameEntity portrait in _portraits)
            {
                if (portrait.hasTowerEnum)
                {
                    float damage = _staticDataService.GetTowerDamage(portrait.TowerEnum);
                    View.Name.text = _staticDataService.GetTowerName(portrait.TowerEnum);

                    FillViewStats(
                        health: 1.ToString(),
                        1f,
                        attack: damage.ToString(),
                        armor: 0.ToString(),
                        moveSpeed: 0.ToString(),
                        portrait.AttackSpeedStat.ToString()
                    );
                }
                else if (portrait.isEnemy)
                {
                    View.Name.text = "Enemy";

                    GameEntity spawner = _gameContext.gameMainEntity;
                    var damage = _staticDataService.GetEnemyDamage(spawner.Round);

                    var enemyHealthPoints = portrait.CurrentHealthPoints < 0
                        ? 0
                        : System.Convert.ToInt32(portrait.CurrentHealthPoints);

                    var enemyMaxHealthPoints = portrait.MaxHealthPoints;

                    FillViewStats(
                        enemyHealthPoints.ToString(),
                        enemyMaxHealthPoints,
                        attack: damage.ToString(),
                        ((int)Mathf.Round(portrait.Armor)).ToString(),
                        portrait.MoveSpeedStat.ToString(),
                        attackSpeed: 0.ToString()
                    );
                }
                else
                {
                    View.Name.text = "Based";
                    var player = _gameContext.GetEntityWithId(portrait.PlayerId);

                    var playerCurrentHealthPoints = player.CurrentHealthPoints < 0
                        ? 0
                        : System.Convert.ToInt32(player.CurrentHealthPoints);

                    var playerMaxHealthPoints = player.MaxHealthPoints;

                    FillViewStats(
                        playerCurrentHealthPoints.ToString(),
                        playerMaxHealthPoints,
                        attack: 0.ToString(),
                        armor: 0.ToString(),
                        moveSpeed: 0.ToString(),
                        attackSpeed: 0.ToString()
                    );
                }
            }
        }

        private void FillViewStats(
            string health,
            float maxHealth,
            string attack,
            string armor,
            string moveSpeed,
            string attackSpeed)
        {
            View.AttackSpeed.text = attackSpeed;
            View.Armor.text = armor;
            View.Damage.text = attack;
            View.MoveSpeed.text = moveSpeed;

            View.Health.text = health;
            View.HealthSlider.value = float.Parse(health) / maxHealth;
        }
    }
}