using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Skills : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    int ishod = 0;
    int isrm = 0;
    string empName;
    bool hideDescription = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            ViewState["HideDescription"] = true;
            binddlyears();
            string year = dlyear.SelectedItem.Value;
            bindgetSkills(year);
        }
    }

    private void ApplyDescriptionToggle()
    {
        bool hideDesc = ViewState["HideDescription"] != null &&
                        Convert.ToBoolean(ViewState["HideDescription"]);

        // column visibility
        SkillsList.Columns[3].Visible = !hideDesc;

        // same CSS logic as button click
        if (hideDesc)
        {
            SkillsList.Attributes["class"] =
                "table table-bordered bordered-ratings";

            SkillsList.AllowPaging = false;
        }
        else
        {
            SkillsList.Attributes["class"] =
                "table table-bordered";

            SkillsList.AllowPaging = true;
            SkillsList.PageSize = 10;
        }
    }
    public void binddlyears()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct Year from [dbo].[SkillsUserwise] order by Year desc");
        dlyear.DataSource = dt;
        dlyear.DataTextField = "Year";
        dlyear.DataTextField = "Year";
        dlyear.DataBind();
    }

    protected void btnToggleDescription_Click(object sender, EventArgs e)
    {
        if (SkillsList.Columns[3].Visible)
        {
            SkillsList.Attributes["class"] =
                "table table-bordered bordered-ratings";

        }
        else
        {
            SkillsList.Attributes["class"] =
                "table table-bordered";
        }
        bool isVisible = SkillsList.Columns[3].Visible;

        SkillsList.Columns[3].Visible = !isVisible;

        hideDescription = !SkillsList.Columns[3].Visible;

        ViewState["HideDescription"] = hideDescription;

        if (!SkillsList.Columns[3].Visible)
        {
            // Description hidden
            SkillsList.AllowPaging = false;
        }
        else
        {
            // Description visible
            SkillsList.AllowPaging = true;
            SkillsList.PageSize = 10;
        }

        string year = dlyear.SelectedItem.Value;
        bindgetSkills(year);
    }
   
    public void bindgetSkills(string year)
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id,EmpName,IsHOD,IsRM from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        if (res.Rows.Count > 0)
        {
            DataRow row2 = res.Rows[0];
            int empId = int.Parse(row2["Id"].ToString());
            empName = row2["EmpName"].ToString();
            ishod = int.Parse(row2["IsHOD"].ToString());
            isrm = int.Parse(row2["IsRM"].ToString());

            string str = txtsearchuser.Text.Trim();
            DataTable dt;

            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
            ManagementSkills, Description, RatingSelf, RMRating, HODRating, OverallScore, HOD, RM,ConsentRating,SDM,ASM,Year from SkillsUserwise suw
            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id WHERE (EmpId="
            + empId + " or HOD = (select EmpName from EmployeeMaster where id=" + empId + ") OR " +
            "RM = (select EmpName from EmployeeMaster where id=" + empId + ") or SDM = (select EmpName from EmployeeMaster where id=" + empId + ") " +
            "or ASM = (select EmpName from EmployeeMaster where id=" + empId + ")) and EmpName like '%" + str + "%' and suw.year = " + year + " order by EmpName");

            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
              ManagementSkills, Description, RatingSelf, RMRating, HODRating, OverallScore, HOD, RM,ConsentRating,SDM,ASM,Year from SkillsUserwise suw
              left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id WHERE (EmpId="
              + empId + " or HOD = (select EmpName from EmployeeMaster where id=" + empId + ") OR " +
              "RM = (select EmpName from EmployeeMaster where id=" + empId + ") or SDM = (select EmpName from EmployeeMaster where id=" + empId + ") " +
              "or ASM = (select EmpName from EmployeeMaster where id=" + empId + ")) and suw.year = " + year + " order by EmpName");

            }


            SkillsList.DataSource = dt;
            ApplyDescriptionToggle();
            
            SkillsList.DataBind();

            if (isrm == 0)
            {
                SkillsList.Columns[14].Visible = false;
            }

            if (ishod == 0)
            {
                SkillsList.Columns[15].Visible = false;
            }
            else
            {
                SkillsList.Columns[14].Visible = true;
            }

            if (ishod == 1 || isrm == 1)
            {
                SkillsList.Columns[17].Visible = true;
            }
            else
            {
                SkillsList.Columns[17].Visible = false;
            }


            //previlegeLogic(dt, empName);
        }
    }

    protected void SkillsList_DataBound(object sender, EventArgs e)
    {
        MergeGridRows();
    }

    private void MergeGridRows()
    {
        for (int rowIndex = SkillsList.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = SkillsList.Rows[rowIndex];
            GridViewRow previousRow = SkillsList.Rows[rowIndex + 1];

            string currentCompetency = row.Cells[1].Text.Trim();
            string nextCompetency = previousRow.Cells[1].Text.Trim();

            string currentManagement = row.Cells[2].Text.Trim();
            string nextManagement = previousRow.Cells[2].Text.Trim();

            bool hideDesc =
     ViewState["HideDescription"] != null &&
     Convert.ToBoolean(ViewState["HideDescription"]);

            if (hideDesc)
            {
               
                if (currentCompetency == nextCompetency &&
                    currentManagement == nextManagement)
                {
                    MergeCell(row, previousRow, 0);
                    MergeCell(row, previousRow, 1);
                    MergeCell(row, previousRow, 2);

                    MergeCell(row, previousRow, 4);
                    MergeCell(row, previousRow, 5);
                    MergeCell(row, previousRow, 6);
                    MergeCell(row, previousRow, 7);
                    MergeCell(row, previousRow, 8);
                     
                    MergeCell(row, previousRow, 13);
                    MergeCell(row, previousRow, 14);
                    MergeCell(row, previousRow, 15);
                    MergeCell(row, previousRow, 17);
                    MergeCell(row, previousRow, 18);
                }
            }
            else
            {
               
                if (currentCompetency == nextCompetency &&
                    currentManagement == nextManagement)
                {

                    MergeCell(row, previousRow, 4);
                    MergeCell(row, previousRow, 0);

                    MergeCell(row, previousRow, 1);

                    MergeCell(row, previousRow, 2);

                    MergeCell(row, previousRow, 13);

                    MergeCell(row, previousRow, 14);

                    MergeCell(row, previousRow, 15);

                    MergeCell(row, previousRow, 17);

                    MergeCell(row, previousRow, 18);
                }
            }
        }
    }

    private void MergeCell(GridViewRow row,
                         GridViewRow previousRow,
                         int cellIndex)
    {
        if (previousRow.Cells[cellIndex].RowSpan < 2)
        {
            row.Cells[cellIndex].RowSpan = 2;
        }
        else
        {
            row.Cells[cellIndex].RowSpan =
                previousRow.Cells[cellIndex].RowSpan + 1;
        }

        previousRow.Cells[cellIndex].Visible = false;

     //   if (cellIndex == 5 ||  // Consent
     //cellIndex == 6 ||  // RM
     //cellIndex == 7 ||  // HOD
     //cellIndex == 8)    // Overall
     //   {
     //       row.Cells[cellIndex].Style["background-color"] = "#f8fafc";

     //       row.Cells[cellIndex].Style["border-left"] =
     //           "1px solid #94a3b8";

     //       row.Cells[cellIndex].Style["border-right"] =
     //           "1px solid #94a3b8";

     //       // IMPORTANT
     //       row.Cells[cellIndex].Style["box-shadow"] =
     //           "inset 1px 0 0 #94a3b8, inset -1px 0 0 #94a3b8";
     //   }
    }
    //protected void previlegeLogic(DataTable dt, string empName)
    //{
    //    for (int grow = 0; grow < SkillsList.Rows.Count; grow++)
    //    {
    //        string gdEmp = SkillsList.Rows[grow].Cells[0].Text;
    //        int gridId = int.Parse(((Label)SkillsList.Rows[grow].FindControl("SkillId")).Text);

    //        foreach (DataRow row in dt.Rows)
    //        {
    //            string hod = row["HOD"].ToString();
    //            string rm = row["RM"].ToString();
    //            string rowEmp = row["EmpName"].ToString();
    //            string RatingSelf = row["RatingSelf"].ToString();
    //            string RMRating = row["RMRating"].ToString();
    //            string HODRating = row["HODRating"].ToString();
    //            string consentrating = row["ConsentRating"].ToString();
    //            int rowId = int.Parse(row["Id"].ToString());

    //            if (gridId == rowId) //gridEmp == dbrowEmp 
    //            {
    //                //if (rowEmp != empName)
    //                //{
    //                //    //self rating off dbrowEmp != loggedinempName && 
    //                //    SkillsList.Rows[grow].Cells[8].Style.Add("visibility", "hidden");
    //                //}
    //                //else if (RatingSelf != "")
    //                //{
    //                //    //hiding self rating if given already
    //                //    SkillsList.Rows[grow].Cells[8].Style.Add("visibility", "hidden");
    //                //}
    //                if (rowEmp == empName) //check this logic must be for logged in emp
    //                {
    //                    //if (hod != "")
    //                    //{
    //                    //    //if not blank then his hod is some one other thne hide
    //                    //    //hod rating and overallscore

    //                    //    //SkillsList.Columns[5].Visible = false;
    //                    //    //SkillsList.Columns[6].Visible = false;
    //                    //    //SkillsList.Columns[7].Visible = false;
    //                    //    //SkillsList.Rows[grow].Cells[5].Style.Add("visibility", "hidden");
    //                    //    SkillsList.Rows[grow].Cells[6].Style.Add("visibility", "hidden");
    //                    //    SkillsList.Rows[grow].Cells[7].Style.Add("visibility", "hidden");

    //                    //}
    //                    //if (rm != "")
    //                    //{
    //                    //    //if not blank then his rm is some one other thne hide
    //                    //    //rm rating and overallscore

    //                    //    SkillsList.Rows[grow].Cells[5].Style.Add("visibility", "hidden");
    //                    //    //SkillsList.Rows[grow].Cells[6].Style.Add("visibility", "hidden");
    //                    //    SkillsList.Rows[grow].Cells[7].Style.Add("visibility", "hidden");
    //                    //}
    //                }

    //                //if (hod != "" || rm != "") // his hod or rm is someone other
    //                //{
    //                //    if (rm != empName || RMRating != "")
    //                //    {
    //                //        //rm off if rm not empName means rm is some one other
    //                //        SkillsList.Rows[grow].Cells[9].Style.Add("visibility", "hidden");
    //                //    }
    //                //    if (hod != empName || HODRating != "")
    //                //    {
    //                //        //hod off if hod not blank means hod is some one other
    //                //        SkillsList.Rows[grow].Cells[10].Style.Add("visibility", "hidden");
    //                //    }
    //                //    if (hod == empName)
    //                //    {
    //                //        if(RMRating == "" && rm != empName)
    //                //        {
    //                //            SkillsList.Rows[grow].Visible = false;
    //                //        }
    //                //    }
    //                //    if (RatingSelf == "" && (rm == empName || hod == empName))
    //                //    {
    //                //        //hide row if employee not given self rating and
    //                //        //his hod or rm is someone other
    //                //        SkillsList.Rows[grow].Visible = false;
    //                //    }

    //                //}
    //                //else if(RMRating != "")
    //                //{
    //                //    SkillsList.Rows[grow].Cells[9].Style.Add("visibility", "hidden");
    //                //}
    //                //else if(HODRating != "")
    //                //{
    //                //    SkillsList.Rows[grow].Cells[10].Style.Add("visibility", "hidden");
    //                //}
    //                //if(ishod > 0)
    //                //{
    //                //    if(consentrating != "")
    //                //    {
    //                //        SkillsList.Rows[grow].Cells[12].Style.Add("visibility", "hidden");
    //                //    }
    //                //}
    //            }
    //        }
    //    }
    //}

    protected void btnsubmit_click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in SkillsList.Rows)
        {
            string Id = (row.Cells[7].FindControl("SkillId") as Label).Text;
            string RatingSelf = (row.Cells[6].FindControl("txtratingself") as TextBox).Text;
            string RatingRM = (row.Cells[8].FindControl("txtratingrm") as TextBox).Text;
            string RatingHOD = (row.Cells[7].FindControl("txthodrating") as TextBox).Text;
            string consentrating = (row.Cells[7].FindControl("txtconsentrating") as TextBox).Text;

            if (RatingSelf != "")
            {
                objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET RatingSelf = " + RatingSelf + "WHERE Id = " + Id + " and year=datepart(year,GETDATE())");
            }
            if (RatingRM != "")
            {
                objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET RMRating = " + RatingRM + "WHERE Id = " + Id + " and year=datepart(year,GETDATE())");
            }
            if (RatingHOD != "")
            {
                objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET HODRating = " + RatingHOD + "WHERE Id = " + Id + " and year=datepart(year,GETDATE())");
            }
            if (consentrating != "")
            {
                objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET ConsentRating = " + consentrating + "WHERE Id = " + Id + " and year=datepart(year,GETDATE())");
            }
            //objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET OverallScore = (coalesce(RatingSelf ,0) + coalesce(RMRating ,0)+ coalesce(HODRating ,0)) WHERE Id = " + Id);
        }
        Response.Redirect("Skills.aspx");
    }

    protected void SkillsList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            bool hideDesc =
    ViewState["HideDescription"] != null &&
    Convert.ToBoolean(ViewState["HideDescription"]);

            if (!hideDesc)
            {
                // Description visible
                e.Row.Cells[2].Style["white-space"] = "normal";
                e.Row.Cells[3].Style["white-space"] = "nowrap";
            }
            else
            {
                // Description hidden
                e.Row.Cells[2].Style["white-space"] = "normal";
                e.Row.Cells[3].Style["white-space"] = "normal";
            }

            e.Row.Cells[0].Style["text-align"] = "center"; // Employee
            e.Row.Cells[1].Style["text-align"] = "center"; // Competency
            e.Row.Cells[2].Style["text-align"] = "center"; // ManagementSkills
            e.Row.Cells[18].Style["text-align"] = "center"; // Year
            e.Row.Cells[4].Style["text-align"] = "center"; // Self Rating
           // e.Row.Cells[2].Style["white-space"] = "nowrap";
           
            e.Row.Cells[0].Style["vertical-align"] = "middle";
            e.Row.Cells[1].Style["vertical-align"] = "middle";
            e.Row.Cells[2].Style["vertical-align"] = "middle";
            e.Row.Cells[18].Style["vertical-align"] = "middle";
            e.Row.Cells[4].Style["vertical-align"] = "middle";

            string currEmp = (((DataRowView)e.Row.DataItem)["EmpName"]).ToString();
            string hod = (((DataRowView)e.Row.DataItem)["HOD"]).ToString();
            string rm = (((DataRowView)e.Row.DataItem)["RM"]).ToString();
            string RatingSelf = (((DataRowView)e.Row.DataItem)["RatingSelf"]).ToString();
            string RMRating = (((DataRowView)e.Row.DataItem)["RMRating"]).ToString();
            string HODRating = (((DataRowView)e.Row.DataItem)["HODRating"]).ToString();
            string consentrating = (((DataRowView)e.Row.DataItem)["ConsentRating"]).ToString();
            string SDM = (((DataRowView)e.Row.DataItem)["SDM"]).ToString();
            string ASM = (((DataRowView)e.Row.DataItem)["ASM"]).ToString();

            if (currEmp != empName)
            {
                //self rating off dbrowEmp != loggedinempName && 
                e.Row.Cells[13].Style.Add("visibility", "hidden");
            }
            else if (RatingSelf != "")
            {
                //hiding self rating if given already
                e.Row.Cells[13].Style.Add("visibility", "hidden");
            }

            //if (RatingSelf == "" && (rm == empName || hod == empName))
            //{
            //    //hide row if employee not given self rating and
            //    //his hod or rm is someone other
            //    e.Row.Visible = false; //not show row till self rating is pending 
            //}

            if (empName == rm)
            {
                if (RMRating == "")
                {
                    e.Row.Cells[14].Style.Add("visibility", "display"); //rm rating box
                }
                else
                {
                    e.Row.Cells[14].Style.Add("visibility", "hidden");//rm rating box
                }
                e.Row.Cells[6].Style.Add("visibility", "display"); // rm rating col
                e.Row.Cells[8].Style.Add("visibility", "display"); // overall score col
                if (consentrating != "")
                {
                    e.Row.Cells[17].Style.Add("visibility", "hidden"); //consent rating box
                }
                else
                {
                    e.Row.Cells[17].Style.Add("visibility", "display"); //consent rating box
                }
                e.Row.Cells[5].Style.Add("visibility", "display"); //consent rating col
                e.Row.Cells[15].Style.Add("visibility", "hidden"); //hod rating box
            }
            else
            {
                e.Row.Cells[14].Style.Add("visibility", "hidden"); //rm rating box
                e.Row.Cells[6].Style.Add("visibility", "hidden"); // rm rating col
                e.Row.Cells[8].Style.Add("visibility", "hidden"); // overall score col
                e.Row.Cells[17].Style.Add("visibility", "hidden"); //consent rating box

            }
            if (empName == hod)
            {
                e.Row.Cells[5].Style.Add("visibility", "display"); //consent rating col
                e.Row.Cells[6].Style.Add("visibility", "display"); // rm rating col
                e.Row.Cells[8].Style.Add("visibility", "display"); // overall score col
                e.Row.Cells[14].Style.Add("visibility", "display"); //rm rating box
                if (consentrating != "")
                {
                    e.Row.Cells[17].Style.Add("visibility", "hidden"); //consent rating box
                }
                else
                {
                    e.Row.Cells[17].Style.Add("visibility", "display"); //consent rating box
                }
                if (HODRating != "")
                {
                    e.Row.Cells[15].Style.Add("visibility", "hidden"); //hod rating box
                }
                else
                {
                    e.Row.Cells[15].Style.Add("visibility", "display"); //hod rating box
                }

            }
            else
            {
                e.Row.Cells[15].Style.Add("visibility", "hidden"); //hod rating box
                if (empName != rm)
                {
                    e.Row.Cells[5].Style.Add("visibility", "hidden"); //consent rating col
                    e.Row.Cells[6].Style.Add("visibility", "hidden"); // rm rating col
                    e.Row.Cells[14].Style.Add("visibility", "hidden"); //rm rating box
                    e.Row.Cells[8].Style.Add("visibility", "hidden"); // overall score col
                    e.Row.Cells[7].Style.Add("visibility", "hidden"); // hod rating col
                }

            }
            if (SDM == empName || ASM == empName)
            {
                e.Row.Cells[8].Style.Add("visibility", "display"); // overall score col
                e.Row.Cells[5].Style.Add("visibility", "display"); //consent rating col
                e.Row.Cells[6].Style.Add("visibility", "display"); // rm rating col
                e.Row.Cells[7].Style.Add("visibility", "display"); // hod rating col
            }


            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchuser.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);


            //if (hod != "" || rm != "") // his hod or rm is someone other
            //{
            //    if (rm != empName || RMRating != "")
            //    {
            //        //rm off if rm not empName means rm is some one other
            //        e.Row.Cells[11].Style.Add("visibility", "hidden"); //box
            //    }
            //    if (hod != empName || HODRating != "")
            //    {
            //        //hod off if hod not blank means hod is some one other
            //        e.Row.Cells[12].Style.Add("visibility", "hidden"); //box
            //    }
            //    if (hod == empName)
            //    {
            //        if (RMRating == "" && rm != empName)
            //        {
            //            e.Row.Visible = false; //not show row till rm rating pending if rm is some other
            //        }
            //    }
            //    if (RatingSelf == "" && (rm == empName || hod == empName))
            //    {
            //        //hide row if employee not given self rating and
            //        //his hod or rm is someone other
            //        e.Row.Visible = false; //not show row till self rating is pending 
            //    }

            //}
            //else if (RMRating != "")
            //{
            //    e.Row.Cells[11].Style.Add("visibility", "hidden");
            //}
            //else if (HODRating != "")
            //{
            //    e.Row.Cells[12].Style.Add("visibility", "hidden");
            //}
            //if (consentrating != "")
            //{
            //    e.Row.Cells[14].Style.Add("visibility", "hidden");
            //}


            //if (empName != hod)
            //{
            //    //if not blank then his hod is some one other thne hide
            //    //hod rating and overallscore

            //    e.Row.Cells[6].Style.Add("visibility", "hidden"); //col
            //    e.Row.Cells[7].Style.Add("visibility", "hidden"); //col
            //}
            //else if (consentrating != "")
            //{
            //    e.Row.Cells[14].Style.Add("visibility", "hidden");
            //}
            //if (empName != rm)
            //{
            //    //if not blank then his rm is some one other thne hide
            //    //rm rating and overallscore

            //    e.Row.Cells[5].Style.Add("visibility", "hidden"); //col
            //    //SkillsList.Rows[grow].Cells[6].Style.Add("visibility", "hidden");
            //    e.Row.Cells[7].Style.Add("visibility", "hidden"); //col
            //}
            //else if (consentrating != "")
            //{
            //    e.Row.Cells[14].Style.Add("visibility", "hidden");
            //}

        }


    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //slow
        bool OT_FLAG = false;
        foreach (GridViewRow row in SkillsList.Rows)
        {
            string Id = (row.Cells[7].FindControl("SkillId") as Label).Text;
            string RatingSelf = (row.Cells[6].FindControl("txtratingself") as TextBox).Text;
            string RatingRM = (row.Cells[8].FindControl("txtratingrm") as TextBox).Text;
            string RatingHOD = (row.Cells[7].FindControl("txthodrating") as TextBox).Text;
            string consentrating = (row.Cells[7].FindControl("txtconsentrating") as TextBox).Text;

            if (RatingSelf != "" || RatingRM != "" || RatingHOD != "" || consentrating != "")
            {
                OT_FLAG = true;
                break;

            }
            //else
            //{
            //    SkillsList.PageIndex = e.NewPageIndex;
            //    this.bindgetSkills();
            //}
        }
        if (OT_FLAG)
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>alert('Please submit the added ratings, or it will be not submitted!');</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "alert", strconfirm, false);
        }
        else
        {
            SkillsList.PageIndex = e.NewPageIndex;
            string year = dlyear.SelectedValue;

            this.bindgetSkills(year);
        }

        //SkillsList.PageIndex = e.NewPageIndex;
        //this.bindgetSkills();
    }
    protected void Search(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        this.bindgetSkills(year);
    }
    protected void dlyears_SelectedIndexChanged(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        bindgetSkills(year);
    }

//    protected void btnForPrev_Click(object sender, EventArgs e)
//    {
//        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO SkillsUserwise( Id, EmpId, Competency, ManagementSkills, Description, [Year])
//SELECT Id, EmpId, Competency, ManagementSkills, Description, YEAR(GETDATE()) AS[Year]
//FROM SkillsUserwise
//WHERE[Year] = YEAR(GETDATE()) - 1");


//       /* DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id,EmpName, EmpId,Competency, 
//              ManagementSkills, Description from SkillsUserwise suw
//			  left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id
//              where year = DATEPART(YEAR, DATEADD(YEAR, -1, GETDATE()))");*/

//    }


}
