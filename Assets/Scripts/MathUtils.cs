using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Cell
    {
        public static bool IsAlive(IBoard board, int fromX, int fromY)
        {
            int count = 0;
            for (int x = fromX - 1; x <= fromX + 1; x++)
                for (int y = fromY - 1; y <= fromY + 1; y++)
                    count += (x != fromX || y != fromY) && board.GetCell(x, y) ? 1 : 0;

            return board.GetCell(fromX, fromY) ? (count == 2 || count == 3) : count == 3;
        }

        public static HashSet<Vector2Int> GetNeighbours(Vector2Int cell)
        {
            HashSet<Vector2Int> deadCells = new HashSet<Vector2Int>(8);

            for (int x = cell.x - 1; x <= cell.x + 1; x++)
                for (int y = cell.y - 1; y <= cell.y + 1; y++)
                    if (x != cell.x || y != cell.y)
                        deadCells.Add(new Vector2Int(x, y));

            return deadCells;
        }
    }

    public class MathUtils
    {
        public static int MathMod(int a, int b)
        {
            return (Math.Abs(a * b) + a) % b;
        }

        public static Vector2Int GetPointerLocation(Camera camera, IBoardPlayer player)
        {
            Vector3 offset = new Vector3(player.Resolution.x / 2f, player.Resolution.y / 2f, 0f);
            Vector3 pointerPosition = camera.ScreenToWorldPoint(Input.mousePosition) + offset;
            return new Vector2Int(Mathf.RoundToInt(pointerPosition.x), Mathf.RoundToInt(pointerPosition.y));
        }

        public static bool IsInsideBoard(Vector2Int location, IBoardPlayer player)
        {
            if (location.x >= 0 && location.y >= 0 && location.x < player.Resolution.x && location.y < player.Resolution.y)
                return true;

            return false;
        }

        public static int GenerateSeed(IBoardPlayer player, ICustomizableBoard board)
        {
            int seed = -player.Resolution.x * player.Resolution.y;

            for (int x = 0; x < player.Resolution.x; x++)
                for (int y = 0; y < player.Resolution.y; y++)
                    seed += board.GetCell(x, y) ? x * y : 0; 
            
            return seed;
        }
    }

}