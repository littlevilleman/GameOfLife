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
        [SerializeField] private Button restartButton;
        [SerializeField] private Slider speedSlider;

        private IBoardPlayerHandler player;
        private IBoardViewport viewport;

        private Action onRestart;

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnClickPlayButton);
            zoomInButton.onClick.AddListener(() => OnClickZoomButton(true));
            zoomOutButton.onClick.AddListener(() => OnClickZoomButton(false));
            speedSlider.onValueChanged.AddListener(OnSpeedValueChanged);
            restartButton.onClick.AddListener(OnClickRestartButton);
        }

        public void Display(object[] parameters, Action restartCallback)
        {
            player = parameters[2] as IBoardPlayerHandler;
            viewport = parameters[3] as IBoardViewport;
            speedSlider.value = (parameters[2] as IBoardPlayer).Speed;
            onRestart = restartCallback;
        }

        private void OnClickZoomButton(bool zoomIn)
        {
            viewport.Zoom(zoomIn);
        }

        private void OnClickPlayButton()
        {
            player.Pause();
        }

        private void OnSpeedValueChanged(float speed)
        {
            player.SetSpeed(speed);
        }

        private void OnClickRestartButton()
        {
            player.Pause(true);
            player.Reset();
            onRestart?.Invoke();
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnClickPlayButton);
            zoomInButton.onClick.RemoveListener(() => OnClickZoomButton(true));
            zoomOutButton.onClick.RemoveListener(() => OnClickZoomButton(false));
            speedSlider.onValueChanged.RemoveListener(OnSpeedValueChanged);
            restartButton.onClick.RemoveListener(OnClickRestartButton);
        }
    }
}