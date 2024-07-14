using Config;
using Core;
using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Client
{
    public class BoardBehavior : MonoBehaviour
    {
        [SerializeField] private PixelPerfectCamera cam;
        [SerializeField] private BoardScreen screen;
        [SerializeField] private BoardConfig config;
        [SerializeField] private Renderer rend;

        private IBoard board;
        private IBoardCustomer customer;
        private IBoardPlayer player;
        private Texture2D texture;

        private int zoomFactor = 1;

        private void OnEnable()
        {
            board = new Board();
            customer = new BoardCustomer();
            player = new BoardPlayer();
            
            board.OnSetup += OnSetup;
            board.OnStepOn += OnStepOn;
            customer.OnRefresh += Refresh;
            customer.OnRefreshCell += RefreshCell;
            player.OnZoom += OnZoom;
        }

        private void OnZoom(bool zoomIn)
        {
            zoomFactor = Mathf.Clamp(Mathf.FloorToInt(zoomFactor * (zoomIn ? 2f : 1f / 2)), 1, 100);

            OnSetup();
        }

        private void Start()
        {
            screen.Display(customer, board, player);
            board.Setup(config);
        }

        private void OnSetup()
        {
            texture = new Texture2D(board.Size.x, board.Size.y);
            rend.transform.localScale = new Vector3(board.Size.x, board.Size.y, 1f) / zoomFactor;
            cam.refResolutionX = 2 * board.Size.x / zoomFactor;
            cam.refResolutionY = 2 * board.Size.y / zoomFactor;
            cam.assetsPPU = zoomFactor;
            rend.material.mainTexture = texture;
            Refresh();
        }

        private void Update()
        {
            if (player.IsPaused)
                return;

            board.StepOn();
        }

        private void OnStepOn(int step)
        {
            Refresh();
        }

        private void Refresh()
        {
            for (int y = 0; y < board.Size.y; y++)
                for (int x = 0; x < board.Size.x; x++)
                    UpdateCell(x, y);

            texture.Apply();
        }

        private void RefreshCell(int x, int y)
        {
            UpdateCell(x, y);
            texture.Apply();
        }

        private void UpdateCell(int x, int y)
        {
            Color color = board.GetCell(x, y) ? Color.white : Color.black;
            texture.SetPixel(x, y, color);
        }

        private void OnDisable()
        {
            board.OnSetup -= OnSetup;
            board.OnStepOn -= OnStepOn;
            customer.OnRefresh -= Refresh;
            customer.OnRefreshCell -= RefreshCell;
        }
    }
}