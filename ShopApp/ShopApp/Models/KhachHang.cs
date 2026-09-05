using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models
{
    public class KhachHang
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        public string? SDT { get; set; }

        public string? DiaChi { get; set; }
    }
}
