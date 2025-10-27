using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;

public class addFormats : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("addformats")]
    [BaseUrlRoute()]
    public async Task<ActionResult> addFormat([FromBody] Formats format)
    {
        if (format == null || string.IsNullOrEmpty(format.discription))
        {
            return BadRequest(" invalid menu");
        }
        DbConn.OpenConn();
        try
        {
            DataTable dt;
            DataRow drow;
            dt = DbConn.GetDataTable("tblFormats");
            drow = dt.NewRow();
            string nextnum = Utility.getnextnum(DbConn, "SRC");
            drow["discription"] = format.discription;
            drow["parentcode"] = format.parentcode;
            drow["formatId"] = nextnum;
            drow["crtby"] = Dns.GetHostName();
            drow["crtws"] = Dns.GetHostName();
            drow["crtdt"] = DateTime.Now;
            string query = DbconUtility.GetQuery(1, drow);
            if (DbConn.Insert(drow, false))
            {
                int num = Convert.ToInt32(nextnum);
                Utility.Updatemnextnum(DbConn, num, "SRC");
            }
            return Ok(new
            {
                Success = true,
                StatusCode = 200,
                Message = SuccessMessages.FormatAddedSucces
            });
        }
        catch (Exception ex)
        {
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Success = false,
                Message = ErrorMessages.UnexpectedError,
            });
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}