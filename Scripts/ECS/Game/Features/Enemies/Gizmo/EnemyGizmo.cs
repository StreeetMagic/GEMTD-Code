using Game.Battle;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemies
{
    [ExecuteInEditMode]
    public class EnemyGizmo : MonoBehaviour
    {
#if UNITY_EDITOR

        [FormerlySerializedAs("entityView")]
        public GameEntityView EntityView;

        [SerializeField]
        private Vector3 healthOffset = new Vector3(0, 2f, 0);

        [SerializeField]
        private Vector3 armorOffset = new Vector3(0, 2.5f, 0);

        [SerializeField]
        private Vector3 speedOffset = new Vector3(0, 1.5f, 0);

        [Header("Armor Display")]
        [SerializeField]
        private Color shieldColor = Color.blue;

        [SerializeField]
        private Color armorTextColor = Color.white;

        [SerializeField]
        private Vector2 shieldSize = new Vector2(0.8f, 0.8f);

        [Header("AttackSpeedStatus Display")]
        [SerializeField]
        private Color speedTextColor = Color.green;

        private void OnDrawGizmos()
        {
            if (!EntityView)
                return;

            if (EntityView == null || EntityView.Entity == null)
                return;

            var entity = EntityView.Entity;

            DrawHealthLabel(entity);
            DrawArmor(entity);
            DrawMoveSpeed(entity);
        }

        private void DrawHealthLabel(GameEntity entity)
        {
            float health = entity.CurrentHealthPoints;

            GUIStyle style = new GUIStyle
            {
                normal = { textColor = Color.red },
                fontSize = 14
            };

            Vector3 labelPosition = transform.position + healthOffset;
            Handles.Label(labelPosition, $"HP: {health}", style);
        }

        private void DrawArmor(GameEntity entity)
        {
            if (!entity.hasBaseStats || !entity.hasStatModifiers)
                return;

            float baseArmor = entity.BaseStats.ContainsKey(StatEnum.Armor)
                ? entity.BaseStats[StatEnum.Armor]
                : 0f;

            float modifier = entity.StatModifiers.ContainsKey(StatEnum.Armor)
                ? entity.StatModifiers[StatEnum.Armor]
                : 0f;

            float currentArmor = baseArmor + modifier;

            Vector3 worldPos = transform.position + armorOffset;
            Vector2 screenPos = HandleUtility.WorldToGUIPoint(worldPos);

            Rect rect = new Rect(
                screenPos.x - shieldSize.x * 50f / 2f,
                screenPos.y - shieldSize.y * 50f / 2f,
                shieldSize.x * 50f,
                shieldSize.y * 50f
            );

            Handles.BeginGUI();
            {
                GUI.color = shieldColor;
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
                GUI.color = Color.white;

                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 14,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = armorTextColor }
                };

                GUI.Label(rect, $"A: {Mathf.RoundToInt(currentArmor)}", style);
            }
            Handles.EndGUI();
        }

        private void DrawMoveSpeed(GameEntity entity)
        {
            if (!entity.hasBaseStats || !entity.hasStatModifiers)
                return;

            float baseSpeed = entity.BaseStats.ContainsKey(StatEnum.MoveSpeed)
                ? entity.BaseStats[StatEnum.MoveSpeed]
                : 0f;

            float modifier = entity.StatModifiers.ContainsKey(StatEnum.MoveSpeed)
                ? entity.StatModifiers[StatEnum.MoveSpeed]
                : 0f;

            float currentSpeed = baseSpeed + modifier;

            GUIStyle style = new GUIStyle
            {
                normal = { textColor = speedTextColor },
                fontSize = 14,
                fontStyle = FontStyle.Normal
            };

            Vector3 labelPosition = transform.position + speedOffset;
            Handles.Label(labelPosition, $"AttackSpeedStatus: {currentSpeed:F1}", style);
        }
#endif
    }
}