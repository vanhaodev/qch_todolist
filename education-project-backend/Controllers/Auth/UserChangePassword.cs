using education_project_backend.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace education_project_backend.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserChangePassword : ControllerBase
    {
        SqlConnection con;
        public UserChangePassword(IConfiguration configuration)
        {
            con = new SqlConnection(configuration["ConnectionStrings:sqlConnectString"]);
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult<string>> ChangePassword(UserAccount_Login request)//có thể dùng model của UserAccount_Login vì tương đồng
        {
            return Ok("Hello world");
        }
    }
}
