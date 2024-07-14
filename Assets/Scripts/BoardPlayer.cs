using static Core.Events;

namespace Core
{
    public interface IBoardPlayer
    {
        public event Pause OnPause;
        public event Zoom OnZoom;
        public bool IsPaused { get; }
        public void Pause();
        public void Pause(bool pause);
        public void Zoom(bool zoomIn);
    }

    public class BoardPlayer : IBoardPlayer
    {
        public event Pause OnPause;
        public event Zoom OnZoom;
        public bool IsPaused { get; protected set; } = true;

        public void Pause()
        {
            IsPaused = !IsPaused;
            OnPause?.Invoke(IsPaused);
        }

        public void Pause(bool pause)
        {
            if (pause != IsPaused)
                Pause();
        }

        public void Zoom(bool zoomIn)
        {
            OnZoom?.Invoke(zoomIn);
        }
    }
}