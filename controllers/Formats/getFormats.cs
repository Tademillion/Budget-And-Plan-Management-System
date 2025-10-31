using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    // getFormats
    [HttpGet("geMainFormats")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getMainFormats()
    {
        DbConn.OpenConn();
        List<object> sources = new List<object>();
        try
        {
            string formatName = string.Empty; string formatID = string.Empty;
            string parentcode = string.Empty;
            DataTable dt = new DataTable();
            string source = "select * from tblMainFormats  ";
            DbConn.FillData(dt, source);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    formatName = row["description"] + "";
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


    [HttpGet("getSubMainFormats")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getSubMainFormats(string parentcode)
    {
        DbConn.OpenConn();
        List<object> sources = new List<object>();
        try
        {
            string formatName = string.Empty; string formatID = string.Empty;
            DataTable dt = new DataTable();
            string source = "select * from tblSubMainFormats  where subMainCode='" + parentcode + "'";
            DbConn.FillData(dt, source);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    formatName = row["description"] + "";
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

    //   get SubFormat with JSon
    [HttpGet("getSubFormatsByLevel")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getSubFormatsLevel(string parentcode)
    {
        DbConn.OpenConn();
        List<object> sources = new List<object>();
        try
        {
            string formatName = string.Empty; string formatID = string.Empty;
            DataTable dt = new DataTable();
            string source = @"DECLARE @jsonResult NVARCHAR(MAX);
SELECT @jsonResult = (
    SELECT  
        m.Id AS MainFormatId,  
        m.Description AS Description,  
        ISNULL((
            SELECT  
                s.Id,  
                s.Description
            FROM tblSubFormats s  
            WHERE s.ParentCode = m.formatId
            FOR JSON PATH
        ), '[]') AS SubFormats  
    FROM tblSubMainFormats m   
    WHERE m.parentcode = '" + parentcode + "'  FOR JSON PATH);SELECT @jsonResult AS JsonResult;";
            DbConn.FillData(dt, source);
            object resultValue = dt.Rows[0][0];

            if (resultValue != null && resultValue != DBNull.Value && !string.IsNullOrEmpty(resultValue.ToString()))
            {
                List<object> result = new List<object>();
                string json = dt.Rows[0][0].ToString();
                Console.WriteLine("the data is ", json);
                var jsonObj = System.Text.Json.JsonSerializer.Deserialize<object>(json);
                Console.WriteLine("the resfreuif", jsonObj);
                result.Add(jsonObj);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Data retrieved successfully.",
                    Data = result
                });
            }
            return NotFound(new ApiResponse<object>
            {
                Success = true,
                Message = "Data retrieved successfully.",

            });

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
    // 
    [HttpGet("getSubFormats")]
    [BaseUrlRoute()]
    public async Task<ActionResult> getSubFormats(string parentcode)
    {
        DbConn.OpenConn();
        List<object> sources = new List<object>();
        try
        {
            string formatName = string.Empty; string formatID = string.Empty;
            DataTable dt = new DataTable();
            string source = "select * from tblSubFormats  where parentcode='" + parentcode + "'";
            DbConn.FillData(dt, source);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    formatName = row["description"] + "";
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



}
