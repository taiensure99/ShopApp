namespace ShopApp.Models.DataModels
{
    public class SanPhamTimKiemDto
    {

        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public decimal GiaBan { get; set; }
        public string? DanhMuc { get; set; }
        public int TonKho { get; set; }

    }
}
