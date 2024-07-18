using Core;
using UnityEngine;

namespace Client
{
    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        private IBoardPlayer player;
        private IBoardViewport viewport;
        private IBoardLocator target;

        public void Display(IBoardPlayer playerSetup, IBoardViewport viewportSetup, IBoardLocator targetSetup)
        {
            player = playerSetup;
            viewport = viewportSetup;
            target = targetSetup;
            viewport.OnZoom += OnZoom;
            viewport.OnMove += OnMove;

            OnZoom(viewportSetup.ZoomFactor);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.D))
                viewport.Move(Vector2Int.right, Time.deltaTime, player.Speed);
            
            if (Input.GetKey(KeyCode.A))
                viewport.Move(Vector2Int.left, Time.deltaTime, player.Speed);
            
            if (Input.GetKey(KeyCode.W))
                viewport.Move(Vector2Int.up, Time.deltaTime, player.Speed);

            if (Input.GetKey(KeyCode.S))
                viewport.Move(Vector2Int.down, Time.deltaTime, player.Speed);
        }

        private void OnZoom(int zoomFactor)
        {
            target.ResetTexture(viewport);
        }

        private void OnMove(Vector2Int source, Vector2Int destiny)
        {
            target.Refresh(source);
            transform.position = new Vector3(destiny.x, destiny.y);
        }

        public Vector2Int GetPointerLocation(Vector3 mousePosition)
        {
            return Cell.GetLocation(cam.ScreenToWorldPoint(mousePosition));
        }
    }

}