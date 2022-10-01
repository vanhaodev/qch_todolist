using education_project_backend.Models.Auth;
using education_project_backend.Modules.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
/*
 * Step 1: Client đăng nhập và gửi username + password đến server
 * Step 2: Server gọi mật khẩu (đã được hash/băm) từ database về
 * Step 3: Server thực hiện so sánh xác thực giữa password từ client và password hash từ database (Hàm verify())
 * Step 4: Nếu xác thực thành công sẽ gửi token về client 
 */
namespace education_project_backend.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogin : ControllerBase
    {
        SqlConnection con;

        public UserLogin(IConfiguration configuration)
        {
            con = new SqlConnection(configuration["ConnectionStrings:sqlConnectString"]);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(UserAccount_Login request)
        {
            try
            {
                string passwordHashFromDatabase = string.Empty;
                SqlCommand cmd = new SqlCommand();
                //Hash băm password
                PasswordHash hash = new PasswordHash();
                DataTable dt = new DataTable();

                cmd.CommandText = $"UserAccount_GetPassword";
                cmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = request.userName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
                if (dt.Rows[0].Field<int>(0) == -1)
                {
                    return Ok("Tên người dùng không tồn tại!");
                }
                else
                {
                    passwordHashFromDatabase = dt.Rows[0].Field<string>(1);                   
                }    
             
                if(passwordHashFromDatabase.Length < 5) //password hashed không bao giờ nhỏ hơn 5 ký tự
                {
                    return Ok("Lỗi xác thực mật khẩu!");
                }    
                else if(hash.Verify(request.password, passwordHashFromDatabase))
                {
                    return Ok("Đăng nhập thành công!");
                }   
                else
                {
                    return Ok("Mật khẩu không đúng!");
                }    
            }
            catch (Exception ex)
            {
                return Ok("Có lỗi xảy ra khi thực hiện đăng nhập." + ex);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
