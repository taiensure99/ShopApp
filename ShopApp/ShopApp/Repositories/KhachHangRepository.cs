using ShopApp.Models;
using ShopApp.Repositories.Interfaces;

namespace ShopApp.Repositories
{
    public class KhachHangRepository : Repositorty<KhachHang>, IKhachHangRepository
    {
        public KhachHangRepository(ShopAppDbContext context) : base(context)
        {

        }
    }
}
