using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
public class getFormatsByID : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpGet("getformats_by_id")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getformatsbyid(BranchCodeM branch)
    {
        DbConn.OpenConn();
        List<object> formats = new List<object>();
        try
        {   // int ids = Convert.ToInt32(id);
            DataTable dt = new DataTable();
            //string source = "select * from tblformData where branch_code='" + data.branch_code + "'";
            string getformat = "select MenuCode,description  from tblMenus where ParentCode='" + branch.parent_code + "'";
            DbConn.FillData(dt, getformat);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string description = row["description"] + "";
                    string id = row["MenuCode"] + "";
                    var userResponse = new
                    {
                        description = description,
                        parent_code = id
                        // pare = row["description"]
                    };
                    formats.Add(userResponse);
                }
            }
            return Ok(Utility.ResponseMessage(formats, false));
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
    [HttpPost("get_bybranch_parentcode")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getbybranch_parentcode([FromBody] BranchCodeM branch)
    {
        DbConn.OpenConn();
        List<object> formats = new List<object>();
        try
        {   // int ids = Convert.ToInt32(id);
            DataTable dt = new DataTable();
            //string source = "select * from tblformData where branch_code='" + data.branch_code + "'";
            //string getformat = "select m.description, f.parent_code,f.* from tblformData as  f left join tblMenus as m on m.MenuCode=f.parent_code where parent_code='" + branch.parent_code + "' and branch_code='" + branch.branch_code + "'";
            //string getformat = "select m.description, f.parent_code,f.* from tblformData as  f left join tblMenus as m on m.MenuCode=f.parent_code where     branch_code='" + branch.branch_code + "' and f.parent_code in(select MenuCode  from tblMenus as s where s.ParentCode='" + branch.parent_code + "')";
            string getformat = @"select m.description, f.branch_code,actual,estimated,projected,jul,aug,sep,oct,Nov,Dec,Jan,Feb,Mar,Apr,May,jun,parent_code
             from tblformData as  f left join tblMenus as m on m.MenuCode=f.parent_code where     branch_code='" + branch.branch_code + "' and f.parent_code" +
              " in(select MenuCode from tblMenus as s where s.ParentCode = '" + branch.parent_code + "')" +
             "union select description,'ET0010001',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,menucode from tblMenus where ParentCode = '" + branch.parent_code + "' and" +
              " menucode not in(select parent_code from tblformData where branch_code = '" + branch.branch_code + "')";
            DbConn.FillData(dt, getformat);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string estimated = row["estimated"] + "";
                    string Branchcode = row["branch_code"] + "";
                    string actual = row["actual"] + "";
                    // string netincrement = row["netincrement"] + "";
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
                        // netincrement = netincrement,
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
                    formats.Add(userResponse);
                }
                return Ok(Utility.ResponseMessage(formats, false));
            }
            else
                return NoContent();
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