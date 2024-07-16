using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BoardPlayerWidget : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button zoomInButton;
        [SerializeField] private Button zoomOutButton;
        [SerializeField] private TMP_Text playButtonText;

        private IBoardPlayer player;

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnClickPlayButton);
            zoomInButton.onClick.AddListener(() => OnClickZoomButton(true));
            zoomOutButton.onClick.AddListener(() => OnClickZoomButton(false));
        }

        public void Display(object[] parameters)
        {
            player = parameters[2] as IBoardPlayer;
            player.OnPause += OnPause;
        }

        private void OnClickZoomButton(bool zoomIn)
        {
            player.Zoom(zoomIn);
        }

        private void OnClickPlayButton()
        {
            player.Pause();
        }

        private void OnPause(bool isPaused)
        {
            playButtonText.text = isPaused ? "Play" : "Pause";
        }

        private void OnDisable()
        {
            player.OnPause -= OnPause;

            playButton.onClick.RemoveListener(OnClickPlayButton);
            zoomInButton.onClick.RemoveListener(() => OnClickZoomButton(true));
            zoomOutButton.onClick.RemoveListener(() => OnClickZoomButton(false));
        }
    }
}