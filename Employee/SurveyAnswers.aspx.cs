using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_SurveyAnswers : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static string SurveyId;
    static int empId;
    StringBuilder ids = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            SurveyId = Request.QueryString["id"].ToString();
            checkIsAnsSubmitted();
            fillDetails();
        }
    }

    protected void checkIsAnsSubmitted()
    {
        User user = new User();
        empId = user.getEmployeeId(Session["EmailId"].ToString());
        try
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT IsAnsSubmitted FROM AllowSurveyEmployee WHERE EmpId= " + empId + " AND SurveyId = " + SurveyId);
            string check = dt.Rows[0]["IsAnsSubmitted"].ToString();

            if (check != "" && check != null)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Already Reponded!')){window.location.href='SurveyList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }

        }
        catch (Exception ex)
        {

        }


    }
    protected void fillDetails()
    {
        try
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT SurveyTitle, SurveyDescription From survey Where Id = " + SurveyId);
            string title = dt.Rows[0]["SurveyTitle"].ToString();
            string desc = dt.Rows[0]["SurveyDescription"].ToString();

            lbltitle.Text = title;
            lblDesc.Text = desc;




            StringBuilder Enqury = new StringBuilder();

            DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select sq.Id, Question, Type, Options from [dbo].[Survey_Questions] sq 
join AllowSurveyEmployee ase
on sq.SuerveryId = ase.SurveyId
where SuerveryId = " + SurveyId + " and EmpId = (select top 1 EmpId from AllowSurveyEmployee where SurveyId = " + SurveyId + " )");

            if (dt2.Rows.Count > 0)
            {
                int i = 1;

                foreach (DataRow row in dt2.Rows)
                {
                    string type = row["Type"].ToString();
                    ids.Append(row["Id"].ToString() + ",");

                    if (type == "select-option")
                    {
                        List<string> valsList = new List<string>();
                        valsList = row["Options"].ToString().Split(',').ToList();
                        StringBuilder options = new StringBuilder();

                        foreach (string val in valsList)
                        {
                            options.Append(string.Format("<option value = '{0}'> {0} </option>", val));
                        }

                        Enqury.Append(string.Format(@"
	                                <div class='col-md-8'>
                                        <div class='form-group'>
	                                        <h4>
		                                        <lable class='form-group'>" + row["Question"].ToString() + @"</lable>
                                            </h4>
                                             <select class='form-control' id='" + i + "' name='" + i + @"'>{0}</select>
                                        </div>
                                     </div>
                                ", options));
                    }
                    else
                    {
                        Enqury.Append(@"
	                                <div class='col-md-8'>
                                        <div class='form-group'>
	                                        <h4>
		                                        <lable class='form-group'>" + row["Question"].ToString() + @"</lable>
                                            </h4>
                                             <input type='text' id='" + i + "' name='" + i + @"' class = 'form-control' />
                                        </div>
                                     </div>
                                ");
                    }

                    i++;
                }
                pnlQues.Controls.Add(new Literal { Text = Enqury.ToString() });

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }

    protected void btnSubmit_click(object sender, EventArgs e)
    {
        try
        {
            bool isAnsInserted = false;
            ///int empId = user.getEmployeeId(Session["EmailId"].ToString());
            //List<string> idList = new List<string>();
            //idList = ids.ToString().Split(',').ToList();
            int i = 1;


            DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select sq.Id, Question, Type, Options from [dbo].[Survey_Questions] sq 
join AllowSurveyEmployee ase
on sq.SuerveryId = ase.SurveyId
where SuerveryId = " + SurveyId + " and EmpId = (select top 1 EmpId from AllowSurveyEmployee where SurveyId = " + SurveyId + " )");

            if (dt2.Rows.Count > 0)
            {
                foreach (DataRow row in dt2.Rows)
                {
                    try
                    {
                        int qid = int.Parse((row["Id"].ToString()));
                        string ans = Request[i.ToString()];
                        i++;

                        objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO Survey_Answer(EmpId, Ans, Ques_Id) VALUES(" + empId + ", '" + ans + "', " + qid + ")");
                        isAnsInserted = true;

                    }
                    catch (Exception ex)
                    {

                    }


                }
            }

            //foreach (string str in idList)
            //{
            //    try
            //    {
            //        int qid = int.Parse(str);
            //        string ans = Request[i.ToString()];
            //        i++;

            //        objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO Survey_Answer(EmpId, Ans, Ques_Id) VALUES(" + empId + ", '" + ans + "', " + qid + ")");
            //        isAnsInserted = true;

            //    }
            //    catch (Exception ex)
            //    {

            //    }

            //}
            if (isAnsInserted)
            {
                objSQLDataFile.ExecuteDataTableSQL(@"UPDATE AllowSurveyEmployee SET IsAnsSubmitted = 1 WHERE EmpId= " + empId + " AND SurveyId = " + SurveyId);

            }
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Thank you for your Time!')){window.location.href='SurveyList.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnBack_click(object sender, EventArgs e)
    {
        Response.Redirect("SurveyList.aspx");
    }
}
