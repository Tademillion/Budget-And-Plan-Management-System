using System.Data;
using BudgetP;

public static class FormatServices
{
    public static ApiResponse<object> GetFormatByLevel(DbconUtility DbConn, string parentcode)
    {
        string formatName = string.Empty; string formatID = string.Empty;
        DataTable dt = new DataTable();
        string source = @"DECLARE @jsonResult NVARCHAR(MAX);
SELECT @jsonResult = (
    SELECT  
        m.formatId AS MainFormatId,  
        m.Description AS Description,  
        ISNULL((
            SELECT  
                s.formatID as id,  
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
            var jsonObj = System.Text.Json.JsonSerializer.Deserialize<object>(json);
            result.Add(jsonObj);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Data retrieved successfully.",
                Data = result
            };
        }
        return new ApiResponse<object>
        {
            Success = true,
            Message = "The Data for this Code is not exist.",
        };
    }

}

