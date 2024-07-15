using Core;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using static Core.Events;

namespace Client
{
    public interface ICameraHandler
    {
        public event Zoom OnZoom;
        public void Zoom(bool zoomIn);
    }

    public class CameraHandler : ICameraHandler
    {
        public event Zoom OnZoom;

        public void Zoom(bool zoomIn)
        {
            OnZoom?.Invoke(zoomIn);
        }
    }

    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField] private PixelPerfectCamera ppCam;

        public int ZoomFactor { get; private set; } = 2;

        public void Setup(IBoardPlayer player)
        {
            player.OnZoom += Zoom;
            ppCam.assetsPPU = ZoomFactor;
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
            transform.position += Time.deltaTime * direction * 100;
        }

        public void Zoom(bool zoomIn)
        {
            ZoomFactor = Mathf.Clamp(Mathf.RoundToInt(ZoomFactor * (zoomIn ? 2f : 1 / 2f)), 1, 16);
            ppCam.assetsPPU = ZoomFactor;
        }
    }

}