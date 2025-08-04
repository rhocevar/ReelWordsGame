using System;
using ReelWords.Config;

namespace ReelWords.Validation;

/// <summary>
/// Validate words for a specified language config. Each language can have their own rules for validating words such as
/// allowing different character sets/ranges or allowing certain Unicode characters.
/// </summary>
public class WordValidator
{
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private readonly CharRange[] m_allowedCharacters;
    private readonly Func<string, bool> m_validator;

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
                m_validator = ValidateWord_Latin;
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
    public bool IsValid(string word)
    {
        return m_validator(word);
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
