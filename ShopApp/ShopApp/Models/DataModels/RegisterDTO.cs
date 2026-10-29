using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models.DataModels
{
    public class RegisterDTO
    {
        [Required, StringLength(100)] public string HoTen { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required, MinLength(6)] public string MatKhau { get; set; } = "";
    }

    public class LoginDTO
    {
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required] public string MatKhau { get; set; } = "";
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; } = "";
        public DateTime HetHan { get; set; }
        public string Email { get; set; } = "";
        public string VaiTro { get; set; } = "";
    }


}
