using ShopApp.Models;

namespace ShopApp.Interfaces
{
    public interface IDatHangService
    {
        void DatHang(int sanPhamId, int khachHangId, int soLuong);

        Task<DonHang> DatHangHoanChinh(DatHangRequest request);
        Task<DonHang> DatHangAsync(DatHangRequest request);

    }
}
