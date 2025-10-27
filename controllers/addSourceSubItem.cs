

using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BudgetP;

public class addSourceSubItem : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("addSubmenuitems")]
    public async Task<ActionResult> addSubItem(SubmenuM menu)
    {
        DbConn.OpenConn();
        try
        {
            DataTable dt;
            DataRow drow;
            dt = DbConn.GetDataTable("tblsubitems");
            drow = dt.NewRow();
            string nextnum = Utility.getnextnum(DbConn, "SRCSM");
            drow["subitem_name"] = menu.subitem_name;
            drow["subitem_id"] = nextnum;
            drow["item_id"] = menu.item_id;
            drow["crtby"] = Dns.GetHostName();
            drow["crtws"] = Dns.GetHostName();
            drow["crtdt"] = DateTime.Now;
            string query = DbconUtility.GetQuery(1, drow);
            if (DbConn.Insert(drow, false))
            {
                int num = Convert.ToInt32(nextnum);
                Utility.Updatemnextnum(DbConn, num, "SRCSM");
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