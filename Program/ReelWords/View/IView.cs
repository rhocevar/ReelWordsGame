namespace ReelWords.View;

public interface IView
{
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public void DisplayText(string text);
    public void DisplayTextLine(string text);
    public string ReadTextLine();
}
