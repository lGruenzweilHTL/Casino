namespace Casino.Games;

public class SlotMachine : CasinoGame {
    private const int NUMBER_SLOTS = 3;
    private const int NUMBER_ITEMS = 10;
    protected override int PlayRound(int bet) {
        int[] result = new int[NUMBER_SLOTS];
        
        for (int i = 0; i < result.Length; i++) {
            result[i] = Random.Shared.Next(0, NUMBER_ITEMS);
        }
        
        Console.Write("Press any key to start...");
        Console.ReadKey(true);
        
        PrintMachine();
        RenderResult(result);

        return (int)(bet * CalculateMoneyMultiplier(result));
    }

    private static void RenderResult(int[] res) {
        // Print dummy-values to make it more engaging
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < res.Length; j++) {
                // Add an artificial delay between items to make it look more natural
                Thread.Sleep(50);

                WriteSlotPosition(j, Random.Shared.Next(0, NUMBER_ITEMS));
            }
            Thread.Sleep(100);
        }
        
        // Print actual values
        for (int i = 0; i < res.Length; i++) {
            Thread.Sleep(50);
            WriteSlotPosition(i, res[i]);
        }
    }

    private static void WriteSlotPosition(int position, int item) {
        Console.SetCursorPosition(50 + position * 5, 5);
        Console.Write(SlotToIcon(item));
    }

    private static double CalculateMoneyMultiplier(int[] items) {
        int same = (from i in items
            let count = items.Count(n => n == i)
            select count).Max();

        return same switch {
            3 => 3,
            2 => 1,
            _ => -1
        };
    }

    private static void PrintMachine() {
        Console.SetCursorPosition(47, 3);
        Console.Write(new string('\u2588', 17));
        Console.SetCursorPosition(47, 7);
        Console.Write(new string('\u2588', 17));

        for (int i = 3; i <= 7; i++) {
            Console.SetCursorPosition(47, i);
            Console.Write('\u2588');
            
            Console.SetCursorPosition(64, i);
            Console.Write('\u2588');
        }
    }

    private static char SlotToIcon(int slotItem) => slotItem switch {
        0 => '߷',
        1 => '៙',
        2 => 'ᨖ',
        3 => '⁜',
        4 => '\u2188',
        5 => '\u2343',
        6 => '\u2344',
        7 => '\u2646',
        8 => 'ⵘ',
        9 => '\u00ae',
        _ => '❌' // Invalid
    };

    protected override void PrintRules() {
        Console.WriteLine("Slot-Machine\n=========\n");

        Console.Write("Do you want to see the rules [y/n]: ");
        if (Console.ReadLine()!.ToLower() != "y") return;

        Console.WriteLine("\n\nWhen you start the machine, the values are rolled");
        Console.WriteLine("\nIf none of your values are the same, you lose your bet");
        Console.WriteLine("\nIf two are the same, you win your bet");
        Console.WriteLine("\nIf all 3 values are the same, you win TRIPLE you bet");
        
        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}