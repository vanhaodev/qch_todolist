using education_project_backend.Models.SQL;
using education_project_backend.Modules.Auth;
using education_project_backend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using education_project_backend.Models.Todo;

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
        [HttpPost, Authorize]
        [Route("CreateWorkspace")]
        public ActionResult<string> CreateWorkspace(string workspaceId, string displayname, string description)
        {
            string uid = userService.GetMyName();
            SqlConnection con = new SqlConnection(ConnectStrings.main);

            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();

                cmd.CommandText = $"CreateWorkspace";
                cmd.Parameters.Add("@workspaceId", SqlDbType.VarChar).Value = workspaceId;
                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = uid;
                cmd.Parameters.Add("@displayname", SqlDbType.NVarChar).Value = displayname;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
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
            return Ok("Tạo thành công");
        }
        [HttpPost, Authorize]
        [Route("AddMember")]
        public ActionResult<string> AddMember(string memberUid, string workspaceId, int role)
        {
            string uid = userService.GetMyName();
            SqlConnection con = new SqlConnection(ConnectStrings.main);

            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();

                cmd.CommandText = $"CreateWorkspace";
                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = memberUid;
                cmd.Parameters.Add("@workspaceId", SqlDbType.VarChar).Value = workspaceId;          
                cmd.Parameters.Add("@description", SqlDbType.TinyInt).Value = role;
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
            return Ok("Thêm tv thành công");
        }
        [HttpGet, Authorize]
        [Route("GetMyWorkspace")]
        public ActionResult<string> GetMyWorkspace()
        {
            string uid = userService.GetMyName();
            
            SqlConnection con = new SqlConnection(ConnectStrings.main);
            //get workspace id
            List<string> wsID = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();

                cmd.CommandText = $"SELECT workspaceid from WorkspaceUser where uid = '" + uid + "'";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    wsID.Add(dt.Rows[i].Field<string>(0));
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
            //get workspace infor
            List<Workspace> ws = new List<Workspace>();
            for (int i = 0; i < wsID.Count; i++)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();

                    DataTable dt = new DataTable();

                    cmd.CommandText = $"SELECT uid, displayname, description from Workspace where workspaceid = '" + wsID[i] + "'";

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(dt);
                    }
                    ws.Add(new Workspace(dt.Rows[0].Field<string>(0),
                            dt.Rows[0].Field<string>(1),
                            dt.Rows[0].Field<string>(2)));
                }
                catch (Exception ex)
                {
                    return Ok("Có lỗi xảy ra khi thực hiện." + ex);
                }
                finally
                {
                    con.Close();
                }
            }
            return Ok(ws);
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

        string GenerateID()
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
