using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;


public class BudgetYears : ControllerBase
{

    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("addBudgetYears")]
    [BaseUrlRoute()]
    public async Task<IActionResult> BudgetYearsAdd(BudgetYear budget)
    {


        DataTable dt;
        DataRow drow;
        dt = DbConn.GetDataTable("tblBudgetYear");
        drow = dt.NewRow();
        DbConn.OpenConn();
        try
        {
            drow["descrFiscalYeaription"] = budget.FiscalYear;
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
                Success = true
            });
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}