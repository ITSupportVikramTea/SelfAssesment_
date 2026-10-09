using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for AddAssessment
/// </summary>
public class AddAssessment
{
    public void AssessmentAdd(DataTable dt2, string hrfeedback, string intfeedback, int candidateId)
    {
        SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
        string finalResult = "";
        //DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select HRFeedback from [dbo].[JobResult] where CandidateId=" + candidateId);
        if (dt2.Rows.Count > 0)
        {
            if (hrfeedback != "")
            {
                if (hrfeedback.ToLower() == "shortlisted" && intfeedback.ToLower() == "shortlisted")
                {
                    finalResult = "Shortlisted";
                }
                else if (hrfeedback.ToLower() == "rejected" && intfeedback.ToLower() == "rejected")
                {
                    finalResult = "Rejected";
                }
                else if (hrfeedback.ToLower() == "rejected" && intfeedback.ToLower() == "shortlisted")
                {
                    finalResult = "Shortlisted by Interviewer";
                }
                else if (hrfeedback.ToLower() == "shortlisted" && intfeedback.ToLower() == "rejected")
                {
                    finalResult = "Rejected by Interviewer";
                }
                else if (intfeedback.ToLower() == "on hold")
                {
                    finalResult = "Holded by Interviewer";
                }
                //else
                //{
                //    if (intfeedback.ToLower() == "shortlisted")
                //    {
                //        finalResult = "Shortlisted by Interviewer";
                //    }
                //    else if (intfeedback.ToLower() == "rejected")
                //    {
                //        finalResult = "Rejected by Interviewer";
                //    }
                //    else
                //    {
                //        finalResult = "Holded by Interviewer";
                //    }
                //}
            }
           
        }
        else
        {
            if (intfeedback.ToLower() == "shortlisted")
            {
                finalResult = "Shortlisted by Interviewer";
            }
            else if (intfeedback.ToLower() == "rejected")
            {
                finalResult = "Rejected by Interviewer";
            }
            else
            {
                finalResult = "Holded by Interviewer";
            }
        }

        if (finalResult != "")
        {
            objSQLDataFile.ExecuteDataTableSQL(@"update JobResult set FinalResult = '" + finalResult + "' where CandidateId=" + candidateId);
            objSQLDataFile.ExecuteDataTableSQL(@"update JobApplications set FinalResult = '" + finalResult + "' where Id=" + candidateId);

            if (finalResult == "Shortlisted" || finalResult == "Shortlisted by Interviewer")
            {
                objSQLDataFile.ExecuteDataTableSQL(@"Update AvailableVacancies set NoOfVC = (NoOfVC-1) where id = (select VacancyId from JobApplications where id=" + candidateId + ")");
            }
        }

       
    }
}