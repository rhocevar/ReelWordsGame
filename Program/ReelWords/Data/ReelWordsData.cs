using System;
using System.Collections.Generic;

namespace ReelWords.Data;

public class ReelWordsData
{
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public Trie Words { get; }
    public List<Queue<char>> Reels { get; }
    public Func<string, bool> IsWordValid { get; }

    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public ReelWordsData(Trie words, List<Queue<char>> reels, Func<string, bool> wordValidator)
    {
        Words = words;
        Reels = reels;
        IsWordValid = wordValidator;
    }
}
