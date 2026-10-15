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
        public async Task<List<ThongKeKhanhHangVip>> BaoCaoKhachHangVip(decimal nguongChiTieu)
        {
            var ketQua = await _context.KhachHangs
                .Include(kh => kh.DonHangs.Where(dh => dh.TrangThai != "da_huy"))
                    .ThenInclude(dh => dh.ChiTietDonHangs)
                        .ThenInclude(ctdh => ctdh.SanPham)
                .AsSplitQuery()
                .Select(kh => new ThongKeKhanhHangVip
                {
                    Id = kh.Id,
                    HoTen = kh.HoTen,
                    SoLuongDon = kh.DonHangs.Count(dh => dh.TrangThai != "da_huy"),
                    TongChiTieu = kh.DonHangs
                        .Where(dh => dh.TrangThai != "da_huy")
                        .Sum(dh => dh.TongTien),
                    Top3SanPham = kh.DonHangs
                        .Where(dh => dh.TrangThai != "da_huy")
                        .SelectMany(dh => dh.ChiTietDonHangs)
                        .GroupBy(ctdh => new { ctdh.SanPhamId, Ten = ctdh.SanPham.Ten, GiaBan = ctdh.SanPham.GiaBan })
                        .Select(g => new SanPham
                        {
                            Id = g.Key.SanPhamId,
                            Ten = g.Key.Ten,
                            GiaBan = g.Key.GiaBan,
                            TongSoLuongBan = g.Sum(x => x.SoLuong)
                        })
                        .OrderByDescending(sp => sp.TongSoLuongBan)
                        .Take(3)
                        .ToList()
                })
                .Where(x => x.TongChiTieu > nguongChiTieu)
                .ToListAsync();

            return ketQua;
        }

        public async Task<List<SanPhamBanChay>> LaySanPhamBanChay()
        {

            //return await _context.ChiTietDonHangs
            //    .Where(ct => ct.DonHang.TrangThai == "da_giao")
            //    .GroupBy(ct => ct.SanPhamId)
            //    .Select(g => new SanPhamBanChay
            //    {
            //        SanPhamId = g.Key,
            //        SoLuongBan = g.Sum(ct => ct.SoLuong)
            //    })
            //    .Join(_context.SanPhams,
            //    sp => sp.SanPhamId, 
            //    s => s.Id, 
            //    (sp, s) => new 
            //    { SanPhamBanChay = sp, SanPham = s })
            //    .Select(x => x.SanPhamBanChay)
            //    .ToListAsync(); 

            var SoLuongDaBan = await _context.SanPhams.Include(sp => sp.ChiTietDonHangs)
                .Select(sp => new SanPhamBanChay
                {
                    SanPhamId = sp.Id,
                    SoLuongBan = sp.ChiTietDonHangs
                        .Where(ct => ct.DonHang.TrangThai == "da_giao")
                        .Sum(ct => (int?)ct.SoLuong) ?? 0
                })
                .OrderByDescending(sp => sp.SoLuongBan)
                .ToListAsync();
            return SoLuongDaBan;
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
