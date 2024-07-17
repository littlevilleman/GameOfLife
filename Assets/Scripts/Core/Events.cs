using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class Events
    {
        public delegate void StepOn(HashSet<Vector2Int> cells, int step);
        public delegate void RefreshBoard();
        public delegate void RefreshCell(int x, int y, bool alive);

        public delegate void Pause(bool pause = true);
        public delegate void Zoom(int zoomFactro = 1);
    }

}