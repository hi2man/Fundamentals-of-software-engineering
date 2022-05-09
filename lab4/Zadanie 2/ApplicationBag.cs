using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class ApplicationBag: Singleton<ApplicationBag>
{
    private HashSet<Form> forms;

    protected ApplicationBag()
    {
        this.forms = new HashSet<Form>();
    }

    public void Add (Form form) 
    {
        this.forms.Add(form);
    }

    public void Remove (Form form)
    {
        this.forms.Remove(form);
        if (this.forms.Count == 0) Application.Exit();
    }
}
