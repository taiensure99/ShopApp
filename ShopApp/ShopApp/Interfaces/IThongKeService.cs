using Microsoft.AspNetCore.Mvc;
using ShopApp.Models;
using ShopApp.Models.DataModels;

namespace ShopApp.Interfaces
{
    public interface IThongKeService
    {
        int DenTongSanPham();
        int DenSanPhamHetHang();
        Task<int> DemDonHangTheoTrangThai(string trangThai);
        Task<List<ThongKeThangNam>> TinhDoanhThuTheoThangNam(int nam);
        Task<List<ThongKeKhanhHangVip>> LayDanhSachKhachHangVIP();
        Task<List<SanPhamBanChay>> LaySanPhamBanChay();
        Task<ThongKeBaoCaoTongHop> LayBaoCaoTongHop();
    }
}
