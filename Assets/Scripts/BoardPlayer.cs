using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardPlayer
    {
        public event Pause OnPause;
        public event Zoom OnZoom;
        public bool IsPaused { get; }
        public Vector2Int Resolution { get; }
        public void Pause();
        public void Pause(bool pause);
        public void Zoom(bool zoomIn);
    }

    public class BoardPlayer : IBoardPlayer
    {
        public event Pause OnPause;
        public event Zoom OnZoom;
        public bool IsPaused { get; protected set; } = true;
        public Vector2Int Resolution { get; private set; } = new Vector2Int(500, 500);

        public BoardPlayer(Vector2Int resolution) 
        {
            Resolution = resolution;
        }

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