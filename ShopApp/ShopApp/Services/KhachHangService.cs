using ShopApp.Interfaces;
using ShopApp.Models;

namespace ShopApp.Services
{
    public class KhachHangService : IKhachHangService
    {
        // List tĩnh để giữ dữ liệu không bị mất giữa các HTTP Request
        private static readonly List<KhachHang> _khachHangs = new List<KhachHang>();

        public bool KiemTraEmailTonTai(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return _khachHangs.Any(k => k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public int DemTongKhach() => _khachHangs.Count;

        public void ThemKhachHang(KhachHang khachHang)
        {
            khachHang.Id = _khachHangs.Any() ? _khachHangs.Max(k => k.Id) + 1 : 1;
            _khachHangs.Add(khachHang);
        }
    }
}
