namespace ShopApp.Models.DataModels
{
    public class DonHangKhacHangDTO
    {
        public int Id { get; set; }
        public string MaDon { get; set; }
        public int KhachHangId { get; set; }
        public DateTime NgayDat { get; set; }
        public string TrangThai { get; set; }
        public decimal TongTien { get; set; }
        public string TenKhachHang { get; set; }
    }
}
