using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BudgetP;
[ApiController]
public class EmployeExpenses : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("addexpenses")]
    [BaseUrlRoute()]
    public async Task<ActionResult> addexpenses(ExpenseM _expense)
    {
        DbConn.OpenConn();
        try
        {
            DataTable dt;
            DataRow row;
            dt = DbConn.GetDataTable("tblEmployeeExpenses");
            row = dt.NewRow();
            row["branch_code"] = _expense.branch_code;
            row["FiscalYear"] = Utility.getFiscalYear(DbConn);
            row["expenseId"] = _expense.expenseId;
            row["TotalAmount"] = _expense.totalAmount;
            row["crtdby"] = _expense.userName;
            row["crtdt"] = DateTime.Now;
            row["crtws"] = Dns.GetHostName();
            string isBefore = "select * from tblEmployeeExpenses where branch_code='" + _expense.branch_code + "' and expenseId='" + _expense.expenseId + "'";
            DataTable dt1 = new DataTable();
            DbConn.FillData(dt1, isBefore);
            string query = string.Empty;
            if (dt1.Rows.Count > 0)
            {
                // its inserted before so we will update it
                query = DbconUtility.GetQuery(2, row);
                query = query + "branch_code='" + _expense.branch_code + "' and expenseId='" + _expense.expenseId + "'";
                if (!DbConn.Execute(query))
                {
                    Utility.setLog("employeeexpense", query, _expense.userName);
                    return StatusCode(StatusCodes.Status500InternalServerError, "data is not updated ");
                }
            }
            else
            {
                // insert it
                query = DbconUtility.GetQuery(1, row);
                if (!DbConn.Insert(row, false))
                {
                    Utility.setLog("employeeexpense", query, _expense.userName);
                }
            }
            return Ok("data is succesfully updated");
        }
        catch (Exception ex)
        {
            Utility.setLog("employeeexpense", ex.Message, _expense.userName);
            return StatusCode(StatusCodes.Status500InternalServerError, "something went wrong please check your input");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    [HttpPost("getexpense")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getBranchExpenses(EmployeeAllowances__M _branch)
    {
        DataTable dt = new DataTable();
        string expense = @"select e.id, expensesdesc,branch_code,FiscalYear,TotalAmount from tblExpenses as e left join  tblEmployeeExpenses as x
                            on e.id=x.expenseId where branch_code='" + _branch.branch_code + "'" + @"union select id,expensesdesc,'" + _branch.branch_code + "','" + Utility.getFiscalYear(DbConn) + "',0 from tblExpenses where id not in((select expenseId from tblEmployeeExpenses as s where s.branch_code='" + _branch.branch_code + "') )";
        DbConn.FillData(dt, expense);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string expensesdesc = row["expensesdesc"] + "";
                string expenseId = row["id"] + "";
                string branch_code = row["branch_code"] + "";
                string FiscalYear = row["FiscalYear"] + "";
                string TotalAmount = row["TotalAmount"] + "";
                var userResponse = new
                {
                    expensesdesc = expensesdesc,
                    expenseId = expenseId,
                    branch_code = branch_code,
                    FiscalYear = FiscalYear,
                    TotalAmount = TotalAmount,
                };
                Obj.Add(userResponse);
            }
        }
        return Ok(Obj);
    }
    // report only
    [HttpPost("expensebybranch")]
    [BaseUrlRoute()]
    public async Task<IActionResult> expenseByBranch(EmployeeAllowances__M _expense)
    {
        DataTable dt = new DataTable();
        string bybranch = @"select x.expensesdesc, e.TotalAmount,FiscalYear,D.DeptDesc,expenseId from tblEmployeeExpenses as e left join  tblExpenses as x
                on e.expenseId=x.id 
				left join hr_pay_data..tblDepartments as D on D.DeptCode=E.branch_code where branch_code='" + _expense.branch_code + "'";
        DbConn.FillData(dt, bybranch);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string expensesdesc = row["expensesdesc"] + "";
                string DeptDesc = row["DeptDesc"] + "";
                string FiscalYear = row["FiscalYear"] + "";
                string TotalAmount = row["TotalAmount"] + "";
                var userResponse = new
                {
                    expensesdesc = expensesdesc,
                    DeptDesc = DeptDesc,
                    FiscalYear = FiscalYear,
                    TotalAmount = TotalAmount,
                };
                Obj.Add(userResponse);
            }
        }
        return Ok(Obj);
    }
    [HttpPost("getexpensebydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getExpenseByDistrict(District dist)
    {
        string expense = @"select * from view_expenseby_Districts where DeptCode='" + dist.district_code + "' order by deptdesc";
        DataTable dt = new DataTable();
        DbConn.FillData(dt, expense);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string expensesdesc = row["expensesdesc"] + "";
                string DeptDesc = row["deptdesc"] + "";
                string FiscalYear = row["FiscalYear"] + "";
                string TotalAmount = row["TotalAmount"] + "";
                var userResponse = new
                {
                    expensesdesc = expensesdesc,
                    DeptDesc = DeptDesc,
                    FiscalYear = FiscalYear,
                    TotalAmount = TotalAmount,
                };
                Obj.Add(userResponse);
            }
        }
        return Ok(Obj);
    }
}
