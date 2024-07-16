namespace Core
{
    public static class Events
    {
        public delegate void StepOn(int step);
        public delegate void RefreshBoard();
        public delegate void RefreshCell(int x, int y, bool alive);

        public delegate void Pause(bool pause = true);
        public delegate void Zoom(int zoomFactro = 1);
    }

}