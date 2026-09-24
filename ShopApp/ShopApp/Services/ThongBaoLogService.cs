using ShopApp.Interfaces;

namespace ShopApp.Services
{
    public class ThongBaoLogService : IThongBaoService
    {
        public void Gui(string noiDung) => Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] LOG: {noiDung}");
    }
}
