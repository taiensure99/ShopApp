using ShopApp.Models;
using ShopApp.Repositories.Interfaces;

namespace ShopApp.Repositories
{
    public class SanPhamRepository : Repositorty<SanPham>, ISanPhamRepository
    {
        public SanPhamRepository(ShopAppDbContext context) : base(context)
        {
        }


    }
}
