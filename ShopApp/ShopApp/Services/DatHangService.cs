using ShopApp.Models;
using ShopApp.Services;
using ShopApp.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace ShopApp.Services
{
    public class DatHangService : IDatHangService
    {
        private readonly IKhachHangService _khachHangService;
        private readonly ISanPhamService _sanPhamService;
        private readonly IDonHangService _donHangService;
        private readonly ShopAppDbContext _context;

        public DatHangService(ShopAppDbContext context,
            IKhachHangService khachHangService,
            ISanPhamService sanPhamService,
            IDonHangService donHangService)
        {
            _khachHangService = khachHangService;
            _sanPhamService = sanPhamService;
            _donHangService = donHangService;
            _context = context;
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

        public async Task<DonHang> DatHangHoanChinh(DatHangRequest request)
        {
            var khachHangId = new SqlParameter("@KhachHangId", request.KhachHangId);
            var soLuong = new SqlParameter("@SoLuong", request.SoLuong);
            var sanPhamId = new SqlParameter("@SanPhamId", request.SanPhamId);

            var donHangIdOutPut = new SqlParameter
            {
                ParameterName = "@DonHangId",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_DatHangHoanChinh @KhachHangId, @SanPhamId, @SoLuong, @DonHangId OUTPUT",
                khachHangId, sanPhamId, soLuong, donHangIdOutPut);
            int donHangId = (int)donHangIdOutPut.Value;

            var kq = await _context.DonHangs.FirstOrDefaultAsync(dh => dh.Id == donHangId);


            return kq;
        }
    }
}
