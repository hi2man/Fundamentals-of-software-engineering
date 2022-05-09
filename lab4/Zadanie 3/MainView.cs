using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

class MainView : Form
{
    private ITextEditor editor;

    private TextBox textArea;
    private MenuStrip menuStrip;

    public MainView(ITextEditor editor) : base()
    {
        this.editor = editor;

        this.Text = "Bloknot";
        this.ClientSize = new Size(800, 600);
        this.FormBorderStyle = FormBorderStyle.Sizable;

        this.textArea = new TextBox {
            Multiline = true,
            AcceptsTab = true,
            AcceptsReturn = true,
            Dock = DockStyle.Fill,
            Font = Styles.MonospaceFont,
            BackColor = Styles.BackgroundColor,
            ScrollBars = ScrollBars.Vertical
        };
        this.textArea.TextChanged += (sender, args) => {
            this.editor.Text = this.textArea.Text;
        };
        this.editor.TextChanged += () => {
            this.textArea.Text = this.editor.Text;
        };

        this.Closed += (sender, args) => {
            this.editor.TryClose();
        };

        this.menuStrip = new MenuStrip {
            Dock = DockStyle.Top
        };
        var fileItem = new ToolStripMenuItem { Text = "File" };
        fileItem.DropDownItems.Add("Create new", null, (sender, args) => {
            editor.TryNew();
        });
        fileItem.DropDownItems.Add("Open file", null, (sender, args) => {
            editor.TryOpen();
        });
        fileItem.DropDownItems.Add("Save as", null, (sender, args) => {
            editor.TrySave();
        });
        var editItem = new ToolStripMenuItem { Text = "Edit" };
        editItem.DropDownItems.Add("Undo", null, (sender, args) => {
            editor.TryUndo();
        });
        editItem.DropDownItems.Add("Redo", null, (sender, args) => {
            editor.TryRedo();
        });
        editItem.DropDownItems.Add("Remove spaces", null, (sender, args) => {
            editor.ShowRemovedSpaces();
        });
        var toolItem = new ToolStripMenuItem { Text = "Tools" };
        toolItem.DropDownItems.Add("Search", null, (sender, args) => {
            editor.ShowSearch();
        });
        toolItem.DropDownItems.Add("Statistics", null, (sender, args) => {
            editor.ShowStats();
        });
        this.menuStrip.Items.Add(fileItem);
        this.menuStrip.Items.Add(editItem);
        this.menuStrip.Items.Add(toolItem);

        this.Controls.Add(this.textArea);
        this.Controls.Add(this.menuStrip);
    }
}
