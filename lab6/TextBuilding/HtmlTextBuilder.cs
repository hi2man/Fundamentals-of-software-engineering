using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class HtmlTextBuilder : TextBuilder
{
    protected const string 
        HTML_BEGIN = "<!DOCTYPE html>\r\n<html>\r\n<head></head>\r\n<body>\r\n",
        HTML_END = "</body>\r\n</html>\r\n",
        P_BEGIN = "<p>",
        P_END = "</p>\r\n";

    protected static IDictionary<TextStyles, string> 
        StylesAndTagNames = new SortedDictionary<TextStyles, string> {
        { TextStyles.Bold, "b" },
        { TextStyles.Italic, "i" },
        { TextStyles.Underline, "u" },
        { TextStyles.Strikethru, "s" }
    };

    public HtmlTextBuilder () : base () { }

    public HtmlTextBuilder (TextBuilder source) : base (source) { }

    public HtmlTextBuilder (IEnumerable<TextChunk> chunks) : base (chunks) { }

    public override void SaveTo (TextWriter writer)
    {
        writer.Write(HTML_BEGIN);
        bool paragraphOpen = false;
        foreach (TextChunk chunk in this.chunks) {
            if (chunk == TextChunk.Paragraph) {
                if (paragraphOpen) {
                    writer.Write(P_END);
                    paragraphOpen = false;
                }
            }
            else {
                if (!paragraphOpen) {
                    paragraphOpen = true;
                    writer.Write(P_BEGIN);
                }
                Stack<string> tags = new Stack<string>();
                foreach (var kvPair in StylesAndTagNames) {
                    if (chunk.Style.HasFlag(kvPair.Key)) {
                        writer.Write("<{0}>", kvPair.Value);
                        tags.Push(kvPair.Value);
                    }
                }
                writer.Write(chunk.Text.HtmlEncoded());
                foreach (string tag in tags) {
                    writer.Write("</{0}>", tag);
                }
            }
        }
        if (paragraphOpen) {
            writer.Write(P_END);
        }   
        writer.Write(HTML_END);
        writer.Flush();
    }
}
