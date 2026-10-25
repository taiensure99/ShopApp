using ShopApp.Models;
using ShopApp.Models.DataModels;
using ShopApp.Repositories.Interfaces;

namespace ShopApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopAppDbContext _context;

        private IRepository<SanPham>? _sanPhamRepository;
        private IRepository<DonHang>? _donHangRepository;
        private IRepository<KhachHang>? _khachHangRepository;

        public UnitOfWork(ShopAppDbContext context)
        {
            _context = context;
        }

        public IRepository<SanPham> SanPhamRepository =>
            _sanPhamRepository ??= new Repositorty<SanPham>(_context);

        public IRepository<DonHang> DonHangRepository =>
            _donHangRepository ??= new Repositorty<DonHang>(_context);

        public IRepository<KhachHang> KhachHangRepository =>
            _khachHangRepository ??= new Repositorty<KhachHang>(_context);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}