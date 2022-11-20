using education_project_backend.Models.SQL;
using education_project_backend.Modules.Auth;
using education_project_backend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using education_project_backend.Models.Todo;
using education_project_backend.Modules;

namespace education_project_backend.Controllers.Todo
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IUserService userService;
        public TodoController(IConfiguration configuration, IUserService userService)
        {
            this.userService = userService;

            System.Diagnostics.Debug.WriteLine("+ Khởi tạo TodoController");
        }
       
        
        [HttpPost, Authorize]
        [Route("CreateTodo")]
        public ActionResult<string> CreateTodo(string workspaceId, int iconId, string title, string description)
        {
            string uid = userService.GetMyName();
            SqlConnection con = new SqlConnection(ConnectStrings.main);
         
            try
            {
                SqlCommand cmd = new SqlCommand();

                DataTable dt = new DataTable();
                cmd.CommandText = $"CreateTodoItem";
                cmd.Parameters.Add("@workspaceId", SqlDbType.VarChar).Value = workspaceId;
                cmd.Parameters.Add("@iconId", SqlDbType.TinyInt).Value = iconId;
                cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = title;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
                //cmd.Parameters.Add("@title", SqlDbType.DateTime).Value = dt;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }

            }
            catch (Exception ex)
            {
                return Ok("Có lỗi xảy ra khi thực hiện." + ex);
            }
            finally
            {
                con.Close();
            }
            return Ok("Thêm việc thành công");
        }
    }
}
