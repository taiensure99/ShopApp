using ShopApp.Models;
using ShopApp.Services;
using ShopApp.Interfaces;
namespace ShopApp.Services
{
    public class DatHangService : IDatHangService
    {
        private readonly IKhachHangService _khachHangService;
        private readonly ISanPhamService _sanPhamService;
        private readonly IDonHangService _donHangService;

        public DatHangService(
            IKhachHangService khachHangService,
            ISanPhamService sanPhamService,
            IDonHangService donHangService)
        {
            _khachHangService = khachHangService;
            _sanPhamService = sanPhamService;
            _donHangService = donHangService;
        }

        public async Task DatHang(int sanPhamId, int khachHangId, int soLuong)
        {
            if (soLuong <= 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0.");
            }
            var TongTien = await _sanPhamService.TinhTongTien(sanPhamId, soLuong);
            var DonHangMoi = new DonHang
            {
                MaDon = Guid.NewGuid().ToString(),
                TongTien =TongTien,
                TrangThai = "cho_xac_nhan",
                NgayDat = DateTime.Now
            };
            _donHangService.ThemDonHang(DonHangMoi);
        
        }

        void IDatHangService.DatHang(int sanPhamId, int khachHangId, int soLuong)
        {
            throw new NotImplementedException();
        }
    }
}
