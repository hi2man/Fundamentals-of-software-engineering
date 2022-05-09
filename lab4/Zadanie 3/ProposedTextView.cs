using System;
using System.Windows.Forms;
using System.Drawing;

class ProposedTextView : Form
{
    public ProposedTextView(string oldText, string newText) : base()
    {
        this.Text = "Comparison";
        this.ClientSize = new Size(900, 600);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Styles.BackgroundColor;

        var panel = new TableLayoutPanel {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(0)
        };  
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        panel.Controls.Add (
            new TextBox { 
                Text = oldText, 
                Font = Styles.MonospaceFont,
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(0)    
            }, 
            row: 0, column: 0
        );
        panel.Controls.Add (
            new TextBox { 
                Text = newText, 
                Font = Styles.MonospaceFont,
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(0)
            }, 
            row: 0, column: 1
        );

        var acceptButton = new Button {
            Text = "Accept new version",
            Dock = DockStyle.Bottom
        };
        acceptButton.Click += (sender, args) => {
            this.DialogResult = DialogResult.OK;
        };

        this.Controls.Add(panel);
        this.Controls.Add(acceptButton);
    }
}
