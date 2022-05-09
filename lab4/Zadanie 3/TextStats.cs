using System;
using System.Linq;
using System.Text;

class TextStats
{
    public double KbCount { get; private set; }
    public int CharCount { get; private set; }
    public int LineCount { get; private set; }
    public int PageCount { get; private set; }
    public int EmptyLineCount { get; private set; }

    public int DigitCount { get; private set; }
    public int LetterCount { get; private set; }
    public int PunctCount { get; private set; }
    public int VowelCount { get; private set; }
    public int ConsCount { get; private set; }
    public int OtherCount { get; private set; }

    public TextStats (string text, Encoding encoding)
    {
        if (text==null) text = string.Empty;
        this.KbCount = (double)(encoding.GetByteCount(text)) / 1024;
        this.CharCount = text.Length;
        this.LineCount = text.Count(c => c == '\n');
        if (!text.EndsWith("\n")) this.LineCount += 1;
        this.PageCount = this.CharCount / PageCharCount + 1;
        this.EmptyLineCount = CountEmptyLines(text);
        
        this.DigitCount = 0;
        this.LetterCount = 0;
        this.PunctCount = 0;
        this.VowelCount = 0;
        this.ConsCount = 0;
        this.OtherCount = 0;
        foreach (char c in text) {
            if (Char.IsDigit(c)) {
                this.DigitCount++;
            }
            else if (Char.IsLetter(c)) {
                this.LetterCount++;
                if (Vowels.Contains(c)) {
                    this.VowelCount++;
                }
                else {
                    this.ConsCount++;
                }
            }
            else if (Char.IsPunctuation(c)) {
                this.PunctCount++;
            }
            else {
                this.OtherCount++;
            }
        }
    }

    static int PageCharCount = 1800;

    static char[] Vowels = "aeiouyаоуеиіяюєїёыэ".ToCharArray();

    static int CountEmptyLines (string text)
    {
        int count = 0;
        int length = text.Length - 1;
        for (int i=0; i<length; i++) {
            if (text[i]=='\n' && (text[i+1]=='\n' || text[i+1]=='\r')) {
                count++;
            } 
        }
        return count;
    }
}
