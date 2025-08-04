using ReelWords.Config;
using ReelWords.Validation;
using Xunit;

namespace ReelWordsTests.ValidationTests;

public class WordValidatorTests
{
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private WordValidator m_wordValidatorEnUs;
    
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public WordValidatorTests()
    {
        m_wordValidatorEnUs = new WordValidator(LanguageConfig.en_us);
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Theory]
    [InlineData("program")]
    [InlineData("amethyst")]
    [InlineData("as")]
    [InlineData("carts")]
    [InlineData("parallel")]
    [InlineData("zebra")]
    public void ValidateWordTest_Success(string word)
    {
        Assert.True(m_wordValidatorEnUs.Validator(word));
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Theory]
    [InlineData("Program")]
    [InlineData("program's")]
    [InlineData("pr0gram")]
    [InlineData("_program ")]
    [InlineData("    program")]
    [InlineData("PrOGRaM")]
    [InlineData("wr0ng1nput")]
    [InlineData("12345")]
    [InlineData("Ångström")]
    [InlineData("éclair")]
    public void ValidateWordTest_Fail(string word)
    {
        Assert.False(m_wordValidatorEnUs.Validator(word));
    }
}
