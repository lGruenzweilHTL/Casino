namespace Casino;

public static class Utils {
    public static void ClearConsoleBuffer() {
        // Due to the new Windows Terminal on Win11, the console only clears the window and not the buffer
        // you can clear the console's buffer with this escape sequence
        // To clear the window and the offscreen buffer, use it in combination with Console.Clear
        Console.Clear();
        Console.WriteLine("\x1b[3J");
    }
}