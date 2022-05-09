using System;

class OutFileProvider 
{
    public string FileName { get; set; }
    public TextBuilder Builder { get; set; }
    public Action Write { get; set; }
}
