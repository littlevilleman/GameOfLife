using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardViewport
    {
        public event Zoom OnZoom;
        public event Move OnMove;

        public Vector2Int Resolution { get; }
        public ViewportBounds Bounds { get; }
        public int ZoomFactor { get; }
        public void Zoom(bool zoomIn);
        public void Move(Vector2Int direction, float deltaTime, float speed);
    }

    public class BoardViewport : IBoardViewport
    {
        public event Zoom OnZoom;
        public event Move OnMove;
        public Vector2Int Resolution { get; private set; }
        public int ZoomFactor { get; protected set; } = 1;
        public Vector2Int Viewport => new Vector2Int(Mathf.FloorToInt(Resolution.x / 2f / ZoomFactor), Mathf.FloorToInt(Resolution.y / 2f / ZoomFactor));
        public ViewportBounds Bounds => new ViewportBounds(location, Viewport);

        private Vector2Int location;

        public BoardViewport(Vector2Int resolution)
        {
            Resolution = resolution;
        }

        public void Zoom(bool zoomIn)
        {
            ZoomFactor = Mathf.Clamp(Mathf.RoundToInt(ZoomFactor * (zoomIn ? 2f : 1 / 2f)), 1, 16);
            OnZoom?.Invoke(ZoomFactor);
        }

        public void Move(Vector2Int direction, float deltaTime, float speed)
        {
            Vector2Int sourceLocation = location;
            location = Cell.GetLocation(new Vector3(location.x, location.y) + new Vector3(direction.x, direction.y) * deltaTime * speed);
            OnMove?.Invoke(sourceLocation, location);
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