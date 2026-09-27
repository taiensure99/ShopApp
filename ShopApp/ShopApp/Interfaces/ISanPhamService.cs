using ShopApp.Models;
namespace ShopApp.Interfaces

{
    public interface ISanPhamService
    {
        Task<bool> KiemTraTonTai(int id);
        Task<decimal> TinhTongTien(int sanPhamId, int soLuong);

        //crud để e tham khảo
        Task<SanPham> GetById(int id);
        Task<List<SanPham>> GetAllSanPham();
        Task<bool> CreateSanPham(SanPham sanPham);
        Task<bool> UpdateSanPham(int id, SanPham sanPham);
        Task<bool> DeleteSanPham(int id);
    }
}
