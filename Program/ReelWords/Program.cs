using System;
using ReelWords.Data;
using ReelWords.Game;

namespace ReelWords;

public static class Program
{
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    static void Main(string[] args)
    {
        // The language config can be defined upon launching the game (e.g. via args).
        // Setting it to american english as the default for simplicity.
        LanguageConfig languageConfig = LanguageConfig.en_us;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        GameManager.Instance.Initialize(languageConfig);
    }
}
