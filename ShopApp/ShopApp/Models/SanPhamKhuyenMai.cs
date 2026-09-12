using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models
{
    public class SanPhamKhuyenMai
    {
        public int Id { get; set; }

        public int SanPhamId { get; set; }

        [Range(1, 100)]
        public int PhanTramGiam { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }
    }
}
