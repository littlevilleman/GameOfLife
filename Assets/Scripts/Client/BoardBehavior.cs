using Config;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class BoardBehavior : MonoBehaviour
    {
        [SerializeField] private CameraBehavior cam;
        [SerializeField] private BoardScreen screen;
        [SerializeField] private BoardConfig config;
        [SerializeField] private Renderer rend;

        [SerializeField] private Color aliveColor = Color.green;
        [SerializeField] private Color deadColor = Color.red;

        private IBoard board;
        private IBoardEditor editor;
        private IBoardPlayer player;
        private Texture2D texture;

        private float refreshTime = 0f;

        private void OnEnable()
        {
            board = new Board();
            editor = new BoardEditor();
            player = new BoardPlayer(new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height));

            //editor.OnEdit += Refresh;
            board.OnStepOn += (cells, step) => Refresh(cells);
            editor.OnEditCell += EditCell;
            player.OnZoom += OnZoom;
        }

        private void Start()
        {
            screen.Display(editor, board, player);
            cam.Setup(player);

            ResetTexture();
        }

        private void Update()
        {
            if (player.IsPaused)
                return;

            refreshTime -= Time.deltaTime;

            if (refreshTime > player.Speed)
                return;

            board.StepOn();
            refreshTime = 1f;
        }

        //private void LateUpdate()
        //{
        //    Refresh();
        //}


        private void Refresh(HashSet<Vector2Int> cells)
        {
            ResetTexture();

            HashSet<Vector2Int> cellsUpdt = new HashSet<Vector2Int>(cells);
            ViewportBounds bounds = new ViewportBounds(Cell.GetLocation(transform.position), player.Viewport);

            cellsUpdt.RemoveWhere(cell => !bounds.Contains(cell));

            foreach (Vector2Int cell in cellsUpdt)
                texture.SetPixel(cell.x - bounds.location.x, cell.y - bounds.location.y, aliveColor);

            texture.Apply();
        }

        private void EditCell(int x, int y, bool alive)
        {
            texture.SetPixel(x, y, alive ? aliveColor : deadColor);
            texture.Apply();
        }

        private void OnZoom(int zoomFactor)
        {
            ResetTexture();
        }

        private void ResetTexture()
        {
            texture = new Texture2D(player.Resolution.x / player.ZoomFactor, player.Resolution.y / player.ZoomFactor, TextureFormat.RGFloat, false);
            //rend.transform.localScale = new Vector3(player.Resolution.x, player.Resolution.y) / player.ZoomFactor;
            rend.material.mainTexture = texture;
        }

        private void OnDisable()
        {
            //editor.OnEdit -= Refresh;
            editor.OnEditCell -= EditCell;
            player.OnZoom -= OnZoom;
        }
    }
}