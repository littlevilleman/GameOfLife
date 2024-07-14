using UnityEngine;
using static Core.Events;

namespace Core
{
    public static class Events
    {
        public delegate void Setup();

        public delegate void StepOn(int step);
        public delegate void RefreshBoard();
        public delegate void RefreshCell(int x, int y);

        public delegate void Pause(bool pause = true);
        public delegate void Zoom(bool zoomIn = true);
    }

    public interface IBoardConfig
    {
        public Vector2Int Size { get; }
        public float RefreshRate { get; }
    }

    public interface IBoard
    {
        public event Setup OnSetup;
        public event StepOn OnStepOn;

        public Vector2Int Size { get; }
        public void Setup(IBoardConfig config);
        public void StepOn();
        public bool GetCell(int x, int y);
    }

    public interface ICustomizableBoard : IBoard
    {
        public void SetCell(int x, int y, bool alive);
        public void Clear();
    }

    public class Board : ICustomizableBoard
    {
        public event Setup OnSetup;
        public event StepOn OnStepOn;
        public Vector2Int Size { get; protected set; } = new Vector2Int(250, 250);

        private bool[,] world;
        private int step = 0;

        public void StepOn()
        {
            bool[,] worldUpdt = new bool[Size.x, Size.y];
            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    worldUpdt[x, y] = Cell.IsAlive(this, x, y);

            step++;
            world = worldUpdt;
            OnStepOn?.Invoke(step);
        }

        public bool GetCell(int x, int y)
        {
            return world[MathUtils.MathMod(x, Size.x), MathUtils.MathMod(y, Size.y)];
        }

        public void Setup(IBoardConfig config)
        {
            Size = config.Size;
            Clear();
            OnSetup?.Invoke();
        }

        public void Clear()
        {
            world = new bool[Size.x, Size.y];
            step = 0;
        }

        public void SetCell(int x, int y, bool alive)
        {
            world[x, y] = alive;
        }
    }
}