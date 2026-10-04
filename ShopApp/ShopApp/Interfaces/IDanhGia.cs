using ShopApp.Models.DataModels;

namespace ShopApp.Interfaces
{
    public interface IDanhGia
    {
        Task <List<TongHopDanhGiaResponse>> TongHopDanhGia(int sanPhamId);

    }
}
