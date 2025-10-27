using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP
{
    public class getformatbyid : ControllerBase
    {
        DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
        // [HttpPost("/getformats_by_brach")]
        [HttpPost("getformats_by_branch")]
        [BaseUrlRoute()]
        public async Task<ActionResult> getFormatsByID([FromBody] Newmodel branch)
        {
            DbConn.OpenConn();
            List<object> sources = new List<object>();
            string source = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                //string source = "select * from tblformData where branch_code='" + data.branch_code + "'";
                string[] districtCodes = branch.district_code;
                if (districtCodes != null && districtCodes.Length > 0)
                {
                    DataTable dt2 = new DataTable();
                    string formattedDistrictCodes = string.Join(",", districtCodes.Select(code => $"'{code}'"));
                    string districtright = $"select  DeptCode as sectioncode from hr_pay_data..tblDepartments where ParentCode in({formattedDistrictCodes})";
                    DbConn.FillData(dt2, districtright);
                    if (dt2.Rows.Count > 0)
                    { // select all data in the branch
                        string storeList = string.Empty;
                        string[] storeCodesArray = new string[0];
                        List<string> storeCodes = new List<string>();
                        foreach (DataRow row in dt2.Rows)
                        {
                            storeCodes.Add(row["sectioncode"].ToString());
                        }

                        storeCodesArray = storeCodes.ToArray();
                        string[] stores = storeCodesArray;
                        storeList = string.Join(", ", stores.Select(s => $"'{s}'")); // join the array
                        source = $"SELECT m.description, f.parent_code, f.* FROM tblformData AS f " +
                     $"LEFT JOIN tblMenus AS m ON m.MenuCode = f.parent_code " +
                     $"WHERE branch_code IN ({storeList})";
                    }
                }
                else
                {
                    source = "select m.description, f.parent_code,f.* from tblformData as  f left join tblMenus as m on m.MenuCode=f.parent_code where branch_code='" + branch.branch_code + "'";
                }
                DbConn.FillData(dt, source);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string estimated = row["estimated"] + "";
                        string Branchcode = row["branch_code"] + "";
                        string actual = row["actual"] + "";
                        string netincrement = row["netincrement"] + "";
                        string projected = row["projected"] + "";
                        string parentcode = row["parent_code"] + "";
                        string description = row["description"] + "";
                        string Jul = row["jul"] + "";
                        string Aug = row["aug"] + "";
                        string Sep = row["sep"] + "";
                        string Oct = row["oct"] + "";
                        string Nov = row["nov"] + "";
                        string Dec = row["dec"] + "";
                        string Jan = row["jan"] + "";
                        string Feb = row["feb"] + "";
                        string Mar = row["mar"] + "";
                        string Apr = row["apr"] + "";
                        string May = row["may"] + "";
                        string Jun = row["jun"] + "";
                        var userResponse = new
                        {
                            estimated = estimated,
                            actual = actual,
                            netincrement = netincrement,
                            projected = projected,
                            // branch_code = branch,
                            parentcode = parentcode,
                            description = description,
                            Jul = Jul,
                            Aug = Aug,
                            Sep = Sep,
                            Oct = Oct,
                            Nov = Nov,
                            Dec = Dec,
                            Jan = Jan,
                            Feb = Feb,
                            Mar = Mar,
                            Apr = Apr,
                            May = May,
                            Jun = Jun
                        };
                        sources.Add(userResponse);
                    }
                }
                return Ok(Utility.ResponseMessage(sources, false));
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
        [HttpPost("getbranchreportss")]
        [BaseUrlRoute()]
        public async Task<IActionResult> getBranchReport([FromBody] EmployeeAllowances__M _branch)
        {
            DataTable dt = new DataTable();
            string report = "select M.description, t.* from tblFormdata as t left join tblMenus as M on t.parent_code=M.MenuCode where branch_code='" + _branch.branch_code + "'";
            List<object> Obj = new List<object>();
            DbConn.FillData(dt, report);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string estimated = row["estimated"] + "";
                    string Branchcode = row["branch_code"] + "";
                    string actual = row["actual"] + "";
                    string netincrement = row["netincrement"] + "";
                    string projected = row["projected"] + "";
                    string parentcode = row["parent_code"] + "";
                    string description = row["description"] + "";
                    string Jul = row["jul"] + "";
                    string Aug = row["aug"] + "";
                    string Sep = row["sep"] + "";
                    string Oct = row["oct"] + "";
                    string Nov = row["nov"] + "";
                    string Dec = row["dec"] + "";
                    string Jan = row["jan"] + "";
                    string Feb = row["feb"] + "";
                    string Mar = row["mar"] + "";
                    string Apr = row["apr"] + "";
                    string May = row["may"] + "";
                    string Jun = row["jun"] + "";
                    var userResponse = new
                    {
                        estimated = estimated,
                        actual = actual,
                        netincrement = netincrement,
                        projected = projected,
                        // branch_code = branch,
                        parentcode = parentcode,
                        description = description,
                        Jul = Jul,
                        Aug = Aug,
                        Sep = Sep,
                        Oct = Oct,
                        Nov = Nov,
                        Dec = Dec,
                        Jan = Jan,
                        Feb = Feb,
                        Mar = Mar,
                        Apr = Apr,
                        May = May,
                        Jun = Jun
                    };
                    Obj.Add(userResponse);
                }
                return Ok(Obj);
            }
            return Ok(Obj);
        }
        [HttpPost("getbranchreports")]
        [BaseUrlRoute()]
        public async Task<IActionResult> getAgregateTotal([FromBody] EmployeeAllowances__M _branch)
        {
            try
            {
                DataTable dt = new DataTable();
                string report = "select * from view_allTotal where branch_code='" + _branch.branch_code + "' order by branch_code,par_order";
                List<object> Obj = new List<object>();
                DbConn.FillData(dt, report);
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
                        Obj.Add(userResponse);
                    }
                    return Ok(Obj);
                }
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "something went wrong please ");
            }
        }
        [HttpPost("getexpensebybranch")]
        [BaseUrlRoute()]
        public async Task<IActionResult> getexpensebybranch([FromBody] EmployeeAllowances__M _branch)
        {
            try
            {
                DataTable dt = new DataTable();
                string report = @"select M.description, D.* from View_reportBy_Branch as D  left  join tblMenus as M  on M.menucode = D.parent_code  left join  tblMenus as T on T.menucode = M.ParentCode
                        where t.ParentCode = 81 and D.branch_code = '" + _branch.branch_code + "'";
                List<object> Obj = new List<object>();
                DbConn.FillData(dt, report);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var userResponse = new
                        {
                            description = row["description"] + "",
                            actual = row["actual"] + "",
                            estimated = row["estimated"] + "",
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
                        Obj.Add(userResponse);
                    }
                    return Ok(Obj);
                }
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "something went wrong please ");
            }
        }
        [HttpPost("getIncomebybranch")]
        [BaseUrlRoute()]
        public async Task<IActionResult> getIncomebybranch([FromBody] EmployeeAllowances__M _branch)
        {
            try
            {
                DataTable dt = new DataTable();
                string report = @"select M.description, D.* from tblformData as D  left  join tblMenus as M  on M.menucode=D.parent_code  left join  tblMenus as T on T.menucode=M.ParentCode
                           where t.ParentCode=80 and D.branch_code='" + _branch.branch_code + "'";
                List<object> Obj = new List<object>();
                DbConn.FillData(dt, report);
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
                        Obj.Add(userResponse);
                    }
                    return Ok(Obj);
                }
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "something went wrong please ");
            }
        }
    }
}

