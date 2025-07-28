using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.Prefabs
{
  [Serializable]
  public class PrefabSetup
  {
    [FormerlySerializedAs("Id")]
    public PrefabEnum Enum;
    public GameObject Prefab;
  }
}