using System.Data;
using System.Net;
namespace BudgetP;

public static class Utility
{
    public static Object ResponseMessage(object data, bool success)
    {
        HttpStatusCode statusCode = (HttpStatusCode)(int)HttpStatusCode.OK;
        if (!success)
        {
            statusCode = (HttpStatusCode)(int)HttpStatusCode.Created;
        }
        var responseResult = new
        {
            statusCode = statusCode,
            data = data
        };
        return responseResult;
    }
    public static Object ResponseMessages(object data, string message, bool success)
    {
        HttpStatusCode statusCode = (HttpStatusCode)(int)HttpStatusCode.OK;
        if (!success)
        {
            statusCode = (HttpStatusCode)(int)HttpStatusCode.Created;
        }
        var responseResult = new
        {
            statusCode = statusCode,
            data = data,
            message = message
        };
        return responseResult;
    }
    public static void setLog(string url, string message, string users)
    {
        DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
        DataTable dt;
        DataRow drow;
        dt = DbConn.GetDataTable("tbllog");
        DbConn.OpenConn();
        try
        {
            drow = dt.NewRow();
            drow["error"] = message;
            drow["dates"] = DateTime.Now;
            drow["url"] = url;
            drow["users"] = users;
            DbConn.insertProc(drow);
            // use  inserproc
        }
        catch (Exception ex)
        {

        }
        finally

        {
            DbConn.CloseConn();
        }
    }
    public static string getnextnum(DbconUtility DbConn, string parentcode)
    {
        try
        {
            List<object> data = new List<object>();
            DataTable dt = new DataTable();
            string exist = "select * from tbldefault where parametre='" + parentcode + "'";
            DbConn.FillData(dt, exist);
            if (dt.Rows.Count > 0)
            {
                int num = Convert.ToInt32(dt.Rows[0]["next_no"].ToString());
                string nextnum = string.Empty;
                if (num < 10)
                {
                    // num = num + 1;
                    nextnum = (num + 1).ToString("D2");
                }
                else
                    nextnum = Convert.ToString(num + 1);
                //update nextnum
                return nextnum;
            }
            return "0";

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static void Updatemnextnum(DbconUtility dbconn, int num, string parentcode)
    {
        dbconn.OpenConn();
        string updatenextnum = "update tbldefault set next_no =" + num + " where parametre='" + parentcode + "'";
        dbconn.Execute(updatenextnum);
        dbconn.CloseConn();
    }
    public static bool createHistory(DbconUtility dbCon, string actionTaken, string userName, string tableName, string criteria)
    {
        string cmdTxt2 = "", cmdTxt = "";
        int count = 0;
        if (criteria != "")
            criteria = " WHERE " + criteria;
        DataTable dt = new DataTable();
        try
        {
            cmdTxt = "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('" + tableName + "')";
            dbCon.FillData(dt, cmdTxt);
            cmdTxt = "INSERT INTO " + tableName + "_history (";
            cmdTxt2 = " SELECT ";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    cmdTxt += row["name"] + ",";
                    cmdTxt2 += row["name"] + ",";
                    count++;
                }
                //cmdTxt2 = cmdTxt2.ToString().Remove(cmdTxt2.Length - 1);//Removing last character (,)
                cmdTxt += "action_taken,updated_by,action_date,update_ws) ";
                cmdTxt2 += "'" + actionTaken + "' as action_taken,'" + userName + "' as updated_by,getdate() as action_date,'" + Environment.MachineName + "' as updated_ws FROM " + tableName + criteria;
                if (count > 0)
                    return dbCon.Execute(cmdTxt + cmdTxt2);
                else return false;
            }
            else return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static bool saveManPowerAllownace(DbconUtility dbcon, string branch_code, int JobPosId, string FiscalYear)
    {
        // first get allowances by position
        // second get the allownace by department
        DataTable dt = new DataTable();
        bool isOk = false;
        try
        {
            string addAllowance = $"insert into tblAlllowances  select '" + branch_code + "','" + JobPosId + "','" + FiscalYear + "',AllowanceTypeId, MonthlyAmount,GetDate(),HOST_NAME(),HOST_NAME() from  hr_pay_data..tblPositionAllowance where JobPosId=" + JobPosId;
            // addd allownaces by department
            string haveHardship = "select AllowanceTypeId,MonthlyAmount from  hr_pay_data..tblDepartmentAllowance where  DeptCode='" + branch_code + "'";
            dbcon.FillData(dt, haveHardship);
            if (dbcon.Execute(addAllowance))
            {
                if (dt.Rows.Count > 0)
                {
                    // have hardship
                    string hardship = "select top 1 BasicSalary BasicSalary from hr_pay_data..tblEmployees where JobPosId='" + JobPosId + "'";
                    dbcon.FillData(dt, hardship);
                    // string basicSalary = dbcon.GetDataTable(hardship).ToString();
                    decimal basicSalary = Convert.ToDecimal(dt.Rows[0]["BasicSalary"]);
                    string addhardship = $"insert into tblAlllowances select '" + branch_code + "','" + JobPosId + "','" + FiscalYear + $"', AllowanceTypeId,(${basicSalary}*MonthlyAmount)/100,GetDate(),HOST_NAME(),HOST_NAME() from  hr_pay_data..tblDepartmentAllowance where  DeptCode ='" + branch_code + "'";
                    //  how can i get cash ideminity
                    if (dbcon.Execute(addhardship))
                    {
                        isOk = true;
                    }
                }
                else
                {
                    isOk = true;
                }
            }
            return isOk;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static string getFiscalYear(DbconUtility dbcon)
    {
        try
        {
            DataTable dt = new DataTable();
            string fiscalyear = "select name from  iAdmindb..tblFiscalYears  order by FiscalYear desc";
            dbcon.FillData(dt, fiscalyear);
            return dt.Rows[0]["name"] + "";
        }
        catch (Exception)
        {
            throw;
        }
    }
    public static bool IsBudgetOpen()
    {
        DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
        try
        {
            //  get the  fiscal years
            var today = DateTime.Today;
            string checkOpen = "select top 1 * from tblbudgetyear where openingDate>='" + today + "' and closingDate<='" + today + "' order by closingDate desc";
            DataTable dt = new DataTable();

            DbConn.FillData(dt, checkOpen);
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

}
