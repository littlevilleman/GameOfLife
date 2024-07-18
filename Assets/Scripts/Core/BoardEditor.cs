using System.Collections.Generic;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IEditableCellMap : ICellMap
    {
        public ICollection<Vector2Int> Cells { get; }
        public void SetCell(Vector2Int location, bool alive);
        public void Clear();
    }

    public interface IBoardEditor
    {
        public event EditBoard OnEdit;
        public event EditCell OnEditCell;

        public void Generate(IEditableCellMap board, int seed);
        public void EditCell(IEditableCellMap board, int x, int y, bool alive);
        public void Clear(IEditableCellMap board);
    }

    public class BoardEditor : IBoardEditor
    {
        public event EditBoard OnEdit;
        public event EditCell OnEditCell;

        private System.Random random = new System.Random();

        public void Generate(IEditableCellMap board, int seed)
        {
            random = new System.Random(seed);
            int aliveCount = random.Next(0, 10000);

            for (int i = 0; i < aliveCount; i++)
                board.SetCell(new Vector2Int(random.Next(-125, 125), random.Next(-125, 250)) / 2, true);

            OnEdit?.Invoke(board.Cells);
        }

        public void EditCell(IEditableCellMap board, int x, int y, bool alive)
        {
            board.SetCell(new Vector2Int(x, y), alive);
            OnEditCell?.Invoke(x, y, alive);
        }

        public void Clear(IEditableCellMap board)
        {
            board.Clear();
            OnEdit?.Invoke(board.Cells);
        }
    }
}