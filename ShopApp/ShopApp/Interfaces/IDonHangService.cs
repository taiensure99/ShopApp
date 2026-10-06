using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
using System.Collections.Generic;
namespace ShopApp.Interfaces

{
    public interface IDonHangService 
    {

        int DemDonHangTheoTrangThai(string trangThai);
        DonHang ThemDonHang(DonHang donHang);

        DonHang DatHang(DatHangRequest request);

        Task<List<DonHang>> ChiTietDonHang();
        Task<List<DonHang>> ChiTietDayDu();
        Task<List<DonHangKhacHangDTO>> ChiTietDonHangKemTenKhach();
        Task<List<ThongKeKhanhHangVip>> ThongKeTheoKhach();
    }
}
