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
            string inp = "";
            int bet;
            do {
                Console.Write("Place your bet: ");

                inp = Console.ReadLine()!;
            } while (!int.TryParse(inp, out bet) && inp.ToLower().Trim() != "all in");

            bet = Math.Abs(bet);
            if (inp.ToLower() == "all in") bet = Math.Abs(moneyWon);

            int won = PlayRound(bet);
            moneyWon += won;

            Console.WriteLine($"\n\nYou have {(won < 0 ? "lost" : "won")} {Math.Abs(won)}🪙.");
            Console.WriteLine($"Your total is {moneyWon}🪙");
            Console.Write("\n\nDo you want to continue playing [y/n]: ");
            continuationKey = Console.ReadLine()!.ToLower();
        } while (continuationKey == "y");

        return moneyWon;
    }

    protected abstract int PlayRound(int bet);
    protected virtual void PrintRules() { }
}