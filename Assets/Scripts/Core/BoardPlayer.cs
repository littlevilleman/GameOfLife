using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardPlayer
    {
        public event Pause OnPause;
        public event Zoom OnZoom;
        public Vector2Int Resolution { get; }
        public float Speed { get; }
        public int ZoomFactor { get; }
        public bool IsPaused { get; }
        public Vector2Int Viewport { get; }
        public void Pause();
        public void Pause(bool pause);
        public void Zoom(bool zoomIn);
        public void SetSpeed(float speed);
    }

    public class BoardPlayer : IBoardPlayer
    {
        public event Pause OnPause;
        public event Zoom OnZoom;

        public Vector2Int Resolution { get; private set; }
        public int ZoomFactor { get; protected set; } = 1;
        public bool IsPaused { get; protected set; } = true;
        public float Speed { get; protected set; } = .5f;
        public Vector2Int Viewport => new Vector2Int(Mathf.RoundToInt(Resolution.x / 2f / ZoomFactor), Mathf.RoundToInt(Resolution.y / 2f / ZoomFactor));


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

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }
    }

    public struct ViewportBounds
    {
        public Vector2Int location;
        public Vector2Int size;

        public ViewportBounds(Vector2Int location, Vector2Int size)
        {
            this.location = location;
            this.size = size;
        }

        public bool Contains(Vector2Int cell)
        {
            return cell.x - location.x >= -size.x && cell.x - location.x < size.x && cell.y - location.y >= -size.y && cell.y - location.y < size.y;
        }
    }
}