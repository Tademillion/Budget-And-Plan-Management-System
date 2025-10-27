using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BudgetP;
public class GetDistricts : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpGet("getalldistricts")]
    [BaseUrlRoute()]
    public async Task<IActionResult> getAllDistricts()
    {
        DbConn.OpenConn();
        try
        {
            string district = @"select  district_code, DISTRICT from hr_pay_data..tbl_district_branch where district_code is not null group by district_code, DISTRICT
           union all select DeptCode,DeptDesc from  hr_pay_data..tblDepartments where ParentCode=1 and DeptCode is not null  ";
            List<object> list = new List<object>();
            DataTable dt = new DataTable();
            DbConn.FillData(dt, district);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var userResponse = new
                    {
                        districtName = row["DISTRICT"],
                        districtCode = row["district_code"],
                    };
                    list.Add(userResponse);
                }
            }
            return Ok(list);
        }
        catch (Exception)
        {
            return BadRequest("something went wrong please contact admin");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}