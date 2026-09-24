namespace ShopApp.Interfaces
{
    public interface IKhachHangService
    {
        bool KiemTraEmailTonTai(string email);
        int DemTongKhach();
        void ThemKhachHang(Models.KhachHang khachHang);
    }
}
