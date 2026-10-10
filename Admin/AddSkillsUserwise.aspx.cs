using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddSkillsUserwise : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static DataTable dtemplist;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            bindgrdskills();
            binddldepartments();
            binddldesignations();
            dtemplist = new DataTable();
           // dtemplist.Columns.Add("Competency");
            dtemplist.Columns.Add("ManagementSkills");
            dtemplist.Columns.Add("EmpName");
            dtemplist.Columns.Add("EmpId");
            dtemplist.Columns.Add("Id");
        }
    }
    public void bindgrdskills()
    {

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from Skills  ");
        dlskills.DataSource = dt;
        //changed this to assign skills dropdown 
        // dlskills.DataTextField = "Competency";
        dlskills.DataTextField = "ManagementSkills";
        dlskills.DataValueField = "id";
        dlskills.DataBind();
        //dlskills.Items.Add(new ListItem("Select KPI", "0"));


    }
    public void binddldepartments()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT * FROM Departments");
        dldepartments.DataSource = dt;
        dldepartments.DataTextField = "Departments";
        dldepartments.DataTextField = "Departments";
        dldepartments.DataBind();
    }
    public void binddldesignations()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpDesignation from [dbo].[EmployeeMaster] where EmpDesignation != ''");
        dlDesg.DataSource = dt;
        dlDesg.DataTextField = "EmpDesignation";
        dlDesg.DataTextField = "EmpDesignation";
        dlDesg.DataBind();
    }
    public void searchemp(string empname)
    {
        if (empname.Trim() != "")
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster where EmpName like '%" + empname + "%'");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }
        else
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster  ");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }
    }
    protected void txtautosearch_TextChanged(object sender, EventArgs e)
    {
        searchemp(txtautosearch.Text);
    }
    protected void dlskills_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dlskills.SelectedValue != "0")
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from Skills  where id=" + dlskills.SelectedValue);
            grdskills.DataSource = dt;
            grdskills.DataBind();

        }
        else
        {
            string message = "Select KPI";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }
    }
    protected void btnAddEmp_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in EmployeeList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                if (chkRow.Checked)
                {

                    string empname = row.Cells[1].Text;
                    string lblEmployeeCode = (row.Cells[2].FindControl("lblEmpId") as Label).Text;

                    dtemplist.Rows.Add(dlskills.SelectedItem.Text, empname, lblEmployeeCode, dlskills.SelectedValue);
                }
            }
        }
        grdAssignSkills.DataSource = dtemplist;
        grdAssignSkills.DataBind();
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (dlskills.SelectedValue != "0")
        {
            User user = new User();
            if (grdAssignSkills.Rows.Count > 0)
            {
                foreach (GridViewRow row in grdAssignSkills.Rows)
                {
                    string empId = (row.Cells[3].FindControl("EmpId") as Label).Text;
                    string SkillId = (row.Cells[3].FindControl("SkillId") as Label).Text;
                    DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Competency, ManagementSkills, Description from Skills  where id=" + SkillId);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row2 = dt.Rows[0];

                        string Competency = row2["Competency"].ToString();
                        string ManagementSkills = row2["ManagementSkills"].ToString();
                        string Description = row2["Description"].ToString();

                        user.AddSkillsUW(empId, Competency, ManagementSkills, Description, Session["EmailId"].ToString());
                    }
                }
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Skills Assinged Successfully')){window.location.href='AddSkillsUserwise.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else if (dlroles.Text != "" && dldepartments.Text != "")
            {
                int checkroles = int.Parse(dlroles.SelectedValue);
                //User user = new User();
                foreach (GridViewRow row in grdskills.Rows)
                {
                    string Competency = row.Cells[0].Text;
                    string ManagementSkills = row.Cells[1].Text; ;
                    string Description = row.Cells[2].Text;
                    DataTable emps = null;
                    switch (checkroles)
                    {
                        case 1:
                            emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where IsHOD = 1 and EmpDepartment = '" + dldepartments.Text + "'");
                            break;
                        case 2:
                            emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where IsRM = 1 and EmpDepartment = '" + dldepartments.Text + "'");
                            break;
                        case 3:
                            emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where IsHOD = 0 and IsRM = 0 and EmpDepartment = '" + dldepartments.Text + "'");
                            break;
                    }
                    if (emps.Rows.Count > 0)
                    {
                        for (int i = 0; i < emps.Rows.Count; i++)
                        {
                            DataRow emprow = emps.Rows[i];
                            string EmpId = emprow["Id"].ToString();
                            user.AddSkillsUW(EmpId, Competency, ManagementSkills, Description, Session["EmailId"].ToString());
                        }
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('Skills Assigned Successfully')){window.location.href='AssingedSkillsList.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                    else
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('There is some Error Please try again')){window.location.href='AddSkillsUserwise.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                }
            }

            //original code

              else if (dlDesg.Text != "")
              {

                  //User user = new User();
                  foreach (GridViewRow row in grdskills.Rows)
                  {
                      string Competency = row.Cells[0].Text;
                      string ManagementSkills = row.Cells[1].Text;
                      string Description = row.Cells[2].Text;

                      DataTable emps = null;

                      //original code
                     // emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where EmpDesignation = '" + dlDesg.Text + "' and cast(DOJ as date) < cast('2021-12-31' as date) and DOJ != ''");

                      // condition to assign skills to emp designation wise whose doj between 01-01-2022 and 31-12-2022 and status = active

                       emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where EmpDesignation = '" + dlDesg.Text + "' and CAST(DOJ AS DATE) BETWEEN CAST('2022-01-01' AS DATE) AND CAST('2022-12-31' AS DATE) and DOJ != '' and Status = 'Active'");

                      if (emps.Rows.Count > 0)
                      {
                          for (int i = 0; i < emps.Rows.Count; i++)
                          {
                              DataRow emprow = emps.Rows[i];
                              string EmpId = emprow["Id"].ToString();
                              user.AddSkillsUW(EmpId,Competency,ManagementSkills,Description, Session["EmailId"].ToString());
                          }
                          ClientScriptManager CSM = Page.ClientScript;
                          string strconfirm = " <script>if(window.confirm('Skills Assigned Successfully')){window.location.href='AssingedSkillsList.aspx'}</script>";
                          CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                      }
                      else
                      {
                          ClientScriptManager CSM = Page.ClientScript;
                          string strconfirm = " <script>alert'Employees not found');</script>";
                          CSM.RegisterClientScriptBlock(this.GetType(), "Alert", strconfirm, false);
                      }

                  }
              }

        }

        //test code (if not worked then remove below written elseif block) 
        //till now it is working when tested (04052023) - need to test again 

      /* else if(dlskills.SelectedValue == "0")
        {
            User user = new User();
            if (dlDesg.Text != "" && dlDesg.SelectedValue != "")
            {
                string designation = dlDesg.Text;

                // Get the skills assigned to the selected designation
                DataTable skills = objSQLDataFile.ExecuteDataTableSQL(@"SELECT distinct Competency, ManagementSkills, Description FROM SkillsUserwise suw
                                                           JOIN EmployeeMaster emp ON suw.EmpId = emp.Id
                                                           WHERE emp.EmpDesignation = '" + designation + "' and suw.Year = year(getdate()) -1 ");

                if (skills.Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdskills.Rows)
                    {
                        string Competency = row.Cells[0].Text;
                        string ManagementSkills = row.Cells[1].Text;
                        string Description = row.Cells[2].Text;

                        DataTable emps = null;

                        // Get the employees with the selected designation
                        emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where EmpDesignation = '" + designation + "' and CAST(DOJ AS DATE) BETWEEN CAST('2022-01-01' AS DATE) AND CAST('2022-12-31' AS DATE) and DOJ != '' and Status = 'Active'");

                        if (emps.Rows.Count > 0)
                        {
                            for (int i = 0; i < emps.Rows.Count; i++)
                            {
                                DataRow emprow = emps.Rows[i];
                                string EmpId = emprow["Id"].ToString();

                                // Assign the skills to each employee
                                foreach (DataRow skillrow in skills.Rows)
                                {
                                    string skillCompetency = skillrow["Competency"].ToString();
                                    string skillManagementSkills = skillrow["ManagementSkills"].ToString();
                                    string skillDescription = skillrow["Description"].ToString();

                                    user.AddSkillsUW(EmpId, skillCompetency, skillManagementSkills, skillDescription, Session["EmailId"].ToString());
                                }
                            }

                            // Show success message
                            ClientScriptManager CSM = Page.ClientScript;
                            string strconfirm = " <script>if(window.confirm('Skills Assigned Successfully')){window.location.href='AssingedSkillsList.aspx'}</script>";
                            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                        }
                        else
                        {
                            // Show error message if no employees with the selected designation are found
                            ClientScriptManager CSM = Page.ClientScript;
                            string strconfirm = " <script>alert('Employees not found');</script>";
                            CSM.RegisterClientScriptBlock(this.GetType(), "Alert", strconfirm, false);
                        }
                    }
                }
            }
        }*/

        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>alert('Please Select Skill or Check all the fields!');</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }
   
    protected void btnaddskills_click(object sender, EventArgs e)
    {
        Response.Redirect("AddSkills.aspx");
    }
    protected void btnskilllist_click(object sender, EventArgs e)
    {
        Response.Redirect("AssingedSkillsList.aspx");
    }

    protected void dlroles_TextChanged(object sender, EventArgs e)
    {
        dlDesg.Visible = false;
        txtautosearch.Visible = false;
        btnAddEmp.Visible = false;
    }

    protected void dlDesg_TextChanged(object sender, EventArgs e)
    {
        dlroles.Visible = false;
        dldepartments.Visible = false;
        txtautosearch.Visible = false;
        btnAddEmp.Visible = false;

    }


    protected void dlDesg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dlDesg.SelectedValue != "")
        {
            string designation = dlDesg.SelectedValue;
          

         /*   DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Competency, ManagementSkills, Description FROM SkillsUserwise suw
                                                            JOIN EmployeeMaster emp ON suw.EmpId = emp.Id
                                                            WHERE emp.EmpDesignation = '" + designation + "' AND emp.Status = 'Active' AND CAST(emp.DOJ AS DATE) BETWEEN CAST('2022-01-01' AS DATE) AND CAST('2022-12-31' AS DATE)");*/


            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT distinct Competency, ManagementSkills, Description FROM SkillsUserwise suw
                                                           JOIN EmployeeMaster emp ON suw.EmpId = emp.Id
                                                           WHERE emp.EmpDesignation = '" + designation + "' and suw.Year = year(getdate()) -1 ");


            grdskills.DataSource = dt;
            grdskills.DataBind();
        }
    }

   
}