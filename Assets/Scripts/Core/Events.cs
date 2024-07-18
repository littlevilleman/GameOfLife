using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class Events
    {
        //Board
        public delegate void StepOn(int step, ICollection<Vector2Int> cells);

        //Player
        public delegate void Pause(bool pause = true);

        //Viewport
        public delegate void Zoom(int zoomFactor = 1);
        public delegate void Move(Vector2Int source, Vector2Int destiny);

        //Generator
        public delegate void EditBoard(ICollection<Vector2Int> cells);
        public delegate void EditCell(int x, int y, bool alive);
    }
}