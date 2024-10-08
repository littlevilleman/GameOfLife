using Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BoardScreen : MonoBehaviour
    {
        [SerializeField] private BoardEditorWidget editorWidget;
        [SerializeField] private BoardPlayerWidget playerWidget;

        [SerializeField] private TMP_Text stepsText;
        [SerializeField] private TMP_Text cellsText;

        [SerializeField] private Image playButtonImage;
        [SerializeField] private Sprite playIcon;
        [SerializeField] private Sprite pauseIcon;

        private IBoardPlayer player;
        private IBoard board;
        private IBoardViewport viewport;

        public void Display(params object[] parameters)
        {
            player= parameters[2] as IBoardPlayer;
            board = parameters[1] as IBoard;
            viewport = parameters[3] as IBoardViewport;

            board.OnStepOn += OnStepOn;
            player.OnPause += OnPause;

            editorWidget.Display(parameters);
            playerWidget.Display(parameters, editorWidget.Regenerate);
        }
        private void OnStepOn(int step, ICollection<Vector2Int> cells)
        {
            stepsText.text = $"Step: {step}";
            cellsText.text = $"Cells: {cells.Count}";
        }

        private void OnPause(bool isPaused)
        {
            playButtonImage.sprite = isPaused ? playIcon : pauseIcon;
        }
    }
}