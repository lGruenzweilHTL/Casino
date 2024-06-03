using System.Numerics;

namespace Casino.Systems;

public static class FileSaver {
    private const string FILENAME = "Session.save";
    public static void SaveNumber(BigInteger money) {
        File.WriteAllText(FILENAME, money.ToString());
    }
    
    public static bool TryReadNumber(out BigInteger result) {
        result = default;
        return File.Exists(FILENAME) && BigInteger.TryParse(File.ReadAllText(FILENAME), out result);
    }
}