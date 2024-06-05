using System.Numerics;

namespace Casino.Systems;

public static class Abbreviater {
    public static string Abbreviate(this BigInteger n)
    {
        // all abbreviations to Ssg (10^200)
        string[] abbr = new string[]
        {
            "M", "B", "T", "Q", "Qi", "Sx", "Sp", "O", "N", "Dc", "Udc", "Ddc", "Tdc", "Qdc",
            "Qn", "Sx", "Sd", "Od", "Nd", "Vg", "Uvg", "Dvg", "Tvg", "Qvg", "Qnvg", "Sxvg",
            "Spvg", "Ovg", "Nvg", "Tg", "Utg", "Dtg", "Trtg", "Qttg", "Qntg", "Stg", "Sntg",
            "Otg", "Ntg", "Qag", "Uqag", "Dqag", "Tqag", "Qaqag", "Qnqag", "Sqag", "Spqag",
            "Oqag", "Nqag", "Qig", "Uqig", "Dqig", "Tqig", "Qqig", "Qnqig", "Sqig", "Spqig", 
            "Oqig", "Nqig", "Sxg", "Usxg", "Dsxg", "Tsxg", "Qsxg", "Qnsxg", "Sxsxg", "Ssg"
        };

        int l = (BigInteger.Abs(n).ToString().Length - 1) / 3;

        // need l anyway so we can use the slightly faster (but more confusing way)
        // this is basically the same is if (n < 1000)
        if (l == 0) return n.ToString();

        BigInteger i = BigInteger.Pow(1000, l);

        return n / i + abbr[l - 1];
    }

    public static string AbbreviateIf(this BigInteger n, bool condition) {
        if (!condition) return n.ToString();
        return Abbreviate(n);
    }
}