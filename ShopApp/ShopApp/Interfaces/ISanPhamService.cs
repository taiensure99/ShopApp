using ShopApp.Models;
namespace ShopApp.Interfaces

{
    public interface ISanPhamService
    {
        Task<bool> KiemTraTonTai(int id);
        Task<decimal> TinhTongTien(int sanPhamId, int soLuong);
        SanPham GetById(int id);
    }
}
