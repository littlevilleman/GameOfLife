using Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class BoardEditorWidget : MonoBehaviour
    {
        [SerializeField] private CameraBehavior cam;
        [SerializeField] private Button clearButton;
        [SerializeField] private Button generateBoardButton;
        [SerializeField] private TMP_Text seedInput;

        private IBoardEditor editor;
        private IEditableCellMap board;
        private IBoardViewport viewport;
        private IBoardPlayerHandler player;

        private int Seed => int.Parse(seedInput.text);

        private void OnEnable()
        {
            generateBoardButton.onClick.AddListener(OnClickGenerateButton);
            clearButton.onClick.AddListener(OnClickClearButton);
        }

        public void Display(object[] parameters)
        {
            editor = parameters[0] as IBoardEditor;
            board = parameters[1] as IEditableCellMap;
            player = parameters[2] as IBoardPlayerHandler;
            viewport = parameters[3] as IBoardViewport;
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
                EditCell(true);

            if (Input.GetMouseButton(1))
                EditCell(false);
        }

        public void Regenerate()
        {
            editor.Clear(board);
            editor.Generate(board, Seed);
        }

        private void OnClickGenerateButton()
        {
            seedInput.text = Random.Range(-999999999, 999999999).ToString();

            Regenerate();
            player.Pause(true);
            player.Reset();
        }

        private void OnClickClearButton()
        {
            editor.Clear(board);
            player.Pause(true);
        }

        private void EditCell(bool paint)
        {
            if (!player.IsPaused || EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2Int location = Cell.GetLocation(cam.GetPointerPosition(Input.mousePosition));
            Physics.Raycast(cam.GetPointerPosition(Input.mousePosition) , Vector3.forward, out RaycastHit hit, 50);

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Board"))
                editor.EditCell(board, location.x, location.y, paint);
        }

        private void OnDisable()
        {
            generateBoardButton.onClick.RemoveListener(OnClickGenerateButton);
            clearButton.onClick.RemoveListener(OnClickClearButton);
        }
    }
}