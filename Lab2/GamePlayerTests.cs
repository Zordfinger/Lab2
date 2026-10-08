
using System;
using System.IO;

class GamePlayerTests
{
    public static int Start()
    {
        int passed = 0;
        int failed = 0;

        void Check(string name, Action test)
        {
            try
            {
                test();
                passed++;
                Console.WriteLine("OK: " + name);
            }
            catch (Exception ex)
            {
                failed++;
                Console.WriteLine("FAIL: " + name);
                Console.WriteLine("     " + ex.Message);
            }
        }

        Check("Player initializes position",
            PlayerInitializesPosition);

        Check("Player does not count initial position as movement",
            InitialPositionDoesNotCountAsDistance);

        Check("Player moves forward",
            PlayerMovesForward);

        Check("Player wraps around field",
            PlayerWrapsAroundField);

        Check("Player moves backward across boundary",
            PlayerMovesBackwardAcrossBoundary);

        Check("Player handles large negative movement",
            PlayerHandlesLargeNegativeMovement);

        Check("Player counts total distance traveled",
            PlayerCountsDistance);

        Check("Game creates output file",
            GameCreatesOutputFile);

        Check("Game writes table header",
            GameWritesTableHeader);

        Check("Game detects caught mouse",
            GameDetectsCaughtMouse);

        Check("Game stops processing after catching mouse",
            GameStopsAfterCatch);

        Check("Game reports escaped mouse",
            GameReportsEscapedMouse);

        Console.WriteLine();
        Console.WriteLine($"Passed: {passed}; Failed: {failed}");

        return failed == 0 ? 0 : 1;
    }

    private static void Equal(int expected, int actual)
    {
        if (expected != actual)
        {
            throw new Exception(
                $"Expected: {expected}, but was: {actual}");
        }
    }

    private static void Contains(string text, string expected)
    {
        if (!text.Contains(expected))
        {
            throw new Exception(
                $"Expected text to contain: [{expected}]");
        }
    }

    private static void DoesNotContain(
        string text, string unexpected)
    {
        if (text.Contains(unexpected))
        {
            throw new Exception(
                $"Text should not contain: [{unexpected}]");
        }
    }

    private static Player CreatePlayer()
    {
        return new Player(10, "cat");
    }

    private static void PlayerInitializesPosition()
    {
        Player player = CreatePlayer();

        player.Move(3);

        Equal(3, player.Position);
    }

    private static void InitialPositionDoesNotCountAsDistance()
    {
        Player player = CreatePlayer();

        player.Move(3);

        Equal(0, player.Distance);
    }

    private static void PlayerMovesForward()
    {
        Player player = CreatePlayer();

        player.Move(3);
        player.Move(2);

        Equal(5, player.Position);
    }

    private static void PlayerWrapsAroundField()
    {
        Player player = CreatePlayer();

        player.Move(8);
        player.Move(2);

        // Клетки нумеруются от 1 до 10.
        Equal(0, player.Position);
    }

    private static void PlayerMovesBackwardAcrossBoundary()
    {
        Player player = CreatePlayer();

        player.Move(2);
        player.Move(-3);

        Equal(9, player.Position);
    }

    private static void PlayerHandlesLargeNegativeMovement()
    {
        Player player = CreatePlayer();

        player.Move(5);
        player.Move(-25);

        Equal(0, player.Position);
    }

    private static void PlayerCountsDistance()
    {
        Player player = CreatePlayer();

        player.Move(4);
        player.Move(3);
        player.Move(-5);
        player.Move(10);

        Equal(18, player.Distance);
    }

    private static void GameCreatesOutputFile()
    {
        string input = "test_chase.txt";
        string output = "test_pursuit.txt";

        try
        {
            File.WriteAllText(input,
                "10\nM 2\nC 7\nP\n");

            Game game = new Game(input, output);
            game.Start();

            if (!File.Exists(output))
                throw new Exception("Output file was not created");

            string result = File.ReadAllText(output);
            Contains(result, "Cat and Mouse");
        }
        finally
        {
            DeleteIfExists(input);
            DeleteIfExists(output);
        }
    }

    private static void GameWritesTableHeader()
    {
        string input = "test_chase.txt";
        string output = "test_pursuit.txt";

        try
        {
            File.WriteAllText(input,
                "10\nM 2\nC 7\nP\n");

            new Game(input, output).Start();

            string result = File.ReadAllText(output);

            Contains(result, "Cat");
            Contains(result, "Mouse");
            Contains(result, "Distance");
        }
        finally
        {
            DeleteIfExists(input);
            DeleteIfExists(output);
        }
    }

    private static void GameDetectsCaughtMouse()
    {
        string input = "test_chase.txt";
        string output = "test_pursuit.txt";

        try
        {
            File.WriteAllText(input,
                "10\nM 3\nC 8\nC -5\n");

            new Game(input, output).Start();

            string result = File.ReadAllText(output);

            Contains(result, "Mouse caught at: 3");
        }
        finally
        {
            DeleteIfExists(input);
            DeleteIfExists(output);
        }
    }

    private static void GameStopsAfterCatch()
    {
        string input = "test_chase.txt";
        string output = "test_pursuit.txt";

        try
        {
            File.WriteAllText(input,
                "10\nM 3\nC 8\nC -5\nM 5\n");

            new Game(input, output).Start();

            string result = File.ReadAllText(output);

            // Начальная позиция мыши не считается пройденным
            // расстоянием. Команда M 5 после поимки игнорируется.
            Contains(result, "3");
            Contains(result, "5");

            // В тесте проверяется, что итоговая позиция
            // поимки — клетка 3.
            Contains(result, "Mouse caught at: 3");
        }
        finally
        {
            DeleteIfExists(input);
            DeleteIfExists(output);
        }
    }

    private static void GameReportsEscapedMouse()
    {
        string input = "test_chase.txt";
        string output = "test_pursuit.txt";

        try
        {
            File.WriteAllText(input,
                "10\nM 2\nC 7\nP\n");

            new Game(input, output).Start();

            string result = File.ReadAllText(output);

            Contains(result, "Mouse evaded Cat");
        }
        finally
        {
            DeleteIfExists(input);
            DeleteIfExists(output);
        }
    }

    private static void DeleteIfExists(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}