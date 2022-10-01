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
        public DateOnly birthday { get; set; }
        public bool isActivate { get; set; }
        public DateTime banTo { get; set; }
        public byte role  { get; set; }
        public DateTime createdAt { get; set; }

    }
}
