using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardConfig
    {
        public Vector2Int Size { get; }
        public float RefreshRate { get; }
    }

    public interface IBoard
    {
        public event StepOn OnStepOn;
        public void StepOn();
        public bool GetCell(Vector2Int location);
    }

    public interface ICustomizableBoard : IBoard
    {
        public int CellsCount { get; }
        public void SetCell(Vector2Int location, bool alive);
        public void Clear();
    }

    public class Board : ICustomizableBoard
    {
        public event StepOn OnStepOn;
        public int CellsCount => cells.Count;

        private HashSet<Vector2Int> cells = new HashSet<Vector2Int>();
        private int step = 0;

        public void StepOn()
        {
            HashSet<Vector2Int> cellsUpdt = new HashSet<Vector2Int>();

            cellsUpdt.AddRange(cells);

            foreach (Vector2Int cell in cells)
                foreach (Vector2Int neighbour in Cell.GetNeighbours(cell))
                    cellsUpdt.Add(neighbour);

            cellsUpdt.RemoveWhere(cell => !Cell.IsAlive(this, cell));

            step++;
            cells = cellsUpdt;
            OnStepOn?.Invoke(cells, step);
        }

        public bool GetCell(Vector2Int location)
        {
            return cells.Contains(location);
        }

        public void SetCell(Vector2Int location, bool alive)
        {
            if (alive)
                cells.Add(new Vector2Int(location.x, location.y));
            else
                cells.Remove(new Vector2Int(location.x, location.y));
        }

        public void Clear()
        {
            cells.Clear();
            step = 0;
        }
    }
}