using System.ComponentModel.DataAnnotations;
/*
 * Schema model khi đăng nhập
 */
namespace education_project_backend.Models.Auth
{
    public class UserAccount_Login
    {
        [Required]
        public string userName { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }
}
