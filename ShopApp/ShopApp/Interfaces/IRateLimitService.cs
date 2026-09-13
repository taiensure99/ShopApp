namespace ShopApp.Interfaces
{
    public interface IRateLimitService
    {
        bool KiemTraVuotGioiHan(string diaChiIp);
        void GhiNhanRequest(string diaChiIp);
    }
}
