using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ReelWords.Game;
using ReelWords.Utilities;
using ReelWords.Validation;

namespace ReelWords.Data;

public class DataLoader
{
    //------------------------------------------------------------------------------------------------------------------
    // Constants
    //------------------------------------------------------------------------------------------------------------------
    private const string c_resourcesDirectoryName = "Resources";
    private const string c_enUsFileName = "american-english-large.txt";
    private const string c_defaultReelsFileName = "reels.txt";
    private const string c_defaultScoresFileName = "scores.txt";
    
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private DirectoryInfo m_resourcesDirectory;
    private WordValidator m_wordValidator;
    private string m_wordsFileName;
    private string m_reelsFileName;
    private string m_scoresFileName;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public DataLoader(LanguageConfig languageConfig)
    {
        SetLanguage(languageConfig);
        SetResourcesDirectory();
    }
    
    //------------------------------------------------------------------------------------------------------------------ 
    private void SetLanguage(LanguageConfig languageConfig)
    {
        switch (languageConfig)
        {
            case LanguageConfig.en_us:
            {
                m_wordsFileName = c_enUsFileName;
                m_reelsFileName = c_defaultReelsFileName;
                m_scoresFileName = c_defaultScoresFileName;
                m_wordValidator = new WordValidator(languageConfig);
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
    private void SetResourcesDirectory()
    {
        m_resourcesDirectory = Utils.TryGetDirectoryInfo(c_resourcesDirectoryName);
        if (m_resourcesDirectory == null)
        {
            Console.WriteLine($"Unable to find {c_resourcesDirectoryName} directory.");
            throw new DirectoryNotFoundException();
        }
        Console.WriteLine("Resources directory found: " + m_resourcesDirectory);
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public ReelWordsData Load()
    {
        Task<Trie> loadWordsTask = LoadWordsAsync();
        Task<List<Queue<Tile>>> loadReelsTask = LoadReelsAsync();
        Task<Dictionary<char, int>> loadScoresTask = LoadScoresAsync();
        Task.WaitAll(loadWordsTask, loadReelsTask, loadScoresTask);
        
        if (!loadWordsTask.IsCompletedSuccessfully || loadWordsTask.Result == null)
        {
            Console.WriteLine($"There was a problem initializing the language dictionary: {loadWordsTask.Exception}");
            return null;
        }
        
        if (!loadReelsTask.IsCompletedSuccessfully || loadReelsTask.Result == null)
        {
            Console.WriteLine($"There was a problem initializing the language dictionary: {loadReelsTask.Exception}");
            return null;
        }
        
        if (!loadScoresTask.IsCompletedSuccessfully || loadScoresTask.Result == null)
        {
            Console.WriteLine($"There was a problem initializing the scores table: {loadReelsTask.Exception}");
            return null;
        }
        
        Trie words = loadWordsTask.Result;
        List<Queue<Tile>> reels = loadReelsTask.Result;
        Dictionary<char, int> scores = loadScoresTask.Result;
        
        // Assign scores
        foreach (Queue<Tile> reel in reels)
        {
            foreach (Tile tile in reel)
            {
                if (scores.TryGetValue(tile.Letter, out int score))
                {
                    tile.Score = score;
                }
                else
                {
                    Console.WriteLine($"Error: Could not find a score for letter '{tile.Letter}'");
                    return null;
                }
            }
        }

        return new ReelWordsData(words, reels, scores, m_wordValidator.Validator);
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
        Console.WriteLine($"Initializing words for file: {m_wordsFileName}");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        Trie trie = new Trie();
        int validWordsCount = 0, invalidWordsCount = 0;
        string wordsFilePath = Path.Combine(m_resourcesDirectory.FullName, m_wordsFileName);
        try
        {
            using StreamReader reader = new StreamReader(wordsFilePath);
            string word;
            while ((word = reader.ReadLine()) != null)
            {
                if (m_wordValidator.Validator(word))
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
            Console.WriteLine($"There was an exception reading the file '{wordsFilePath}': {ex.Message}");
            return null;
        }
        stopwatch.Stop();
        
        Console.WriteLine($"Found a total of {validWordsCount + invalidWordsCount} words in dictionary file." + 
                          $" Valid: {validWordsCount}. Invalid: {invalidWordsCount}");
        
        Console.WriteLine($"Words dictionary initialized successfully ({stopwatch.Elapsed.TotalMilliseconds}ms)");
        return trie;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private async Task<List<Queue<Tile>>> LoadReelsAsync()
    {
        List<Queue<Tile>> reels = null;
        await Task.Run(() => { reels = LoadReelsData(); });
        return reels;
    }
    
    //------------------------------------------------------------------------------------------------------------------ 
    private List<Queue<Tile>> LoadReelsData()
    {
        Console.WriteLine($"Initializing reels for file: {m_reelsFileName}");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        List<Queue<Tile>> reelsList = new List<Queue<Tile>>();
        bool initializedReels = false;
        string reelsFilePath = Path.Combine(m_resourcesDirectory.FullName, m_reelsFileName);
        try
        {
            using StreamReader reader = new StreamReader(reelsFilePath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] letters = line.Split(" ");
                if (!initializedReels)
                {
                    InitializeReels(letters.Length);
                }

                for (int i = 0; i < letters.Length; i++)
                {
                    char c = letters[i][0];
                    reelsList[i].Enqueue(new Tile(c));
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"There was an exception reading the file '{m_reelsFileName}': {ex.Message}");
            return null;
        }
        
        // Reels should start at random positions as a slot machine would end at random positions
        foreach (Queue<Tile> reel in reelsList)
        {
            reel.Shuffle();
        }
        
        stopwatch.Stop();
        
        Console.WriteLine($"Reels initialized successfully ({stopwatch.Elapsed.TotalMilliseconds}ms)");
        return reelsList;
        
        //--------------------------------------------------------------------------------------------------------------
        // Local Methods
        //--------------------------------------------------------------------------------------------------------------
        void InitializeReels(int nReels)
        {
            for (int i = 0; i < nReels; i++)
            {
                reelsList.Add(new Queue<Tile>());
            }

            initializedReels = true;
        }
    }
        
    //------------------------------------------------------------------------------------------------------------------
    private async Task<Dictionary<char, int>> LoadScoresAsync()
    {
        Dictionary<char, int> scores = null;
        await Task.Run(() => { scores = LoadScoresData(); });
        return scores;
    }
    
    //------------------------------------------------------------------------------------------------------------------ 
    private Dictionary<char, int> LoadScoresData()
    {
        Console.WriteLine($"Initializing scores for file: {m_scoresFileName}");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        Dictionary<char, int> scores = new Dictionary<char, int>();
        string scoresFilePath = Path.Combine(m_resourcesDirectory.FullName, m_scoresFileName);
        try
        {
            using StreamReader reader = new StreamReader(scoresFilePath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] split = line.Split(" ");
                char letter = split[0][0];
                int score = int.Parse(split[1]);
                scores.Add(letter, score);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"There was an exception reading the file '{scoresFilePath}': {ex.Message}");
            return null;
        }
        stopwatch.Stop();
        
        Console.WriteLine($"Scores table initialized successfully ({stopwatch.Elapsed.TotalMilliseconds}ms)");
        return scores;
    }
}
