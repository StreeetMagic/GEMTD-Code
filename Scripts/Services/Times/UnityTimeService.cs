using System;

namespace Services.Times
{
    public class UnityTimeService : ITimeService
    {
        private bool _paused;
        
        public float TimeMultiplier { set; get; } = 1f;
        public bool IsPaused => _paused;

        public float DeltaTime =>
            !_paused
                ? UnityEngine.Time.deltaTime * TimeMultiplier
                : 0;

        public DateTime UtcNow => DateTime.UtcNow;

        public void Pause()
        {
            _paused = true;
        }

        public void UnPause()
        {
            _paused = false;
        }
    }
}