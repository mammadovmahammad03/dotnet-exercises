using Xunit;

public class MarkdownTests
{
    private static void AssertMarkdown(string markdown, string expectedHtml)
    {
        var result = Markdown.Parse(markdown);
        Assert.Equal(expectedHtml, result);
    }

    [Fact]
    public void NormalTextIsParsedAsParagraph()
    {
        AssertMarkdown("This will be a paragraph", "<p>This will be a paragraph</p>");
    }

    [Fact]
    public void ItalicTextIsParsedCorrectly()
    {
        AssertMarkdown("_This will be italic_", "<p><em>This will be italic</em></p>");
    }

    [Fact]
    public void BoldTextIsParsedCorrectly()
    {
        AssertMarkdown("__This will be bold__", "<p><strong>This will be bold</strong></p>");
    }

    [Fact]
    public void MixedTextFormattingIsParsedCorrectly()
    {
        var markdown = "This will _be_ __mixed__";
        var expected = "<p>This will <em>be</em> <strong>mixed</strong></p>";
        AssertMarkdown(markdown, expected);
    }

    [Theory]
    [InlineData("# This will be an h1", "<h1>This will be an h1</h1>")]
    [InlineData("## This will be an h2", "<h2>This will be an h2</h2>")]
    [InlineData("###### This will be an h6", "<h6>This will be an h6</h6>")]
    public void HeadersAreParsedCorrectly(string markdown, string expectedHtml)
    {
        AssertMarkdown(markdown, expectedHtml);
    }

    [Fact]
    public void UnorderedListsAreParsedCorrectly()
    {
        var markdown = "* Item 1\n* Item 2";
        var expected = "<ul><li>Item 1</li><li>Item 2</li></ul>";
        AssertMarkdown(markdown, expected);
    }

    [Fact]
    public void MixedContentIsParsedCorrectly()
    {
        var markdown = "# Header!\n* __Bold Item__\n* _Italic Item_";
        var expected = "<h1>Header!</h1><ul><li><strong>Bold Item</strong></li><li><em>Italic Item</em></li></ul>";
        AssertMarkdown(markdown, expected);
    }

    [Fact]
    public void HeaderWithMarkdownSymbolsInText()
    {
        var markdown = "# This is a header with # and * in the text";
        var expected = "<h1>This is a header with # and * in the text</h1>";
        AssertMarkdown(markdown, expected);
    }

    [Fact]
    public void ListItemWithMarkdownSymbolsInText()
    {
        var markdown = "* Item 1 with a # in the text\n* Item 2 with * in the text";
        var expected = "<ul><li>Item 1 with a # in the text</li><li>Item 2 with * in the text</li></ul>";
        AssertMarkdown(markdown, expected);
    }

    [Fact]
    public void ParagraphWithMarkdownSymbolsInText()
    {
        var markdown = "This is a paragraph with # and * in the text";
        var expected = "<p>This is a paragraph with # and * in the text</p>";
        AssertMarkdown(markdown, expected);
    }

    [Fact]
    public void UnorderedListsCloseProperly()
    {
        var markdown = "# Start a list\n* Item 1\n* Item 2\nEnd a list";
        var expected = "<h1>Start a list</h1><ul><li>Item 1</li><li>Item 2</li></ul><p>End a list</p>";
        AssertMarkdown(markdown, expected);
    }
}