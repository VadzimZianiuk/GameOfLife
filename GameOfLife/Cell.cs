namespace GameOfLife
{
    class Cell
    {
        public int Age { get; set; }
        public bool IsAlive { get; set; }
        public byte CountNeighbors { get; set; }

        public void Clear()
        {
            Age = 0;
            IsAlive = false;
            CountNeighbors = 0;
        }
    }
}