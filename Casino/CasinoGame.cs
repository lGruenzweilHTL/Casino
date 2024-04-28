namespace Casino;

public abstract class CasinoGame {
    public virtual int Play() {
        Console.CursorVisible = true;
        int moneyWon = Program.MoneyWon;
        Console.Clear();

        PrintRules();

        string continuationKey;
        do {
            Console.Clear();
            int bet;
            do {
                Console.Write("Place your bet: ");
            } while (!int.TryParse(Console.ReadLine()!, out bet));

            int won = PlayRound(bet);
            moneyWon += won;

            Console.WriteLine($"\n\nYou have {(won < 0 ? "lost" : "won")} {Math.Abs(won)}€.");
            Console.WriteLine($"Your total is {moneyWon}€");
            Console.Write("\n\nDo you want to continue playing [y/n]: ");
            continuationKey = Console.ReadLine()!.ToLower();
        } while (continuationKey == "y");

        return moneyWon;
    }

    protected abstract int PlayRound(int bet);
    protected virtual void PrintRules() { }
}