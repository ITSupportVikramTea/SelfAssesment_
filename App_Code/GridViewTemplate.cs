using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for GridViewTemplate
/// </summary>
public class GridViewTemplate : ITemplate
{
    private string columnNameBinding;
    private string columnname;

    public GridViewTemplate(string colname, string colNameBinding)
    {
        columnNameBinding = colNameBinding;
        columnname = colname;
    }

    public void InstantiateIn(System.Web.UI.Control container)
    {
        TextBox tb = new TextBox();
        tb.ID = "txtDynamic" + columnNameBinding;
        tb.Text = columnname;//here to set the value of textbox
        container.Controls.Add(tb);
    }
}