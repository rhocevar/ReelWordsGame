using System;
using ReelWords.Data;

namespace ReelWords.Validation;

public class WordValidator
{
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public Func<string, bool> Validator { get; private set; }
    
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private CharRange[] m_allowedCharacters;

    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public WordValidator(LanguageConfig languageConfig)
    {
        switch (languageConfig)
        {
            case LanguageConfig.en_us:
            case LanguageConfig.en_gb:
            case LanguageConfig.pt_br: {
                m_allowedCharacters = new[] { new CharRange('a', 'z') };
                Validator = ValidateWord_Latin;
                break;
            }
            // Other languages may have a different set of allowed characters and validation rules
            // e.g. zh-cn, de, ja, ko, etc...
            default:
            {
                throw new NotImplementedException($"Language config {languageConfig} is not yet supported.");
            }
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    // Filter out invalid latin words: words containing characters not included in the language dictionary,
    // one-letter words, capitalized words, null or empty words, etc.
    private bool ValidateWord_Latin(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return false;
        }
        
        if (word.Length <= 1)
        {
            return false;
        }
        
        foreach (char c in word)
        {
            foreach (CharRange range in m_allowedCharacters)
            {
                if (c < range.Min || c > range.Max)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
}
