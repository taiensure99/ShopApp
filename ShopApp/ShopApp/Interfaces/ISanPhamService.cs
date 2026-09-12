namespace ShopApp.Interfaces
{
    public interface ISanPhamService
    {
        Task<bool> KiemTraTonTai(int id); //ddang chay async awai
    }
}
