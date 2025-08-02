using System;
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
    private ReelWordsData m_data;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    private GameManager() { }
    
    //------------------------------------------------------------------------------------------------------------------
    public void StartGame()
    {
        Console.WriteLine("Game initialization started...");
        DataLoader dataLoader = new DataLoader(DataLoader.LanguageConfig.en_us);
        m_data = dataLoader.Load();

        if (m_data == null)
        {
            Console.WriteLine("There was a problem loading the game data.");
            return;
        }
        Console.WriteLine("Game initialization complete.");

        Console.WriteLine("\n***************************\n***** Reel Words Game *****\n***************************");
        Console.WriteLine("--> Type '0' to end the game."); 
        while (true)
        {
            // TODO: Present the set of letters on the tray
            // ...
            
            
            Console.Write("\nCreate a word using the letters from your tray: "); 
            string input = Console.ReadLine();
            if (input == "0")
            {
                EndGame();
                return;
            }
            
            // Transform user input to lower case so it can still be considered if entered with capital letters
            input = input?.ToLower();
            
            if (!m_data.IsWordValid(input))
            {
                Console.WriteLine($"The word '{input}' is not valid. Please try again.");
                continue;
            }
            
            bool wordExists = m_data.Words.Search(input);
            if (wordExists)
            {
                // TODO: Increase score
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
}
