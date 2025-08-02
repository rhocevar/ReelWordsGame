using System;
using System.IO;
using System.Threading.Tasks;

namespace ReelWords;

public class DataLoader
{
    //------------------------------------------------------------------------------------------------------------------
    // Types
    //------------------------------------------------------------------------------------------------------------------
    public enum LanguageConfig
    {
        en_us, en_gb, pt_br
    }
    
    //------------------------------------------------------------------------------------------------------------------
    // Constants
    //------------------------------------------------------------------------------------------------------------------
    private const string c_resourcesDirectoryName = "Resources";
    private const string c_enUsFileName = "american-english-large.txt";
    
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private string m_dictionaryFileName;
    private CharRange[] m_allowedCharacters;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public DataLoader(LanguageConfig languageConfig)
    {
        SetLanguage(languageConfig);
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public ReelWordsData Load()
    {
        Task<Trie> loadWordsTask = LoadWordsAsync();
        Task.WaitAll(loadWordsTask);
        
        if (!loadWordsTask.IsCompletedSuccessfully || loadWordsTask.Result == null)
        {
            Console.WriteLine("There was a problem initializing the language dictionary.");
            return null;
        }

        return new ReelWordsData(loadWordsTask.Result, ValidateWord);
    }
    
    //------------------------------------------------------------------------------------------------------------------ 
    private void SetLanguage(LanguageConfig languageConfig)
    {
        switch (languageConfig)
        {
            case LanguageConfig.en_us:
            {
                m_dictionaryFileName = c_enUsFileName;
                m_allowedCharacters = new[] { new CharRange('a', 'z') };
                Console.WriteLine($"Language set to '{languageConfig}'");
                break;
            }
            case LanguageConfig.en_gb:
            {
                Console.WriteLine($"Language config {languageConfig} is not yet supported.");
                throw new NotImplementedException();
            }
            case LanguageConfig.pt_br:
            {
                Console.WriteLine($"Language config {languageConfig} is not yet supported.");
                throw new NotImplementedException();
            }
            default:
            {
                Console.WriteLine($"Language config {languageConfig} is not yet supported.");
                throw new NotImplementedException();
            }
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private async Task<Trie> LoadWordsAsync()
    {
        Trie words = null;
        await Task.Run(() => { words = LoadWordsData(); });
        return words;
    }

    //------------------------------------------------------------------------------------------------------------------ 
    private Trie LoadWordsData()
    {
        Console.WriteLine($"Initializing words for file: {m_dictionaryFileName}");
        
        DirectoryInfo resourcesDirectory = Utils.TryGetDirectoryInfo(c_resourcesDirectoryName);
        if (resourcesDirectory == null)
        {
            Console.WriteLine($"Unable to find {c_resourcesDirectoryName} directory.");
            return null;
        }
        Console.WriteLine("Resources directory found: " + resourcesDirectory);
        
        Trie trie = new Trie();
        int validWordsCount = 0, invalidWordsCount = 0;
        string wordsFilePath = Path.Combine(resourcesDirectory.FullName, m_dictionaryFileName);
        try
        {
            using StreamReader reader = new StreamReader(wordsFilePath);
            string word;
            while ((word = reader.ReadLine()) != null)
            {
                if (ValidateWord(word))
                {
                    ++validWordsCount;
                    trie.Insert(word);
                }
                else
                {
                    ++invalidWordsCount;
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"There was an exception reading file '{wordsFilePath}': {ex.Message}");
            throw;
        }
        
        Console.WriteLine($"Found a total of {validWordsCount + invalidWordsCount} words in dictionary file." + 
                          $" Valid: {validWordsCount}. Invalid: {invalidWordsCount}");
        Console.WriteLine("Words dictionary initialized successfully.");
        return trie;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    // Filter out invalid words: words containing characters not included in the language dictionary, one-letter words,
    // capitalized words, null or empty words, etc.
    private bool ValidateWord(string word)
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