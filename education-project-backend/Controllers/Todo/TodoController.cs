using education_project_backend.Models.SQL;
using education_project_backend.Modules.Auth;
using education_project_backend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace education_project_backend.Controllers.Todo
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        string connectString;
        private readonly IUserService userService;
        public TodoController(IConfiguration configuration, IUserService userService)
        {
            connectString = configuration["ConnectionStrings:sqlConnectString"];
            this.userService = userService;

            System.Diagnostics.Debug.WriteLine("================KHỞI TẠO    ====================");
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
            string fullName = "";

            SqlConnection con = new SqlConnection(ConnectStrings.main);
            try
            {
                string passwordHashFromDatabase = string.Empty;
                SqlCommand cmd = new SqlCommand();
                //Hash băm password
                PasswordHash hash = new PasswordHash();
                DataTable dt = new DataTable();

                cmd.CommandText = $"SELECT displayname from UserAccount where uid = '" + uid +"'";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
                fullName = dt.Rows[0].Field<string>(0);
            }
            catch (Exception ex)
            {
                return Ok("Có lỗi xảy ra khi thực hiện đăng nhập." + ex);
            }
            finally
            {
                con.Close();
            }
            return Ok(fullName);
        }
    }
}
