using System.Collections.Generic;

namespace Services.SaveLoadServices
{
  public interface ISaveLoadService
  {
    List<IProgressReader> ProgressReaders { get; }
    bool HasSavedProgress { get; }
    void SaveProgress();
    void LoadProgress();
    void DeleteSaves();
  }
}