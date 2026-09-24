using ShopApp.Models;
using ShopApp.Interfaces;
using System.Collections.Generic;
namespace ShopApp.Interfaces

{
    public interface IDonHangService 
    {

        int DemDonHangTheoTrangThai(string trangThai);
        DonHang ThemDonHang(DonHang donHang);

        DonHang DatHang(DatHangRequest request);
    }
}
