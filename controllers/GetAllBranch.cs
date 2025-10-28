using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;

public class GetAllBranch : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpGet("getAllBranches")]
    [BaseUrlRoute()]
    public async Task<IActionResult> GetBranches()
    {
        DataTable dt = new DataTable();
        try
        { // dbCon.Update
            DbConn.OpenConn();
            string sql = "select * from hr_pay_data..tblDepartments order by DeptDesc";
            DbConn.FillData(dt, sql);
            if (dt.Rows.Count == 0)
            {
                return NoContent();
            }
            var userResponses = new List<object>();
            foreach (DataRow row in dt.Rows)
            {
                string DeptCode = row["DeptCode"] + "";
                string DeptDesc = row["DeptDesc"] + "";
                var userResponse = new
                {
                    DeptCode = DeptCode,
                    DeptDesc = DeptDesc
                };
                userResponses.Add(userResponse);
            }
            return Ok(Utility.ResponseMessage(userResponses, true));
        }
        catch (Exception ex)
        {
            DbConn.CloseConn();
            return Ok(Utility.ResponseMessage("something went wrong check your input", false));
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}
