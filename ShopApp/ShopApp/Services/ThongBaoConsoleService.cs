namespace ShopApp.Services
{
    public class ThongBaoConsoleService : Interfaces.IThongBaoService
    {
        public void Gui(string noiDung) => Console.WriteLine($"[CONSOLE] {noiDung}");
    }
}
