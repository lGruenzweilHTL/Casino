using System.Numerics;
using Casino.DataStructures;
using Casino.Systems;

namespace Casino.Games;

public class Roulette : CasinoGame {
    private RouletteBettingType _betType;
    private int? _betNumber;

    // European layout
    private static readonly int[] CurrentLayout = [
        0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18,
        29, 7, 28, 12, 35, 3, 26
    ];

    private static readonly Dictionary<int, char> UnicodeMap = new() {
        [0] = '\u24ea', [1] = '\u2460', [2] = '\u2461', [3] = '\u2462', [4] = '\u2463', [5] = '\u2464', [6] = '\u2465',
        [7] = '\u2466', [8] = '\u2467', [9] = '\u2468', [10] = '\u2469', [11] = '\u246a', [12] = '\u246b',
        [13] = '\u246c', [14] = '\u246d', [15] = '\u246e', [16] = '\u246f', [17] = '\u2470', [18] = '\u2471',
        [19] = '\u2472', [20] = '\u2473', [21] = '\u3251', [22] = '\u3252', [23] = '\u3253', [24] = '\u3254',
        [25] = '\u3255', [26] = '\u3256', [27] = '\u3257', [28] = '\u3258', [29] = '\u3259', [30] = '\u325a',
        [31] = '\u325b', [32] = '\u325c', [33] = '\u325d', [34] = '\u325e', [35] = '\u325f', [36] = '\u32b1',
    };

    protected override BigInteger PlayRound(BigInteger bet) {
        int result = Random.Shared.Next(1, 37);
        AudioManager.PlayAudio("Media\\Roulette.mp3");
        for (int i = 0; i < 30; i++) {
            DrawRouletteWheel(CurrentLayout, i);
            Thread.Sleep(100);
        }
        
        // TODO: find solution without rotating array (will also fix error with colors)
        int arrayRotation = CurrentLayout.Length - Array.IndexOf(CurrentLayout, result);
        DrawRouletteWheel(RotateArray(CurrentLayout, arrayRotation), 0);

        Console.SetCursorPosition(0, Console.WindowHeight - 1); // reset cursor
        return CalculatePayout(IsBetWinning(_betType, result, _betNumber), _betType, bet);
    }

    private static bool IsBetWinning(RouletteBettingType type, int rolled, int? betNumber) {
        return type switch {
            RouletteBettingType.Red => Array.IndexOf(CurrentLayout, rolled) % 2 == 1,
            RouletteBettingType.Black => Array.IndexOf(CurrentLayout, rolled) % 2 == 0,
            RouletteBettingType.Low => rolled is > 0 and <= 18,
            RouletteBettingType.High => rolled is > 18 and <= 36,
            RouletteBettingType.First12 => rolled is > 0 and <= 12,
            RouletteBettingType.Second12 => rolled is > 12 and <= 24,
            RouletteBettingType.Third12 => rolled is > 24 and <= 36,
            RouletteBettingType.Even => rolled % 2 == 0,
            RouletteBettingType.Odd => rolled % 2 == 1,
            RouletteBettingType.Number => rolled == betNumber,
            _ => false
        };
    }

    private static BigInteger CalculatePayout(bool won, RouletteBettingType type, BigInteger bet) {
        if (!won) return -bet;
        return bet * type switch {
            RouletteBettingType.Even or RouletteBettingType.Odd or RouletteBettingType.High
                or RouletteBettingType.Low or RouletteBettingType.Red or RouletteBettingType.Black => 1,
            RouletteBettingType.First12 or RouletteBettingType.Second12 or RouletteBettingType.Third12 => 3,
            RouletteBettingType.Number => 36,
            _ => 0
        };
    }


    protected override BigInteger ReadBet() {
        string bettingTypes = string.Join(", ", Enum.GetNames<RouletteBettingType>());
        do {
            Console.Write($"What do you want to bet on [{bettingTypes}]: ");
        } while (!Enum.TryParse(Console.ReadLine(), true, out _betType));

        if (_betType == RouletteBettingType.Number) {
            _betNumber = InputReader.ReadInputOfType<int>("Enter the number you want to bet on: ",
                predicate: i => i is >= 0 and <= 36);
        }

        return base.ReadBet();
    }

    private static void DrawRouletteWheel(IReadOnlyList<int> layout, double angleRad) {
        Console.Clear();

        const int RADIUS = 10;
        const int CENTER_X = 20;
        const int CENTER_Y = 15;
        bool red = false;
        int numPoints = layout.Count;
        double angleStep = Math.Tau / numPoints;

        for (int i = 0; i < numPoints; i++) {
            double angle = i * angleStep + angleRad;
            int x = (int)(CENTER_X + RADIUS * Math.Sin(angle) * 2);
            int y = (int)(CENTER_Y + RADIUS * Math.Cos(angle));

            Console.ForegroundColor = layout[i] == 0
                ? ConsoleColor.Green
                : red
                    ? ConsoleColor.Red
                    : ConsoleColor.DarkGray;
            Console.SetCursorPosition(x, y);
            Console.Write(UnicodeMap[layout[i]]);

            red = !red;
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(CENTER_X, CENTER_Y + RADIUS + 2);
        Console.Write('\u2191');
    }
    
    // This is my solution to LeetCode problem 189
    // There is probably a better way for the case it's used for (see usages)
    // But I'm too tired to think of a good solution
    private static int[] RotateArray(int[] nums, int k) {
        int[] result = new int[nums.Length];

        for (int i = 0; i < nums.Length; i++){
            int newIndex = (i + k) % nums.Length;
            result[newIndex] = nums[i];
        }

        return result;
    }

    protected override void PrintRules() {
        Console.WriteLine("Roulette\n=========\n");

        Console.Write("Do you want to see the rules [y/n]: ");
        if (Console.ReadLine()!.ToLower() != "y") return;

        Console.WriteLine("\n\nRoulette is a game where you bet on numbers rolled on a disk");
        Console.WriteLine("\nThere are multiple types of bets:");
        Console.WriteLine("You can bet on if the number will be even or odd");
        Console.WriteLine("Or you can bet if it's high (19-36) or low (1-18)");
        Console.WriteLine("You can also bet on thirds (1-12), (13-24), (25-36)");
        Console.WriteLine("The riskiest option is to bet on a single number. " +
                          "This will give you the most payout if you get it right");

        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}