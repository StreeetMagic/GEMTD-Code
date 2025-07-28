using System;

namespace Infrastructure.SceneLoading
{
    public interface ISceneLoader
    {
        void LoadScene(string name, Action onLoaded = null);
    }
}