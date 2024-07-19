using UnityEngine;
using Core;
using Config;

namespace Client
{
    public class ClientManager : MonoBehaviour
    {
        [SerializeField] private CameraBehavior cam;
        [SerializeField] private BoardBehavior boardBhv;
        [SerializeField] private BoardScreen boardScreen;
        [SerializeField] private BoardConfig config;

        public void Awake()
        {
            //Application.targetFrameRate = 60;
        }

        public void Start()
        {
            IBoard board = new Board();
            IBoardEditor editor = new BoardEditor();
            IBoardPlayer player = new BoardPlayer();
            IBoardViewport viewport = new BoardViewport(config);

            boardBhv.Display(board, player, viewport, editor, config);
            boardScreen.Display(editor, board, player, viewport);
            cam.Display(viewport, boardBhv);
        }
    }
}