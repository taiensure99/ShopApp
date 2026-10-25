using ShopApp.Models;
using ShopApp.Models.DataModels;

namespace ShopApp.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<SanPham> SanPhamRepository { get; }
        IRepository<DonHang> DonHangRepository { get; }
        IRepository<KhachHang> KhachHangRepository { get; }

        Task<int> SaveChangesAsync();
    }
}