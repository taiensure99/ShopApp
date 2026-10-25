namespace ShopApp.Models
{
    public class DatHangRequest
    {
        public int KhachHangId { get; set; }
        public int SanPhamId { get; set; }
        public int SoLuong { get; set; }
        public List<ChiTietDatHangRequest> DanhSachSanPham { get; set; } = new();
    }

    public class ChiTietDatHangRequest
    {
        public int SanPhamId { get; set; }
        public int SoLuong { get; set; }
    }
}