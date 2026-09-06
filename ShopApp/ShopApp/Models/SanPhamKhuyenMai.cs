namespace ShopApp.Models
{
    public class SanPhamKhuyenMai
    {
        public int Id { get; set; }

        public int SanPhamId { get; set; }

        public int PhanTramGiam { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }
    }
}
