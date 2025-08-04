using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReelWords.View;

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
    private IView m_view;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public Rack(List<Queue<Tile>> reels, IView view)
    {
        m_reels = reels;
        m_view = view;
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

        m_stringBuilder.Append("------------------------------------\n");
        m_stringBuilder.Append("    ");
        foreach (Tile tile in m_rack)
        {
            m_stringBuilder.Append($" {tile} ");
        }
        m_stringBuilder.Append("    ");
        m_stringBuilder.Append("\n------------------------------------");
        
        m_view.DisplayTextLine(m_stringBuilder.ToString());
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public bool TryPlay(string word, out int score)
    {
        score = 0;
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
        // Update the reels correspondent to the tiles that have been used and calculate the score
        for (int i = 0; i < tempRack.Count; i++)
        {
            char tileLetter = tempRack[i];
            if (tileLetter == c_tilePlayedMarker)
            {
                Queue<Tile> reel = m_reels[i];
                Tile tile = reel.Dequeue();
                reel.Enqueue(tile);

                score += tile.Score;
            }
        }

        // Refresh the rack with the new tiles
        Update();
        
        return true;
    }
    
}
