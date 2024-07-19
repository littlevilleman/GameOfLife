using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public interface IBoardLocator
    {
        public void ResetTexture(IBoardViewport viewport);
        public void Refresh(Vector2Int location);
    }

    public class BoardBehavior : MonoBehaviour, IBoardLocator
    {
        [SerializeField] private Renderer rend;
        [SerializeField] private Renderer grid;

        private IBoard board;
        private IBoardPlayer player;
        private IBoardViewport viewport;
        private IBoardConfig config;

        private HashSet<Vector2Int> currentCells = new HashSet<Vector2Int>();
        private Texture2D texture;

        public void Display(IBoard board, IBoardPlayer player, IBoardViewport viewport, IBoardEditor editor, IBoardConfig config)
        {
            this.board = board;
            this.player = player;
            this.viewport = viewport;
            this.config = config;

            board.OnStepOn += (step, cells) => Refresh(cells);
            editor.OnEdit += Refresh;
            editor.OnEditCell += RefreshCell;

            ResetTexture(viewport);
        }

        private void Update()
        {
            player.Update(board, Time.deltaTime);
        }

        private void Refresh(ICollection<Vector2Int> cells)
        {
            ViewportBounds bounds = viewport.Bounds;

            foreach (Vector2Int cell in currentCells)
                if(!cells.Contains(cell))
                    texture.SetPixel(cell.x - bounds.location.x, cell.y - bounds.location.y, config.DeadColor);

            foreach (Vector2Int cell in cells)
                if(bounds.Contains(cell))
                    texture.SetPixel(cell.x - bounds.location.x, cell.y - bounds.location.y, config.AliveColor);

            texture.Apply();
            currentCells = new HashSet<Vector2Int>(cells);
        }

        private void RefreshCell(int x, int y, bool alive)
        {
            currentCells.Add(new Vector2Int(x, y));
            texture.SetPixel(x, y, alive ? config.AliveColor : config.DeadColor);
            texture.Apply();
        }

        public void Refresh(Vector2Int location)
        {
            foreach (Vector2Int cell in currentCells)
                texture.SetPixel(cell.x - location.x, cell.y - location.y, config.DeadColor);

            currentCells.Clear();
        }

        public void ResetTexture(IBoardViewport viewport)
        {
            Vector2Int res = new Vector2Int(viewport.Resolution.x / viewport.ZoomFactor, viewport.Resolution.y / viewport.ZoomFactor);
            texture = new Texture2D(res.x, res.y, TextureFormat.RGFloat, false);
            grid.material.SetVector("_size", new Vector4(res.x, res.y));
            grid.gameObject.SetActive(viewport.ZoomFactor > 4);
            rend.material.mainTexture = texture;
            currentCells.Clear();
        }
    }
}