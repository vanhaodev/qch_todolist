using education_project_backend.Models.SQL;
using education_project_backend.Models.Todo;
using education_project_backend.Modules;
using education_project_backend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using education_project_backend.Models.Auth;

namespace education_project_backend.Controllers.Todo
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IUserService userService;
        public WorkspaceController(IConfiguration configuration, IUserService userService)
        {
            this.userService = userService;
            System.Diagnostics.Debug.WriteLine("+ Khởi tạo WorkspaceController");
        }
        [HttpPost, Authorize]
        [Route("CreateWorkspace")]
        public ActionResult<string> CreateWorkspace(string displayname, string description)
        {
            string uid = userService.GetMyName();
            SqlConnection con = new SqlConnection(ConnectStrings.main);

            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();

                cmd.CommandText = $"CreateWorkspace";
                cmd.Parameters.Add("@workspaceId", SqlDbType.VarChar).Value = GenerateUniqueID.GenerateID(IDType.WorkspaceID);
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
            string mgs = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();

                cmd.CommandText = $"AddMember";
                cmd.Parameters.Add("@inviteruid", SqlDbType.VarChar).Value = uid;
                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = memberUid;
                cmd.Parameters.Add("@workspaceId", SqlDbType.VarChar).Value = workspaceId;
                cmd.Parameters.Add("@role", SqlDbType.TinyInt).Value = role;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
                mgs = dt.Rows[0].Field<string>(0);
            }
            catch (Exception ex)
            {
                mgs = "Lỗi Exception " + ex + "";
            }
            finally
            {
                con.Close();
            }
            return Ok(mgs);
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
                for (int i = 0; i < dt.Rows.Count; i++)
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
        [HttpGet, Authorize]
        [Route("GetMyWorkspaceMember")]
        public ActionResult<string> GetMyWorkspaceMember(string workspaceID)
        {
            string uid = userService.GetMyName();

            SqlConnection con = new SqlConnection(ConnectStrings.main);
            List<UserAccount_Details> member = new List<UserAccount_Details>();
            try
            {
                SqlCommand cmd = new SqlCommand();

                DataTable dt = new DataTable();

                //cmd.CommandText = $"Select u.*, w.role" +
                //    $" from WorkspaceUser w" +
                //    $" LEFT JOIN UserAccount u on u.uid = w.uid" +
                //    $" where workspaceID = '{workspaceID}'";
                cmd.CommandText = $"Select u.*, w.role from WorkspaceUser w" +
                    $" LEFT JOIN UserAccount u on u.uid = w.uid" +
                    $" LEFT JOIN Workspace ws on ws.WorkspaceID = w.workspaceID" +
                    $" where ws.workspaceID = '{workspaceID}' AND ws.uid = '{uid}'";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    member.Add(new UserAccount_Details(
                        dt.Rows[i].Field<string>(0),
                        dt.Rows[i].Field<string>(1),
                        dt.Rows[i].Field<string>(3),
                        dt.Rows[i].Field<string>(4),
                        dt.Rows[i].Field<string>(5),
                        dt.Rows[i].Field<string>(6),
                        dt.Rows[i].Field<string>(7),
                        dt.Rows[i].Field<DateTime>(8),
                        dt.Rows[i].Field<bool>(9),
                        dt.Rows[i].Field<DateTime>(10),
                        dt.Rows[i].Field<byte>(11),
                        dt.Rows[i].Field<DateTime>(12),
                        dt.Rows[i].Field<byte>(13)
                        ));
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
            
            return Ok(member);
        }
        [HttpPost, Authorize]
        [Route("DeleteMyWorkspaceMember")]
        public ActionResult<string> DeleteMyWorkspaceMember(string workspaceID, string memberID)
        {
            string uid = userService.GetMyName();
            string mgs = "";
            SqlConnection con = new SqlConnection(ConnectStrings.main);
            List<UserAccount_Details> member = new List<UserAccount_Details>();
            try
            {
                SqlCommand cmd = new SqlCommand();

                DataTable dt = new DataTable();
                cmd.CommandText = $"DeleteMember";
                cmd.Parameters.Add("@deleterUID", SqlDbType.VarChar).Value = uid;
                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = memberID;
                cmd.Parameters.Add("@workspaceID", SqlDbType.VarChar).Value = workspaceID;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(dt);
                }
                mgs = dt.Rows[0].Field<string>(0);
                
            }
            catch (Exception ex)
            {
                mgs = "Có lỗi xảy ra khi thực hiện." + ex+"";
            }
            finally
            {
                con.Close();
            }

            return Ok(mgs);
        }
    }
}
