using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardConfig
    {
        public Vector2Int Size { get; }
        public Color AliveColor { get;}
        public Color DeadColor { get; }
    }

    public interface ICellMap
    {
        public ICollection<Vector2Int> Cells { get; }
        public bool GetCell(Vector2Int location);
    }

    public interface IBoard : ICellMap
    {
        public event StepOn OnStepOn;
        public void PlayStep(int step);
    }

    public class Board : IBoard, IEditableCellMap
    {
        public event StepOn OnStepOn;
        public ICollection<Vector2Int> Cells => cells;

        private HashSet<Vector2Int> cells = new HashSet<Vector2Int>();

        public void PlayStep(int step)
        {
            HashSet<Vector2Int> cellsUpdt = new HashSet<Vector2Int>(cells);

            foreach (Vector2Int cell in cells)
                cellsUpdt.AddRange(Cell.GetNeighbours(cell));

            cellsUpdt.RemoveWhere(cell => !Cell.IsAlive(this, cell));
            cells = new HashSet<Vector2Int>(cellsUpdt);
            OnStepOn?.Invoke(step, cells);
        }

        public bool GetCell(Vector2Int location)
        {
            return cells.Contains(location);
        }

        public void SetCell(Vector2Int location, bool alive)
        {
            if (alive)
                cells.Add(location);
            else
                cells.Remove(location);
        }

        public void Clear()
        {
            cells.Clear();
        }
    }
}