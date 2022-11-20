using education_project_backend.Models.SQL;
using education_project_backend.Modules.Auth;
using education_project_backend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using education_project_backend.Models.Auth;

namespace education_project_backend.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService userService;
        public ProfileController(IConfiguration configuration, IUserService userService)
        {
            this.userService = userService;
            System.Diagnostics.Debug.WriteLine("+ Khởi tạo WorkspaceController");
        }
        /// <summary>
        /// GET user bằng uid được lưu trong token
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authorize]
        [Route("GetMyName")]
        public ActionResult<string> GetMe()
        {

            var uid = userService.GetMyName();

            UserAccount_Details userAccount = new UserAccount_Details();
            SqlConnection con = new SqlConnection(ConnectStrings.main);
            try
            {
                string passwordHashFromDatabase = string.Empty;
                SqlCommand cmd = new SqlCommand();
                //Hash băm password
                PasswordHash hash = new PasswordHash();
                DataTable dt = new DataTable();

                cmd.CommandText = $"SELECT * from UserAccount where uid = '" + uid + "'";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
                //fullName = dt.Rows[0].Field<string>(0);
                userAccount.uid = dt.Rows[0].Field<string>(0);
                userAccount.userName = dt.Rows[0].Field<string>(1);
                userAccount.displayName = dt.Rows[0].Field<string>(3);
                userAccount.email = dt.Rows[0].Field<string>(4);
                userAccount.phoneNumber = dt.Rows[0].Field<string>(5);
                userAccount.avatarUrl = dt.Rows[0].Field<string>(6);
                userAccount.quote = dt.Rows[0].Field<string>(7);
                userAccount.birthday = dt.Rows[0].Field<DateTime>(8);
                userAccount.isActivate = dt.Rows[0].Field<bool>(9);
                userAccount.banTo = dt.Rows[0].Field<DateTime>(10);
                userAccount.role = dt.Rows[0].Field<byte>(11);
                userAccount.createdAt = dt.Rows[0].Field<DateTime>(12);
            }
            catch (Exception ex)
            {
                return Ok("Có lỗi xảy ra khi thực hiện đăng nhập." + ex);
            }
            finally
            {
                con.Close();
            }
            return Ok(userAccount);
        }
    }
    
}
