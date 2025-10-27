using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
[ApiController]
public class EmployeeAllowances : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("getallowances")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getAllowances([FromBody] EmployeeAllowances__M _allowances)
    {
        try
        {
            string allowance = "select * from view_Allow_byBranch where   branch_code='" + _allowances.branch_code + "'";
            DataTable dt = new DataTable();
            DbConn.FillData(dt, allowance);
            List<object> employeAllowances = new List<object>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string AllowanceDesc = row["AllowanceDesc"] + "";
                    string DeptDesc = row["DeptDesc"] + "";
                    string jul = row["jul"] + "";
                    string aug = row["aug"] + "";
                    string sep = row["sep"] + "";
                    string oct = row["oct"] + "";
                    string nov = row["nov"] + "";
                    string dec = row["dec"] + "";
                    string jan = row["jan"] + "";
                    string feb = row["feb"] + "";
                    string mar = row["mar"] + "";
                    string apr = row["apr"] + "";
                    string may = row["may"] + "";
                    string jun = row["jun"] + "";
                    var userResponse = new
                    {
                        AllowanceDesc = AllowanceDesc,
                        DeptDesc = DeptDesc,

                        jul = jul,
                        aug = aug,
                        sep = sep,
                        oct = oct,
                        nov = nov,
                        dec = dec,
                        jan = jan,
                        feb = feb,
                        mar = mar,
                        apr = apr,
                        may = may,
                        jun = jun,
                    };
                    employeAllowances.Add(userResponse);
                }
                return Ok(employeAllowances);
            }
            else return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(" somethignwent wrong");
        }
    }
    [HttpPost("allowancesbydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> allowancesbydistrict(District dist)
    {
        string allowance = @" select * from view_allowance_By_districts where district_code='" + dist.district_code + "'  order by district";
        DataTable dt = new DataTable();
        DbConn.FillData(dt, allowance);
        List<object> employeAllowances = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                {
                    string AllowanceDesc = row["AllowanceDesc"] + "";
                    string District = row["District"] + "";
                    string jul = row["jul"] + "";
                    string aug = row["aug"] + "";
                    string sep = row["sep"] + "";
                    string oct = row["oct"] + "";
                    string nov = row["nov"] + "";
                    string dec = row["dec"] + "";
                    string jan = row["jan"] + "";
                    string feb = row["feb"] + "";
                    string mar = row["mar"] + "";
                    string apr = row["apr"] + "";
                    string may = row["may"] + "";
                    string jun = row["jun"] + "";

                    var userResponse = new
                    {
                        AllowanceDesc = AllowanceDesc,
                        District = District,
                        jul = jul,
                        aug = aug,
                        sep = sep,
                        oct = oct,
                        nov = nov,
                        dec = dec,
                        jan = jan,
                        feb = feb,
                        mar = mar,
                        apr = apr,
                        may = may,
                        jun = jun,

                    };
                    employeAllowances.Add(userResponse);
                }
            }
        }
        return Ok(employeAllowances);
    }
    [HttpPost("allowancesbybranch")]
    [BaseUrlRoute()]
    public async Task<IActionResult> allowancesbybranch(EmployeeAllowances__M _allow)
    {
        string allowance = @"select * from view_Allow_byBranch where branch_code='" + _allow.branch_code + "'";
        DataTable dt = new DataTable();
        DbConn.FillData(dt, allowance);
        List<object> employeAllowances = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                {
                    string AllowanceDesc = row["AllowanceDesc"] + "";
                    string DeptDesc = row["DeptDesc"] + "";
                    string jul = row["jul"] + "";
                    string aug = row["aug"] + "";
                    string sep = row["sep"] + "";
                    string oct = row["oct"] + "";
                    string nov = row["nov"] + "";
                    string dec = row["dec"] + "";
                    string jan = row["jan"] + "";
                    string feb = row["feb"] + "";
                    string mar = row["mar"] + "";
                    string apr = row["apr"] + "";
                    string may = row["may"] + "";
                    string jun = row["jun"] + "";

                    var userResponse = new
                    {
                        AllowanceDesc = AllowanceDesc,
                        DeptDesc = DeptDesc,
                        jul = jul,
                        aug = aug,
                        sep = sep,
                        oct = oct,
                        nov = nov,
                        dec = dec,
                        jan = jan,
                        feb = feb,
                        mar = mar,
                        apr = apr,
                        may = may,
                        jun = jun,
                    };
                    employeAllowances.Add(userResponse);
                }
            }
            return Ok(employeAllowances);
        }
        else
            return NoContent();
    }
}