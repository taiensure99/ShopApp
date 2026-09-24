using ShopApp.Models;
using ShopApp.Interfaces;

namespace ShopApp.Services
{
    public class DonHangService : IDonHangService
    {
        private static readonly List<DonHang> _donHangs = new List<DonHang>
        {
            new DonHang { Id = 1, MaDon = "DH001", TongTien = 250000, TrangThai = "Chờ xử lý" ,NgayDat = new  DateTime(2026, 8, 15)},
            new DonHang { Id = 2, MaDon = "DH002", TongTien = 550000, TrangThai = "Đã giao" ,NgayDat = new DateTime(2024, 8, 20)},
            new DonHang { Id = 3, MaDon = "DH003", TongTien = 120000, TrangThai = "Đã hủy",NgayDat = new DateTime(2026, 9, 5) },
            new DonHang { Id = 4, MaDon = "DH004", TongTien = 890000, TrangThai = "Đã giao" , NgayDat = new DateTime(2025, 9, 5) },
            new DonHang { Id = 5, MaDon = "DH005", TongTien = 300000, TrangThai = "Chờ xử lý", NgayDat =  new DateTime(2026, 9, 5)}
        };
        public int DemDonHangTheoTrangThai(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                return 0;
            }

            return _donHangs.Count(d => d.TrangThai != null &&
                                        d.TrangThai.Trim().Equals(trangThai.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
