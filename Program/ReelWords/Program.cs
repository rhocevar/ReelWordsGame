using System.Text;
using ReelWords.Config;
using ReelWords.Data.Loaders;
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
        // The language config could be defined upon launching the game (e.g. via args).
        // Setting it to american english as the default.
        LanguageConfig languageConfig = LanguageConfig.en_us;
        
        // The default view for this game is the console. We could create a new type of view to display the game output
        // to a different system, as long as it implements the IView interface.
        // Encoding is set to UTF8 so we can display the tile scores as subscripts
        IView consoleView = new ConsoleView(Encoding.UTF8);
        
        // The default data loader for this game is the file data loader. We could create a new type of loader that
        // pulls data from a different source, such as a database, as long as it implements the IDataLoader interface.
        IDataLoader fileDataLoader = new FileDataLoader(languageConfig, consoleView);
        
        GameManager.Instance.Initialize(consoleView, fileDataLoader);
    }
}
