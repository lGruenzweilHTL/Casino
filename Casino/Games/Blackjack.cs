using Casino.DataStructures;

namespace Casino.Games;

// ReSharper disable once ClassNeverInstantiated.Global
public class Blackjack : CasinoGame {
    protected override int PlayRound(int moneyBet) {
        List<int> cards = [
            Random.Shared.Next(1, 11),
            Random.Shared.Next(1, 11)
        ];
        if (AceWorth11(cards)) {
            int ind = cards.IndexOf(1);
            cards[ind] = 11;
        }
        
        List<int> dealerCards = [
            Random.Shared.Next(1, 11)
        ];

        Console.WriteLine(
            $"\nThe dealer's cards are: {FormatCards(dealerCards.ToArray())}, \ud83c\udca0 (unknown)");
        dealerCards.Add(Random.Shared.Next(1, 11));


        string playerTakes;
        do {
            Console.WriteLine("\nYour cards are: " + FormatCards(cards.ToArray()));
            Console.Write("Do you want to take a card [y/n]: ");
            playerTakes = Console.ReadLine()!.ToLower();

            if (playerTakes == "y") {
                cards.Add(Random.Shared.Next(1, 11));
            }
        } while (playerTakes == "y" && cards.Sum() <= 21);

        Console.WriteLine("\nYour final cards are: " + FormatCards(cards.ToArray()));
        int playerHand = cards.Sum();

        Console.WriteLine();

        if (playerHand <= 21) {
            Console.WriteLine("The dealer's cards are: " + FormatCards(dealerCards.ToArray()));
            while (DealerTakesCard(dealerCards.Sum())) {
                int card = Random.Shared.Next(1, 11);
                dealerCards.Add(card);

                Thread.Sleep(1000);
                Console.WriteLine($"The dealer took: {CardToImage(card)} ({card})");
            }

            Console.WriteLine("The dealer's final cards are: " + FormatCards(dealerCards.ToArray()));
        }
        else {
            Console.WriteLine("You are over 21. You lose!");
            return -moneyBet;
        }

        int dealerHand = dealerCards.Sum();
        Console.Write("\n\nYour hand is: " + playerHand);
        Console.Write("\nThe dealer's hand is: " + dealerHand);
        if (dealerHand > 21) Console.Write(" (over)");

        if (dealerHand > 21) return moneyBet;

        return moneyBet * (playerHand > dealerHand ? 1 : -1);
    }

    private static string FormatCards(params int[] cards) {
        return string.Join(", ",
            Array.ConvertAll<int, string>(cards, c => $"{CardToImage(c)} ({c.ToString()})"));
    }

    private static bool DealerTakesCard(int hand) => hand < 18;

    private static bool AceWorth11(List<int> hand) {
        if (hand.Count != 2) return false;
        return (hand[0] == 1 && hand[1] == 10) || (hand[0] == 10 && hand[1] == 1);
    }

    private static string CardToImage(int card) => card switch {
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
        11 => "\ud83c\udca1", // Ace
        _ => ""
    };


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
        Console.WriteLine("Every card is worth their numeric value");
        Console.WriteLine("If an ace appears with a ten, it's worth 11");
        Console.WriteLine("There are no Jacks, Queens or Kings");

        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}