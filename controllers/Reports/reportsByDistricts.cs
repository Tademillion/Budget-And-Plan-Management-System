using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace BudgetP;

public class reportsByDistricts : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));

    [HttpPost("Depositbydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getreportsbydistrict([FromBody] District district)
    {
        DataTable dt = new DataTable();
        string getreport = @"select * from View_TotalDeposit where 
                ParentCode in(
                select menucode from tblMenus where ParentCode = 4
                ) and district_code = '" + district.district_code + "' order by district_code,orders";
        DbConn.FillData(dt, getreport);
        List<object> OBJ = new List<object>();
        try
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var userResponse = new
                    {
                        description = row["description"] + "",
                        actual = row["actual"] + "",
                        estimated = row["estimated"] + "",
                        netincrement = row["netincrement"] + "",
                        parent_code = row["parent_code"] + "",
                        projected = row["projected"] + "",
                        Jul = row["jul"] + "",
                        Aug = row["aug"] + "",
                        Sep = row["sep"] + "",
                        Oct = row["oct"] + "",
                        Nov = row["nov"] + "",
                        Dec = row["dec"] + "",
                        Jan = row["jan"] + "",
                        Feb = row["feb"] + "",
                        Mar = row["mar"] + "",
                        Apr = row["apr"] + "",
                        May = row["may"] + "",
                        Jun = row["jun"] + ""
                    };
                    OBJ.Add(userResponse);
                }
                return Ok(OBJ);
            }
            else
            {
                return NoContent();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "something wennt wrong");
        }
    }



    [HttpPost("Allocationbydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> Allocationbydistrict([FromBody] District district)
    {
        DataTable dt = new DataTable();
        string getreport = @"select * from View_TotalDeposit where 
                ParentCode in(
                select menucode from tblMenus where ParentCode = 6
                ) and district_code = '" + district.district_code + "' order by district_code,orders";
        DbConn.FillData(dt, getreport);
        List<object> OBJ = new List<object>();
        try
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var userResponse = new
                    {
                        description = row["description"] + "",
                        actual = row["actual"] + "",
                        estimated = row["estimated"] + "",
                        netincrement = row["netincrement"] + "",
                        parent_code = row["parent_code"] + "",
                        projected = row["projected"] + "",
                        Jul = row["jul"] + "",
                        Aug = row["aug"] + "",
                        Sep = row["sep"] + "",
                        Oct = row["oct"] + "",
                        Nov = row["nov"] + "",
                        Dec = row["dec"] + "",
                        Jan = row["jan"] + "",
                        Feb = row["feb"] + "",
                        Mar = row["mar"] + "",
                        Apr = row["apr"] + "",
                        May = row["may"] + "",
                        Jun = row["jun"] + ""
                    };
                    OBJ.Add(userResponse);
                }
                return Ok(OBJ);
            }
            else
            {
                return NoContent();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "something wennt wrong");
        }
    }


    [HttpPost("Incomebydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> Incomebydistrict([FromBody] District district)
    {
        DataTable dt = new DataTable();
        string getreport = @"select * from View_TotalDeposit where 
                ParentCode in(
                select menucode from tblMenus where ParentCode = 80
                ) and district_code = '" + district.district_code + "' order by district_code,orders";
        DbConn.FillData(dt, getreport);
        List<object> OBJ = new List<object>();
        try
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var userResponse = new
                    {
                        description = row["description"] + "",
                        actual = row["actual"] + "",
                        estimated = row["estimated"] + "",
                        netincrement = row["netincrement"] + "",
                        parent_code = row["parent_code"] + "",
                        projected = row["projected"] + "",
                        Jul = row["jul"] + "",
                        Aug = row["aug"] + "",
                        Sep = row["sep"] + "",
                        Oct = row["oct"] + "",
                        Nov = row["nov"] + "",
                        Dec = row["dec"] + "",
                        Jan = row["jan"] + "",
                        Feb = row["feb"] + "",
                        Mar = row["mar"] + "",
                        Apr = row["apr"] + "",
                        May = row["may"] + "",
                        Jun = row["jun"] + ""
                    };
                    OBJ.Add(userResponse);
                }
                return Ok(OBJ);
            }
            else
            {
                return NoContent();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "something wennt wrong");
        }
    }

    [HttpPost("Expensebydistrict")]
    [BaseUrlRoute()]
    public async Task<IActionResult> Expensebydistrict([FromBody] District district)
    {
        DataTable dt = new DataTable();
        string getreport = @"select * from View_TotalDeposit where 
                ParentCode in(
                select menucode from tblMenus where ParentCode = 81
                ) and district_code = '" + district.district_code + "' order by district_code,orders";
        DbConn.FillData(dt, getreport);
        List<object> OBJ = new List<object>();
        try
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var userResponse = new
                    {
                        description = row["description"] + "",
                        actual = row["actual"] + "",
                        estimated = row["estimated"] + "",
                        netincrement = row["netincrement"] + "",
                        parent_code = row["parent_code"] + "",
                        projected = row["projected"] + "",
                        Jul = row["jul"] + "",
                        Aug = row["aug"] + "",
                        Sep = row["sep"] + "",
                        Oct = row["oct"] + "",
                        Nov = row["nov"] + "",
                        Dec = row["dec"] + "",
                        Jan = row["jan"] + "",
                        Feb = row["feb"] + "",
                        Mar = row["mar"] + "",
                        Apr = row["apr"] + "",
                        May = row["may"] + "",
                        Jun = row["jun"] + ""
                    };
                    OBJ.Add(userResponse);
                }
                return Ok(OBJ);
            }
            else
            {
                return NoContent();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "something wennt wrong");
        }
    }


}
