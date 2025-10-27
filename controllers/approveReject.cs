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
            string isapproved = "select ISNULL( district_status,0) as district_status from tblformData where branch_code='" + status.branch_code + "'";
            DataTable dt2 = new DataTable();
            DbConn.FillData(dt2, isapproved);
            if (Convert.ToInt16(dt2.Rows[0]["district_status"]) == 1)
            {
                return Ok(Utility.ResponseMessages("", "data is already approved", false));
            }
            // take history
            // if (Utility.createHistory(DbConn, "update", status.userId, "tblformData", "parent_code in(select MenuCode from tblMenus where ParentCode='" + status.branch_code + "')"))
            // { 
            changestatus = "update  tblformData set district_status='" + status.status + "',Reason='" + status.reason + "' where branch_code='" + status.branch_code + "'";
            if (DbConn.Execute(changestatus))
            {
                if (Convert.ToInt16(status.status) == 1)
                    return Ok(Utility.ResponseMessages("", "data is approved succesfully", false));
                else if (Convert.ToInt16(status.status) == 2)
                    return Ok(Utility.ResponseMessages("", "data is rejected", false));
                else
                    return Ok(Utility.ResponseMessages("", "invalid status", false));
            }
            else
                return BadRequest(Utility.ResponseMessages("", "something went wrong please try againg", false));
            // }
            // else
            // {
            //     return StatusCode(StatusCodes.Status500InternalServerError, "cannot create history");
            // }
        }
        catch (Exception ex)
        {
            // set log
            return BadRequest(Utility.ResponseMessages("", "something went wrong please try againg", false));
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    [HttpPost("approverejectbydistrict")]
    [BaseUrlRoute()]
    public async Task<ActionResult> approverejectbydistrict([FromBody] approvedbyModel status)
    {
        // create history
        DbConn.OpenConn();
        string changestatus = string.Empty;
        try
        {
            DataTable dt;
            DataRow row;
            dt = DbConn.GetDataTable("tbl_district_status");
            row = dt.NewRow();
            row["districtcode"] = status.district_code;
            row["fiscalYear"] = Utility.getFiscalYear(DbConn);
            row["updatedept"] = status.bywhom;
            row["reason"] = status.reason;
            row["status"] = status.status;
            row["UserId"] = status.UserId;
            string checkapprovedrejected = "select * from tbl_district_status where districtcode='" + status.district_code + "' and fiscalYear='" + Utility.getFiscalYear(DbConn) + "'  and  updatedept='" + status.bywhom + "'";
            string isapprovedbefore = "select * from tbl_district_status where districtcode='" + status.district_code + "' and fiscalYear='" + Utility.getFiscalYear(DbConn) + "'  and status='1' and updatedept='" + status.bywhom + "' ";
            DataTable dt2 = new DataTable();
            DbConn.FillData(dt2, isapprovedbefore);
            if (dt2.Rows.Count > 0)
            {
                return Ok(Utility.ResponseMessages("", "its already approved", false));
            }
            DataTable dt1 = new DataTable();
            DbConn.FillData(dt1, checkapprovedrejected);
            string query = string.Empty;
            if (dt1.Rows.Count > 0)
            {
                query = DbconUtility.GetQuery(2, row);
                query = query + " districtcode='" + status.district_code + "' and fiscalYear='" + Utility.getFiscalYear(DbConn) + "' ";
                DbConn.Execute(query);
                if (Convert.ToInt16(status.status) == 1)
                {
                    return Ok(Utility.ResponseMessages("", "data is approved successfully", false));
                }
                else if (Convert.ToInt16(status.status) == 2)
                {
                    return Ok(Utility.ResponseMessages("", "data is rejected ", false));
                }
                else
                    return Ok(Utility.ResponseMessages("", "invalid status", false));
                // update status
            }
            else
            {
                // insert new row
                query = DbconUtility.GetQuery(1, row);
                if (!DbConn.Insert(row, false))
                {
                    Utility.setLog("approvereject", query, status.UserId);
                    return Ok(Utility.ResponseMessages("", "something went wrong please check your input", false));
                }
                else
                {
                    if (Convert.ToInt16(status.status) == 1)
                        return Ok(Utility.ResponseMessages("", "data is successfully approved", false));
                    else if (Convert.ToInt16(status.status) == 2)
                        return Ok(Utility.ResponseMessages("", "data is  rejected", false));
                    else
                        return Ok(Utility.ResponseMessages("", "invalid status", false));
                }
            }
        }
        catch (Exception ex)
        {
            // set log
            // return StatusCode(StatusCodes.Status500InternalServerError, "status is not changed");
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
