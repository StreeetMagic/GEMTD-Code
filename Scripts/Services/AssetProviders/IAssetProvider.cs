using UnityEngine;

namespace Services.AssetProviders
{
  public interface IAssetProvider
  {
    GameObject LoadAsset(string path);
    T LoadAsset<T>(string path) where T : Component;
    T LoadScriptable<T>(string path) where T : ScriptableObject;
  }
}