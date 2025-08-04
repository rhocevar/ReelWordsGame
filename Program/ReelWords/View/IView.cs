namespace ReelWords.View;

/// <summary>
/// Abstraction of a View entity
/// </summary>
public interface IView
{
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public void DisplayText(string text);
    public void DisplayTextLine(string text);
    public string ReadTextLine();
}
