using UnityEditor;
using UnityEngine;

namespace Core.Scripts.Tools.MazeDesigner.Editor
{
    public class Tools
    {
        [MenuItem("Tools/ClearPrefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}