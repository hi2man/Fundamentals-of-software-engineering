using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class TextEditorReadonly : Multiton<TextEditorReadonly>, ITextEditor
{
    private string text;
    private string fileName;

    private IEnumerable<TextChunk> chunks;

    public Action Close { get; set; }
    public Func<OutFileProvider> SaveSelector { get; set; }
    public Action<TextStats> ShowStatsPopup { get; set; }
    public Action<string> ShowSearchPopup { get; set; }

    public event Action TextChanged;
    public event Action CaptionChanged;

    protected TextEditorReadonly ()
    {

    }

    public virtual string Text { 
        get {
            return this.text;
        } 
        set {
            this.text = value;
            this.TextChanged.Invoke();
        } 
    }

    public string Caption {
        get {
            return String.Format("{0} (Readonly) - Bloknot", this.fileName);
        }
    }

    public void TryUndo()
    {
    }

    public void TryRedo()
    {
    }

    public bool TryNew()
    {
        return false;
    }

    public bool TryClose()
    {
        try {
            Delete(this);
            if (this.Close != null) this.Close();
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    public bool TryOpen() 
    {
        return false;
    }

    public void SetFileProvider(InFileProvider provider)
    {
        this.fileName = provider.FileName;
        this.CaptionChanged.Invoke();
        this.chunks = provider.Parser.Chunks;
        this.Text = new PlainTextBuilder(provider.Parser.Chunks).ConvertToString();
    }

    public bool TrySave()
    {
        try {
            OutFileProvider provider = this.SaveSelector();
            provider.Builder.AddManyTextChunks(this.chunks);
            provider.Write();
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    public void ShowStats()
    {
        this.ShowStatsPopup (new TextStats(this.Text, Encoding.UTF8));
    }

    public void ShowSearch()
    {
        this.ShowSearchPopup (this.Text);
    }

    public void ShowRemovedSpaces()
    {
    }
}
