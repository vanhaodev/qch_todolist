using education_project_backend.Models.Auth;
using education_project_backend.Models.SQL;
using education_project_backend.Modules.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        IConfiguration configuration;
        public UserLogin(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(UserAccount_Login request)
        {
            SqlConnection con = new SqlConnection(ConnectStrings.main);
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
                    //lấy mật khẩu bị mã hóa trên database về
                    passwordHashFromDatabase = dt.Rows[0].Field<string>(1);                   
                }    
             
                if(passwordHashFromDatabase.Length < 5) //password hashed không bao giờ nhỏ hơn 5 ký tự
                {
                    return Ok("Lỗi xác thực mật khẩu!");
                }    
                else if(hash.Verify(request.password, passwordHashFromDatabase)) //giải mã để so sánh với mật khẩu đăng nhập
                {
                    string message = "Đăng nhập thành công!";
                    string token = CreateToken(GetUID(request.userName));
                    return Ok(new { message , token});
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
        /// <summary>
        /// Lấy UID trên database để lưu vào token
        /// </summary>
        /// <returns></returns>
        string GetUID(string userName)
        {
            SqlConnection con = new SqlConnection(ConnectStrings.main);
            SqlCommand cmd = new SqlCommand();

            string uid = "null";
            
            try
            {
                cmd.CommandText = $"UserAccount_GetUID";
                cmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = userName;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = con;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        uid = reader[0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("================string GetUID(string userName)====================" + ex);
            }
            finally
            {
                con.Close();
            }

            return uid;
        }
        private string CreateToken(string uid)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("uid", uid),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
