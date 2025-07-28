using UnityEngine;

namespace Game.Extensions
{
    public static class Vector3Extension
    {
        private static float SqrDistance(this Vector3 start, Vector3 end)
        {
            return (end - start).sqrMagnitude;
        }

        public static bool IsEnoughClose(this Vector3 start, Vector3 end)
        {
            int close = 0;

            return start.SqrDistance(end) <= close;
        }

        public static float SqrDistance(this Vector3 start, Vector2Int end)
        {
            return (new Vector3(end.x, 0, end.y) - start).sqrMagnitude;
        }

        public static bool IsEnoughClose(this Vector3 start, Vector2Int end)
        {
            int close = 0;

            return start.SqrDistance(end) <= close;
        }
    }
}