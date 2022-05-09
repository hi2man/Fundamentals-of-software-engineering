using System;
using System.Text;

static class Utils
{
    static string[] LineSeparators = new string[]{"\r\n", "\n"};
    static string[] Whitespaces = new string[]{" ", "\t"};
    static StringSplitOptions NoEmpty = StringSplitOptions.RemoveEmptyEntries;

    public static string RemoveSpaces (string text)
    {
        string[] lines = text.Split(LineSeparators, NoEmpty);
        for (int i=0; i<lines.Length; i++) {
            lines[i] = String.Join(" ", lines[i].Split(Whitespaces, NoEmpty));
        }
        return String.Join("\r\n", lines);
    }
}
