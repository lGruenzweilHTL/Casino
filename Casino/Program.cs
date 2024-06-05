using System.Numerics;
using System.Text;
using Casino.DataStructures;
using Casino.Games;
using Casino.Systems;

namespace Casino;

internal static class Program {
    private static int MenuOffsetY => Console.WindowHeight / Enum.GetNames<MenuLocation>().Length;
    private const int MENU_SPACING = 3;
    private static int ScoreboardPositionY => Console.WindowHeight - SCOREBOARD_NUM_PLAYERS - 4;
    private const int SCOREBOARD_NUM_PLAYERS = 5;
    private const int SCOREBOARD_SPACING = 1;
    private const int SCOREBOARD_POSITION_X = 1;
    private const int SCORES_POSITION_X = SCOREBOARD_POSITION_X + 30;

    public static BigInteger MoneyWon { get; private set; }
    private static string _username = "";

    private static int Main() {
        Console.OutputEncoding = Encoding.Unicode;

        // Recover last session
        MoneyWon = TitleScreen();

        AudioManager.PlayAudio("Media\\casino-intro.mp3");
        MenuLocation menuLocation = 0;

        while (true) {
            Console.CursorVisible = false;

            Utils.ClearConsoleBuffer();

            DrawBorder();
            do {
                DrawMenu(menuLocation, MoneyWon);
            } while (InteractWithMenu(ref menuLocation));

            if (menuLocation == MenuLocation.Quit) {
                List<string> scores = File.ReadAllLines("Scores.csv").ToList();
                scores.RemoveAll(p => p.Split(';')[0] == _username);
                
                File.WriteAllLines("Scores.csv", scores.Append($"{_username};{MoneyWon}"));
                return 0;
            }

            MoneyWon = menuLocation switch {
                MenuLocation.Blackjack => new Blackjack().Play(),
                MenuLocation.Kings => new Kings().Play(),
                MenuLocation.SlotMachine => new SlotMachine().Play(),
                MenuLocation.Roulette => new Roulette().Play(),
                _ => MoneyWon
            };
        }
    }

    // Displays title screen. Returns: current money (1000 as default)
    private static BigInteger TitleScreen() {
        // Write logo
        Console.WriteLine(File.ReadAllText("Logo.txt"));
        Console.Write("\n\n\nEnter username: ");

        string name = Console.ReadLine() ?? $"User {Random.Shared.Next(0, 10000)}";
        _username = name;
        string[][] players = File.ReadAllLines("Scores.csv")
            .Select(s => s.Split(';'))
            .ToArray();
        foreach (string[] data in players) {
            if (name == data[0]) {
                return BigInteger.Parse(data[1]);
            }
        }

        return 1000; // Starting money
    }

    private static void DrawBorder() {
        // Top line
        Console.SetCursorPosition(0, 0);
        Console.Write('\u250f' + new string('\u2501', Console.WindowWidth - 2) + '\u2513');

        // Bottom line
        Console.SetCursorPosition(0, Console.WindowHeight - 2);
        Console.Write('\u2517' + new string('\u2501', Console.WindowWidth - 2) + '\u251b');

        for (int i = 2; i < Console.WindowHeight - 1; i++) {
            // Left line
            Console.SetCursorPosition(0, Console.WindowHeight - i - 1);
            Console.Write('\u2503');

            // Right line
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - i - 1);
            Console.Write('\u2503');
        }
    }

    private static void DrawMenu(MenuLocation location, BigInteger moneyWon) {
        string[] options = Enum.GetNames<MenuLocation>();

        for (int i = 0; i < options.Length; i++) {
            string styled = StyleMenuLocation((MenuLocation)i) ?? options[i];

            Console.SetCursorPosition((Console.WindowWidth - styled.Length) / 2, MenuOffsetY + i * MENU_SPACING);
            if (location.ToString() == options[i]) Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(styled);
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.SetCursorPosition(1, 1);
        Console.Write($"Total won: {moneyWon.AbbreviateIf(moneyWon.GetBitLength() > 512)}🪙");
        DrawScoreboard(SCOREBOARD_NUM_PLAYERS);
    }

    private static bool InteractWithMenu(ref MenuLocation location) {
        ConsoleKey key = Console.ReadKey(true).Key;

        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (key) {
            case ConsoleKey.UpArrow:
                location--;
                break;
            case ConsoleKey.DownArrow:
                location++;
                break;
            case ConsoleKey.Enter:
                return false;
        }

        int length = Enum.GetNames<MenuLocation>().Length;
        location = (MenuLocation)(((int)location + length) % length);

        return true;
    }

    private static string? StyleMenuLocation(MenuLocation location) => location switch {
        MenuLocation.SlotMachine => "Slot-Machine",
        MenuLocation.Quit => "Save and Quit",
        _ => null
    };

    private static void DrawScoreboard(int maxNumPlayers) {
        const string FILENAME = "Scores.csv";
        if (!File.Exists(FILENAME)) return;
        string[][] scores = File.ReadAllLines(FILENAME)
            .Select(s => s.Split(';'))
            .OrderBy(s => BigInteger.Parse(s[^1]))
            .Reverse()
            .ToArray();

        // Header
        Console.SetCursorPosition(SCOREBOARD_POSITION_X, ScoreboardPositionY);
        Console.Write("Player");
        Console.SetCursorPosition(SCORES_POSITION_X, ScoreboardPositionY);
        Console.Write("Scores");

        // Contents
        for (int i = 0; i < scores.Length && i < maxNumPlayers; i++) {
            Console.SetCursorPosition(SCOREBOARD_POSITION_X, ScoreboardPositionY + (i + 1) * SCOREBOARD_SPACING);
            Console.Write(scores[i][0]);
            Console.SetCursorPosition(SCORES_POSITION_X, ScoreboardPositionY + (i + 1) * SCOREBOARD_SPACING); 
            Console.Write(BigInteger.Parse(scores[i][1]).AbbreviateIf(scores[i][1].Length >= 15));
        }
    }
}