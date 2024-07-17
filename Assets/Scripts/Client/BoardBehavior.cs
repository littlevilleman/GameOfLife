using Config;
using Core;
using UnityEngine;

namespace Client
{
    public class BoardBehavior : MonoBehaviour
    {
        [SerializeField] private CameraBehavior cam;
        [SerializeField] private BoardScreen screen;
        [SerializeField] private BoardConfig config;
        [SerializeField] private Renderer rend;

        [SerializeField] private Color aliveColor = Color.white;
        [SerializeField] private Color deadColor = Color.black;

        private IBoard board;
        private IBoardEditor editor;
        private IBoardPlayer player;
        private Texture2D texture;

        private float refreshTime = 0f;

        private void OnEnable()
        {
            board = new Board();
            editor = new BoardEditor();
            player = new BoardPlayer(new Vector2Int(480, 270));

            //editor.OnEdit += Refresh;
            //board.OnStepOn += (step) => Refresh();
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

        private void LateUpdate()
        {
            Refresh();
        }

        private void Refresh()
        {
            Vector2Int viewport = player.Viewport;
            Vector2Int location = Cell.GetLocation(transform.position);

            for (int y = -viewport.y; y < viewport.y; y++)
                for (int x = -viewport.x; x < viewport.x; x++)
                    RefreshCell(x, y, board.GetCell(location + new Vector2Int(x, y)));

            texture.Apply();
        }

        private void EditCell(int x, int y, bool alive)
        {
            RefreshCell(x, y, alive);
            texture.Apply();
        }

        private void RefreshCell(int x, int y, bool alive)
        {
            texture.SetPixel(x, y, alive ? aliveColor : deadColor);
        }

        private void OnZoom(int zoomFactor)
        {
            ResetTexture();
        }

        private void ResetTexture()
        {
            texture = new Texture2D(player.Resolution.x, player.Resolution.y, TextureFormat.RGBA32, true);
            rend.transform.localScale = new Vector3(player.Resolution.x, player.Resolution.y);
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