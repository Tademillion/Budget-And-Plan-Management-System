using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
namespace BudgetP;
[ApiController]
public class getAllPositions : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    // [EnableCors("AllowSpecificOrigins")]
    [HttpGet("getAllPosition")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getAllPosition()
    {
        try
        {
            string position = "select JobPosId,JobPosDesc from hr_pay_data..tblJobPosition";
            List<object> jobpos = new List<object>();
            DataTable dt = new DataTable();
            DbConn.FillData(dt, position);
            foreach (DataRow row in dt.Rows)
            {
                string JobPosId = row["JobPosId"] + "";
                string JobPosDesc = row["JobPosDesc"] + "";
                var userResponse = new
                {
                    JobPosId = JobPosId,
                    JobPosDesc = JobPosDesc
                };
                jobpos.Add(userResponse);
            }
            return Ok(jobpos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}
