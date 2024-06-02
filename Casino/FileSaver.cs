namespace Casino;

public static class FileSaver {
    private const string FILENAME = "Session.save";
    public static void SaveNumber(int money) {
        File.WriteAllText(FILENAME, money.ToString());
    }
    
    public static bool TryReadNumber(out int result) {
        result = default;
        return File.Exists(FILENAME) && int.TryParse(File.ReadAllText(FILENAME), out result);
    }
}