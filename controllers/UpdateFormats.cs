using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;

public class UpdateFormats : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPut("updateFormats")]
    [BaseUrlRoute()]
    public async Task<ActionResult> updateFormat([FromBody] formatDatModel data)
    {
        try
        {
            //    take history  add fiscal year
            if (Utility.createHistory(DbConn, "update the data", "Aba0098u7", "tblFormdata", "branch_code='" + data.branch_code + "' and parent_code='" + data.parent_code + "' "))
            {
                // only update data after take history
                string updatedata = "update tblFormdata set actual='" + data.actual + "' ,estimated='" + data.estimated +
                           "',jul='" + data.Jul + "' ,aug='" + data.Aug + "' ,sep='" + data.Sep
                           + "',oct='" + data.Oct + "' ,nov='" + data.Nov + "' ,dec='" + data.Dec + "' ,jan='" + data.Jan + "' ,feb='" + data.Feb + "' ,mar='" + data.Mar +
                           "',apr='" + data.Apr + "',may='" + data.May + "' ,crtdt='" + DateTime.Now + "' ,crtby='" + Dns.GetHostName + "',crtws='" + Dns.GetHostName +
                            "'   where branch_code='" + data.branch_code + "' and parent_code='" + data.parent_code + "'";
                // you have take history
                if (Utility.createHistory(DbConn, "Deleted", "Kennesa", "tblformData", " branch_code=" + data.branch_code))
                {
                    if (DbConn.Execute(updatedata))

                    {
                        return Ok(Utility.ResponseMessage("data is updated successfully", false));
                    }
                    else
                    {
                        Utility.setLog("addformat", updatedata, Convert.ToString(Dns.GetHostName));
                        return Ok(Utility.ResponseMessage("something went wrong please check yoour input", false));
                    }
                }
                else
                    return BadRequest("the data is not inserted ");
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "cannot create history please check your input");
        }
        catch (Exception ex)
        {
            Utility.setLog("addformat", ex.ToString(), Convert.ToString(Dns.GetHostName));
            return Ok(Utility.ResponseMessage("something went wrong please check yoour input", false));
        }
    }
}
