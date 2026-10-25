using ShopApp.Models;
using ShopApp.Repositories.Interfaces;
using ShopApp.Models.DataModels;

namespace ShopApp.Repositories
{
    public class DonHangRepository : Repositorty<DonHang>, IDonHangRepository
    {
        public DonHangRepository(ShopAppDbContext context) : base(context)
        {
        }
    }
}
