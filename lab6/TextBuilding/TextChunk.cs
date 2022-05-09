using System;

public class TextChunk 
{
    public static readonly TextChunk Paragraph = new TextChunk();

    public TextStyles Style { get; set; }
    public string Text { get; set; }
}
