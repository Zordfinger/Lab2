public class Programm()
{
    static void Main(string[] args)
    {
        string path = "C:\\C#\\Lab2\\Lab2\\";

        for (int i = 1; i <= 3; i++)
        {
            Game game = new Game($"{path}{i}.ChaseData.txt", $"{path}{i}.PursuitLog.txt");
            game.Start();
        }
    }
}
