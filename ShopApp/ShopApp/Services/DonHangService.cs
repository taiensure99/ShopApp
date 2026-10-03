using ShopApp.Models;
using ShopApp.Interfaces;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.Services
{
    public class DonHangService : IDonHangService
    {
        private readonly ShopAppDbContext _context;
        public DonHangService(ShopAppDbContext context)
        {
            _context = context;
        }

        public DonHang DatHang(DatHangRequest request)
        {
            throw new NotImplementedException();
        }

        public int DemDonHangTheoTrangThai(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                return 0;
            }

            return _context.DonHangs.Count(d => d.TrangThai != null &&
                                        d.TrangThai.Trim().Equals(trangThai.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public int ThemDonHang(DonHang donHang)
        {
            throw new NotImplementedException();
        }

        DonHang IDonHangService.ThemDonHang(DonHang donHang)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DonHang>> ChiTietDonHang()
        {
            var KetQua = await _context.DonHangs
                .Join (_context.KhachHangs,
                    dh => dh.KhachHangId,kh => kh.Id,
                    (dh,kh) => new DonHang
                    {
                        Id = dh.Id,
                        MaDon = dh.MaDon,
                        KhachHangId = dh.KhachHangId,
                        NgayDat = dh.NgayDat,
                        TrangThai = dh.TrangThai,
                        TongTien = dh.TongTien,
                        KhachHang = kh
                    }).ToListAsync();
            return KetQua;
        }

        public async Task<List<DonHang>> ChiTietDayDu()
        {
            var KetQua = await _context.DonHangs
                .Join(_context.KhachHangs,
                    dh => dh.KhachHangId, kh => kh.Id,
                    (dh,kh) => new {dh, kh}
                    ).Join
                (_context.ChiTietDonHangs,
                    d => d.dh.Id, ctdh => ctdh.DonHangId,
                    (d, ctdh) => new DonHang
                    {
                        Id = d.dh.Id,
                        MaDon = d.dh.MaDon,
                        KhachHangId = d.dh.KhachHangId,
                        NgayDat = d.dh.NgayDat,
                        TrangThai = d.dh.TrangThai,
                        TongTien = d.dh.TongTien,
                        KhachHang = d.kh,
                        ChiTietDonHangs = new List<ChiTietDonHang> { ctdh }
                    }).ToListAsync();


            return KetQua;
        }
    }
}
