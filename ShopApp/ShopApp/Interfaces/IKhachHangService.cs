using ShopApp.Models;

namespace ShopApp.Interfaces
{
    public interface IKhachHangService
    {
        bool KiemTraEmailTonTai(string email);
        int DemTongKhach();
        Task<bool> ThemKhachHang(KhachHang khachHang);

        Task<List<KhachHang>> GetAll();
        Task<List<KhachHang>> TimKiemTheoTen(string ten);
        Task<KhachHang> GetById(int id);
      
        KhachHang GetByEmail(string email);
        Task<bool> UpdateKhachHang(int id, KhachHang khachHang);
        Task<bool> DeleteKhachHang(int id);
    }
}
