using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
public class getFOrmdata : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpGet("getformatdata")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getFormDatas()
    {
        DbConn.OpenConn();
        List<object> items = new List<object>();
        try
        {
            string estimated = string.Empty; string actual = string.Empty; string netincrement = string.Empty;
            string projected = string.Empty;
            string quarternet = string.Empty;
            string monthlynet = string.Empty;
            string branch_code = string.Empty;
            DataTable dt = new DataTable();
            string item = "select * from tblFormdata";
            DbConn.FillData(dt, item);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    estimated = row["estimated"] + "";
                    branch_code = row["branch_code"] + "";
                    actual = row["actual"] + "";
                    netincrement = row["netincrement"] + "";
                    // projected = row["projected"] + "";
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
                        estimated = estimated,
                        actual = actual,
                        netincrement = netincrement,
                        // projected = projected,
                        quarternet = quarternet,
                        monthlynet = monthlynet,
                        branch_code = branch_code,
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
                        jun = jun
                    };
                    items.Add(userResponse);
                }
                return Ok(Utility.ResponseMessage(items, false));
            }
            else
                return NoContent();// 204 nocontent
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
