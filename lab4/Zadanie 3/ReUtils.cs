using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

static class ReUtils
{
    static string 
        RE_EMAIL = @"(?<=(^|\s))[a-z0-9-]+@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?(?=($|\W))",
        RE_SURNAME_NAME = @"\b\p{Lu}\p{Ll}+\s\p{Lu}\p{Ll}+\b",
        RE_SURNAME_INIT = @"\b\p{Lu}\p{Ll}+\s\p{Lu}\.\p{Lu}\.(?=($|\W))",
        RE_DOMAINS_UA = @"(?<=(^|\s))(?i)([a-z]+[a-z0-9-]*[a-z0-9]\.){1,60}(com|net|org|edu|in)\.ua(?=($|\W))",
        RE_ENQUOT_REAL = @"(\p{Pi}|\p{Pf}|\""|\')(\+|\-)??[0-9]+\.[0-9]+(\p{Pi}|\p{Pf}|\""|\')",
        RE_ENQUOT_INT = @"(\p{Pi}|\p{Pf}|\""|\')(\+|\-)??[0-9]+(\p{Pi}|\p{Pf}|\""|\')",
        RE_ENQUOT_COMPLEX = @"(\p{Pi}|\p{Pf}|\""|\')(\s*((\+|\-)??\s*[0-9]+(\.[0-9]+)??)??\s*(\+|\-))??\s*([0-9]+(\.[0-9]+)??)??i\s*(\p{Pi}|\p{Pf}|\""|\')",
        RE_POSTAL_CODE = @"\b[0-9]{5}\b",
        RE_POSTAL_ADDRESS = @"(?<=(^|\s))((\p{Lu}\p{Ll}+)((\s+\p{Lu}\p{Ll}+){1,2}|(\s+\p{Lu}\.\s*\p{Lu}\.))\s*\,)??(\w|\-|\s|\d|\.){5,30},\s*((\w+\.\s??)??\d{1,4}(/\d{1,3}|\w)??){1,3},((\w|\-|\s|\d|\.){5,20},){1,3}\s*[0-9]{5}(?=($|\W))",
        RE_CSHARP_KEYWORDS = @"\b(abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|while)\b";
    
    public static IDictionary<string, Regex> KnownRegex = new SortedDictionary<string, Regex> {
        { "Email", new Regex(RE_EMAIL) },
        { "Surname Name", new Regex(RE_SURNAME_NAME) },
        { "Surname N.P.", new Regex(RE_SURNAME_INIT) },
        { "(com|net|org|edu|in).ua domains", new Regex(RE_DOMAINS_UA) },
        { "Enquoted integers", new Regex(RE_ENQUOT_INT) },
        { "Enquoted reals", new Regex(RE_ENQUOT_REAL) },
        { "Enquoted complex", new Regex(RE_ENQUOT_COMPLEX) },
        { "5-digit Postal code", new Regex(RE_POSTAL_CODE) },
        { "Postal address", new Regex(RE_POSTAL_ADDRESS) },
        { "C Sharp keywords", new Regex(RE_CSHARP_KEYWORDS) }
    };

    public static string FindAndReport (string text, Regex regex) {
        StringBuilder sb = new StringBuilder();
        int length = text.Length;
        List<int> linebreaks = new List<int> { 0 };
        for (int i=0; i<length; i++) {
            if (text[i] == '\n') linebreaks.Add(i+1);
        }
        int line = 0;
        int linePos = 0;
        foreach (Match match in regex.Matches(text)) {
            while (linebreaks.Count > 0 && match.Index >= linebreaks[0]) {
                linePos = linebreaks[0];
                line++;
                linebreaks.RemoveAt(0);
            }
            sb.AppendFormat ("Line {0}, pos {1}: {2} \r\n", 
                             line, match.Index - linePos, match.Value);
        }
        return sb.ToString();
    }

    public static string FindAndReport (string text, string pattern) {
        try {
            return FindAndReport(text, new Regex(pattern));
        }
        catch (ArgumentException ex) {
            return string.Format ("Exception: {0} \r\n{1}", 
                                  ex.Message, ex.StackTrace);
        }
    }
}
