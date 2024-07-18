using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class Cell
    {
        public static bool IsAlive(ICellMap board, Vector2Int cell)
        {
            int aliveNeighbours = 0;

            foreach (Vector2Int neighbour in GetNeighbours(cell))
                aliveNeighbours += board.GetCell(neighbour) ? 1 : 0;

            return board.GetCell(cell) ? (aliveNeighbours == 2 || aliveNeighbours == 3) : aliveNeighbours == 3;
        }

        public static HashSet<Vector2Int> GetNeighbours(Vector2Int cell)
        {
            HashSet<Vector2Int> deadCells = new HashSet<Vector2Int>(9);

            for (int x = cell.x - 1; x <= cell.x + 1; x++)
                for (int y = cell.y - 1; y <= cell.y + 1; y++)
                    deadCells.Add(new Vector2Int(x, y));

            deadCells.Remove(cell);

            return deadCells;
        }

        public static Vector2Int GetLocation(Vector3 position)
        {
            return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        }
    }
}