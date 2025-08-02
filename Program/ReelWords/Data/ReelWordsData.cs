using System;

namespace ReelWords.Data;

public class ReelWordsData
{
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public Trie Words { get; }
    public Func<string, bool> IsWordValid { get; }

    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public ReelWordsData(Trie words, Func<string, bool> wordValidator)
    {
        Words = words;
        IsWordValid = wordValidator;
    }
}
