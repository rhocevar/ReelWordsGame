using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReelWords.Game;

public class Rack
{
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private const char c_tilePlayedMarker = '*';
    
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private List<Queue<Tile>> m_reels;
    private List<Tile> m_rack;
    private StringBuilder m_stringBuilder;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public Rack(List<Queue<Tile>> reels)
    {
        m_reels = reels;
        m_rack = new List<Tile>();
        m_stringBuilder = new StringBuilder();
        Update();
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        m_rack.Clear();
        foreach (Queue<Tile> reel in m_reels)
        {
            Tile tile = reel.Peek();
            m_rack.Add(tile);
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public void Display()
    {
        m_stringBuilder.Clear();

        m_stringBuilder.Append("\n------------------------------------\n");
        
        m_stringBuilder.Append("    ");
        foreach (Tile t in m_rack)
        {
            m_stringBuilder.Append(" ");
            string letter = t.ToString().ToUpper();
            m_stringBuilder.Append(letter);
            m_stringBuilder.Append(" ");
        }
        m_stringBuilder.Append("    \n");

        m_stringBuilder.Append("------------------------------------");
        
        Console.WriteLine(m_stringBuilder.ToString());
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public bool TryPlay(string word)
    {
        // Check if the player has enough letters on their rack to create this word
        if (m_rack.Count < word.Length)
        {
            return false;
        }

        // Create a temporary rack to simulate the play. Used tiles are replaced with a marker.
        List<char> tempRack = m_rack.Select(t => t.Letter).ToList();
        foreach (char letter in word)
        {
            bool hasLetterInRack = false;
            for (int i = 0; i < tempRack.Count; i++)
            {
                char tileLetter = tempRack[i];
                if (tileLetter == letter)
                {
                    tempRack[i] = c_tilePlayedMarker;
                    hasLetterInRack = true;
                    break;
                }
            }

            if (!hasLetterInRack)
            {
                return false;
            }
        }

        // If we reached this far, the play was successful.
        // Update the reels correspondent to the tiles that have been used
        for (int i = 0; i < tempRack.Count; i++)
        {
            char tileLetter = tempRack[i];
            if (tileLetter == c_tilePlayedMarker)
            {
                Queue<Tile> reel = m_reels[i];
                Tile tile = reel.Dequeue();
                reel.Enqueue(tile);
            }
        }

        // Refresh the rack with the new tiles
        Update();
        
        return true;
    }
    
}
