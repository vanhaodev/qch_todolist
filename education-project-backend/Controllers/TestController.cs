using education_project_backend.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using education_project_backend.Services.UserService;
using Microsoft.AspNetCore.Cors;

namespace education_project_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TestController : ControllerBase
    {     
        string connectString;
        private readonly IUserService userService;
        public TestController(IConfiguration configuration, IUserService userService)
        {
            connectString = configuration["ConnectionStrings:sqlConnectString"];
            this.userService = userService;

            System.Diagnostics.Debug.WriteLine("================KHỞI TẠO    ====================");
        }

        [HttpGet, Authorize]
        [Route("GetMe")]
        public ActionResult<string> GetMe()
        {
            
            var userName = userService.GetMyName();
            return Ok(userName);
        }
        [HttpPost]
        [Authorize]
        [Route("GenerateUID")]
        public async Task<ActionResult<string>> GenerateUID()
        {
            DateTime timeCount = DateTime.Now;
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
            AddUID(new string(id), loop.ToString());

            return new string(id) + $" lặp {loop}";         
        }
        bool UIDIsContains(string value)
        {
            SqlConnection con = new SqlConnection(connectString);
            SqlCommand cmd = new SqlCommand($"select uid from Test_UserID where uid = '{value}'", con);

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
        void AddUID(string value, string loopCount)
        {
            SqlConnection con = new SqlConnection(connectString);
            try
            {
                using (var command = new SqlCommand($"insert into Test_UserID (uid, loopCount) values ('{value}', {loopCount})", con))
                {
                    command.CommandType = CommandType.Text;
                    con.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("================AddUID====================" + ex);
            }
            finally
            {
                con.Close();
            }
        }

    }
}
