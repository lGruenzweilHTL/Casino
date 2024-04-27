using System.Text;
using Casino.DataStructures;
using Casino.Games;

namespace Casino;

internal static class Program {
    private static void Main() {
        Console.OutputEncoding = Encoding.Unicode;
        
        MenuLocation currentLocation = 0;
        var moneyWon = 0;

        while (true) {
            Console.CursorVisible = false;
            Console.Clear();
            DrawBorder();
            do {
                DrawMenu(currentLocation, moneyWon);
            } while (InteractWithMenu(ref currentLocation));
            
            if (currentLocation == MenuLocation.Quit) return;

            moneyWon += currentLocation switch {
                MenuLocation.Blackjack => Blackjack.Play(),
                MenuLocation.Kings => Kings.Play(),
                _ => 0
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
        var options = Enum.GetNames<MenuLocation>();

        for (int i = 0; i < options.Length; i++) {
            Console.SetCursorPosition((Console.WindowWidth - options[i].Length) / 2, 10 + i * 3);
            if (location.ToString() == options[i]) Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(options[i]);
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        Console.SetCursorPosition(1, 1);
        Console.Write($"Total won: {moneyWon}€");
    }

    private static bool InteractWithMenu(ref MenuLocation location) {
        var key = Console.ReadKey(true).Key;

        switch (key) {
            case ConsoleKey.UpArrow:
                location--;
                if ((int)location == -1) location = (MenuLocation)Enum.GetNames<MenuLocation>().Length - 1;
                break;
            case ConsoleKey.DownArrow:
                location = (MenuLocation)(((int)location + 1) % Enum.GetNames<MenuLocation>().Length);
                break;
            case ConsoleKey.Enter:
                return false;
        }

        return true;
    }
}