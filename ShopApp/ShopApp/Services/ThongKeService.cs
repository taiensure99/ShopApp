using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;

namespace ShopApp.Services
{
    public class ThongKeService : IThongKeService
    {
        private readonly ShopAppDbContext _context;
        public ThongKeService(ShopAppDbContext context)
        {
            _context = context;
        }
        public int DenTongSanPham()
        {
            // Implementation for counting total products
            return _context.SanPhams.Count();
        }

        public int DenSanPhamHetHang()
        {
            // Implementation for counting out-of-stock products
            return _context.SanPhams.Count(sp => sp.TonKho == 0);
        }
        public async Task<int> DemDonHangTheoTrangThai(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                return 0;
            }
            return await _context.DonHangs
                .CountAsync(d => d.TrangThai != null && d.TrangThai == trangThai);
                                 
        }


        public async Task<List<ThongKeThangNam>> TinhDoanhThuTheoThangNam(int nam)
        {
            var KetQua = await _context.DonHangs
                .Where(dh => dh.NgayDat.Year == nam && dh.TrangThai == "da_giao")
                .GroupBy(dh => new { dh.NgayDat.Month, dh.NgayDat.Year })
                .Select(g => new ThongKeThangNam { Thang = g.Key.Month, DoanhThu = g.Sum(d => (decimal?)d.TongTien) ?? 0 })
                .ToListAsync();
            return KetQua;
        }
    }
}
