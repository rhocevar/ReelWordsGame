using System.Text;
using ReelWords.Data;
using ReelWords.Game;
using ReelWords.View;

namespace ReelWords;

public static class Program
{
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    static void Main(string[] args)
    {
        // The default view for this game is the console. We could create a new type of view to display the game output
        // to a different system, as long as it implements the IView interface.
        // Encoding is set to UTF8 so we can display the tile scores as subscripts
        IView consoleView = new ConsoleView(Encoding.UTF8);
        
        // The language config could be defined upon launching the game (e.g. via args).
        // Setting it to american english as the default.
        LanguageConfig languageConfig = LanguageConfig.en_us;
        
        GameManager.Instance.Initialize(consoleView, languageConfig);
    }
}
