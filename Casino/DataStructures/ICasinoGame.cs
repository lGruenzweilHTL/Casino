namespace Casino.DataStructures;

public interface ICasinoGame {
    /// <summary>
    /// The function to start the game
    /// </summary>
    /// <returns>How much money was won/lost</returns>
    public int Play();
}