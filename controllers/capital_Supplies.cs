using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
namespace BudgetP;
[ApiController]
public class capital_Supplies : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("addCapitalSupplies")]
    [BaseUrlRoute()]
    public async Task<ActionResult> addCapitalSupplies(capital_Supplies_M[] capital)
    {
        DbConn.OpenConn();
        try
        {
            if (capital.Length < 1)
            {
                BadRequest("body must have appropriate data");
            }

            foreach (var data in capital)
            {
                string singleprice = "select unit_price from  tblItems where item_code='" + data.item + "'";
                DataTable dt2 = new DataTable();
                DbConn.FillData(dt2, singleprice);
                if (dt2.Rows.Count < 1)
                {
                    return BadRequest("unit price of this item is not set");
                }
                decimal price = Convert.ToInt16(data.quantity) * Convert.ToDecimal(dt2.Rows[0]["unit_price"]);
                DataRow drow;
                DataTable dt;
                dt = DbConn.GetDataTable("tbl_capital_supplies");
                drow = dt.NewRow();
                drow["branch_code"] = data.branch_code;
                drow["parent_code"] = data.parent_code;
                drow["quarter"] = data.quarter;
                drow["item_code"] = data.item;
                drow["qty"] = data.quantity;
                drow["unitpirce"] = dt2.Rows[0]["unit_price"].ToString();
                drow["totalbudget"] = price.ToString();
                drow["new"] = data.New;
                drow["replacement"] = data.replacement;
                drow["isfinalized"] = 0;
                drow["district_status"] = 0;
                drow["corp_hr_status"] = 0;
                drow["finalapproved"] = 0;
                string query = DbconUtility.GetQuery(1, drow);
                if (!DbConn.Insert(drow, false))
                {
                    var url = HttpContext.Request.Host + HttpContext.Request.Path;
                    Utility.setLog(url, query, Dns.GetHostName());
                    return StatusCode(StatusCodes.Status500InternalServerError, "something went wrong please check your input");
                }
            }
            return Ok(Utility.ResponseMessage("data is succesfully inserted", false));
        }
        catch (Exception ex)
        {
            //  set log
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Internal server error occurred.");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    [HttpPost("getsuppliesbybranch")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getsuppliesbybranch(Capital capital)
    {
        DataTable dt = new DataTable();
        string data = @"select  D.DeptDesc, I.item_name,I.model, S.* from tbl_capital_supplies as S
        left join stock_data..tblitems as I on S.Item_code=I.item_code
        left join hr_pay_data..tblDepartments as D on D.DeptCode=S.branch_code
        where  branch_code='" + capital.branch_code + "' and s.item_code like'1%'";
        DbConn.FillData(dt, data);
        List<Object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {

                var userResponse = new
                {
                    DeptDesc = row["DeptDesc"] + "",
                    item_name = row["item_name"] + "",
                    item_code = row["item_code"] + "",
                    model = row["model"] + "",
                    quarter = row["quarter"] + "",
                    qty = row["qty"] + "",
                    unitPrice = row["unitpirce"] + "",
                    totalbudget = row["totalbudget"] + ""
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else return NoContent();
    }

    [HttpPost("getfixedbybranch")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getfixedbybranch(Capital capital)
    {
        DataTable dt = new DataTable();
        string data = @"select  D.DeptDesc, I.item_name,I.model, S.* from tbl_capital_supplies as S
        left join stock_data..tblitems as I on S.Item_code=I.item_code
        left join hr_pay_data..tblDepartments as D on D.DeptCode=S.branch_code
        where  branch_code='" + capital.branch_code + "' and s.item_code like'0%'";
        DbConn.FillData(dt, data);
        List<Object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {

                var userResponse = new
                {
                    DeptDesc = row["DeptDesc"] + "",
                    item_name = row["item_name"] + "",
                    Item_code = row["Item_code"] + "",
                    model = row["model"] + "",
                    quarter = row["quarter"] + "",
                    qty = row["qty"] + "",
                    unitPrice = row["unitpirce"] + "",
                    totalbudget = row["totalbudget"] + ""
                };
                Obj.Add(userResponse);
            }

            return Ok(Obj);
        }
        else return NoContent();
    }
    [HttpPost("getcapitalexpenditure")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getCapitalexpenditure(EmployeeAllowances__M _branch)
    {
        DataTable dt = new DataTable();
        string data = @"select  D.DeptDesc, I.item_name,I.model, S.* from tbl_capital_supplies as S
        left join stock_data..tblitems as I on S.Item_code=I.item_code
        left join hr_pay_data..tblDepartments as D on D.DeptCode=S.branch_code
        where  branch_code='" + _branch.branch_code + "'";
        DbConn.FillData(dt, data);
        List<Object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    DeptDesc = row["DeptDesc"] + "",
                    item_name = row["item_name"] + "",
                    model = row["model"] + "",
                    quarter = row["quarter"] + "",
                    qty = row["qty"] + "",
                    unitPrice = row["unitpirce"] + "",
                    totalbudget = row["totalbudget"] + ""
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else return NoContent();
    }
    [HttpPost("getexpenditureBydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getCapitalexpenditure(District dist)
    {
        DataTable dt = new DataTable();
        string data = "select * from ViewExpenditure_ByDistricts where deptcode='" + dist.district_code + "' order by deptdesc";
        DbConn.FillData(dt, data);
        List<Object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    DeptDesc = row["DeptDesc"] + "",
                    SubGroup = row["name"] + "",
                    qty = row["qty"] + "",
                    Price = row["price"] + "",
                    totalbudget = row["totalbudget"] + ""
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else return NoContent();
    }
    [HttpPost("getsuppliesdata")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getsuppliesdata(EmployeeAllowances__M _branch)
    {
        DataTable dt = new DataTable();
        string data = "select  i.item_name, c.* from  tbl_capital_supplies as c left join stock_data..tblitems as i" +
          " on i.item_code=c.Item_code where c.Item_code like '1%' and branch_code='" + _branch.branch_code + "'";
        List<Object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {

                var userResponse = new
                {
                    // DeptDesc = row["DeptDesc"] + "",
                    item_name = row["item_name"] + "",
                    // model = row["model"] + "",
                    quarter = row["quarter"] + "",
                    Item_code = row["Item_code"] + "",
                    qty = row["qty"] + "",
                    unitPrice = row["unitpirce"] + "",
                    totalbudget = row["totalbudget"] + ""
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else return NoContent();
    }
    [HttpPost("getfixeddata")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getfixeddata(EmployeeAllowances__M _branch)
    {
        DataTable dt = new DataTable();
        string data = "select  i.item_name, c.* from  tbl_capital_supplies as c left join stock_data..tblitems as i" +
           " on i.item_code=c.Item_code where c.Item_code like '1%' and branch_code='" + _branch.branch_code + "'";
        DbConn.FillData(dt, data);
        List<Object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    // DeptDesc = row["DeptDesc"] + "",
                    item_name = row["item_name"] + "",
                    // model = row["model"] + "",
                    quarter = row["quarter"] + "",
                    qty = row["qty"] + "",
                    unitPrice = row["unitpirce"] + "",
                    totalbudget = row["totalbudget"] + ""
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else
            return NoContent();
    }
    [HttpGet("getItemsByBranch")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getItemsByBranch(string branch_code)
    {
        DataTable dt = new DataTable();
        string data = @"select  D.DeptDesc, I.item_name,I.model, S.* from tbl_capital_supplies as S
        left join stock_data..tblitems as I on S.Item_code=I.item_code
        left join hr_pay_data..tblDepartments as D on D.DeptCode=S.branch_code
        where  branch_code='" + branch_code + "'";
        DbConn.FillData(dt, data);
        List<Object> Obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                var userResponse = new
                {
                    DeptDesc = row["DeptDesc"] + "",
                    item_name = row["item_name"] + "",
                    model = row["model"] + "",
                    quarter = row["quarter"] + "",
                    qty = row["qty"] + "",
                    unitPrice = row["unitpirce"] + "",
                    totalbudget = row["totalbudget"] + ""
                };
                Obj.Add(userResponse);
            }
            return Ok(Obj);
        }
        else return NoContent();
    }
    [HttpPut("updateCapitalSupplies")]
    [BaseUrlRoute()]
    public async Task<ActionResult> updateCapitalSupplies(CapitalSupply data)
    {
        DbConn.OpenConn();
        try
        {
            string singleprice = "select unit_price from  tblItems where item_code='" + data.item_code + "'";
            DataTable dt2 = new DataTable();
            DbConn.FillData(dt2, singleprice);
            if (dt2.Rows.Count < 1)
            {
                return BadRequest("unit price of this item is not set");
            }
            decimal total = Convert.ToDecimal(data.qty) * Convert.ToDecimal(dt2.Rows[0]["unit_price"]);

            string update = "update tbl_capital_supplies set quarter='" + data.quarter + "',qty='" + data.qty + "',totalbudget='" + total + "' where" +
            " branch_code='" + data.branch_code + "' and item_code='" + data.item_code + "'";

            if (!DbConn.Execute(update))
            {
                var url = HttpContext.Request.Host + HttpContext.Request.Path;
                Utility.setLog(url, update, Dns.GetHostName());
                return StatusCode(StatusCodes.Status500InternalServerError, "something went wrong please check your input");
            }
            return Ok(Utility.ResponseMessage("data is succesfully inserted", false));
        }
        catch (Exception ex)
        {
            //  set log
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred.");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}
