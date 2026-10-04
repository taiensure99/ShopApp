using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
namespace ShopApp.Services
{
    public class DanhGiaService : IDanhGia
    {
        private readonly ShopAppDbContext _context;
        public DanhGiaService(ShopAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TongHopDanhGiaResponse>> TongHopDanhGia(int sanPhamId) {
            var KetQua = await _context.DanhGiaSanPhams
                .Join(_context.KhachHangs, dg => dg.KhachHangId, kh => kh.Id, (dg, kh) => new { DanhGia = dg, KhachHang = kh })
                .Join(_context.SanPhams, x => x.DanhGia.SanPhamId, sp => sp.Id, (x, sp) => new { x.DanhGia, x.KhachHang, SanPham = sp })
                .Where(x => x.SanPham.Id == sanPhamId)
                .GroupBy(x => x.DanhGia.SoSao)
                .Select(g => new TongHopDanhGiaResponse
                {
                    SanPhamId = g.Select(x => x.SanPham.Id).FirstOrDefault(),
                    TenKhach = g.Select(x => x.KhachHang.HoTen).FirstOrDefault(),
                    TenSanPham = g.Select(x => x.SanPham.Ten).FirstOrDefault(),
                    NgayDanhGia = g.Select(x => x.DanhGia.NgayDanhGia).FirstOrDefault(),
                    Sao = g.Key, 
                    SoLuong = g.Count() 
                })
                .ToListAsync();

            return KetQua;
        }
    }
}
