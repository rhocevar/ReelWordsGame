using ReelWords.Data;
using Xunit;

namespace ReelWordsTests.DataTests;

public class TrieTests
{
    //------------------------------------------------------------------------------------------------------------------
    // Constants
    //------------------------------------------------------------------------------------------------------------------
    private const string c_singleWord = "parallel";
    private readonly string[] m_multipleWords =
    {
        "can",
        "carts",
        "cart",
        "cats",
        "card"
    };

    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private readonly Trie m_trie;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public TrieTests()
    {
        // Create a new trie before each test
        m_trie = new Trie();
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Fact]
    public void TrieInsertTest_SingleWord()
    {
        m_trie.Insert(c_singleWord);
        Assert.True(m_trie.Search(c_singleWord));
        Assert.True(m_trie.Count == c_singleWord.Length + 1); // Also considering the root
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Fact]
    public void TrieDeleteTest_SingleWord()
    {
        m_trie.Insert(c_singleWord);
        m_trie.Delete(c_singleWord);
        Assert.False(m_trie.Search(c_singleWord));
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Fact]
    public void TrieInsertTest_MultipleWords()
    {
        foreach (string word in m_multipleWords)
        {
            m_trie.Insert(word);
        }
        
        foreach (string word in m_multipleWords)
        {
            Assert.True(m_trie.Search(word));
        }

        Assert.True(m_trie.Count == 10);
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Fact]
    public void TrieDeleteTest_MultipleWords()
    {
        foreach (string word in m_multipleWords)
        {
            m_trie.Insert(word);
            Assert.True(m_trie.Search(word));
        }
        
        foreach (string word in m_multipleWords)
        {
            m_trie.Delete(word);
            Assert.False(m_trie.Search(word));
        }
        
        Assert.True(m_trie.Count == 1); // Only the root node should be left
    }
}
