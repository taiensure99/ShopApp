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

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(100, MinimumLength = 2)]
        public string SDT { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string? DiaChi { get; set; }
    }

}
