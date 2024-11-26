using BCrypt.Net;
namespace real_estate_api.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Hash mật khẩu và tự động tạo salt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Kiểm tra mật khẩu đã nhập với hash lưu trữ
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}
