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

            int won = PlayRound(ReadBet());
            moneyWon += won;

            PrintPayout(moneyWon, won);
            continuationKey = Console.ReadLine()!.ToLower();
        } while (continuationKey == "y");

        return moneyWon;
    }

    protected virtual void PrintPayout(int total, int won) {
        Thread sound;
        if (won < 0) {
            sound = AudioManager.PlayAudio("Media\\fail.mp3");
            Console.WriteLine($"\n\nYou lost {-won}🪙.");
        }
        else {
            sound = AudioManager.PlayAudio("Media\\coinSound.mp3");
            Console.WriteLine($"\n\nYou have won {won}🪙.");
        }

        Console.WriteLine($"Your total is {total}🪙");
        sound.Join(); // Wait for the sound to finish
        Console.Write("\n\nDo you want to continue playing [y/n]: ");
    }

    protected virtual int ReadBet() {
        return InputReader.ReadInputOfType<int>("Place your bet: ", "Invalid input", i => i > 0);
    }

    protected abstract int PlayRound(int bet);
    protected virtual void PrintRules() { }
}