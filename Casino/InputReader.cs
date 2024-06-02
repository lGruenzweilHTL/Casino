namespace Casino;

public static class InputReader {
    public static T ReadInputOfType<T>(string prompt, string invalidMsg = "Invalid input", Predicate<T>? predicate = null)
        where T : IParsable<T> {
        T result;
        Console.Write(prompt);
        while (!T.TryParse(Console.ReadLine(), null, out result!) || (predicate != null && !predicate(result))) {
            Console.WriteLine(invalidMsg);
            Console.Write(prompt);
        }

        return result;
    }
}