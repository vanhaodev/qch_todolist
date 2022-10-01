/*
 * Lưu ý: Không thể so sánh bằng cách hash, phải dùng BCrypt.Verify() mật khẩu giống nhau nhưng khi hash sẽ là ngẫu nhiên kí tự.
 */
namespace education_project_backend.Modules.Auth
{
    public class PasswordHash
    {
        /// <summary>
        /// Truyền vào mật khẩu sẽ được băm thành mảng byte với độ phức tạp là 12
        /// </summary>
        /// <param name="password">mật khẩu dạng text có nghĩa truyền vào</param>
        /// <returns></returns>
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        /// <summary>
        /// Dùng mật khẩu người dùng gửi đến khi đăng nhập và so sánh với mật khẩu được hash ở database
        /// </summary>
        /// <param name="loginPass">Mật khẩu người dùng gửi đến khi đăng nhập</param>
        /// <param name="databasePass">Mật khẩu dạng hash truy vấn từ database</param>
        /// <returns></returns>
        public bool Verify(string loginPass, string databasePass)
        {
            return BCrypt.Net.BCrypt.Verify(loginPass, databasePass);
        }
    }
}
