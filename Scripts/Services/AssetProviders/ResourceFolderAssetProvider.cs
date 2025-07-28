using UnityEngine;

namespace Services.AssetProviders
{
    public class ResourceFolderAssetProvider : IAssetProvider
    {
        public GameObject LoadAsset(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public T LoadAsset<T>(string path) where T : Component
        {
            return Resources.Load<T>(path);
        }

        public T LoadScriptable<T>(string path) where T : ScriptableObject
        {
            return Resources.Load<T>(path); 
        }
    }
}