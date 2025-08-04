using System.Collections.Generic;
using System.Text;
using ReelWords.Game;
using ReelWords.View;
using Xunit;

namespace ReelWordsTests.GameTests;

public class RackTests
{
    //------------------------------------------------------------------------------------------------------------------
    // Constants
    //------------------------------------------------------------------------------------------------------------------
    private const string c_testWordSuccess = "program";
    private const string c_testWordFail = "walnut";
    private const int c_rows = 6;
    private const int c_columns = 7;
    private readonly char[,] c_reelsTest = new char[c_rows, c_columns]{ 
        { 'm', 'a', 'r', 'g', 'o', 'r', 'p' }, 
        { 'h', 'i', 'j', 'k', 'l', 'm', 'n' }, 
        { 'o', 'p', 'q', 'r', 's', 't', 'u' }, 
        { 'v', 'w', 'x', 'y', 'z', 'a', 'b' }, 
        { 'c', 'd', 'e', 'f', 'g', 'h', 'i' }, 
        { 'j', 'k', 'l', 'm', 'n', 'o', 'p' }
    };
    
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private readonly Rack m_rack;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public RackTests()
    {
        List<Queue<Tile>> reels = CreateReels();
        m_rack = new Rack(reels, new ConsoleView(Encoding.UTF8));
    }
    
    //------------------------------------------------------------------------------------------------------------------
    private List<Queue<Tile>> CreateReels()
    {
        List<Queue<Tile>> reels = new List<Queue<Tile>>();

        for (int i = 0; i < c_columns; i++)
        {
            for (int j = 0; j < c_rows; j++)
            {
                if (j == 0)
                {
                    reels.Add(new Queue<Tile>());
                }

                char c = c_reelsTest[j, i];
                Tile newTile = new Tile(c);
                newTile.Score = 1;
                reels[i].Enqueue(newTile);
            }
        }
        
        return reels;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Fact]
    public void TryPlayTest_Success()
    {
        // Ensure that the current rack contains the letters from the first row in reels test data
        for(int i = 0; i < c_columns; i++)
        {
            Assert.True(m_rack.CurrentRack[i].Letter == c_reelsTest[0, i]);
        }
        
        bool success = m_rack.TryPlay(c_testWordSuccess, out int score);
        Assert.True(success);
        Assert.True(score == c_testWordSuccess.Length);
        
        // Ensure that the current rack contains the letters from the second row in reels test data
        for(int i = 0; i < c_columns; i++)
        {
            Assert.True(m_rack.CurrentRack[i].Letter == c_reelsTest[1, i]);
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Fact]
    public void TryPlayTest_Fail()
    {
        // Ensure that the current rack contains the letters from the first row in reels test data
        for(int i = 0; i < c_columns; i++)
        {
            Assert.True(m_rack.CurrentRack[i].Letter == c_reelsTest[0, i]);
        }
        
        bool success = m_rack.TryPlay(c_testWordFail, out int score);
        Assert.False(success);
        Assert.True(score == 0);
        
        // Ensure that the current rack STILL contains the letters from the first row in reels test data
        for(int i = 0; i < c_columns; i++)
        {
            Assert.True(m_rack.CurrentRack[i].Letter == c_reelsTest[0, i]);
        }
    }
}
