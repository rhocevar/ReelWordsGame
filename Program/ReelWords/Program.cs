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
        // The language config can be defined via args. Setting it to english as default for simplicity.
        LanguageConfig languageConfig = LanguageConfig.en_us;
        
        GameManager.Instance.Initialize(languageConfig);
    }
}
