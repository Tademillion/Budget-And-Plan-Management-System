
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
[ApiController]
public class newMenu : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));

    [HttpPost("addNemwMenus")]

    [BaseUrlRoute()]
    public async Task<ActionResult> addNewMEnus(DescriptionModification desc)
    {
        DataTable dt;
        DataRow drow;
        dt = DbConn.GetDataTable("tblMenus");
        drow = dt.NewRow();
        DbConn.OpenConn();
        try
        {
            drow["description"] = desc.description;
            drow["ParentCode"] = desc.parent_code;
            drow["price"] = desc.price;
            string query = DbconUtility.GetQuery(1, drow);
            if (!DbConn.Insert(drow, false))
            {
                // set    log
                var url = HttpContext.Request.Host + HttpContext.Request.Path;
                Utility.setLog(url, query, Dns.GetHostName());

            }
            return Ok(Utility.ResponseMessage("data is inserted succesfully", false));
        }
        catch (Exception ex)
        {
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            return StatusCode(StatusCodes.Status500InternalServerError, " the data is not inserted succesfuly");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    // [HttpOptions("addNewMenus")]
    // public IActionResult Options()
    // {
    //     Response.Headers.Add("Access-Control-Allow-Origin", "*"); // Adjust as needed
    //     Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS"); // Allowed methods
    //     Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type"); // Allowed headers
    //     return Ok();
    // }
    [HttpPatch("updatemenus")]
    [BaseUrlRoute()]
    public async Task<ActionResult> updatemenus(DescriptionModification modify)
    {
        DbConn.OpenConn();
        string updatedata = string.Empty;
        try
        {
            if (modify.description is null)
            {
                updatedata = "update tblMenus set price=" + modify.price;
            }
            else if (modify.price < 0)
            {
                updatedata = "update tblMenus set description=" + modify.description;
            }
            else if (modify.description is not null && modify.price > 0)
            {
                updatedata = "update tblMenus set  description='" + modify.description + "',price=" + modify.price;
            }
            if (!DbConn.Execute(updatedata))
            {
                return Ok("data is succesfully  updated");
            }
            else
            {
                return BadRequest(" the data is not update");
            }

        }
        catch (Exception ex)
        {
            // set log
            return StatusCode(StatusCodes.Status500InternalServerError, " the data is not updated succesfuly");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }

    [HttpGet("getParentMenus")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getMenuname()
    {
        DataTable dt = new DataTable();
        string description = "select * from tblMenus where ParentCode=3";
        DbConn.FillData(dt, description);
        string parent_code = string.Empty;
        string desctiptions = string.Empty;

        List<object> items = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                parent_code = row["ParentCode"] + "";
                desctiptions = row["description"] + "";
                var userResponse = new
                {
                    parent_code = parent_code,
                    desctiptions = desctiptions,

                };
                items.Add(userResponse);
            }
            return Ok(Utility.ResponseMessage(items, false));
        }
        else
        {
            return NotFound("the items are not found");
        }
    }
}