using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public class InfiniteBoard : ICustomizableBoard
    {
        public event Setup OnSetup;
        public event StepOn OnStepOn;
        public Vector2Int Size { get; protected set; } = new Vector2Int(250, 250);

        private HashSet<Vector2Int> cells = new HashSet<Vector2Int>();
        private int step = 0;

        public void StepOn()
        {
            HashSet<Vector2Int> cellsUpdt = new HashSet<Vector2Int>();
            HashSet<Vector2Int> closedCells = new HashSet<Vector2Int>();

            cellsUpdt.AddRange(cells.Where(cell => Cell.IsAlive(this, cell.x, cell.y)));

            foreach (Vector2Int cell in cells)
                foreach (Vector2Int neighbour in Cell.GetNeighbours(cell).Where(deadCell => !cellsUpdt.Contains(deadCell) && !closedCells.Contains(deadCell)))
                    if (Cell.IsAlive(this, neighbour.x, neighbour.y))
                        cellsUpdt.Add(neighbour);
                    else
                        closedCells.Add(neighbour);

            step++;
            cells = cellsUpdt;
            OnStepOn?.Invoke(step);
        }

        public bool GetCell(int x, int y)
        {
            return cells.Contains(new Vector2Int(x, y));
        }

        public void Setup(IBoardConfig config)
        {
            Size = config.Size;
            Clear();
            OnSetup?.Invoke();
        }

        public void Clear()
        {
            cells.Clear();
            step = 0;
        }

        public void SetCell(int x, int y, bool alive)
        {
            Vector2Int location = new Vector2Int(x, y);
            if (alive)
                cells.Add(location);
            else
                cells.Remove(location);
        }
    }
}