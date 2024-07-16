using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BoardScreen : MonoBehaviour
    {
        [SerializeField] private Button clearButton;
        [SerializeField] private Button randomizeSeedButton;
        [SerializeField] private Button generateBoardButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button zoomInButton;
        [SerializeField] private Button zoomOutButton;
        [SerializeField] private TMP_Text seedInput;
        [SerializeField] private TMP_Text stepsText;
        [SerializeField] private TMP_Text playButtonText;
        [SerializeField] private int aliveCount = 40000;

        private IBoardCustomer customer;
        private ICustomizableBoard board;
        private IBoardPlayer player;
        private int Seed =>  int.Parse(seedInput.text);

        private void OnEnable()
        {
            generateBoardButton.onClick.AddListener(OnClickGenerateBoardButton);
            clearButton.onClick.AddListener(OnClickClearButton);
            playButton.onClick.AddListener(OnClickPlayButton);
            randomizeSeedButton.onClick.AddListener(OnClickRandomizeSeedButton);
            zoomInButton.onClick.AddListener(() => OnClickZoomButton(true));
            zoomOutButton.onClick.AddListener(() => OnClickZoomButton(false));
        }

        public void Display(params object[] parameters)
        {
            customer = parameters[0] as IBoardCustomer;
            board = parameters[1] as ICustomizableBoard;
            player = parameters[2] as IBoardPlayer;

            board.OnStepOn += OnStepOn;
            player.OnPause += OnPause;

            OnClickRandomizeSeedButton();
        }

        public void Update()
        {
            if (!player.IsPaused)
                return;

            if (Input.GetMouseButton(0))
                PaintCell(true);

            if (Input.GetMouseButton(1))
                PaintCell(false);
        }

        private void PaintCell(bool paint)
        {
            Vector2Int location = MathUtils.GetPointerLocation(Camera.main, player);
            if (MathUtils.IsInsideBoard(location, player))//******
                customer.PaintCell(board, location.x, location.y, paint);
        }

        private void OnClickZoomButton(bool zoomIn)
        {
            player.Zoom(zoomIn);
        }

        private void OnClickClearButton()
        {
            customer.Clear(board);
            player.Pause(true);
        }

        private void OnClickGenerateBoardButton()
        {
            customer.Clear(board);
            customer.Generate(player, board, Seed);
            player.Pause(true);
        }

        private void OnClickRandomizeSeedButton()
        {
            seedInput.text = Random.Range(-999999999, 999999999).ToString();
        }

        private void OnClickPlayButton()
        {
            player.Pause();
        }

        private void OnPause(bool isPaused)
        {
            playButtonText.text = isPaused ? "Play" : "Pause";
        }

        private void OnStepOn(int step)
        {
            stepsText.text = $"Step: {step}";
        }

        private void OnDisable()
        {
            board.OnStepOn -= OnStepOn;
            player.OnPause -= OnPause;

            generateBoardButton.onClick.RemoveListener(OnClickGenerateBoardButton);
            clearButton.onClick.RemoveListener(OnClickClearButton);
            playButton.onClick.RemoveListener(OnClickPlayButton);
            randomizeSeedButton.onClick.RemoveListener(OnClickRandomizeSeedButton);
            zoomInButton.onClick.RemoveListener(() => OnClickZoomButton(true));
            zoomOutButton.onClick.RemoveListener(() => OnClickZoomButton(false));
        }
    }
}