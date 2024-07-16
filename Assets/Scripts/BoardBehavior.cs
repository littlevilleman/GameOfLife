using Config;
using Core;
using System;
using UnityEngine;

namespace Client
{
    public class BoardBehavior : MonoBehaviour
    {
        [SerializeField] private CameraBehavior cam;
        [SerializeField] private BoardScreen screen;
        [SerializeField] private BoardConfig config;
        [SerializeField] private Renderer rend;

        private IBoard board;
        private IBoardCustomer customer;
        private IBoardPlayer player;
        private Texture2D texture;

        private void OnEnable()
        {
            board = new InfiniteBoard();
            customer = new BoardCustomer();
            player = new BoardPlayer(new Vector2Int(500, 500));

            board.OnSetup += OnSetup;
            customer.OnRefresh += Refresh;
            customer.OnRefreshCell += RefreshCell;
            player.OnZoom += OnZoom;
        }

        private void OnZoom(bool zoomIn)
        {
            texture = new Texture2D(player.Resolution.x, player.Resolution.y);
            rend.material.mainTexture = texture;
            rend.transform.localScale = new Vector3(player.Resolution.x, player.Resolution.y);
        }

        private void Start()
        {
            screen.Display(customer, board, player);
            cam.Setup(player);
            board.Setup(config);
        }

        private void OnSetup()
        {
            texture = new Texture2D(player.Resolution.x, player.Resolution.y);
            rend.material.mainTexture = texture;
            Refresh();
        }

        private void Update()
        {
            if (player.IsPaused)
                return;

            board.StepOn();
        }

        private void LateUpdate()
        {
            Refresh();
        }

        private void Refresh()
        {
            Vector2Int viewport = new Vector2Int(Mathf.FloorToInt(player.Resolution.x / cam.ZoomFactor / 2f), Mathf.FloorToInt(player.Resolution.y / cam.ZoomFactor / 2f));
            Vector2Int location = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));

            for (int y = -viewport.y; y < viewport.y; y++)
                for (int x = -viewport.x; x < viewport.x; x++)
                    UpdateCell(x, y, board.GetCell(location.x + x, location.y + y));

            texture.Apply();
        }

        private void RefreshCell(int x, int y)
        {
            UpdateCell(x, y, true);
            texture.Apply();
        }

        private void UpdateCell(int x, int y, bool alive)
        {
            Color color = alive ? Color.white : Color.black;
            texture.SetPixel(x, y, color);
        }

        private void OnDisable()
        {
            board.OnSetup -= OnSetup;
            customer.OnRefresh -= Refresh;
            customer.OnRefreshCell -= RefreshCell;
        }
    }
}