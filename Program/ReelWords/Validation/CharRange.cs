using System;

namespace ReelWords.Validation;

/// <summary>
/// Determine a valid range of characters (letters) that can be used in the game.
/// </summary>
public class CharRange
{
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public int Min { get; }
    public int Max { get; }

    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------        
    public CharRange(int min, int max)
    {
        if (min > max)
        {
            throw new ArgumentException($"Invalid char range arguments: min = {min} | max = {max}");
        }
            
        Min = min;
        Max = max;
    }
}
