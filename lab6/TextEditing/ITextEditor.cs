using System;

interface ITextEditor
{
    string Text { get; set; }
    string Caption { get; }
    bool TryNew();
    bool TrySave();
    bool TryOpen();
    void TryUndo();
    void TryRedo();
    bool TryClose();
    void ShowStats();
    void ShowSearch();
    void ShowRemovedSpaces();
    void SetFileProvider(InFileProvider provider);
    event Action TextChanged;
    event Action CaptionChanged;
}
