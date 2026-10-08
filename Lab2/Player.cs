class Player(int GameFieldSize)
{
    private PlayerState State = PlayerState.UnInicialized;
    private int Position = -1;
    private int Distance = 0;

    void Move(int steps)
    {
        int posbysteps = Position + steps % GameFieldSize;
        if (State == PlayerState.UnInicialized)
        {
            Position++;
            posbysteps = Position + steps % GameFieldSize;
            State = PlayerState.InGame;
            Position = posbysteps < 0 ? posbysteps + GameFieldSize : posbysteps;
            return;
        }

        Position = posbysteps < 0 ? posbysteps + GameFieldSize : posbysteps;
        Distance += Math.Abs(steps);
    }
}
