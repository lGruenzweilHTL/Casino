namespace Casino;

public static class FileSaver {
    private const string DIRECTORY = @"C:\Gambling";
    private const string FILENAME = @"\Session.save";
    public static void SaveNumber(int money) {
        const string PATH = DIRECTORY + FILENAME;
        if (!Directory.Exists(DIRECTORY)) Directory.CreateDirectory(DIRECTORY);
        
        File.WriteAllText(PATH, money.ToString());
    }
    
    public static bool TryReadNumber(out int result) {
        const string PATH = DIRECTORY + FILENAME;
        result = default;
        
        return File.Exists(PATH) && int.TryParse(File.ReadAllText(PATH), out result);
    }
}