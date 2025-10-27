using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BudgetP
{
    public class UserLogin : ControllerBase
    {
        private TokenServices _tokenService;
        DbconUtility DbConn = new DbconUtility(DbconUtility.GetConn("Budgetplanconnstring"));
        public UserLogin(TokenServices tokenService)
        {
            this._tokenService = tokenService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> AuthenticateUser(userslogin users)
        {
            DbConn.OpenConn();
            DataTable dt = new DataTable();
            var ApiResponse = new List<object>();
            try
            {
                // string isExist = "select * from iAdmindb..tblUsers where UserName='" + users.Username + "'and Password ='" + TMPLTCrypto.Encrypt(users.Password, users.Username) + "'";
                // DbConn.FillData(dt, isExist);
                // if (dt.Rows.Count > 0)
                // {
                if (users.Username == "admin")
                {
                    var token = _tokenService.GenerateToken(users.Username, users.Password);
                    var responses = new
                    {
                        token = token,
                        UserName = users.Username,
                    };
                    ApiResponse.Add(responses);
                    return Ok(Utility.ResponseMessage(ApiResponse, false));
                }
                else
                {
                    return Ok("username or password is not found");
                }
            }
            catch (Exception ex)
            {
                return Ok(Utility.ResponseMessage(ex.Message, false));
            }
            finally
            {
                DbConn.CloseConn();
            }
        }
    }
}