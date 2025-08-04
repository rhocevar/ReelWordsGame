using System.Collections.Generic;
using System.Text;
using ReelWords.Config;
using ReelWords.Data;
using ReelWords.Data.Loaders;
using ReelWords.Game;
using ReelWords.View;
using Xunit;

namespace ReelWordsTests.DataTests;

public class FileDataLoaderTests
{
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private readonly FileDataLoader m_fileDataLoader;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public FileDataLoaderTests()
    {
        m_fileDataLoader = new FileDataLoader(
            languageConfig:LanguageConfig.en_us, 
            view:new ConsoleView(Encoding.UTF8),
            directoryName: "ResourcesTest"
        );
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Fact]
    public void LoadTest()
    {
        ReelWordsData data = m_fileDataLoader.Load();
        Assert.NotNull(data);
        
        Assert.NotNull(data.Words);
        Assert.True(data.Words.Count > 1);
        
        Assert.NotNull(data.Reels);
        Assert.NotEmpty(data.Reels);

        // Ensure that all tiles have scores assigned
        foreach (Queue<Tile> reel in data.Reels)
        {
            foreach (Tile tile in reel)
            {
                Assert.True(tile.Score > 0);
            }
        }
    }
}
