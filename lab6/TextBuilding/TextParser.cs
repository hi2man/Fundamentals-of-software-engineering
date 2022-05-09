using System;
using System.Collections.Generic;
using System.IO;

public abstract class TextParser
{
    protected List<TextChunk> chunks;

    public TextParser (string source)
    {
        this.chunks = this.ParseImpl(source);
    }

    public TextParser (TextReader reader) : this(reader.ReadToEnd()) { }

    public IEnumerable<TextChunk> Chunks {
        get {
            return this.chunks;
        }
    }

    protected abstract List<TextChunk> ParseImpl (string source);
}
