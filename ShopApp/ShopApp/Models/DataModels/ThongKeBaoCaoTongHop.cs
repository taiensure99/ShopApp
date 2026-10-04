namespace ShopApp.Models.DataModels
{
    public class ThongKeBaoCaoTongHop
    {
        public decimal TongSanPham { get; set; }
        public List<ThongKeThangNam> DoanhThuThang { get; set; }
        public List<TongChiTieuKhachHang> TopKhachHang { get; set; }
        public List<TopSanPham> TopSanPham { get; set; }
        public List<ThongKeTrangThai> TrangThai { get; set; }
    }
    public class TongChiTieuKhachHang
    {
        public int KhachHangId { get; set; }
        public decimal TongChiTieu { get; set; }
    }
    public class TopSanPham
    {
        public int SanPhamId { get; set; }
        public int SoLuongBan { get; set; }
    }
    public class ThongKeTrangThai
    {
        public string TrangThai { get; set; }
        public int SoLuong { get; set; }
    }
}
