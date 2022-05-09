using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PlainTextParser : TextParser
{
    static string[] LineSeparators = new string[]{"\r\n", "\n"};

    public PlainTextParser (string source) : base (source) { }

    public PlainTextParser (TextReader reader) : base(reader) { }

    protected override List<TextChunk> ParseImpl (string source)
    {
        List<TextChunk> result = new List<TextChunk>();
        string[] lines = source.Split(LineSeparators, StringSplitOptions.None);
        foreach (string line in lines) {
            result.Add (new TextChunk {
                Text = line,
                Style = TextStyles.Plain
            });
            result.Add (TextChunk.Paragraph);
        }
        return result;
    }
}
