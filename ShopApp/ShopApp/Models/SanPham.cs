using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models
{
    public class SanPham
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, MinimumLength = 3)]
        public string Ten { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public double GiaBan { get; set; }

        [Range(0, int.MaxValue)]
        public int TonKho { get; set; }

        public string DanhMuc { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
