namespace Casino;

public static class FileSaver {
    private const string DIRECTORY = @"C:\Gambling";
    private const string FILE = @"\Session.save";
    public static void SaveMoneyWon(int money) {
        string path = DIRECTORY + FILE;
        if (!Directory.Exists(DIRECTORY)) Directory.CreateDirectory(DIRECTORY);
        
        File.WriteAllText(path, money.ToString());
    }
    
    public static bool TryReadMoneyWon(out int result) {
        string path = DIRECTORY + FILE;
        result = default;
        
        if (!File.Exists(path)) return false;
        
        return int.TryParse(File.ReadAllText(path), out result);
    }
}