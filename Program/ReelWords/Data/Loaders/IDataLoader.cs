namespace ReelWords.Data.Loaders;

/// <summary>
/// Abstraction of a Data Loader class
/// </summary>
public interface IDataLoader
{
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public ReelWordsData Load();
}
