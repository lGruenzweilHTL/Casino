namespace Casino.DataStructures;

public abstract class CasinoGame {
    public int Play() {
        Console.CursorVisible = true;
        int moneyWon = Program.MoneyWon;
        Utils.ClearConsoleBuffer();

        PrintRules();

        string continuationKey;
        do {
            Utils.ClearConsoleBuffer();

            int won = PlayRound(ReadBet(moneyWon));
            moneyWon += won;

            PrintPayout(moneyWon, won);
            continuationKey = Console.ReadLine()!.ToLower();
        } while (continuationKey == "y");

        return moneyWon;
    }

    protected virtual void PrintPayout(int total, int won) {
        Console.WriteLine($"\n\nYou have {(won < 0 ? "lost" : "won")} {Math.Abs(won)}🪙.");
        Console.WriteLine($"Your total is {total}🪙");
        Console.Write("\n\nDo you want to continue playing [y/n]: ");
    }

    protected virtual int ReadBet(int allInValue) {
        string inp = "";
        int bet;
        do {
            Console.Write("Place your bet: ");

            inp = Console.ReadLine()!;
        } while (!int.TryParse(inp, out bet) && inp.ToLower().Trim() != "all in");

        bet = Math.Abs(bet);
        if (inp.ToLower() == "all in") bet = Math.Abs(allInValue);

        return bet;
    }

    protected abstract int PlayRound(int bet);
    protected virtual void PrintRules() { }
}