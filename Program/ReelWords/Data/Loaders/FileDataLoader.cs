using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ReelWords.Config;
using ReelWords.Game;
using ReelWords.Utilities;
using ReelWords.Validation;
using ReelWords.View;

namespace ReelWords.Data.Loaders;

/// <summary>
/// Loads data asynchronously from multiple files contained in the specified directory. A directory with the given name
/// should be present upward in the directory tree and contain the files for the dictionary, reels and scores.
/// 
/// A language can be specified on initialization, so that the files correspondent to that language will be used for the
/// data load. For this assessment, only american english (en-us) is supported.
/// 
/// The Load function returns a 'ReelWordsData' data structure which contains the words added to a Trie data structure,
/// the reels and a word validator for the specified language.
/// </summary>
public class FileDataLoader : IDataLoader
{
    //------------------------------------------------------------------------------------------------------------------
    // Constants
    //------------------------------------------------------------------------------------------------------------------
    private const string c_enUsFileName = "american-english-large.txt";
    private const string c_defaultReelsFileName = "reels.txt";
    private const string c_defaultScoresFileName = "scores.txt";
    
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private readonly IView m_view;
    private DirectoryInfo m_directory;
    private WordValidator m_wordValidator;
    private string m_wordsFileName;
    private string m_reelsFileName;
    private string m_scoresFileName;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public FileDataLoader(LanguageConfig languageConfig, IView view, string directoryName, uint maxWordLength)
    {
        m_view = view;
        SetLanguage(languageConfig, maxWordLength);
        SetDirectory(directoryName);
    }
    
    //------------------------------------------------------------------------------------------------------------------ 
    private void SetLanguage(LanguageConfig languageConfig, uint maxWordLength)
    {
        switch (languageConfig)
        {
            case LanguageConfig.en_us:
            {
                m_wordsFileName = c_enUsFileName;
                m_reelsFileName = c_defaultReelsFileName;
                m_scoresFileName = c_defaultScoresFileName;
                m_wordValidator = new WordValidator(languageConfig, maxWordLength);
                m_view.DisplayTextLine($"Language set to '{languageConfig}'. Max word length is '{maxWordLength}'");
                break;
            }
            case LanguageConfig.en_gb:
            {
                m_view.DisplayTextLine($"Language config {languageConfig} is not yet supported.");
                throw new NotImplementedException();
            }
            case LanguageConfig.pt_br:
            {
                m_view.DisplayTextLine($"Language config {languageConfig} is not yet supported.");
                throw new NotImplementedException();
            }
            default:
            {
                m_view.DisplayTextLine($"Language config {languageConfig} is not yet supported.");
                throw new NotImplementedException();
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    private void SetDirectory(string directoryName)
    {
        m_directory = Utils.TryGetDirectoryInfo(directoryName);
        if (m_directory == null)
        {
            m_view.DisplayTextLine($"Unable to find {directoryName} directory.");
            throw new DirectoryNotFoundException();
        }
        m_view.DisplayTextLine("File directory found: " + m_directory);
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
            m_view.DisplayTextLine($"There was a problem initializing the language dictionary: {loadWordsTask.Exception}");
            return null;
        }
        
        if (!loadReelsTask.IsCompletedSuccessfully || loadReelsTask.Result == null)
        {
            m_view.DisplayTextLine($"There was a problem initializing the reels data: {loadReelsTask.Exception}");
            return null;
        }
        
        if (!loadScoresTask.IsCompletedSuccessfully || loadScoresTask.Result == null)
        {
            m_view.DisplayTextLine($"There was a problem initializing the scores table: {loadReelsTask.Exception}");
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
                    m_view.DisplayTextLine($"Error: Could not find a score for letter '{tile.Letter}'");
                    return null;
                }
            }
        }

        return new ReelWordsData(words, reels, m_wordValidator);
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
        m_view.DisplayTextLine($"Initializing words for file: {m_wordsFileName}");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        Trie trie = new Trie();
        int validWordsCount = 0, invalidWordsCount = 0;
        string wordsFilePath = Path.Combine(m_directory.FullName, m_wordsFileName);
        try
        {
            using StreamReader reader = new StreamReader(wordsFilePath);
            string word;
            while ((word = reader.ReadLine()) != null)
            {
                if (m_wordValidator.IsValid(word))
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
            m_view.DisplayTextLine($"There was an exception reading the file '{wordsFilePath}': {ex.Message}");
            return null;
        }
        stopwatch.Stop();
        
        m_view.DisplayTextLine($"Found a total of {validWordsCount + invalidWordsCount} words in dictionary file." + 
                               $" Valid: {validWordsCount}. Invalid: {invalidWordsCount}." + 
                               $" Number of nodes: {trie.Count}");
        
        m_view.DisplayTextLine($"Words dictionary initialized successfully ({stopwatch.Elapsed.TotalMilliseconds}ms)");
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
        m_view.DisplayTextLine($"Initializing reels for file: {m_reelsFileName}");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        List<Queue<Tile>> reelsList = new List<Queue<Tile>>();
        bool initializedReel = false;
        string reelsFilePath = Path.Combine(m_directory.FullName, m_reelsFileName);
        try
        {
            using StreamReader reader = new StreamReader(reelsFilePath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] letters = line.Split(" ");
                if (!initializedReel)
                {
                    InitializeReel(letters.Length);
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
            m_view.DisplayTextLine($"There was an exception reading the file '{m_reelsFileName}': {ex.Message}");
            return null;
        }
        
        // Reels should start at random positions as a slot machine would end at random positions
        foreach (Queue<Tile> reel in reelsList)
        {
            reel.Shuffle();
        }
        
        stopwatch.Stop();
        
        m_view.DisplayTextLine($"Reels initialized successfully ({stopwatch.Elapsed.TotalMilliseconds}ms)");
        return reelsList;
        
        //--------------------------------------------------------------------------------------------------------------
        // Local Methods
        //--------------------------------------------------------------------------------------------------------------
        void InitializeReel(int nReels)
        {
            for (int i = 0; i < nReels; i++)
            {
                reelsList.Add(new Queue<Tile>());
            }

            initializedReel = true;
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
        m_view.DisplayTextLine($"Initializing scores for file: {m_scoresFileName}");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        Dictionary<char, int> scores = new Dictionary<char, int>();
        string scoresFilePath = Path.Combine(m_directory.FullName, m_scoresFileName);
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
            m_view.DisplayTextLine($"There was an exception reading the file '{scoresFilePath}': {ex.Message}");
            return null;
        }
        stopwatch.Stop();
        
        m_view.DisplayTextLine($"Scores table initialized successfully ({stopwatch.Elapsed.TotalMilliseconds}ms)");
        return scores;
    }
}
