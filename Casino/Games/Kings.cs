namespace Casino.Games;

// ReSharper disable once ClassNeverInstantiated.Global
public class Kings : CasinoGame {
    protected override int PlayRound(int bet) {
        const int numBots = 3;
        bool playerIsIn = true;
        int botsIn = numBots;
        int playerBet = bet;
        int totalMoney = bet * (numBots + 1);
        int kingDice = 0;
        int kingIndex = -1;


        while (playerIsIn && botsIn > 0) {
            // if you've been king or an entire round, you win
            if (kingIndex == 0) break;

            int playerDice = Random.Shared.Next(1, 7);
            // if you roll 1, you're out
            if (playerDice == 1) break;

            if (IsNewKing(playerDice, kingDice, out bool battle)) {
                if (battle) {
                    botsIn--;
                }
                kingIndex = 0;
                kingDice = playerDice;
            }
            else if (battle) {
                break; // if you lose the battle, you're out
            }

            for (int i = 0; i < botsIn; i++) {
                // if you've been king or an entire round, you win
                if (kingIndex == i + 1) break;

                int botDice = Random.Shared.Next(1, 7);
                // if you roll 1, you're out
                if (botDice == 1) {
                    botsIn--;
                    continue;
                }
                
                if (IsNewKing(botDice, kingDice, out bool wasBattle)) {
                    if (wasBattle) {
                        if (kingIndex == 0) {
                            kingIndex = i + 1;
                            break;
                        }

                        botsIn--;
                        continue;
                    }

                    kingIndex = i + 1;
                    kingDice = botDice;
                }
                else if (wasBattle) botsIn--;
            }

            playerBet += bet;
            totalMoney += bet * (botsIn + 1);
        }

        return kingIndex == 0 ? totalMoney : -playerBet;
    }

    private static bool IsNewKing(int roll, int kingRoll, out bool wasBattle) {
        wasBattle = false;

        // equal means battle
        while (kingRoll == roll) {
            wasBattle = true;
            // In a battle, you roll until the rolls are not equal
            // The one who wins is king, the one who loses is out
            roll = Random.Shared.Next(1, 7);
        }

        // high beats low
        if (roll > kingRoll) return true;

        // 2 beats 6
        return kingRoll == 6 && roll == 2;
    }

    protected override void PrintRules() {
        Console.WriteLine("Kings\n=====\n");

        Console.Write("Do you want to see the rules [y/n]: ");
        if (Console.ReadLine()!.ToLower() != "y") return;

        Console.WriteLine("Every player rolls a dice (You will play against 3 bots)");
        Console.WriteLine("The player with the highest roll becomes KING");
        Console.WriteLine("But a 2 wins against a 6, so if the king has 6 and you have 2, you are the new king");
        Console.WriteLine("If the king stays king until it's their turn again, he wins");
        Console.WriteLine("If you roll a 1, you are completely out");
        Console.WriteLine("\nIf you get the same roll as the king, you enter a battle");
        Console.WriteLine("In a battle, the one who rolls lower is completely out");
        Console.WriteLine("The winner of the battle becomes King");

        Console.WriteLine("\nAt the end of every round, your initial bet is added to the total");

        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}