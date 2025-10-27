using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
public class getSourceItem : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpGet("getsourceItems")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getsources()
    {
        DbConn.OpenConn();
        List<object> items = new List<object>();
        try
        {
            string itemName = string.Empty; string sourceId = string.Empty; string itemID = string.Empty;
            DataTable dt = new DataTable();
            string item = "select * from tblFormats";
            DbConn.FillData(dt, item);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    itemName = row["discription"] + "";
                    itemID = row["formatId"] + "";
                    sourceId = row["parentcode"] + "";
                    var userResponse = new
                    {
                        formatID = itemName,
                        formatid = itemID,
                        parentcode = sourceId,
                    };
                    items.Add(userResponse);
                }
            }
            return Ok(Utility.ResponseMessage(items, false));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}
