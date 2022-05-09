using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public abstract class TextBuilder
{
    protected List<TextChunk> chunks;

    // Initialization
    public TextBuilder ()
    {
        this.chunks = new List<TextChunk>();
    }

    public TextBuilder (TextBuilder source) : this ()
    {
        this.chunks.AddRange(source.chunks);
    }

    public TextBuilder (IEnumerable<TextChunk> chunks) : this ()
    {
        this.chunks.AddRange(chunks);
    }

    public void AddTextChunk (TextChunk chunk)
    {
        this.chunks.Add(chunk);
    }

    public void AddManyTextChunks (IEnumerable<TextChunk> chunks) 
    {
        this.chunks.AddRange(chunks);
    }

    public void AddText (string text, TextStyles style = TextStyles.Plain) 
    {
        this.chunks.Add (new TextChunk {
            Text = text,
            Style = style
        });
    }

    public void Paragraph ()
    {
        this.chunks.Add (TextChunk.Paragraph);
    }

    public void SaveTo (Stream stream) 
    {
        using (var writer = new StreamWriter(stream)) {
            this.SaveTo(writer);
            writer.Flush();
        }
    }

    public string ConvertToString () 
    {
        StringBuilder sb = new StringBuilder();
        this.SaveTo(new StringWriter(sb));
        return sb.ToString();
    }

    public abstract void SaveTo (TextWriter writer);
}
