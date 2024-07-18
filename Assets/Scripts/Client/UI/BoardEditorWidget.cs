using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BoardEditorWidget : MonoBehaviour
    {
        [SerializeField] private CameraBehavior cam;
        [SerializeField] private Button clearButton;
        [SerializeField] private Button generateBoardButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private TMP_Text seedInput;

        private IBoardEditor editor;
        private IEditableCellMap board;
        private IBoardPlayerHandler player;

        private int Seed => int.Parse(seedInput.text);

        private void OnEnable()
        {
            generateBoardButton.onClick.AddListener(OnClickGenerateButton);
            clearButton.onClick.AddListener(OnClickClearButton);
            restartButton.onClick.AddListener(OnClickRestartButton);
        }

        public void Display(object[] parameters)
        {
            editor = parameters[0] as IBoardEditor;
            board = parameters[1] as IEditableCellMap;
            player = parameters[2] as IBoardPlayerHandler;
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
                PaintCell(true);

            if (Input.GetMouseButton(1))
                PaintCell(false);
        }

        private void OnClickGenerateButton()
        {
            seedInput.text = Random.Range(-999999999, 999999999).ToString();

            editor.Clear(board);
            editor.Generate(board, Seed);
            player.Pause(true);
            player.Reset();
        }

        private void OnClickClearButton()
        {
            editor.Clear(board);
            player.Pause(true);
        }

        private void OnClickRestartButton()
        {
            editor.Clear(board);
            editor.Generate(board, Seed);
            player.Pause(true);
            player.Reset();
        }

        private void PaintCell(bool paint)
        {
            if (player.IsPaused)
                return;

            Vector2Int location = cam.GetPointerLocation(Input.mousePosition);
            editor.EditCell(board, location.x, location.y, paint);
        }

        private void OnDisable()
        {
            generateBoardButton.onClick.RemoveListener(OnClickGenerateButton);
            clearButton.onClick.RemoveListener(OnClickClearButton);
            restartButton.onClick.RemoveListener(OnClickRestartButton);
        }
    }
}