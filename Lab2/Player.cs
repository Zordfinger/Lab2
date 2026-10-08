public abstract class Player
{
    public enum PlayerType
    {
        Cat,
        Mouse,
        Unknown
    }

    public PlayerType Type { get; private set; }
    public PlayerState State = PlayerState.UnInicialized;
    public int Position { get; private set; } = -1;
    public int Distance { get; private set; } = 0;
    private int GameFieldSize;

    public Player(int gameFieldSize, string type)
    {
        GameFieldSize = gameFieldSize;
        switch (type)
        {
            case    "cat": Type = PlayerType.Cat; break;
            case "mouse": Type = PlayerType.Mouse; break;
            default: break;
        }
    }

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
