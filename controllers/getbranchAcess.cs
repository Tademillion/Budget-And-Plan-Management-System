using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
public class getbranchAcess : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("getbranch_access")]
    [BaseUrlRoute()]
    public async Task<ActionResult> BranchAccess([FromBody] Newmodel branch)
    {
        DbConn.OpenConn();
        try
        {
            List<object> branchs = new List<object>();
            string access = string.Empty;
            string[] districtCodes = branch.district_code ?? Array.Empty<string>();
            districtCodes = districtCodes.Where(code => !string.IsNullOrWhiteSpace(code)).ToArray();
            DataTable dt = new DataTable();
            string branchright = string.Empty;
            bool val = false;
            for (int i = 0; i < districtCodes.Length; i++)
            {
                if (districtCodes[i].Length > 0)
                {
                    val = true;
                }
            }
            if (val)
            {
                string formattedDistrictCodes = string.Join(",", districtCodes.Select(code => $"'{code}'"));
                // branchright = $"select distinct [Section Code] as sectionCode,[Section Name] as sectioName from hr_pay_data..ViewEmpSearch where  [Section Code] in( select [Section Code] from hr_pay_data..ViewEmpSearch where [Dept. Code] in({formattedDistrictCodes}))";
                branchright = @"select COMPANY_CODE as sectionCode,COMPANY_NAME as sectioName from hr_pay_data..tbl_district_branch where  COMPANY_CODE in (" + formattedDistrictCodes + ")union select DeptCode,DeptDesc from hr_pay_data..tblDepartments where ParentCode in (" + formattedDistrictCodes + ")";
            }
            else
            {
                // branchright = "select [Section Code] as sectionCode,[Section Name] as sectioName from hr_pay_data..ViewEmpSearch where [Section Code]='" + branch.branch_code + "'";
                branchright = @"select COMPANY_CODE as sectionCode,COMPANY_NAME as sectioName from hr_pay_data..tbl_district_branch where  COMPANY_CODE='" + branch.branch_code + "'union select DeptCode,DeptDesc from hr_pay_data..tblDepartments where ParentCode = '" + branch.branch_code + "'";
            }
            DbConn.FillData(dt, branchright);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string branch_code = row["sectionCode"] + "";
                    string branch_name = row["sectioName"] + "";
                    var userResponse = new
                    {
                        branch_code = branch_code,
                        branch_name = branch_name
                    };
                    branchs.Add(userResponse);
                }
            }
            return Ok(Utility.ResponseMessage(branchs, false));
        }
        catch (Exception ex)
        {
            return BadRequest("the request is not proccessed");
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
}
