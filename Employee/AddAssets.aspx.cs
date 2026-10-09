using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_AddAssets : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static DataTable assetList;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            assetList = new DataTable();
            assetList.Columns.Add("AssetName");

            if (Request.QueryString.AllKeys.Contains("id"))
            {

                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT AssetName from [dbo].[CompanyAssetList] where AssetId=" + Request.QueryString["id"].ToString());
                if (dt.Rows.Count > 0)
                {
                    txtassetname.Text = dt.Rows[0]["AssetName"].ToString();

                    btnSubmit.Text = "Update";
                    btnAdd.Visible = false;
                }
                //else
                //    btnsave.Text = "Save";
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        assetList.Rows.Add(txtassetname.Text);
        gdAssets.DataSource = assetList;
        gdAssets.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Update")
        {
            objSQLDataFile.ExecuteDataTableSQL(@"UPDATE CompanyAssetList SET AssetName='" + txtassetname.Text + "' where AssetId=" + Request.QueryString["id"].ToString());
            
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Assets Updated')){window.location.href='AssetList.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
        else
        {
            string date = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            foreach (GridViewRow row in gdAssets.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string assetname = row.Cells[0].Text;
                    objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO CompanyAssetList VALUES('" + assetname + "', '" + Session["EmailId"].ToString() + "', '" + date + "')");
                }
            }

            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Assets Added')){window.location.href='AssetList.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
        
    }

    protected void gdAssets_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            int row = int.Parse(e.CommandArgument.ToString());
            assetList.Rows.RemoveAt(row);
            assetList.AcceptChanges();
            gdAssets.DataSource = assetList;
            gdAssets.DataBind();

        }
    }
}