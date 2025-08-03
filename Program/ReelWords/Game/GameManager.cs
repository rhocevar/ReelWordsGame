using System;
using System.Diagnostics;
using ReelWords.Data;

namespace ReelWords.Game;

public class GameManager
{
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
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private static GameManager m_instance;
    private DataLoader m_dataLoader;
    private ReelWordsData m_data;
    private Rack m_rack;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    private GameManager() { }
    
    //------------------------------------------------------------------------------------------------------------------
    public void Initialize(LanguageConfig languageConfig)
    {
        Console.WriteLine("Initializing game data...");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        m_dataLoader = new DataLoader(languageConfig);
        
        m_data = m_dataLoader.Load();
        if (m_data == null)
        {
            Console.WriteLine("There was a problem loading the game data.");
            return;
        }

        m_rack = new Rack(m_data.Reels);
        
        stopwatch.Stop();
        
        Console.WriteLine($"Game data initialized successfully ({stopwatch.Elapsed.TotalMilliseconds}ms)");
        
        StartGame();
    }
    
    //------------------------------------------------------------------------------------------------------------------
    // TODO: consider moving all view related function to a View class.
    // Could maybe created an interface IView, and a class ConsoleView that implements it
    private void StartGame()
    {
        Console.WriteLine("\n***************************\n***** Reel Words Game *****\n***************************");
        Console.WriteLine("--> Type '0' to end the game."); 
        while (true)
        {
            m_rack.Display();
            
            Console.Write("Create a word using the letters from your tray: ");
            string input;
            while (true)
            {
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }
                
                if (input == "0")
                {
                    EndGame();
                    return;
                }

                break;
            }
            
            string inputLower = input.ToLower(); // Lower case used for data validation
            string inputUpper = input.ToUpper(); // Upper case used for display
            
            if (!m_data.IsWordValid(inputLower))
            {
                Console.WriteLine($"The word '{inputUpper}' is not valid.");
                continue;
            }
            
            bool wordExists = m_data.Words.Search(inputLower);
            if (wordExists)
            {
                if (m_rack.TryPlay(inputLower))
                {
                    // TODO: Increase score
                    Console.WriteLine($"Good job! You earned X points from the word '{inputUpper}'.");
                }
                else
                {
                    Console.WriteLine($"The word '{inputUpper}' can't be created using the letters from your tray.");
                }
            }
            else
            {
                Console.WriteLine($"The word '{inputUpper}' does not exist in the dictionary.");
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
}
