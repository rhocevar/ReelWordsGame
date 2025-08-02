using System;
using System.IO;
using System.Threading.Tasks;

namespace ReelWords;

public class GameManager
{
    //------------------------------------------------------------------------------------------------------------------
    // Types
    //------------------------------------------------------------------------------------------------------------------
    private enum LanguageConfig
    {
        en_us, en_gb, pt_br
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private class CharRange
    {
        public int Min { get; }
        public int Max { get; }

        public CharRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameManager();
            }
            
            return m_instance;
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    // Constants
    //------------------------------------------------------------------------------------------------------------------
    private const string c_resourcesDirectoryName = "Resources";
    private const string c_enUsFileName = "american-english-large.txt";
    
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private static GameManager m_instance;
    private string m_dictionaryFileName;
    private CharRange[] m_allowedCharacters;
    private Trie m_words;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    private GameManager() { }
    
    // TODO: move file loading logic into a separate class: DataLoader
    
    //------------------------------------------------------------------------------------------------------------------
    public void StartGame()
    {
        Console.WriteLine("Game initialization started...");
        Task initLanguageTask = InitializeLanguageAsync(LanguageConfig.en_us);
        Task.WaitAll(initLanguageTask);
        
        if (!initLanguageTask.IsCompletedSuccessfully)
        {
            Console.WriteLine("There was a problem initializing the language dictionary.");
            return;
        }
        
        Console.WriteLine("Game initialization complete.");

        Console.WriteLine("\n***************************\n***** Reel Words Game *****\n***************************");
        Console.WriteLine("--> Type '0' to end the game."); 
        while (true)
        {
            Console.Write("\nCreate a word using the letters from your reel: "); 
            string input = Console.ReadLine();
            if (input == "0")
            {
                EndGame();
                return;
            }
            
            // Transform user input to lower case so it can still be considered if entered with capital letters
            input = input?.ToLower();
            
            if (!IsWordValid(input))
            {
                Console.WriteLine($"The word '{input}' is not valid. Please try again.");
                continue;
            }
            
            bool wordExists = m_words.Search(input);
            if (wordExists)
            {
                // TODO: Increase score
                // TODO: Present new set of letters in reel
                Console.WriteLine($"Good job! You earned X points from the word '{input}'.");
            }
            else
            {
                Console.WriteLine($"The word '{input}' does not exist in the dictionary. Please try again.");
            }
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private void EndGame()
    {
        Console.WriteLine("The Reel Words Game is over. Your total score is X.");
        Console.WriteLine("CONGRATULATIONS! WELL PLAYED!");
        Environment.Exit(0);
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private async Task InitializeLanguageAsync(LanguageConfig languageConfig)
    {
        await Task.Run(() =>
        {
            SetLanguage(languageConfig);
            m_words = InitializeWordsData();
        });
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
    private Trie InitializeWordsData()
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
                if (IsWordValid(word))
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
                          " Valid: {validWordsCount}. Invalid: {invalidWordsCount}");
        Console.WriteLine("Words dictionary initialized successfully.");
        return trie;
    }

    //------------------------------------------------------------------------------------------------------------------
    // Filter out invalid words: words containing characters not included in the language dictionary, one-letter words,
    // capitalized words, null or empty words, etc.
    private bool IsWordValid(string word)
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
