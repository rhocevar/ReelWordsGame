using System;
using System.IO;
using System.Text;

namespace ReelWords.View;

/// <summary>
/// Concrete implementation of a Console View. It handles input and output to a Console window.
/// </summary>
public class ConsoleView : IView
{
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private readonly TextReader m_in;
    private readonly TextWriter m_out;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public ConsoleView(Encoding encoding)
    {
        Console.InputEncoding = encoding;
        Console.OutputEncoding = encoding;
        m_in = Console.In;
        m_out = Console.Out;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public void DisplayText(string text)
    {
        m_out.Write(text);
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public void DisplayTextLine(string text)
    {
        m_out.WriteLine(text);
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public string ReadTextLine()
    {
        return m_in.ReadLine();
    }
}
