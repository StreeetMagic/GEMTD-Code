using Services.PersistentProgresses;

namespace Services.SaveLoadServices
{
    public interface IProgressWriter : IProgressReader
    {
        void WriteProgress(ProjectProgress projectProgress);
    }
}