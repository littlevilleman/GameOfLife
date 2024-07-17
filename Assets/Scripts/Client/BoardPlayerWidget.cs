using Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BoardPlayerWidget : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button zoomInButton;
        [SerializeField] private Button zoomOutButton;
        [SerializeField] private Slider speedSlider;

        [SerializeField] private Image playButtonImage;
        [SerializeField] private Sprite playIcon;
        [SerializeField] private Sprite pauseIcon;

        private IBoardPlayer player;

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnClickPlayButton);
            zoomInButton.onClick.AddListener(() => OnClickZoomButton(true));
            zoomOutButton.onClick.AddListener(() => OnClickZoomButton(false));
            speedSlider.onValueChanged.AddListener(OnSpeedValueChanged);
        }

        public void Display(object[] parameters)
        {
            player = parameters[2] as IBoardPlayer;
            player.OnPause += OnPause;

            speedSlider.value = player.Speed;
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
            playButtonImage.sprite = isPaused ? playIcon : pauseIcon;
        }

        private void OnSpeedValueChanged(float speed)
        {
            player.SetSpeed(speed);
        }

        private void OnDisable()
        {
            player.OnPause -= OnPause;

            playButton.onClick.RemoveListener(OnClickPlayButton);
            zoomInButton.onClick.RemoveListener(() => OnClickZoomButton(true));
            zoomOutButton.onClick.RemoveListener(() => OnClickZoomButton(false));
            speedSlider.onValueChanged.RemoveListener(OnSpeedValueChanged);
        }
    }
}