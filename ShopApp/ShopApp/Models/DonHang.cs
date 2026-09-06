using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models
{
    namespace MyApp.Models
    {
        public class DonHang
        {
            public int Id { get; set; }
            public string MaDon { get; set; } = string.Empty;
            public double TongTien { get; set; }
            public string TrangThai { get; set; } = string.Empty;

            public DateTime NgayDat { get; set; }
        }
    }
}
