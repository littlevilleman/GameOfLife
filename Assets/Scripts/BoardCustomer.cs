using static Core.Events;

namespace Core
{
    public interface IBoardCustomer
    {
        public event RefreshBoard OnRefresh;
        public event RefreshCell OnRefreshCell;

        public void Generate(ICustomizableBoard board, int seed);
        public void PaintCell(ICustomizableBoard board, int x, int y, bool alive);
        public void Clear(ICustomizableBoard board);
    }

    public class BoardCustomer : IBoardCustomer
    {
        public event RefreshBoard OnRefresh;
        public event RefreshCell OnRefreshCell;

        private System.Random random = new System.Random();

        public void Generate(ICustomizableBoard board, int seed)
        {
            random = new System.Random(seed);
            int aliveCount = random.Next(0, 250*250);

            for (int i = 0; i < aliveCount; i++)
                board.SetCell(random.Next(0, board.Size.x), random.Next(0, board.Size.y), true);

            OnRefresh?.Invoke();
        }

        public void PaintCell(ICustomizableBoard board, int x, int y, bool alive)
        {
            board.SetCell(x, y, alive);
            OnRefreshCell?.Invoke(x, y);
        }

        public void Clear(ICustomizableBoard board)
        {
            board.Clear();
            OnRefresh?.Invoke();
        }
    }
}