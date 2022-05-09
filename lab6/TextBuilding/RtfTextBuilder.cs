using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class RtfTextBuilder : TextBuilder
{
    protected const string 
        RTF_BEGIN = "{\\rtf1 \r\n",
        RTF_END = "}\r\n",
        P_END = "\\par\r\n";

    protected static IDictionary<TextStyles, string> 
        StylesAndTagNames = new SortedDictionary<TextStyles, string> {
        { TextStyles.Bold, "\\b" },
        { TextStyles.Italic, "\\i" },
        { TextStyles.Underline, "\\ul" },
        { TextStyles.Strikethru, "\\strike" }
    };

    public RtfTextBuilder () : base () { }

    public RtfTextBuilder (TextBuilder source) : base (source) { }

    public RtfTextBuilder (IEnumerable<TextChunk> chunks) : base (chunks) { }

    public override void SaveTo (TextWriter writer)
    {
        writer.Write(RTF_BEGIN);
        foreach (TextChunk chunk in this.chunks) {
            if (chunk == TextChunk.Paragraph) {
                writer.Write(P_END);
            }
            else {
                bool regionOpen = false;
                foreach (var kvPair in StylesAndTagNames) {
                    if (chunk.Style.HasFlag(kvPair.Key)) {
                        if (!regionOpen) {
                            regionOpen = true;
                            writer.Write("{");
                        }
                        writer.Write(kvPair.Value);
                    }
                }

                if (regionOpen) {
                    writer.Write(" {0}}}", chunk.Text.RtfEncoded());
                }
                else {
                    writer.Write(chunk.Text.RtfEncoded());
                }
            }
        }
        writer.Write(RTF_END);
        writer.Flush();
    }
}
