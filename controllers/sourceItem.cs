using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetP;

public class sourceItem : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("addsourceitem")]
    [BaseUrlRoute()]
    public async Task<ActionResult> addSourceItem([FromBody] sourceItemsM sourceitem)
    {
        DbConn.OpenConn();
        try
        {
            DataTable dt;
            DataRow drow;
            dt = DbConn.GetDataTable("tblsource_item");
            drow = dt.NewRow();
            // ,crtby,crtws,crtdt
            string item_id = Utility.getnextnum(DbConn, "SRCM");
            drow["item_id"] = item_id;
            drow["source_item_name"] = sourceitem.submenu;
            drow["source_id"] = sourceitem.menuid;
            drow["crtby"] = sourceitem.menuid;
            drow["crtws"] = Dns.GetHostName();
            drow["crtdt"] = DateTime.Now;
            string query = DbconUtility.GetQuery(1, drow);
            if (DbConn.Insert(drow, false))
            {
                int num = Convert.ToInt32(item_id);
                Utility.Updatemnextnum(DbConn, num, "SRCM");
            }
            return Ok(Utility.ResponseMessage("the  data is inserted successfully", false));
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
}
