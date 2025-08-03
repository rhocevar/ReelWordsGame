using System;
using System.Collections.Generic;
using ReelWords.Game;

namespace ReelWords.Data;

public class ReelWordsData
{
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public Trie Words { get; }
    public List<Queue<Tile>> Reels { get; }
    public Dictionary<char, int> Scores { get; }
    public Func<string, bool> IsWordValid { get; }

    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public ReelWordsData(
        Trie words, 
        List<Queue<Tile>> reels, 
        Dictionary<char, int> scores,
        Func<string, bool> wordValidator)
    {
        Words = words;
        Reels = reels;
        Scores = scores;
        IsWordValid = wordValidator;
    }
}
