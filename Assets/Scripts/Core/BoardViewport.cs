using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardViewport
    {
        public event Zoom OnZoom;
        public event Move OnMove;

        public Vector2Int Location { get; }
        public Vector2Int Resolution { get; }
        public ViewportBounds Bounds { get; }
        public int ZoomFactor { get; }
        public void Zoom(bool zoomIn);
        public void Move(Vector2Int direction, float deltaTime);
    }

    public class BoardViewport : IBoardViewport
    {
        public event Zoom OnZoom;
        public event Move OnMove;
        public Vector2Int Location { get; protected set; }
        public Vector2Int Resolution { get; private set; }
        public int ZoomFactor { get; protected set; } = 1;
        public ViewportBounds Bounds => new ViewportBounds(Location, Viewport);
        private Vector2Int Viewport => new Vector2Int(Mathf.FloorToInt(Resolution.x / 2f / ZoomFactor), Mathf.FloorToInt(Resolution.y / 2f / ZoomFactor));
        private float speed = 150f;

        public BoardViewport(IBoardConfig config)
        {
            Resolution = config.Resolution;
            speed = config.ViewportSpeed;
        }

        public void Zoom(bool zoomIn)
        {
            ZoomFactor = Mathf.Clamp(Mathf.CeilToInt(ZoomFactor * (zoomIn ? 2f : 1 / 2f)), 2, 32);
            OnZoom?.Invoke(ZoomFactor);
        }

        public void Move(Vector2Int direction, float deltaTime)
        {
            Vector2Int sourceLocation = Location;
            Location = new Vector2Int(Mathf.RoundToInt(Location.x + direction.x * deltaTime * speed), Mathf.RoundToInt(Location.y + direction.y * deltaTime * speed));
            OnMove?.Invoke(sourceLocation, Location);
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