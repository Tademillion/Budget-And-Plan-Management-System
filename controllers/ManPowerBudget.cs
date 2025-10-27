using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace BudgetP;
[ApiController]
public class ManPowerBudget : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    // [EnableCors("AllowSpecificOrigins")]
    [HttpPost("addManPower")]
    [BaseUrlRoute()]
    public async Task<ActionResult> addManPower(ManPowerBudget_M[] _power)
    {
        DbConn.OpenConn();
        DataTable dt;
        DataRow drow;
        dt = DbConn.GetDataTable("tblManPower");
        drow = dt.NewRow();
        try
        {
            if (_power.Length < 1)
            {
                return BadRequest("body must have appropriate data");
            }
            // before add man power we have to add and save the allwances
            foreach (var power in _power)
            {
                if (Utility.saveManPowerAllownace(DbConn, power.branch_code, Convert.ToInt16(power.JobPosId), Utility.getFiscalYear(DbConn)))
                {
                    drow["branch_code"] = power.branch_code;
                    drow["JobPosId"] = power.JobPosId;
                    drow["ExistIng_Emp"] = power.existingStaff;
                    drow["Additional_Emp"] = power.additionalStaff;
                    drow["jul"] = power.jul;
                    drow["aug"] = power.aug;
                    drow["sep"] = power.sep;
                    drow["oct"] = power.oct;
                    drow["nov"] = power.nov;
                    drow["dec"] = power.dec;
                    drow["jan"] = power.jan;
                    drow["feb"] = power.feb;
                    drow["mar"] = power.mar;
                    drow["apr"] = power.apr;
                    drow["may"] = power.mar;
                    drow["jun"] = power.jan;
                    drow["New"] = power.New;
                    drow["Replacement"] = power.replacement;
                    drow["crtby"] = Dns.GetHostName();
                    drow["crtws"] = Dns.GetHostName();
                    drow["crtdt"] = DateTime.Now;
                    string querys = DbconUtility.GetQuery(1, drow);
                    if (!DbConn.Insert(drow, false))
                    {
                        var url = HttpContext.Request.Host + HttpContext.Request.Path;
                        Utility.setLog(url, querys, Dns.GetHostName());
                        return BadRequest(Utility.ResponseMessage("something went wrong please check your input", false));
                        // const response = {     
                        //     StatusCode:Boolen, 
                        //     message:"",        
                        //     data:{             
                        //         nameof,        
                        //     }                  
                        // }                       
                    }
                }
            }
            return Ok(Utility.ResponseMessage("the data is added successfully", false));
        }
        catch (Exception ex)
        {
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            return BadRequest("data is not added succesfully");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    // [EnableCors("AllowSpecificOrigins")]
    [HttpPost("getManpowerBydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> GetManPowerByDiestrict(District dist)
    {
        DataTable dt = new DataTable();
        string getdata = "select  * from view_manPower_ByDistricts where DeptCode='" + dist.district_code + "' order by deptdesc";
        DbConn.FillData(dt, getdata);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    ExistingStaff = row["ExistingStaff"] + "",
                    AdditionalStaff = row["AdditionalStaff"] + "",
                    jul = row["jul"] + "",
                    aug = row["aug"] + "",
                    sep = row["sep"] + "",
                    oct = row["oct"] + "",
                    nov = row["nov"] + "",
                    dec = row["dec"] + "",
                    jan = row["jan"] + "",
                    feb = row["feb"] + "",
                    mar = row["mar"] + "",
                    apr = row["apr"] + "",
                    may = row["may"] + "",
                    jun = row["jun"] + "",
                    New = row["new"] + "",
                    Replacement = row["Replacement"] + "",
                    DeptDesc = row["deptdesc"] + "",
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else
            return NoContent();
    }
    // [EnableCors("AllowSpecificOrigins")]
    [HttpPost("getManpowerBybranch")]
    [BaseUrlRoute()]
    public async Task<IActionResult> GetManPowerBybranch([FromBody] EmployeeAllowances__M branch)
    {
        DataTable dt = new DataTable();
        string getdata = "select * from View_manPowerByBranch where branch_code='" + branch.branch_code + "'";
        DbConn.FillData(dt, getdata);
        List<object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    DeptDesc = row["DeptDesc"] + "",
                    ExistingStaff = row["ExistingStaff"] + "",
                    AdditionalStaff = row["AdditionalStaff"] + "",
                    JobPosId = row["JobPosId"] + "",
                    JobPosDesc = row["JobPosDesc"] + "",
                    jul = row["jul"] + "",
                    aug = row["aug"] + "",
                    sep = row["sep"] + "",
                    oct = row["oct"] + "",
                    nov = row["nov"] + "",
                    dec = row["dec"] + "",
                    jan = row["jan"] + "",
                    feb = row["feb"] + "",
                    mar = row["mar"] + "",
                    apr = row["apr"] + "",
                    may = row["may"] + "",
                    jun = row["jun"] + "",
                    New = row["new"] + "",
                    Replacement = row["Replacement"] + ""
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else
            return NoContent();
    }
    // update amn power
    [HttpPut("updateManpower")]
    [BaseUrlRoute()]
    public async Task<ActionResult> updateManpower(ManPowerBudget_M power)
    {
        DbConn.OpenConn();
        DataTable dt;
        DataRow drow;
        dt = DbConn.GetDataTable("tblManPower");
        drow = dt.NewRow();
        try
        {
            if (Utility.saveManPowerAllownace(DbConn, power.branch_code, Convert.ToInt16(power.JobPosId), Utility.getFiscalYear(DbConn)))
            {
                drow["branch_code"] = power.branch_code;
                drow["JobPosId"] = power.JobPosId;
                drow["ExistIng_Emp"] = power.existingStaff;
                drow["Additional_Emp"] = power.additionalStaff;
                drow["jul"] = power.jul;
                drow["aug"] = power.aug;
                drow["sep"] = power.sep;
                drow["oct"] = power.oct;
                drow["nov"] = power.nov;
                drow["dec"] = power.dec;
                drow["jan"] = power.jan;
                drow["feb"] = power.feb;
                drow["mar"] = power.mar;
                drow["apr"] = power.apr;
                drow["may"] = power.mar;
                drow["jun"] = power.jan;
                drow["New"] = power.New;
                drow["Replacement"] = power.replacement;
                drow["crtby"] = Dns.GetHostName();
                drow["crtws"] = Dns.GetHostName();
                drow["crtdt"] = DateTime.Now;
                string querys = DbconUtility.GetQuery(2, drow);
                querys = querys + " branch_code='" + power.branch_code + "' and JobPosId='" + power.JobPosId + "'";
                if (!DbConn.Execute(querys))
                {
                    var url = HttpContext.Request.Host + HttpContext.Request.Path;
                    Utility.setLog(url, querys, Dns.GetHostName());
                    return BadRequest(Utility.ResponseMessage("something went wrong please check your input", false));
                }
            }
            return Ok(Utility.ResponseMessage("the data is added successfully", false));
        }
        catch (Exception ex)
        {
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            return BadRequest("data is not added succesfully");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}