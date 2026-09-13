using ShopApp.Models;
using ShopApp.Interfaces;

namespace ShopApp.Services
{
    public class ThongKeService : IThongKeService
    {
        private static List<SanPham> _danhSach = new List<SanPham>
        {
            new SanPham { Id = 1, Ten = "Áo thun nam basic", GiaBan = 250000, TonKho = 15, DanhMuc = "Áo" },
            new SanPham { Id = 2, Ten = "Quần jean slim fit", GiaBan = 450000, TonKho = 10, DanhMuc = "ao" },
            new SanPham { Id = 3, Ten = "Giày sneaker trắng", GiaBan = 800000, TonKho = 5,  DanhMuc = "Giày" },
            new SanPham { Id = 4, Ten = "Mũ lưỡi trai", GiaBan = 150000, TonKho = 20, DanhMuc = "Phụ kiện" }
        };
        public int DenTongSanPham()
        {
            // Implementation for counting total products
            return _danhSach.Count;
        }

        public int DenSanPhamHetHang()
        {
            // Implementation for counting out-of-stock products
            return _danhSach.Count(sp => sp.TonKho == 0);
        }
    }
}
