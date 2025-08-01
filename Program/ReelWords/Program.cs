using System;
using System.Globalization;
using System.IO;

namespace ReelWords;

public static class Program
{
    // TODO: create a game manager class or similar to be initialized from Main
    
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
    // Constants
    //------------------------------------------------------------------------------------------------------------------
    private const string c_resourcesDirectoryName = "Resources";
    private const string c_enUsFileName = "american-english-large.txt";
    
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private static string m_dictionaryFileName;
    private static CharRange[] m_allowedCharacters;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    private static void Main(string[] args)
    {
        SetLanguage(LanguageConfig.en_us);
        
        // TODO: perform initialization in a separate thread
        Trie words = InitializeWordsTrie();
        if (words == null)
        {
            Console.WriteLine($"Unable to initialize words trie for file: {m_dictionaryFileName}");
            return;
        }

        // TODO: create a better game loop (see Hearthstone assessment)
        bool playing = true;

        while (playing)
        {
            Console.Write("\nCreate a word using the letters from your reel: "); 
            string input = Console.ReadLine();
            
            // Transform user input to lower case so it can still be considered if entered with capital letters
            input = input?.ToLower(CultureInfo.InvariantCulture);
            
            if (!IsWordValid(input))
            {
                Console.WriteLine($"The word '{input}' is not valid. Please try again.");
                continue;
            }
            
            bool wordExists = words.Search(input);
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
    private static void SetLanguage(LanguageConfig languageConfig)
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
    private static Trie InitializeWordsTrie()
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
            Console.WriteLine($"There was an exception reading file: {ex.Message}");
            return null;
        }
        
        Console.WriteLine($"Found a total of {validWordsCount + invalidWordsCount} words in dictionary file. Valid: {validWordsCount}. Invalid: {invalidWordsCount}");
        Console.WriteLine("Words Trie initialized successfully.");
        return trie;
    }

    //------------------------------------------------------------------------------------------------------------------
    /*
     * Filter out invalid words: proper nouns, hyphenated words, abbreviations, one-letter words, capitalized words,
     * words containing characters not included in the English alphabet (a-z)
     */
    private static bool IsWordValid(string word)
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