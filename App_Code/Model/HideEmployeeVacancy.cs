using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HideEmployeeVacancy
/// </summary>
public class HideEmployeeVacancy
{
    int[] empIds;
    int[] vacIds;
    int j = 0;
    public void addEmpIds(int[] empId)
    {
        foreach(int i in empId)
        {
            empIds[j] = i;
            j++;
        }
        j = 0;
    }

    public void addVacIds(int[] vacId)
    {
        foreach (int i in vacId)
        {
            vacIds[j] = i;
            j++;
        }
        j = 0;
    }

    //public int[] getEmpId()
    //{
    //    return empIds;
    //}
    //public int[] getVacId()
    //{
    //    return vacIds;
    //}

    public int addHiddenVacanciesToDB( ArrayList empobj,ArrayList vacobj, string createdBy)
    {
        User user = new User();
        int k = 0;
        foreach(int i in empobj)
        {
            foreach(int j in vacobj)
            {
                k = user.AddHiddenVacancy(i,j, createdBy);
            }
        }
        return k;
    }



    public int addSurveytoDb(ArrayList empobj, ArrayList survobj, string createdBy)
    {
        User user = new User();
        int k = 0;
        foreach (int i in empobj)
        {
            foreach (int j in survobj)
            {
                k = user.AddHideSurvey(i, j, createdBy);
            }
        }
        return k;
    }
}