using System.Collections.Generic;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardConfig
    {
        public Vector2Int Size { get; }
        public Color AliveColor { get;}
        public Color DeadColor { get; }
        public float ViewportSpeed { get; }
        public Vector2Int Resolution {get; }
    }

    public interface ICellMap
    {
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
                foreach (Vector2Int neighbour in Cell.GetNeighbours(cell))
                    cellsUpdt.Add(neighbour);

            cellsUpdt.RemoveWhere(cell => !Cell.IsAlive(this, cell));
            cells = new HashSet<Vector2Int>(cellsUpdt);
            OnStepOn(step, cells);
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