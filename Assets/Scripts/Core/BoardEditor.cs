using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardEditor
    {
        public event RefreshBoard OnEdit;
        public event RefreshCell OnEditCell;

        public void Generate(ICustomizableBoard board, int seed);
        public void EditCell(ICustomizableBoard board, int x, int y, bool alive);
        public void Clear(ICustomizableBoard board);
    }

    public class BoardEditor : IBoardEditor
    {
        public event RefreshBoard OnEdit;
        public event RefreshCell OnEditCell;

        private System.Random random = new System.Random();

        public void Generate(ICustomizableBoard board, int seed)
        {
            random = new System.Random(seed);
            int aliveCount = random.Next(0, 10000);

            for (int i = 0; i < aliveCount; i++)
                board.SetCell(new Vector2Int(random.Next(-125, 125), random.Next(-125, 250)) / 2, true);

            OnEdit?.Invoke();
        }

        public void EditCell(ICustomizableBoard board, int x, int y, bool alive)
        {
            board.SetCell(new Vector2Int(x, y), alive);
            OnEditCell?.Invoke(x, y, alive);
        }

        public void Clear(ICustomizableBoard board)
        {
            board.Clear();
            OnEdit?.Invoke();
        }
    }
}