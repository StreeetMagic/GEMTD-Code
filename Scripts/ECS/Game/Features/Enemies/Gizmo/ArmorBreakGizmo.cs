using Game.Battle;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Game.Enemies
{
    namespace Prefabs.Resources.Enemies
    {
        [ExecuteInEditMode]
        public class ArmorBreakGizmo : MonoBehaviour
        {
#if UNITY_EDITOR

            public GameEntityView EntityView;

            [SerializeField]
            private Vector3 offset = new Vector3(0, 2.5f, 0);

            [SerializeField]
            private Color shieldColor = Color.blue; // яркий щит

            [SerializeField]
            private Color textColor = Color.white;

            [SerializeField]
            private Vector2 shieldSize = new Vector2(0.8f, 0.8f);

            private void OnDrawGizmos()
            {
                if (!EntityView || EntityView.Entity == null)
                    return;

                var entity = EntityView.Entity;

                if (!entity.hasBaseStats || !entity.hasStatModifiers)
                    return;

                float baseArmor = entity.BaseStats.ContainsKey(StatEnum.Armor)
                    ? entity.BaseStats[StatEnum.Armor]
                    : 0f;

                float modifier = entity.StatModifiers.ContainsKey(StatEnum.Armor)
                    ? entity.StatModifiers[StatEnum.Armor]
                    : 0f;

                if (modifier >= 0)
                    return; // броня не снижена — не показываем

                float reduced = -modifier;

                Vector3 worldPos = transform.position + offset;
                Vector2 screenPos = HandleUtility.WorldToGUIPoint(worldPos);

                Rect rect = new Rect(
                    screenPos.x - shieldSize.x * 50f / 2f,
                    screenPos.y - shieldSize.y * 50f / 2f,
                    shieldSize.x * 50f,
                    shieldSize.y * 50f
                );

                Handles.BeginGUI();
                {
                    // ЯРКИЙ ЩИТ
                    GUI.color = shieldColor;
                    GUI.DrawTexture(rect, Texture2D.whiteTexture);
                    GUI.color = Color.white;

                    // ТЕКСТ
                    GUIStyle style = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 14,
                        fontStyle = FontStyle.Bold,
                        normal = { textColor = textColor }
                    };

                    GUI.Label(rect, $"-{Mathf.RoundToInt(reduced)}", style);
                }
                Handles.EndGUI();
            }
#endif
        }
    }
}