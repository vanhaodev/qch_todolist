using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
/*
 * Schema model khi đăng ký
 */
namespace education_project_backend.Models.Auth
{
    public class UserAccount_Register
    {
        [Required]
        public string userName { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
        [Required]
        public string displayName { get; set; } = string.Empty;
        [Required]
        public string email { get; set; } = string.Empty;
    }
}
