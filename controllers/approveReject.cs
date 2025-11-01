using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace BudgetP;

public class approveReject : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpPost("approvereject")]
    [BaseUrlRoute()]
    public async Task<ActionResult> approverejected([FromBody] ApproveRejected_M status)
    {
        // create history
        DbConn.OpenConn();
        string changestatus = string.Empty;
        try
        {
            if (status == null)
            {
                return BadRequest("body cannot be null");
            }
            string isapproved = "select ISNULL( status,0) as status from tblapproveRejectBudget where branch_code='" + status.branch_code + "' and parentcode='" + status.parentcode + "'";
            DataTable dt2 = new DataTable();
            DbConn.FillData(dt2, isapproved);
            if (dt2.Rows.Count > 0)
            {
                if (Convert.ToInt16(dt2.Rows[0]["status"]) == 1)
                {
                    return Ok(Utility.ResponseMessages("", "data is already approved", false));
                }
                string reject = "update  tblapproveRejectBudget set status='" + status.status + "' ,reason='" + status.reason + "' where branch_code='" + status.branch_code + "' and parentcode='" + status.parentcode + "'";
                var result = DbConn.Execute(reject);
                if (result)
                    return Ok(new ApiResponse<object>
                    {
                        Message = "the data is rejected succesfully",
                        Success = true
                    });
            }
            // take history
            // if (Utility.createHistory(DbConn, "update", status.userId, "tblformData", "parent_code in(select MenuCode from tblMenus where ParentCode='" + status.branch_code + "')"))
            // { 
            //  reject the data

            DataTable dt;
            DataRow drow;
            dt = DbConn.GetDataTable("tblapproveRejectBudget");
            drow = dt.NewRow();
            drow["branch_code"] = status.branch_code;
            drow["parentcode"] = status.parentcode;
            drow["reason"] = status.reason;
            drow["status"] = status.status;
            drow["crtby"] = status.UserId;
            drow["budgetYear"] = "2025";
            DbConn.Insert(drow, false);

            return Ok(new ApiResponse<object>
            {
                Message = "the data is approved succesfully",
                Success = true
            });

        }
        catch (Exception ex)
        {
            // set log
            Console.WriteLine(ex);
            return BadRequest(Utility.ResponseMessages("", "something went wrong please try againg", false));
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    [HttpPost("approverejectByStrategy")]
    [BaseUrlRoute()]
    public async Task<ActionResult> approverejectbydistrict([FromBody] ApproveRejected_M status)
    {
        DbConn.OpenConn();
        string changestatus = string.Empty;
        try
        {
            if (status == null)
            {
                return BadRequest("body cannot be null");
            }
            string isapproved = "select ISNULL( status,0) as status from tblapproveRejectBudget where branch_code='" + status.branch_code + "' and parentcode='" + status.parentcode + "'";
            DataTable dt2 = new DataTable();
            DbConn.FillData(dt2, isapproved);
            if (dt2.Rows.Count > 0)
            {
                if (Convert.ToInt16(dt2.Rows[0]["status"]) == 1)
                {
                    return Ok(Utility.ResponseMessages("", "data is already approved", false));
                }
                string reject = "update  tblapproveRejectBudget set status='" + status.status + "' ,reason='" + status.reason + "' where district_code='" + status.districtcode + "' and parentcode='" + status.parentcode + "'";
                var result = DbConn.Execute(reject);
                if (result)
                    return Ok(new ApiResponse<object>
                    {
                        Message = "the data is rejected succesfully",
                        Success = true
                    });
            }
            // take history
            // if (Utility.createHistory(DbConn, "update", status.userId, "tblformData", "parent_code in(select MenuCode from tblMenus where ParentCode='" + status.branch_code + "')"))
            // { 
            //  reject the data

            DataTable dt;
            DataRow drow;
            dt = DbConn.GetDataTable("tblapproveRejectBudget");
            drow = dt.NewRow();
            drow["district_code"] = status.districtcode;
            drow["parentcode"] = status.parentcode;
            drow["reason"] = status.reason;
            drow["status"] = status.status;
            drow["crtby"] = status.UserId;
            drow["budgetYear"] = "2025";
            DbConn.Insert(drow, false);

            return Ok(new ApiResponse<object>
            {
                Message = "the data is approved succesfully",
                Success = true
            });

        }
        catch (Exception ex)
        {
            // set log
            Console.WriteLine(ex);
            return BadRequest(Utility.ResponseMessages("", "something went wrong please try againg", false));
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    // get data for hr and other bussinesses
    [HttpPost("getAllDeptData")]
    [BaseUrlRoute()]
    public async Task<IActionResult> RespectiveOrgandata(int id)
    {
        DataTable dt = new DataTable();
        DbConn.OpenConn();
        string data = "";
        if (id == 1)
        {
            // hr
            data = "select D.DeptDesc,J.JobPosDesc, P.* from tblManPower as p left join hr_pay_data..tblDepartments as D on D.DeptCode=P.branch_code  left join hr_pay_data..tblJobPosition as J on J.JobPosId=P.JobPosId";
        }
        else if (id == 2)
        {
            //    coorporation
            data = "select D.DeptDesc,C.* from tbl_capital_supplies as C  left join  hr_pay_data..tblDepartments as D  on D.DeptCode=c.branch_code";
        }
        else if (id == 3)
        {
            // district
        }
        else if (id == 4)
        {
            // 
        }
        else if (id == 5)
        {
            //  bus
        }
        else if (id == 6)
        {
            //  Ban
        }
        DbConn.FillData(dt, data);
        List<object> obj = new List<object>();
        if (dt.Rows.Count > 0)
        {
            if (id == 1)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string jul = row["jul"] + "";
                    string aug = row["aug"] + "";
                    string sep = row["sep"] + "";
                    string oct = row["oct"] + "";
                    string nov = row["nov"] + "";
                    string dec = row["dec"] + "";
                    string jan = row["jan"] + "";
                    string feb = row["feb"] + "";
                    string mar = row["mar"] + "";
                    string apr = row["apr"] + "";
                    string may = row["may"] + "";
                    string jun = row["jun"] + "";
                    string existingStaff = row["ExistIng_Emp"] + "";
                    string Additional_Emp = row["Additional_Emp"] + "";
                    string DeptDesc = row["DeptDesc"] + "";
                    string JobPosDesc = row["JobPosDesc"] + "";
                    var userResponse = new
                    {
                        DeptDesc = DeptDesc,
                        JobPosDesc = JobPosDesc,
                        jul = jul,
                        aug = aug,
                        sep = sep,
                        oct = oct,
                        nov = nov,
                        dec = dec,
                        jan = jan,
                        feb = feb,
                        mar = mar,
                        apr = apr,
                        may = may,
                        jun = jun,
                        Additional_Emp = Additional_Emp,
                        ExistIng_Emp = existingStaff
                    };
                    obj.Add(userResponse);
                }
                return Ok(obj);
            }
            //
            else if (id == 2)
            {
                // corporate
                foreach (DataRow row in dt.Rows)
                {
                    string DeptDesc = row["DeptDesc"] + "";
                    string qty = row["qty"] + "";
                    string unitprice = row["unitpirce"] + "";
                    string totalBudget = row["totalbudget"] + "";
                    string New = row["New"] + "";
                    string Replacement = row["replacement"] + "";
                    var userResponse = new
                    {
                        DeptDesc = DeptDesc,
                        qty = qty,
                        unitprice = unitprice,
                        totalBudget = totalBudget,
                        New = New,
                        Replacement = Replacement,
                    };

                    obj.Add(userResponse);
                }
                return Ok(obj);
            }
        }
        else
        {
            // hr
            return NoContent();
        }

        DbConn.CloseConn();
        return Ok("");
    }
    // get all the man power by distrinc
}
