using ShopApp.Interfaces;

namespace ShopApp.Services
{
    public class SanPhamKhuyenMaiService : ISanPhamKhuyenMaiService
    {
        private readonly ISanPhamService _sanPhamService;

        public SanPhamKhuyenMaiService(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        public double TinhGiaSauKhuyenMai(int sanPhamId, double phanTramGiam)
        {
            var sp = _sanPhamService.GetById(sanPhamId);
            if (sp == null) return 0;
            return (double)sp.GiaBan * (1 - phanTramGiam / 100);
        }
    }
}
