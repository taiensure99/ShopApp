namespace ShopApp.Models.DataModels
{
    public class ThongKeKhanhHangVip
    {
        public int Id { get; set; }
        public int SoLuongDon { get; set; }
        public decimal TongChiTieu { get; set; }
        public string HoTen { get; internal set; }
        public object Top3SanPham { get; internal set; }
        public int TongSoLuongBan { get; set; }
    }
}
