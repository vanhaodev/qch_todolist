/*
 * Schema thông tin
 * Trả về thông tin cơ bản của User
 */
namespace education_project_backend.Models.Auth
{
    public class UserAccount_Details
    {
        public string uid { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string avatarUrl { get; set; } = string.Empty;
        public string quote { get; set; } = string.Empty;
        public DateTime birthday { get; set; }
        public bool isActivate { get; set; }
        public DateTime banTo { get; set; }
        public byte role  { get; set; }
        public DateTime createdAt { get; set; }
        /// <summary>
        /// Khi get thành viên workspace mới dùng đến data này
        /// </summary>
        public byte workspaceRole { get; set; }
        public UserAccount_Details()
        {

        }
        public UserAccount_Details(string uid, string userName, string displayName, string email, string phoneNumber, string avatarUrl, string quote, DateTime birthday, bool isActivate, DateTime banTo, byte role, DateTime createdAt)
        {
            this.uid = uid;
            this.userName = userName;
            this.displayName = displayName;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.avatarUrl = avatarUrl;
            this.quote = quote;
            this.birthday = birthday;
            this.isActivate = isActivate;
            this.banTo = banTo;
            this.role = role;
            this.createdAt = createdAt;
        }
        /// <summary>
        /// Dành cho get member trong workspace
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="userName"></param>
        /// <param name="displayName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="quote"></param>
        /// <param name="birthday"></param>
        /// <param name="isActivate"></param>
        /// <param name="banTo"></param>
        /// <param name="role"></param>
        /// <param name="workspaceRole"></param>
        /// <param name="createdAt"></param>
        public UserAccount_Details(string uid, string userName, string displayName, string email, string phoneNumber, string avatarUrl, string quote, DateTime birthday, bool isActivate, DateTime banTo, byte role, DateTime createdAt, byte workspaceRole)
        {
            this.uid = uid;
            this.userName = userName;
            this.displayName = displayName;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.avatarUrl = avatarUrl;
            this.quote = quote;
            this.birthday = birthday;
            this.isActivate = isActivate;
            this.banTo = banTo;
            this.role = role;
            this.createdAt = createdAt;
            this.workspaceRole = workspaceRole;
        }
    }
}
