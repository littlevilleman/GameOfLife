using Core;
using UnityEngine;

namespace Client
{
    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        private IBoardViewport viewport;
        private IBoardLocator target;

        public void Display(IBoardViewport viewportSetup, IBoardLocator targetSetup)
        {
            viewport = viewportSetup;
            target = targetSetup;
            viewport.OnZoom += OnZoom;
            viewport.OnMove += OnMove;

            OnZoom(viewportSetup.ZoomFactor);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.D))
                viewport.Move(Vector2Int.right, Time.deltaTime);
            
            if (Input.GetKey(KeyCode.A))
                viewport.Move(Vector2Int.left, Time.deltaTime);
            
            if (Input.GetKey(KeyCode.W))
                viewport.Move(Vector2Int.up, Time.deltaTime);

            if (Input.GetKey(KeyCode.S))
                viewport.Move(Vector2Int.down, Time.deltaTime);
        }

        private void OnZoom(int zoomFactor)
        {
            target.ResetTexture(viewport);
        }

        private void OnMove(Vector2Int source, Vector2Int destiny)
        {
            target.Refresh(source);
            transform.position = new Vector3(viewport.Location.x, viewport.Location.y);
        }

        public Vector3 GetPointerPosition(Vector3 mousePosition)
        {
            return cam.ScreenToWorldPoint(mousePosition) / viewport.ZoomFactor;
        }
    }
}