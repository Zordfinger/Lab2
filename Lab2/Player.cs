public abstract class Player(int GameFieldSize)
{
    public PlayerState State = PlayerState.UnInicialized;
    public int Position { get; private set; } = -1;
    public int Distance { get; private set; } = 0;


    public void Move(int steps)
    {
        int posbysteps = (Position + steps) % (GameFieldSize);

        if (State == PlayerState.UnInicialized)
        {
            posbysteps = (0 + steps) % (GameFieldSize);
            State = PlayerState.InGame;
            Position = posbysteps < 0 ? posbysteps + GameFieldSize : posbysteps;
            return;
        }

        Position = posbysteps < 0 ? posbysteps + GameFieldSize : posbysteps;
        Distance += Math.Abs(steps);
    }
}
