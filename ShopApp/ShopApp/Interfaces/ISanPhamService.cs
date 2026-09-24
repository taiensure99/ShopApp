using ShopApp.Models;
namespace ShopApp.Interfaces

{
    public interface ISanPhamService
    {
        Task<bool> KiemTraTonTai(int id);
        SanPham GetById(int id);
    }
}
