using Microsoft.AspNetCore.Mvc;
using ShopApp.Models;
using ShopApp.Models.DataModels;
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
        Task<List<SanPham>> LocAsync(string? danhMuc, decimal? giaToiDa);
        Task<Dictionary<string, int>> ThongKeTheoDanhMucAsync();
        Task<bool> ChuyenSanPhamGiuaDanhMucAsync(int spId1, int spId2);
        Task<List<SanPham>> LocGiaAsync(decimal giamin, decimal giamax);
        Task<List<SanPham>> DanhSachRutGonAsync();
        Task<DenSoDonHang> DemTheoDanhMucAsync();
        Task<List<SanPham>> TopDatNhatTheoDanhMucAsync();

    }
}
