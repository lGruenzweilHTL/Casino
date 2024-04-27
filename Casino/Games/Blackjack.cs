namespace Casino.Games;

// ReSharper disable once ClassNeverInstantiated.Global
public class Blackjack : CasinoGame {
    protected override int PlayRound(int moneyBet) {
        List<int> cards = [];
        int dealerHand = Random.Shared.Next(1, 21);

        cards.Add(Random.Shared.Next(1, 11));
        cards.Add(Random.Shared.Next(1, 11));

        string playerTakes;
        do {
            string[] yourCards = Array.ConvertAll<int, string>(cards.ToArray(), c => $"{CardToImage(c)} ({c.ToString()})");
            Console.WriteLine("\nYour cards are: " + string.Join(", ", yourCards));
            Console.Write("Do you want to take a card [y/n]: ");
            playerTakes = Console.ReadLine()!.ToLower();

            if (playerTakes == "y") {
                cards.Add(Random.Shared.Next(1, 11));
            }
        } while (playerTakes == "y" && cards.Sum() <= 21);

        while (DealerTakesCard(dealerHand)) {
            dealerHand += Random.Shared.Next(1, 11);
        }

        int playerHand = cards.Sum();
        Console.Write("\n\nYour hand is: " + playerHand);
        if (playerHand > 21) Console.Write(" (over)");
        Console.Write("\nThe dealer's hand is: " + dealerHand);
        if (dealerHand > 21) Console.Write(" (over)");

        if (playerHand > 21) return -moneyBet;
        if (dealerHand > 21) return moneyBet;

        return moneyBet * (playerHand > dealerHand ? 1 : -1);
    }

    private static bool DealerTakesCard(int hand) => hand < 18;

    private static string CardToImage(int card) {
        return card switch {
            1 => "\ud83c\udca1",
            2 => "\ud83c\udca2",
            3 => "\ud83c\udca3",
            4 => "\ud83c\udca4",
            5 => "\ud83c\udca5",
            6 => "\ud83c\udca6",
            7 => "\ud83c\udca7",
            8 => "\ud83c\udca8",
            9 => "\ud83c\udca9",
            10 => "\ud83c\udcaa",
            _ => ""
        };
    }

    protected override void PrintRules() {
        Console.WriteLine("Blackjack\n=========\n");
        
        Console.Write("Do you want to see the rules [y/n]: ");
        if (Console.ReadLine()!.ToLower() != "y") return;

        Console.WriteLine("\n\nYou are dealt two cards. You can either take another card or end your turn");
        Console.WriteLine("The dealer does the same");
        Console.WriteLine("The one with the higher hand wins");
        Console.WriteLine("\nBUT:\n");
        Console.WriteLine("If you are over 21, you immediately lose");

        Console.WriteLine("\n\nCards:\n");
        Console.WriteLine("Every card is worth their value");
        Console.WriteLine("In normal Blackjack, the value of aces can change");
        Console.WriteLine("Here, it's always 1");
        Console.WriteLine("There are also no Jacks, Queens or Kings");

        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}