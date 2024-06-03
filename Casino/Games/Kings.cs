using System.Numerics;
using Casino.DataStructures;
using Casino.Systems;

namespace Casino.Games;

public class Kings : CasinoGame {
    private const int NUMBER_BOTS = 3;

    protected override BigInteger PlayRound(BigInteger bet) {
        int botsIn = NUMBER_BOTS;
        BigInteger playerBet = 0;
        BigInteger totalMoney = 0;
        int kingDice = 0;
        int kingIndex = -1;
        bool roundOver = false;

        // Condition doesn't cover all cases, because the 'break' keyword is used for exiting
        while (botsIn > 0) {
            playerBet += bet;
            totalMoney += bet * (botsIn + 1);

            Console.WriteLine($"\n\nA new round has started:\nYour bet: {playerBet}\nTotal money: {totalMoney}");
            string king = kingIndex switch {
                -1 => "No one",
                0 => "You",
                _ => $"Bot {kingIndex}"
            };
            Console.WriteLine("Current King: " + king);
            
            // if you've been king or an entire round, you win
            if (kingIndex == 0) {
                Console.Write("You have won by being King for 1 round\n\n");
                break;
            }

            int playerDice = Random.Shared.Next(1, 7);
            AudioManager.PlayAudio("Media\\dice.mp3").Join();
            Console.WriteLine("\nYou have rolled: " + NumberToSymbol(playerDice));
            // if you roll 1, you're out
            if (playerDice == 1) {
                Console.Write("You have lost by rolling a 1\n\n");
                break;
            }

            if (IsNewKing(playerDice, kingDice, out bool battle)) {
                if (battle) {
                    Console.WriteLine("\nYou have beaten a bot in battle");
                    botsIn--;
                }
                Console.WriteLine("You are now the King!");
                kingIndex = 0;
                kingDice = playerDice;
            }
            else if (battle) {
                Console.Write("You have lost a battle. You're out\n\n");
                break; // if you lose the battle, you're out
            }

            for (int i = 0; i < botsIn; i++) {
                // if you've been king or an entire round, you win
                if (kingIndex == i + 1) {
                    Console.Write($"Bot {i+1} has won by being King for 1 round\n\n");
                    roundOver = true;
                    break;
                }

                AudioManager.PlayAudio("Media\\dice.mp3").Join();
                int botDice = Random.Shared.Next(1, 7);
                Console.WriteLine($"\nBot {i+1} has rolled: " + NumberToSymbol(botDice));
                // if you roll 1, you're out
                if (botDice == 1) {
                    botsIn--;
                    Console.WriteLine("A bot has been eliminated by rolling 1");
                    continue;
                }
                
                if (IsNewKing(botDice, kingDice, out bool wasBattle)) {
                    if (wasBattle) {
                        if (kingIndex == 0) {
                            kingIndex = i + 1;
                            Console.Write("You have lost a battle. You're out\n\n");
                            roundOver = true;
                            break;
                        }

                        Console.WriteLine("A bot has fallen in battle");
                        botsIn--;
                        continue;
                    }

                    Console.WriteLine($"Bot {i+1} is now the King!");
                    kingIndex = i + 1;
                    kingDice = botDice;
                }
                else if (wasBattle) {
                    Console.WriteLine("A bot has fallen in battle");
                    botsIn--;
                }
            }
            
            if (roundOver) break;
        }

        return kingIndex == 0 ? totalMoney : -playerBet;
    }

    private static bool IsNewKing(int roll, int kingRoll, out bool wasBattle) {
        wasBattle = false;

        if (kingRoll == roll) Console.WriteLine("A battle has started!");
        // equal means battle
        while (kingRoll == roll) {
            wasBattle = true;
            // In a battle, you must roll until the rolls are not equal
            // The one who wins is king, the one who loses is out
            AudioManager.PlayAudio("Media\\dice.mp3").Join();
            AudioManager.PlayAudio("Media\\dice.mp3").Join();
            roll = Random.Shared.Next(1, 7);
            kingRoll = Random.Shared.Next(1, 7);

            Console.WriteLine($"Battle: King has rolled {NumberToSymbol(kingRoll)}. Contestant has rolled {NumberToSymbol(roll)}");
        }

        // 2 beats 6 (needs to be checked first)
        if (roll == 2 && kingRoll == 6) return true;
        if (roll == 6 && kingRoll == 2) return false;

        // high beats low
        return roll > kingRoll;
    }

    private static char NumberToSymbol(int num) => num switch {
        1 => '\u2680',
        2 => '\u2681',
        3 => '\u2682',
        4 => '\u2683',
        5 => '\u2684',
        6 => '\u2685',
        _ => '\0'
    };

    protected override void PrintRules() {
        Console.WriteLine("Kings\n=====\n");

        Console.Write("Do you want to see the rules [y/n]: ");
        if (Console.ReadLine()!.ToLower() != "y") return;

        Console.WriteLine($"\n\nEvery player rolls a dice (You will play against {NUMBER_BOTS} bots)");
        Console.WriteLine("\nThe player with the highest roll becomes KING");
        Console.WriteLine("The values work in normal order, but 2 beats 6");
        Console.WriteLine("If you roll a 1, you are out");
        Console.WriteLine("\nIf the king stays king until it's their turn again, he wins");
        Console.WriteLine("\nIf you get the same roll as the king, you enter a battle");
        Console.WriteLine("In a battle, the one who rolls lower is completely out");
        Console.WriteLine("The winner of the battle becomes King");
        Console.WriteLine("\nAt the end of every round, your initial bet is added to the total");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nThe game will run completely on its own");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey(true);
    }
}