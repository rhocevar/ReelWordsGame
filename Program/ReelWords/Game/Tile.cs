using ReelWords.Utilities;

namespace ReelWords.Game;

/// <summary>
/// The tile class represents a single tile or letter contained in a reel or rack. It has a letter and a score.
/// </summary>
public class Tile
{
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public char Letter { get; }
    public int Score { get; set; }
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public Tile(char letter)
    {
        Letter = letter;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public override string ToString()
    {
        return Letter.ToString().ToUpper() + Score.ToSubscript();
    }
}
