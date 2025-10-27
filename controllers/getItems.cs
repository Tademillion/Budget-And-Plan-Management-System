using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
[ApiController]
public class getItems : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpGet("getSupplies")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getSupplies(string parentcode)
    {
        DataTable dt = new DataTable();
        string items = "select  distinct item_code,item_name from stock_data..tblitems where item_sub_group='" + parentcode + "' ";
        List<object> item = new List<object>();
        DbConn.FillData(dt, items);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string ItemName = row["item_name"] + "";
                string ItemCode = row["item_code"] + "";
                var userResponse = new
                {
                    ItemName = ItemName,
                    ItemCode = ItemCode
                };
                item.Add(userResponse);
            }
        }
        return Ok(item);
    }
    // get 
    [HttpGet("getIncomeandSupply")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getIncomeandSupply(string parentcode)
    {
        DataTable dt = new DataTable();
        string items = "select * from tblMenus where ParentCode='" + parentcode + "'";
        List<object> item = new List<object>();
        DbConn.FillData(dt, items);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    ItemName = row["description"].ToString(),
                    ItemCode = row["menucode"].ToString()
                };
                item.Add(userResponse);
            }
        }
        return Ok(item);
    }



    [HttpGet("getFixedAsset")]
    public async Task<IActionResult> getFixedAsset(string branch_code)
    {
        DbConn.OpenConn();
        DataTable dt = new DataTable();
        string items = @"select   D.DeptDesc, I.item_name,I.model, S.* from tbl_capital_supplies as S
left join stock_data..tblitems as I on S.Item_code = I.item_code
left join hr_pay_data..tblDepartments as D on D.DeptCode = S.branch_code
where Parent_code like'0%' and branch_code = '" + branch_code + "' ";
        List<object> item = new List<object>();
        DbConn.FillData(dt, items);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string ItemName = row["item_name"] + "";
                string DeptDesc = row["DeptDesc"] + "";
                string qty = row["qty"] + "";
                string unitpirce = row["unitpirce"] + "";
                string totalbudget = row["totalbudget"] + "";
                var userResponse = new
                {
                    ItemName = ItemName,
                    DeptDesc = DeptDesc,
                    qty = qty,
                    unitpirce = unitpirce,
                    totalbudget = totalbudget
                };
                item.Add(userResponse);
            }
        }
        DbConn.CloseConn();
        return Ok(item);
    }
    [HttpGet("getallitems")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getAllItems()
    {
        DbConn.OpenConn();
        DataTable dt = new DataTable();
        List<object> item = new List<object>();
        string allitems = "select distinct item_name,item_code from  stock_data..tblitems ";
        DbConn.FillData(dt, allitems);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string ItemName = row["item_name"] + "";
                string ItemCode = row["item_code"] + "";

                var userResponse = new
                {
                    ItemName = ItemName,
                    ItemCode = ItemCode
                };
                item.Add(userResponse);
            }
        }
        DbConn.CloseConn();
        return Ok(item);
    }
    // edit and registre items
    [HttpPost("registerItem")]
    [BaseUrlRoute()]
    public async Task<IActionResult> registerItem(Items item)
    {
        DbConn.OpenConn();
        try
        {
            //string additem="insert into tblitems(item_code,unit_price,crtdt,crtby,crtws) values()"
            string additem = "insert into tblitems values('" + item.item + "','" + item.unitPrice + "','" + DateTime.Now + "','admin','" + Dns.GetHostName + "')";
            if (!DbConn.Execute(additem))
            {
                // set log 
                return StatusCode(StatusCodes.Status500InternalServerError, "something went wrong please check your input");
            }
            return Ok("data is added successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "data is not registered");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    // edit items
    [HttpPut("edititems")]
    [BaseUrlRoute()]
    public async Task<IActionResult> editItem(Items item)
    {
        DbConn.OpenConn();
        try
        {
            string updateitem = " update tblItems set unit_price='" + item.unitPrice + "' where item_code=" + item.item;
            if (!DbConn.Execute(updateitem))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "data is not updated");
            }
            return Ok("item is updated succssfully");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "data is not updated");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}
