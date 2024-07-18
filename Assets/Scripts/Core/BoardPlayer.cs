using static Core.Events;

namespace Core
{
    public interface IBoardPlayerHandler
    {
        public bool IsPaused { get; }
        public void Pause();
        public void Pause(bool pause);
        public void Reset();
        public void SetSpeed(float speed);
    }

    public interface IBoardPlayer
    {
        public event Pause OnPause;

        public float Speed { get; }
        public bool IsPaused { get; }

        public void Update(IBoard board, float deltaTime);
    }

    public class BoardPlayer : IBoardPlayer, IBoardPlayerHandler
    {
        public event Pause OnPause;
        public bool IsPaused { get; protected set; } = true;
        public float Speed { get; protected set; } = 100f;
        public void SetSpeed(float speed) { Speed = speed; }

        private float refreshTime = 0f;
        private int step = 0;

        public void Update(IBoard board, float deltaTime)
        {
            refreshTime -= IsPaused ? 0f : deltaTime;

            if (IsPaused || refreshTime > Speed)
                return ;

            refreshTime = 1f;
            step++;
            board.PlayStep(step);
        }

        public void Pause()
        {
            IsPaused = !IsPaused;
            OnPause?.Invoke(IsPaused);
        }

        public void Pause(bool pause)
        {
            if (pause != IsPaused)
                Pause();
        }

        public void Reset()
        {
            step = 0;
        }
    }
}