using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PlainTextBuilder : TextBuilder
{
    public PlainTextBuilder () : base () { }

    public PlainTextBuilder (TextBuilder source) : base (source) { }

    public PlainTextBuilder (IEnumerable<TextChunk> chunks) : base (chunks) { }

    public override void SaveTo (TextWriter writer)
    {
        foreach (TextChunk chunk in this.chunks) {
            if (chunk == TextChunk.Paragraph) {
                writer.Write("\r\n");
            } 
            else {
                writer.Write(chunk.Text);
            }
        }
        writer.Flush();
    }
}
