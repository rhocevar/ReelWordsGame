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
    public Func<string, bool> IsWordValid { get; }

    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public ReelWordsData(
        Trie words, 
        List<Queue<Tile>> reels,
        Func<string, bool> wordValidator)
    {
        Words = words;
        Reels = reels;
        IsWordValid = wordValidator;
    }
}
