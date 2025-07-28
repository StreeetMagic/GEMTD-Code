using System;
using UnityEditor;
using UnityEngine;

namespace Infrastructure
{
  public partial class Config<T> : ScriptableObject where T : Enum
  {
    [Tooltip("Идентификатор")] 
    public T Id;
  }

  public partial class Config<T>
  {
#if UNITY_EDITOR
    private const float Delay = 10;

    // ReSharper disable once StaticMemberInGenericType
    private static float s_nextRenameTime;

    // ReSharper disable once StaticMemberInGenericType
    private static bool s_renameScheduled;

    private void OnValidate()
    {
      ScheduleRenameAsset();
    }

    private void ScheduleRenameAsset()
    {
      if (s_renameScheduled)
        return;

      s_renameScheduled = true;
      s_nextRenameTime = Time.realtimeSinceStartup + Delay;
      EditorApplication.update += CheckRenameAsset;
    }

    private void CheckRenameAsset()
    {
      if (!(Time.realtimeSinceStartup >= s_nextRenameTime))
        return;

      RenameAsset();
      s_renameScheduled = false;
      EditorApplication.update -= CheckRenameAsset;
    }

    private void RenameAsset()
    {
      string path = AssetDatabase.GetAssetPath(this);
      string newFileName = Convert.ToInt32(Id) + "_" + Id;

      AssetDatabase.RenameAsset(path, newFileName);
      AssetDatabase.SaveAssets();
    }
#endif
  }
}