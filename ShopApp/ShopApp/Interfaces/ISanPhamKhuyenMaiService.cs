namespace ShopApp.Interfaces
{
    public interface ISanPhamKhuyenMaiService
    {
        Task<double> TinhGiaSauKhuyenMai(int sanPhamId, double phanTramGiam);
    }
}
