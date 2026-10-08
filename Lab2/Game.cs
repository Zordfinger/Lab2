using System.Text;
using System.Text.RegularExpressions;

class Game
{
    string CommandFile;
    string ResultFile;
    int GameFieldSize;


    public Game(string CommandFile, string ResultFile)
    {
        this.CommandFile = CommandFile;
        this.ResultFile = ResultFile;
    }

    private List<string[]> ReadCommandFile()
    {
        List<string[]> commands = new List<string[]>();
        try
        {
            using (StreamReader reader = new StreamReader(CommandFile))
            { 
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] command = Regex.Split(line.Trim(), "\\s+");
                    commands.Add(command);
                }
            }
        } catch (Exception ex) {
            Console.WriteLine("ERROR:" + ex.Message);
        }
        return commands;
    }

    public void Start()
    {
        StringBuilder sb = new StringBuilder();
        List<string[]> commands = ReadCommandFile();
        string Separator = "-------------------";

        string[] FirstCommand = commands[0];
        GameFieldSize = int.Parse(FirstCommand[0]);

        Mouse Mouse = new(GameFieldSize, "mouse");
        Cat Cat = new(GameFieldSize, "cat");

        sb.AppendLine("Cat and Mouse");
        sb.AppendLine("");
        sb.AppendLine($"{"Cat",4} {"Mouse",7} {"Distance", 10}");
        sb.AppendLine(Separator);

        foreach (string[] command in commands.Skip(1))
        {

            switch (command[0]) {
                case "M": 
                {
                    int steps = int.Parse(command[1]);
                    Mouse.Move(steps);
                };
                break;

                case "C": 
                {
                    int steps = int.Parse(command[1]);
                    Cat.Move(steps);
                };
                break;

                case "P": 
                {
                        string mousepos = Mouse.State == PlayerState.UnInicialized ? "??" : Mouse.Position.ToString();
                        string catpos = Cat.State == PlayerState.UnInicialized ? "??" : Cat.Position.ToString();
                        int? dist = DistanceBP(Mouse, Cat);
                        if(dist == null)
                            sb.AppendLine($"{catpos,4}{mousepos,7}");
                        else
                            sb.AppendLine($"{catpos,4}{mousepos,7}{dist, 10}");
                };
                break;

                default: continue;
            }

            if (DistanceBP(Mouse, Cat) == 0)
            {
                Cat.State = PlayerState.Winner;
                Mouse.State = PlayerState.Loser;
                break;
            }
        }

        sb.AppendLine(Separator);
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine($"{"Distance traveled", -17} {"Cat", -5} {"Mouse", -5}");
        sb.AppendLine($"{"",17} {Cat.Distance,-5} {Mouse.Distance,-5}");
        sb.AppendLine();
        sb.AppendLine(
            Cat.State == PlayerState.Winner ? $"Mouse caught at: {Cat.Position}" : "Mouse evaded Cat"
            );

        try
        {
            using (StreamWriter writer = new StreamWriter(ResultFile))
            {
                writer.Write(sb.ToString());
            }
        }
        catch (Exception ex) {
            Console.WriteLine("ERROR:" + ex.Message);
        }
    }

    private int? DistanceBP(Player p1, Player p2)
    { 
        if(p1.State == PlayerState.UnInicialized || p2.State == PlayerState.UnInicialized)
            return null;
        int dist = Math.Abs(p1.Position - p2.Position);
        return Math.Min(dist, GameFieldSize - dist);
    }
}
