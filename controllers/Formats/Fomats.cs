using System.Data;
using System.Net;
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
            return Ok(FormatServices.GetFormatByLevel(DbConn, parentcode));

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
    // add SubFormats
    [HttpPost("addSubFormats")]
    [BaseUrlRoute()]
    public async Task<ActionResult> addSubFormats([FromBody] Formats format)
    {
        if (format == null || string.IsNullOrEmpty(format.discription))
        {
            return BadRequest(" invalid menu");
        }
        DbConn.OpenConn();
        try
        {
            DataTable dt;
            DataRow drow;
            dt = DbConn.GetDataTable("tblSubFormats");
            drow = dt.NewRow();
            string nextnum = Utility.getnextnum(DbConn, format.parentcode);
            if (Convert.ToInt32(nextnum) < 1)
            {
                return NotFound(new
                {
                    Success = false,
                    StatusCode = 200,
                    Message = "the parent Code is not exist"
                });
            }
            drow["description"] = format.discription;
            drow["parentcode"] = format.parentcode;
            drow["formatId"] = format.parentcode + nextnum;
            drow["crtby"] = Dns.GetHostName();
            drow["crtws"] = Dns.GetHostName();
            drow["crtdt"] = DateTime.Now;
            string query = DbconUtility.GetQuery(1, drow);
            if (DbConn.Insert(drow, false))
            {
                int num = Convert.ToInt32(nextnum);
                Utility.Updatemnextnum(DbConn, num, format.parentcode);
            }
            return Ok(new
            {
                Success = true,
                StatusCode = 200,
                Message = SuccessMessages.FormatAddedSucces
            });
        }
        catch (Exception ex)
        {
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Success = false,
                Message = ErrorMessages.UnexpectedError,
            });
        }
        finally
        {
            DbConn.CloseConn();
        }
    }


    //  add

    [HttpPost("addSubMainFormats")]
    [BaseUrlRoute()]
    public async Task<ActionResult> addSubMainFormats([FromBody] Formats format)
    {
        if (format == null || string.IsNullOrEmpty(format.discription))
        {
            return BadRequest(" invalid menu");
        }
        DbConn.OpenConn();
        try
        {
            DataTable dt;
            DataRow drow;
            dt = DbConn.GetDataTable("tblSubMainFormats");
            drow = dt.NewRow();
            string nextnum = Utility.getnextnum(DbConn, format.parentcode);
            if (Convert.ToInt32(nextnum) < 1)
            {
                return NotFound(new
                {
                    Success = false,
                    StatusCode = 200,
                    Message = "the parent Code is not exist"
                });
            }
            drow["description"] = format.discription;
            drow["parentcode"] = format.parentcode;
            drow["formatId"] = format.parentcode + nextnum;
            drow["crtby"] = Dns.GetHostName();
            drow["crtws"] = Dns.GetHostName();
            drow["crtdt"] = DateTime.Now;
            string query = DbconUtility.GetQuery(1, drow);
            if (DbConn.Insert(drow, false))
            {
                int num = Convert.ToInt32(nextnum);
                Utility.Updatemnextnum(DbConn, num, format.parentcode);
            }
            return Ok(new
            {
                Success = true,
                StatusCode = 200,
                Message = SuccessMessages.FormatAddedSucces
            });
        }
        catch (Exception ex)
        {
            var url = HttpContext.Request.Host + HttpContext.Request.Path;
            Utility.setLog(url, ex.Message, Dns.GetHostName());
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Success = false,
                Message = ErrorMessages.UnexpectedError,
            });
        }
        finally
        {
            DbConn.CloseConn();
        }
    }
    //   update subMainformats
    [HttpPut("updateSubMainFormats/{formatId}")]
    [BaseUrlRoute()]
    public async Task<ActionResult> updateSubMainFormats(string formatId, string parentcode, [FromBody] UpdateformatModel model)
    {
        try
        {
            //  check existence of the id
            DataTable dt = new DataTable();
            string checkformatid = "select * from tblSubMainFormats where formatId='" + formatId + "' ";
            DbConn.FillData(dt, checkformatid);
            if (dt.Rows.Count < 1)
                return NotFound(new ApiResponse<object>
                {
                    Message = "the data with give Id Is not exist",
                    Success = true
                });
            string query = "update tblSubMainFormats set parentcode='" + model.parentcode + "' , description='" + model.description + "' where formatId='" + formatId + "'";
            DbConn.Execute(query);

            return Ok(new ApiResponse<object>
            {
                Message = "the data is updated succesfully",
                Success = true
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Ok(new ApiResponse<object>
            {
                Message = ErrorMessages.UnexpectedError,
                Success = false
            });

        }
    }

    //   update subformats
    [HttpPut("updateSubFormats/{formatId}")]
    [BaseUrlRoute()]
    public async Task<ActionResult> updateSubFormats(string formatId, string parentcode, [FromBody] UpdateformatModel model)
    {
        try
        {
            DataTable dt = new DataTable();
            string checkformatid = "select * from tblSubFormats where formatId='" + formatId + "' ";
            DbConn.FillData(dt, checkformatid);
            if (dt.Rows.Count < 1)
                return NotFound(new ApiResponse<object>
                {
                    Message = "the data with give Id Is not exist",
                    Success = true
                });
            string query = "update tblSubFormats set parentcode='" + model.parentcode + "' , description='" + model.description + "' where formatId='" + formatId + "'";
            DbConn.Execute(query);

            return Ok(new ApiResponse<object>
            {
                Message = "the data is updated succesfully",
                Success = true
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Ok(new ApiResponse<object>
            {
                Message = ErrorMessages.UnexpectedError,
                Success = false
            });

        }
    }

    //   delete subMainformats
    [HttpDelete("deleteSubMainFormats/{formatId}")]
    [BaseUrlRoute()]
    public async Task<ActionResult> deleteSubMainFormats(string formatId)
    {
        try
        {
            DataTable dt = new DataTable();
            string checkformatid = "select * from tblSubMainFormats where formatId='" + formatId + "' ";
            DbConn.FillData(dt, checkformatid);
            if (dt.Rows.Count < 1)
                return NotFound(new ApiResponse<object>
                {
                    Message = "the data with give Id Is not exist",
                    Success = true
                });
            string query = "delete tblSubMainFormats where formatId='" + formatId + "' ";
            DbConn.Execute(query);

            return Ok(new ApiResponse<object>
            {
                Message = "the data is updated succesfully",
                Success = true
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Ok(new ApiResponse<object>
            {
                Message = ErrorMessages.UnexpectedError,
                Success = false
            });

        }
    }

    //   delete subformats
    [HttpDelete("deleteSubFormats/{formatId}")]
    [BaseUrlRoute()]
    public async Task<ActionResult> deleteSubFormats(string formatId)
    {
        try
        {
            DataTable dt = new DataTable();
            string checkformatid = "select * from tblSubFormats where formatId='" + formatId + "' ";
            DbConn.FillData(dt, checkformatid);
            if (dt.Rows.Count < 1)
                return NotFound(new ApiResponse<object>
                {
                    Message = "the data with give Id Is not exist",
                    Success = true
                });
            string query = "delete tblSubFormats where formatId='" + formatId + "' ";
            DbConn.Execute(query);

            return Ok(new ApiResponse<object>
            {
                Message = "the data is updated succesfully",
                Success = true
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Ok(new ApiResponse<object>
            {
                Message = ErrorMessages.UnexpectedError,
                Success = false
            });

        }
    }
}
