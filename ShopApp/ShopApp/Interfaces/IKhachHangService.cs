using ShopApp.Models;

namespace ShopApp.Interfaces
{
    public interface IKhachHangService
    {
        bool KiemTraEmailTonTai(string email);
        int DemTongKhach();
        void ThemKhachHang(KhachHang khachHang);

        List<KhachHang> GetAll();
        List<KhachHang> TimKiemTheoTen(string ten);
        KhachHang GetById(int id);
      
        KhachHang GetByEmail(string email);
        bool UpdateKhachHang(int id, KhachHang khachHang);
        bool DeleteKhachHang(int id);
    }
}
