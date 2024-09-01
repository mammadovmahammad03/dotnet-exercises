using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class Markdown
{
    private static string Wrap(string text, string tag) => $"<{tag}>{text}</{tag}>";

    private static bool IsTag(string text, string tag) => text.StartsWith($"<{tag}>");

    private static string Parse(string markdown, string delimiter, string tag)
    {
        var escapedDelimiter = Regex.Escape(delimiter);
        var pattern = $"{escapedDelimiter}(.+?){escapedDelimiter}";
        var replacement = $"<{tag}>$1</{tag}>";

        return Regex.Replace(markdown, pattern, replacement);
    }

    private static string Parse__(string markdown) => Parse(markdown, "__", "strong");

    private static string Parse_(string markdown) => Parse(markdown, "_", "em");

    private static string ParseText(string markdown, bool list)
    {
        var parsedText = Parse_(Parse__(markdown));

        return list ? parsedText : Wrap(parsedText, "p");
    }

    private static string ParseHeader(string markdown, bool list, out bool inListAfter)
    {
        int count = markdown.TakeWhile(c => c == '#').Count();

        if (count == 0)
        {
            inListAfter = list;
            return null;
        }

        var headerHtml = Wrap(markdown[(count + 1)..].TrimStart(), $"h{count}");

        inListAfter = false;

        return list ? $"</ul>{headerHtml}" : headerHtml;
    }

    private static string ParseLineItem(string markdown, bool list, out bool inListAfter)
    {
        if (markdown.StartsWith("*"))
        {
            string innerHtml = Wrap(ParseText(markdown[1..].TrimStart(), true), "li");
            inListAfter = true;

            return list ? innerHtml : $"<ul>{innerHtml}";
        }

        inListAfter = list;

        return null;
    }

    private static string ParseParagraph(string markdown, bool list, out bool inListAfter)
    {
        inListAfter = false;

        return list ? $"</ul>{ParseText(markdown, false)}" : ParseText(markdown, list);
    }

    private static string ParseLine(string markdown, bool list, out bool inListAfter)
    {
        string result = ParseHeader(markdown, list, out inListAfter)
                    ?? ParseLineItem(markdown, list, out inListAfter)
                    ?? ParseParagraph(markdown, list, out inListAfter)
                    ?? throw new ArgumentException(ErrorTokens.InvalidMarkdown);

        return result;
    }

    public static string Parse(string markdown)
    {
        var lines = markdown.Split('\n');
        var result = new StringBuilder();
        var list = false;

        foreach (var line in lines)
        {
            var lineResult = ParseLine(line, list, out list);
            result.Append(lineResult);
        }

        if (list)
        {
            result.Append("</ul>");
        }

        return result.ToString();
    }
}