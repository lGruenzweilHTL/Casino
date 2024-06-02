using System.Text;
using Casino.DataStructures;
using Casino.Games;

namespace Casino;

internal static class Program {
    private const int MENU_OFFSET_Y = 8;
    private const int MENU_SPACING = 3;
    
    public static int MoneyWon { get; private set; }

    private static int Main() {
        Console.OutputEncoding = Encoding.Unicode;

        // Recover last session
        { if (FileSaver.TryReadNumber(out int newMoney)) MoneyWon = newMoney; }
        
        //AudioManager.PlayAudio("Media\\casino-intro.mp3");
        MenuLocation menuLocation = 0;

        while (true) {
            Console.CursorVisible = false;

            Utils.ClearConsoleBuffer();

            DrawBorder();
            do {
                DrawMenu(menuLocation, MoneyWon);
            } while (InteractWithMenu(ref menuLocation));

            if (menuLocation == MenuLocation.Quit) {
                FileSaver.SaveNumber(MoneyWon);
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

    private static void DrawMenu(MenuLocation location, int moneyWon) {
        string[] options = Enum.GetNames<MenuLocation>();

        for (int i = 0; i < options.Length; i++) {
            string styled = StyleMenuLocation((MenuLocation)i) ?? options[i];

            Console.SetCursorPosition((Console.WindowWidth - styled.Length) / 2, MENU_OFFSET_Y + i * MENU_SPACING);
            if (location.ToString() == options[i]) Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(styled);
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.SetCursorPosition(1, 1);
        Console.Write($"Total won: {moneyWon}🪙");
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
}