using System;
using System.Diagnostics;
using ReelWords.Data;
using ReelWords.Data.Loaders;
using ReelWords.View;

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
    private IView m_view;
    private ReelWordsData m_data;
    private Rack m_rack;
    private int m_totalScore;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    private GameManager() { }
    
    //------------------------------------------------------------------------------------------------------------------
    public void Initialize(IView view, IDataLoader dataLoader)
    {
        m_view = view;
        
        view.DisplayTextLine("Initializing game data...");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        m_data = dataLoader.Load();
        if (m_data == null)
        {
            view.DisplayTextLine("There was a problem loading the game data.");
            return;
        }

        m_rack = new Rack(m_data.Reels, view);
        m_totalScore = 0;
        
        stopwatch.Stop();
        
        view.DisplayTextLine($"Game data initialized successfully ({stopwatch.Elapsed.TotalMilliseconds}ms)");
        
        StartGame();
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private void StartGame()
    {
        m_view.DisplayTextLine("\n***************************\n***** Reel Words Game *****\n***************************");
        m_view.DisplayTextLine("--> Type '0' to end the game.\n"); 
        while (true)
        {
            m_view.DisplayTextLine($"Total score: {m_totalScore}");
            m_rack.Display();
            
            m_view.DisplayText("Create a word using the letters from your tray: ");
            string input;
            while (true)
            {
                input = m_view.ReadTextLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                input = input.Trim();
                
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
                m_view.DisplayTextLine($"The word '{inputUpper}' is not valid.");
                continue;
            }
            
            bool wordExists = m_data.Words.Search(inputLower);
            if (wordExists)
            {
                if (m_rack.TryPlay(inputLower, out int score))
                {
                    m_totalScore += score;
                    m_view.DisplayTextLine($"Good job! You earned {score} points from the word '{inputUpper}'.");
                }
                else
                {
                    m_view.DisplayTextLine($"The word '{inputUpper}' can't be created using the letters from your tray.");
                }
            }
            else
            {
                m_view.DisplayTextLine($"The word '{inputUpper}' does not exist in the dictionary.");
            }
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private void EndGame()
    {
        m_view.DisplayTextLine($"\nThe Reel Words Game is over. Your total score is {m_totalScore}.");
        m_view.DisplayTextLine("CONGRATULATIONS! WELL PLAYED!");
        Environment.Exit(0);
    }
}
