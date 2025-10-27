using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
[ApiController]
public class GetOperationalReportsByBranch : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("OperationreportBybranch")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getOperational(EmployeeAllowances__M _branch)
    {
        string report = "select * from View_reportBy_Branch where branch_code='" + _branch.branch_code + "'";
        DataTable dt = new DataTable();
        DbConn.FillData(dt, report);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    actual = row["actual"].ToString(),
                    estimated = row["estimated"].ToString(),
                    projected = row["projected"].ToString(),
                    NetIncrement = row["NetIncrement"].ToString(),
                    jul = row["jul"].ToString(),
                    aug = row["aug"].ToString(),
                    sep = row["sep"].ToString(),
                    oct = row["oct"].ToString(),
                    nov = row["nov"].ToString(),
                    dec = row["dec"].ToString(),
                    jan = row["jan"].ToString(),
                    feb = row["feb"].ToString(),
                    mar = row["mar"].ToString(),
                    apr = row["apr"].ToString(),
                    may = row["may"].ToString(),
                    jun = row["jun"].ToString(),
                    description = row["description"].ToString(),
                    BranchName = row["DeptDesc"].ToString()
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else
            return NoContent();
    }
    [HttpGet("OperationreportByDistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getOperationReportByDistrict()
    {
        string report = "select * from View_reportBy_Districts order by deptdesc ";
        DataTable dt = new DataTable();
        DbConn.FillData(dt, report);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    actual = row["actual"].ToString(),
                    estimated = row["estimated"].ToString(),
                    projected = row["projected"].ToString(),
                    NetIncrement = row["NetIncrement"].ToString(),
                    jul = row["jul"].ToString(),
                    aug = row["aug"].ToString(),
                    sep = row["sep"].ToString(),
                    oct = row["oct"].ToString(),
                    nov = row["nov"].ToString(),
                    dec = row["dec"].ToString(),
                    jan = row["jan"].ToString(),
                    feb = row["feb"].ToString(),
                    mar = row["mar"].ToString(),
                    apr = row["apr"].ToString(),
                    may = row["may"].ToString(),
                    jun = row["jun"].ToString(),
                    description = row["description"].ToString(),
                    District = row["DeptDesc"].ToString()
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else
            return NoContent();
    }
    [HttpGet("FCYbybranch")]
    [BaseUrlRoute()]
    public async Task<IActionResult> FCYbybranch(string branch_code)
    {
        string report = "select * from View_reportBy_Branch where parent_code in (select menucode from  tblMenus where ParentCode=10) and branch_code='" + branch_code + "' ";
        DataTable dt = new DataTable();
        DbConn.FillData(dt, report);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    actual = row["actual"].ToString(),
                    estimated = row["estimated"].ToString(),
                    projected = row["projected"].ToString(),
                    NetIncrement = row["NetIncrement"].ToString(),
                    jul = row["jul"].ToString(),
                    aug = row["aug"].ToString(),
                    sep = row["sep"].ToString(),
                    oct = row["oct"].ToString(),
                    nov = row["nov"].ToString(),
                    dec = row["dec"].ToString(),
                    jan = row["jan"].ToString(),
                    feb = row["feb"].ToString(),
                    mar = row["mar"].ToString(),
                    apr = row["apr"].ToString(),
                    may = row["may"].ToString(),
                    jun = row["jun"].ToString(),
                    description = row["description"].ToString(),
                    District = row["DeptDesc"].ToString()
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else
            return NoContent();
    }
    [HttpGet("FCYbydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> FCYbydistrict(string district_code)
    {
        string report = " select * from View_reportBy_Districts where parent_code  in (select menucode from  tblMenus where ParentCode=10) and district_code='" + district_code + "'";
        DataTable dt = new DataTable();
        DbConn.FillData(dt, report);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    actual = row["actual"].ToString(),
                    estimated = row["estimated"].ToString(),
                    projected = row["projected"].ToString(),
                    NetIncrement = row["NetIncrement"].ToString(),
                    jul = row["jul"].ToString(),
                    aug = row["aug"].ToString(),
                    sep = row["sep"].ToString(),
                    oct = row["oct"].ToString(),
                    nov = row["nov"].ToString(),
                    dec = row["dec"].ToString(),
                    jan = row["jan"].ToString(),
                    feb = row["feb"].ToString(),
                    mar = row["mar"].ToString(),
                    apr = row["apr"].ToString(),
                    may = row["may"].ToString(),
                    jun = row["jun"].ToString(),
                    description = row["description"].ToString(),
                    District = row["DISTRICT"].ToString()
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else
            return NoContent();
    }
}
