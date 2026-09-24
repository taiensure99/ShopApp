using ShopApp.Interfaces;
using ShopApp.Models;

namespace ShopApp.Services
{
    public class KhachHangService : IKhachHangService
    {
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

        public List<KhachHang> GetAll()
        {
            return _khachHangs;
        }

        public KhachHang GetById(int id)
        {
            return _khachHangs.FirstOrDefault(k => k.Id == id);
        }

        public List<KhachHang> TimKiemTheoTen(string ten)
        {
            if (string.IsNullOrWhiteSpace(ten)) return _khachHangs;

            return _khachHangs
                .Where(k => k.HoTen != null &&
                            k.HoTen.Contains(ten, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public KhachHang GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return _khachHangs.FirstOrDefault(
                k => k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public bool UpdateKhachHang(int id, KhachHang khachHang)
        {
            var kh = GetById(id);
            if (kh == null) return false;

            kh.HoTen = khachHang.HoTen;
            kh.Email = khachHang.Email;
            // TODO: cập nhật thêm các field khác nếu có (SoDienThoai, DiaChi,...)

            return true;
        }

        public bool DeleteKhachHang(int id)
        {
            var kh = GetById(id);
            if (kh == null) return false;

            _khachHangs.Remove(kh);
            return true;
        }
    }
}