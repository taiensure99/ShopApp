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
        public async Task<List<ThongKeKhanhHangVip>> LayDanhSachKhachHangVIP()
        {
            var danhSachKhachHangVIP = await _context.DonHangs
                .Where(dh => dh.TrangThai == "da_giao")
                // SỬA LỖI TẠI ĐÂY: Giữ lại cả DonHang (dh) và KhachHang (kh) bằng một object vô danh (new { ... })
                .Join(_context.KhachHangs,
                dh => dh.KhachHangId, 
                kh => kh.Id, (dh, kh) => new { DonHang = dh, KhachHang = kh })
                // Group theo Id của Khách hàng
                .GroupBy(x => x.KhachHang.Id)
                .Select(g => new ThongKeKhanhHangVip
                {
                    Id = g.Key,
                    // SỬA LỖI TẠI ĐÂY: Trỏ chính xác vào x.DonHang để tính tổng tiền
                    TongChiTieu = g.Sum(x => x.DonHang.TongTien),
                    SoLuongDon = g.Count()
                })
                .Where(kh => kh.TongChiTieu > 300000)
                .OrderByDescending(kh => kh.TongChiTieu)
                .ToListAsync();

            return danhSachKhachHangVIP;
        }

        public async Task<List<SanPhamBanChay>> LaySanPhamBanChay()
        {

            return await _context.ChiTietDonHangs
                .Where(ct => ct.DonHang.TrangThai == "da_giao")
                .GroupBy(ct => ct.SanPhamId)
                .Select(g => new SanPhamBanChay
                {
                    SanPhamId = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong)
                })
                .Join(_context.SanPhams,
                sp => sp.SanPhamId, 
                s => s.Id, 
                (sp, s) => new 
                { SanPhamBanChay = sp, SanPham = s })
                .Select(x => x.SanPhamBanChay)
                .ToListAsync();
        }
        public async Task<ThongKeBaoCaoTongHop> LayBaoCaoTongHop()
        {
            var tongSanPham = await _context.DonHangs
                .AsNoTracking()
                .Where(x => x.TrangThai == "da_giao")
                .SumAsync(x => (decimal?)x.TongTien) ?? 0;
            var DoanhThuThang = await _context.DonHangs
                .AsNoTracking()
                .Where(x => x.TrangThai == "da_giao" && x.NgayDat.Year == DateTime.Now.Year)
                .GroupBy(x => x.NgayDat.Month)
                .Select(x => new ThongKeThangNam { Thang = x.Key, DoanhThu = x.Sum(dh => (decimal?)dh.TongTien) ?? 0 })
                .ToListAsync();

            var TopKhachHang = await _context.DonHangs
                .AsNoTracking()
                .Where(x => x.TrangThai == "da_giao")
                .GroupBy(x => x.KhachHangId)
                .Select(g => new TongChiTieuKhachHang { KhachHangId = g.Key, TongChiTieu = g.Sum(dh => (decimal?)dh.TongTien) ?? 0 })
                .OrderByDescending(x => x.TongChiTieu)
                .Take(5)
                .ToListAsync();
            var TopSanPham = await _context.ChiTietDonHangs
                .AsNoTracking()
                .Where(ct => ct.DonHang.TrangThai == "da_giao")
                .GroupBy(ct => ct.SanPhamId)
                .Select(g => new TopSanPham { SanPhamId = g.Key, SoLuongBan = g.Sum(ct => ct.SoLuong) })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(5)
                .ToListAsync();
            var TrangThai = await _context.DonHangs
                .AsNoTracking()
                .GroupBy(x => x.TrangThai)
                .Select(g => new ThongKeTrangThai { TrangThai = g.Key, SoLuong = g.Count() })
                .ToListAsync();
            var baoCaoTongHop = new ThongKeBaoCaoTongHop
            {
                TongSanPham = tongSanPham,
                DoanhThuThang = DoanhThuThang,
                TopKhachHang = TopKhachHang,
                TopSanPham = TopSanPham,
                TrangThai = TrangThai
            };
            return baoCaoTongHop;
        }
    }
}
