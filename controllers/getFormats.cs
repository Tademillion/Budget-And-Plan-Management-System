using System.Data;
using Microsoft.AspNetCore.Mvc;
namespace BudgetP;
public class getFormats : ControllerBase
{
    DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
    [HttpGet("getformats")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getFormat()
    {
        DbConn.OpenConn();
        List<object> sources = new List<object>();
        try
        {
            string formatName = string.Empty; string formatID = string.Empty;
            string parentcode = string.Empty;
            DataTable dt = new DataTable();
            string source = "select * from tblFormats where parentcode in(select formatId from tblFormats where parentcode =0) ";
            DbConn.FillData(dt, source);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    formatName = row["discription"] + "";
                    formatID = row["formatId"] + "";
                    parentcode = row["parentcode"] + "";

                    var userResponse = new
                    {
                        FormatName = formatName,
                        FormatId = formatID,
                        Parentcode = parentcode,
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
    [HttpGet("getformatmenu")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getformatmenu()
    {
        DbConn.OpenConn();
        List<object> sources = new List<object>();
        try
        {
            string formatName = string.Empty; string formatID = string.Empty;
            string parentcode = string.Empty;
            DataTable dt = new DataTable();
            string source = "select * from tblFormats where parentcode='0'";
            DbConn.FillData(dt, source);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    formatName = row["discription"] + "";
                    formatID = row["formatId"] + "";
                    var userResponse = new
                    {
                        FormatName = formatName,
                        FormatId = formatID
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
    [HttpGet("getformatbyid")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getFormatsByID(int id)
    {
        DbConn.OpenConn();
        List<object> sources = new List<object>();
        try
        {
            string formatName = string.Empty; string formatID = string.Empty;
            string parentcode = string.Empty;
            DataTable dt = new DataTable();
            string source = "select * from tblFormats where parentcode='" + id + "'";
            DbConn.FillData(dt, source);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    formatName = row["discription"] + "";
                    formatID = row["formatId"] + "";

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

                    var userResponse = new
                    {
                        FormatName = formatName,
                        FormatId = formatID
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
}
