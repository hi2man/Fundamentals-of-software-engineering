using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

class Actions 
{
    public static InFileProvider SelectFileToOpen()
    {
        var dialog = new OpenFileDialog {
            RestoreDirectory = true,
            Title = "Select a file to open",
            Filter = "All files | *.*"
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            string filename = dialog.SafeFileName;
            TextParser parser = null;
            StreamReader sReader = new StreamReader(dialog.OpenFile());
            if (filename.EndsWith(".rtf")) {
                parser = new RtfTextParser(sReader);
            }
            else {
                parser = new PlainTextParser(sReader);
            }
            return new InFileProvider {
                FileName = filename,
                Parser = parser
            };
        }
        else {
            return null;
        }
    }

    public static OutFileProvider SelectFileToSave()
    {
        var dialog = new SaveFileDialog {
            RestoreDirectory = true,
            Title = "Save as",
            Filter = "All files | *.*"
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            string filepath = dialog.FileName;
            TextBuilder builder = null;
            if (filepath.EndsWith(".rtf")) {
                builder = new RtfTextBuilder();
            }
            else if (filepath.EndsWith(".html")) {
                builder = new HtmlTextBuilder();
            }
            else {
                builder = new PlainTextBuilder();
            }
            return new OutFileProvider {
                FileName = filepath,
                Builder = builder,
                Write = () => {
                    builder.SaveTo(File.Create(filepath));
                }
            };
        }
        else {
            return null;
        }
    }

    public static void ShowStatsPopup (TextStats stats)
    {
        var statsView = new StatsView(stats);
        statsView.ShowDialog();
    }

    public static bool ShowProposedTextPopup (string oldText, string newText)
    {
        var proposedTextView = new ProposedTextView(oldText, newText);
        return proposedTextView.ShowDialog() == DialogResult.OK;
    }

    public static void ShowSearch (string text)
    {
        var SearchView = new SearchView(text);
        SearchView.ShowDialog();
    }
}
