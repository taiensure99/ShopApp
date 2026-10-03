using ShopApp.Interfaces;
using ShopApp.Models;
using System.Runtime.CompilerServices;

namespace ShopApp.Services
{
    public class SanPhamKhuyenMaiService : ISanPhamKhuyenMaiService
    {
        private readonly ISanPhamService _sanPhamService;
        private readonly ShopAppDbContext _context;

        public SanPhamKhuyenMaiService(ISanPhamService sanPhamService, ShopAppDbContext context)
        {
            _sanPhamService = sanPhamService;
            _context = context;
        }
        

        public async Task<double> TinhGiaSauKhuyenMai(int sanPhamId, double phanTramGiam)
        {
            var sp = await _sanPhamService.GetById(sanPhamId);
            if (sp == null) return 0;
            return (double)sp.GiaBan * (1 - phanTramGiam / 100);
        }
    }
}
