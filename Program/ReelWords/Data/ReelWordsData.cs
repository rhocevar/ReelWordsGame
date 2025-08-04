using System.Collections.Generic;
using ReelWords.Game;
using ReelWords.Validation;

namespace ReelWords.Data;

/// <summary>
/// Data class containing all necessary data for game play loaded up in memory. It has the words stored in a Trie data
/// structure, a list reels containing the letters that can be played and a word validator. 
/// </summary>
public class ReelWordsData
{
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public Trie Words { get; }
    public List<Queue<Tile>> Reels { get; }
    public WordValidator Validator { get; }

    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public ReelWordsData(
        Trie words, 
        List<Queue<Tile>> reels,
        WordValidator wordValidator)
    {
        Words = words;
        Reels = reels;
        Validator = wordValidator;
    }
}
