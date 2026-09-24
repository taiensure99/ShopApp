using ShopApp.Interfaces;
using ShopApp.Models;

namespace ShopApp.Services
{
    public class SanPhamService : ISanPhamService
    {

        private List<SanPham> _danhSach = new List<SanPham>
        {
            new SanPham { Id = 1, Ten = "Áo thun nam basic", GiaBan = 250000, TonKho = 15, DanhMuc = "Áo" },
            new SanPham { Id = 2, Ten = "Quần jean slim fit", GiaBan = 450000, TonKho = 10, DanhMuc = "ao" },
            new SanPham { Id = 3, Ten = "Giày sneaker trắng", GiaBan = 800000, TonKho = 5,  DanhMuc = "Giày" },
            new SanPham { Id = 4, Ten = "Mũ lưỡi trai", GiaBan = 150000, TonKho = 20, DanhMuc = "kiện" }
        };

        public async Task<bool> KiemTraTonTai(int id)
        {
            bool tonTai =  _danhSach.Any(sp => sp.Id == id);
            return tonTai;
        }
        public SanPham GetById(int id)
        {

            return _danhSach.FirstOrDefault(sp => sp.Id == id);
        }

        public async Task<decimal> TinhTongTien(int sanPhamId, int soLuong)
        {
            var sanPham = GetById(sanPhamId);
            if (sanPham == null)
            {
                throw new ArgumentException($"Sản phẩm với Id {sanPhamId} không tồn tại.");
            }
            return decimal.Multiply((decimal)sanPham.GiaBan, soLuong);
        }

    }
}
