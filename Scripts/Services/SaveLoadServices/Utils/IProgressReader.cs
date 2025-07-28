using Services.PersistentProgresses;

namespace Services.SaveLoadServices
{
    public interface IProgressReader
    {
        void ReadProgress(ProjectProgress projectProgress);
    }
}