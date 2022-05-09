using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

class Actions 
{
    public static Stream SelectFileToOpen()
    {
        var dialog = new OpenFileDialog {
            RestoreDirectory = true,
            Title = "Select a file to open",
            Filter = "All files | *.*"
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            return dialog.OpenFile();
        }
        else {
            return null;
        }
    }

    public static Stream SelectFileToSave()
    {
        var dialog = new SaveFileDialog {
            RestoreDirectory = true,
            Title = "Save as",
            Filter = "All files | *.*"
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            return dialog.OpenFile();
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
