using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

class StatsView : Form
{
    public StatsView (TextStats stats) : base()
    {
        this.Text = "Statistics";
        this.ClientSize = new Size(240, 280);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Padding = new Padding(5);
        this.BackColor = Styles.BackgroundColor;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("Size: {0:F2} Kb\n", stats.KbCount);
        sb.AppendFormat("{0} pages\n", stats.PageCount);
        sb.AppendFormat("{0} lines\n", stats.LineCount);
        sb.AppendFormat(" └ {0} empty lines\n", stats.EmptyLineCount);

        sb.AppendFormat("{0} chars\n", stats.CharCount);
        sb.AppendFormat(" ├ {0} letters\n", stats.LetterCount);
        sb.AppendFormat(" │  ├ {0} vowels\n", stats.VowelCount);
        sb.AppendFormat(" │  └ {0} consonants\n", stats.ConsCount);
        sb.AppendFormat(" ├ {0} digits\n", stats.DigitCount);
        sb.AppendFormat(" ├ {0} punctuations\n", stats.PunctCount);
        sb.AppendFormat(" └ {0} other symbols\n", stats.OtherCount);

        this.Controls.Add (new Label {
            Dock = DockStyle.Fill,
            Text = sb.ToString(),
            Font = Styles.MonospaceFont
        });
    }
}
