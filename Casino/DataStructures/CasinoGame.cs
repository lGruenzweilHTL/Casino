using System.Numerics;
using Casino.Systems;

namespace Casino.DataStructures;

public abstract class CasinoGame {
    public BigInteger Play() {
        Console.CursorVisible = true;
        BigInteger moneyWon = Program.MoneyWon;
        Utils.ClearConsoleBuffer();

        PrintRules();

        string continuationKey;
        do {
            Utils.ClearConsoleBuffer();

            BigInteger won = PlayRound(ReadBet());
            moneyWon += won;

            PrintPayout(moneyWon, won);
            continuationKey = Console.ReadLine()!.ToLower();
        } while (continuationKey == "y");

        return moneyWon;
    }

    protected virtual void PrintPayout(BigInteger total, BigInteger won) {
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

    protected virtual BigInteger ReadBet() {
        return InputReader.ReadInputOfType<BigInteger>($"Place your bet: ", "Invalid input", i => i > 0);
    }

    protected abstract BigInteger PlayRound(BigInteger bet);
    protected virtual void PrintRules() { }
}