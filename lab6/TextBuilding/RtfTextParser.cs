using System;
using System.IO;
using System.Collections.Generic;

public class RtfTextParser : TextParser
{
    public RtfTextParser (string source) : base (source) { }

    public RtfTextParser (TextReader reader) : base(reader) { }

    static IDictionary<string, TextStyles> 
        TagsAndStyles = new Dictionary<string, TextStyles> {
        { "b", TextStyles.Bold },
        { "i", TextStyles.Italic },
        { "ul", TextStyles.Underline },
        { "strike", TextStyles.Strikethru }
    };

    static string escapedChars = "}{\\";
    static Predicate<char> isEscaped = (c) => (escapedChars.IndexOf(c) >= 0);
    static Predicate<char> isDirective = (c) => (char.IsLetterOrDigit(c));

    protected override List<TextChunk> ParseImpl (string source)
    {
        List<TextChunk> result = new List<TextChunk>();

        int length = source.Length;

        char[] chars = new char[length];
        int chunkLength = 0;

        Stack<TextStyles> styles = new Stack<TextStyles>();
        styles.Push(TextStyles.Plain);

        Action chunkIsDone = () => {
            if (chunkLength == 0) return;
            string text = new String(chars, 0, chunkLength);
            result.Add (new TextChunk {
                Text = text,
                Style = styles.Peek()
            });
            chunkLength = 0;
        };

        Action doDirective = () => {
            string dString = new String(chars, 0, chunkLength);
            chunkLength = 0;
            if (dString == "par") {
                result.Add(TextChunk.Paragraph);
                return;
            }
            TextStyles newStyle;
            if (TagsAndStyles.TryGetValue(dString, out newStyle)) {
                styles.Push(styles.Pop() | newStyle);
                return;
            }
        };

        for (int index = 0; index < length; index++) {
            char current = source[index];
            if ((index < length - 1) && (current == '\\')) {
                char next = source[++index];
                if (isEscaped(next)) {
                    chars[chunkLength++] = next;
                }
                else {
                    chunkIsDone();
                    for (; index < length; index++) {
                        char c = source[index];
                        if (isDirective(c)) {
                            chars[chunkLength++] = c;
                        }
                        else {
                            doDirective();
                            if (c != ' ') index--;
                            break;
                        }
                    }
                }
            }
            else if (current == '}') {
                chunkIsDone();
                styles.Pop();
            }
            else if (current == '{') {
                if (index > 0) {
                    chunkIsDone();
                    styles.Push(styles.Peek());
                }
            }
            else if (current != '\n' && current != '\r') {
                chars[chunkLength++] = current;
            }
        }
        chunkIsDone();
        return result;
    }
}
