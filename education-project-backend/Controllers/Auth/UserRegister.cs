using education_project_backend.Models.Auth;
using education_project_backend.Models.SQL;
using education_project_backend.Modules.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Xml.Linq;
/*
 * Step 1: Client gửi thông tin đăng ký đến server
 * Step 2: Server kiểm tra tính hợp lệ, trùng lặp
 * Step 3: Nếu thông tin hợp lệ sẽ tiến hành insert vào database 
 * Step 4: Password trước khi lưu sẽ được hash/băm để bảo mật không thể dịch ngược
 */
namespace education_project_backend.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegister : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register(UserAccount_Register request)
        {
            SqlConnection con = new SqlConnection(ConnectStrings.main);
            try
            {
                if (request.userName.Length < 8 
                    || request.userName.Length > 30 
                    || Regex.IsMatch(request.userName, @"^[a-zA-Z0-9]+$") == false)
                {
                    return Ok("Tên người dùng không hợp lệ!");
                }
                else if (request.password.Length < 8 
                    || request.password.Length > 30 
                    || Regex.IsMatch(request.password, @"^[a-zA-Z0-9]+$") == false)
                {
                    return Ok("Mật khẩu không hợp lệ!");
                }
                else if (Regex.IsMatch(request.email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") == false)
                {
                    return Ok("Email không hợp lệ");
                }
                else if (request.displayName.Length < 4 
                    || request.displayName.Length > 30)
                {
                    return Ok("Tên hiển thị không hợp lệ!");
                }  
                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = $"UserAccount_Register";
                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = GenerateUID();
                cmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = request.userName;

                //Hash băm password
                PasswordHash hash = new PasswordHash(); 
                cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = hash.Hash(request.password);

                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = request.email;
                cmd.Parameters.Add("@displayName", SqlDbType.NVarChar).Value = request.displayName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                con.Open();

                DataTable dt = new DataTable();

                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
                if (dt.Rows[0].Field<int>(0) == -1)
                {
                    return Ok("Tên người dùng đã tồn tại! hãy chọn tên khác.");
                }
                else if (dt.Rows[0].Field<int>(0) == -2)
                {
                    return Ok("Email đã tồn tại! hãy chọ email khác.");
                }
                return Ok("Đăng ký tài khoản thành công!");
            }
            catch (Exception ex)
            {
                return Ok("Có lỗi xảy ra khi thực hiện đăng ký." + ex);
            }
            finally
            {
                con.Close();
            }
        }
        string GenerateUID()
        {
            //26 chữ cái hoa và 10 số
            //36^10
            Random random = new Random();
            int loop = 0;
            char[] id = new char[11];
            id[0] = 'U'; //set tiền tố U (user)

            for (int i = 1; i < id.Length; i++) //i = 1 vì bỏ qua chữ "u"
            {
                //0 = chữ; 1 = số
                int index = random.Next(0, 2) == 0 ? random.Next(65, 91) : random.Next(48, 58);
                id[i] = (char)(index);
            }

            while (UIDIsContains(new string(id)))
            {
                loop++;
                for (int i = 1; i < id.Length; i++) //i = 1 vì bỏ qua chữ "u"
                {
                    //0 = chữ; 1 = số
                    int index = random.Next(0, 2) == 0 ? random.Next(65, 91) : random.Next(48, 58);
                    id[i] = (char)(index);
                }
            }

            return new string(id);
        }
        bool UIDIsContains(string value)
        {
            SqlConnection con = new SqlConnection(ConnectStrings.main);
            SqlCommand cmd = new SqlCommand($"select uid from UserAccount where uid = '{value}'", con);

            bool contains = false;
            try
            {
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["uid"].ToString() == value)
                        {
                            contains = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("================UIDIsContains====================" + ex);
            }
            finally
            {
                con.Close();
            }

            return contains;
        }
    }
}
        
    

