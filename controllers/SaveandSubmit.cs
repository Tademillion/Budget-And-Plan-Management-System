using System.Data;
using System.Net;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;

[EnableCors("AllowSpecificOrigins")]
public class SaveandSubmit : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("sumbit")]
    [BaseUrlRoute()]
    public async Task<ActionResult> addFormatdatas([FromBody] formatDatModel[] data)
    {
        DbConn.OpenConn();
        try
        {
            if (data == null)
            {
                return BadRequest("data Cannot be Null");
            }
            // check wether the api contain body data or not
            if (data.Length < 1)
            {
                return BadRequest("the body must have appropriate  data");
            }
            DataTable dt;
            DataRow drow;
            dt = DbConn.GetDataTable("tblFormdata");
            drow = dt.NewRow();
            // ,crtby,crtws,crtdt
            //  prevent the saved data
            DataTable dt2 = new DataTable();
            foreach (var m in data)
            {
                // check the data is inserted before or not if its inserted before update it 
                string isavedbefore = "select * from tblFormdata where parent_code='" + m.parent_code + "' and branch_code='" + m.branch_code + "' ";
                DbConn.FillData(dt2, isavedbefore);
                if (dt2.Rows.Count > 0)
                {
                    // its saved before so update is
                    drow["branch_code"] = m.branch_code;
                    drow["actual"] = m.actual;
                    drow["estimated"] = m.estimated;
                    // drow["netincrement"] = m.netincrement;
                    // drow["projected"] = m.projected;
                    drow["parent_code"] = m.parent_code;
                    drow["jul"] = m.Jul;
                    drow["aug"] = m.Aug;
                    drow["sep"] = m.Sep;
                    drow["oct"] = m.Oct;
                    drow["nov"] = m.Nov;
                    drow["dec"] = m.Dec;
                    drow["jan"] = m.Jan;
                    drow["feb"] = m.Feb;
                    drow["mar"] = m.Mar;
                    drow["apr"] = m.Apr;
                    drow["may"] = m.May;
                    drow["jun"] = m.Jun;
                    drow["crtby"] = Dns.GetHostName();
                    drow["crtws"] = Dns.GetHostName();
                    drow["crtdt"] = DateTime.Now;
                    string querys = DbconUtility.GetQuery(2, drow);
                    querys = querys + "branch_code='" + m.branch_code + "' and  parent_code='" + m.parent_code + "'";
                    if (!DbConn.Execute(querys))
                    {
                        var url = HttpContext.Request.Host + HttpContext.Request.Path;
                        Utility.setLog(url, querys, Dns.GetHostName());
                        return Ok(Utility.ResponseMessage("something went wrong please check your input", false));
                    }
                }
                else
                {
                    drow["branch_code"] = m.branch_code;
                    drow["actual"] = m.actual;
                    drow["estimated"] = m.estimated;
                    // drow["netincrement"] = m.netincrement;
                    // drow["projected"] = m.projected;
                    drow["parent_code"] = m.parent_code;
                    drow["jul"] = m.Jul;
                    drow["aug"] = m.Aug;
                    drow["sep"] = m.Sep;
                    drow["oct"] = m.Oct;
                    drow["nov"] = m.Nov;
                    drow["dec"] = m.Dec;
                    drow["jan"] = m.Jan;
                    drow["feb"] = m.Feb;
                    drow["mar"] = m.Mar;
                    drow["apr"] = m.Apr;
                    drow["may"] = m.May;
                    drow["jun"] = m.Jun;
                    drow["crtby"] = Dns.GetHostName();
                    drow["crtws"] = Dns.GetHostName();
                    drow["crtdt"] = DateTime.Now;
                    string querys = DbconUtility.GetQuery(1, drow);

                    if (!DbConn.Insert(drow, false))
                    {
                        var url = HttpContext.Request.Host + HttpContext.Request.Path;
                        Utility.setLog(url, querys, Dns.GetHostName());
                        //return Ok(Utility.ResponseMessage("something went wrong please check your input", false));
                    }
                }
            }
            return Ok(data);
        }
        catch (Exception ex)
        {
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            return Ok(Utility.ResponseMessage("something went wrong please check your input", false));
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    [HttpPost("save")]
    [BaseUrlRoute()]
    public async Task<ActionResult> savedata([FromBody] formatDatModel save)
    {
        DbConn.OpenConn();
        DataTable dt;
        DataRow drow;
        dt = DbConn.GetDataTable("tblFormdata");
        drow = dt.NewRow();
        try
        {
            DataTable dt1 = new DataTable();
            string isaved = "select * from tblformData where parent_code='" + save.parent_code + "' and branch_code='" + save.branch_code + "'";
            DbConn.FillData(dt1, isaved);

            if (dt1.Rows.Count > 0)
            {
                drow["branch_code"] = save.branch_code;
                drow["actual"] = save.actual;
                drow["estimated"] = save.estimated;
                // drow["netincrement"] = save.netincrement;
                // drow["projected"] = save.projected;
                drow["parent_code"] = save.parent_code;
                drow["jul"] = save.Jul;
                drow["aug"] = save.Aug;
                drow["sep"] = save.Sep;
                drow["oct"] = save.Oct;
                drow["nov"] = save.Nov;
                drow["dec"] = save.Dec;
                drow["jan"] = save.Jan;
                drow["feb"] = save.Feb;
                drow["mar"] = save.Mar;
                drow["apr"] = save.Apr;
                drow["may"] = save.May;
                drow["jun"] = save.Jun;
                drow["crtby"] = Dns.GetHostName();
                drow["crtws"] = Dns.GetHostName();
                drow["crtdt"] = DateTime.Now;
                string querys = DbconUtility.GetQuery(2, drow);
                querys = querys + "branch_code='" + save.branch_code + "' and parent_code='" + save.parent_code + "'";
                if (!DbConn.Execute(querys))
                {
                    var url = HttpContext.Request.Host + HttpContext.Request.Path;
                    Utility.setLog(url, querys, Dns.GetHostName());
                    //return Ok(Utility.ResponseMessage("something went wrong please check your input", false));
                }
            }
            else
            {
                drow["branch_code"] = save.branch_code;
                drow["actual"] = save.actual;
                drow["estimated"] = save.estimated;
                // drow["netincrement"] = save.netincrement;
                // drow["projected"] = save.projected;
                drow["parent_code"] = save.parent_code;
                drow["jul"] = save.Jul;
                drow["aug"] = save.Aug;
                drow["sep"] = save.Sep;
                drow["oct"] = save.Oct;
                drow["nov"] = save.Nov;
                drow["dec"] = save.Dec;
                drow["jan"] = save.Jan;
                drow["feb"] = save.Feb;
                drow["mar"] = save.Mar;
                drow["apr"] = save.Apr;
                drow["may"] = save.May;
                drow["jun"] = save.Jun;
                drow["crtby"] = Dns.GetHostName();
                drow["crtws"] = Dns.GetHostName();
                drow["crtdt"] = DateTime.Now;
                string querys = DbconUtility.GetQuery(1, drow);

                if (!DbConn.Insert(drow, false))
                {
                    var url = HttpContext.Request.Host + HttpContext.Request.Path;
                    Utility.setLog(url, querys, Dns.GetHostName());
                    //return Ok(Utility.ResponseMessage("something went wrong please check your input", false));
                }
            }
            return Ok(new ApiResponse<object>
            {
                Message = "the data is updated Succesfully",
                Success = true
            });
        }
        catch (Exception ex)
        {
            // set log
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
            {
                Message = ErrorMessages.UnexpectedError,
                Success = false
            });
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}
