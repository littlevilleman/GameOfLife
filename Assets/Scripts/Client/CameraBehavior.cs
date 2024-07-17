using Core;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Client
{
    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField] private PixelPerfectCamera ppCam;
        private IBoardPlayer Player;

        public void Setup(IBoardPlayer player)
        {
            Player = player;
            Player.OnZoom += Zoom;

            Zoom(player.ZoomFactor);
        }

        void LateUpdate()
        {
            if (Input.GetKey(KeyCode.D))
                Move(Vector3.right);
            
            if (Input.GetKey(KeyCode.A))
                Move(Vector3.left);
            
            if (Input.GetKey(KeyCode.W))
                Move(Vector3.up);

            if (Input.GetKey(KeyCode.S))
                Move(Vector3.down);
        }

        private void Move(Vector3 direction)
        {
            Vector2Int location = Cell.GetLocation(transform.position + Time.deltaTime * direction * Player.Resolution.x / 8);
            transform.position = new Vector3(location.x, location.y);
        }

        public void Zoom(int zoomFactor)
        {
            ppCam.refResolutionX = Player.Resolution.x / zoomFactor;
            ppCam.refResolutionY = Player.Resolution.y / zoomFactor;
        }
    }

}