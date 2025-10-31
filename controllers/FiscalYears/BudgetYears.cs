using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;

[ApiController]
public class BudgetYears : ControllerBase
{

    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("addBudgetYears")]
    [BaseUrlRoute()]
    public async Task<ActionResult> BudgetYearsAdd([FromBody] BudgetYear budget)
    {


        DataTable dt;
        DataRow drow;
        dt = DbConn.GetDataTable("tblBudgetYear");
        drow = dt.NewRow();
        DbConn.OpenConn();
        try
        {
            Console.WriteLine("the fiscal years", budget.FiscalYear);
            drow["FiscalYear"] = budget.FiscalYear;
            drow["openingDate"] = budget.openingDate;
            drow["closingDate"] = budget.closingDate;

            DbConn.Insert(drow, false);
            return Ok(new ApiResponse<object>
            {
                Message = SuccessMessages.SavedDataSuccess,
                Success = true
            });
        }
        catch (Exception ex)
        {

            return Ok(new ApiResponse<object>
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
    // 
    [HttpPut("updateBudgetFall/{budgetyear}")]
    [BaseUrlRoute()]
    public async Task<ActionResult> UpdateBudgetYears([FromBody] updateBudgetYearModel budget, string budgetyear)
    {
        try
        {

            string query = "update  tblBudgetYear set openingDate='" + budget.openingDate + "',closingDate='" + budget.closingDate + "' where FiscalYear='" + budgetyear + "'";
            DbConn.Execute(query);
            return Ok(new ApiResponse<object>
            {
                Message = SuccessMessages.UpdatedDataSuccess,
                Success = false
            });
        }
        catch (Exception ex)
        {
            return Ok(new ApiResponse<object>
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