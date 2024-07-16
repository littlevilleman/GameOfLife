using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardPlayer
    {
        public event Pause OnPause;
        public event Zoom OnZoom;
        public Vector2Int Resolution { get; }
        public int ZoomFactor { get; }
        public bool IsPaused { get; }
        public Vector2Int Viewport { get; }
        public void Pause();
        public void Pause(bool pause);
        public void Zoom(bool zoomIn);
    }

    public class BoardPlayer : IBoardPlayer
    {
        public event Pause OnPause;
        public event Zoom OnZoom;

        public Vector2Int Resolution { get; private set; } = new Vector2Int(500, 500);
        public int ZoomFactor { get; protected set; } = 2;
        public bool IsPaused { get; protected set; } = true;
        public Vector2Int Viewport => new Vector2Int(Mathf.FloorToInt(Resolution.x / 2f / ZoomFactor), Mathf.FloorToInt(Resolution.y / 2f / ZoomFactor));

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
            ZoomFactor = Mathf.Clamp(Mathf.RoundToInt(ZoomFactor * (zoomIn ? 2f : 1 / 2f)), 1, 16);
            OnZoom?.Invoke(ZoomFactor);
        }
    }
}