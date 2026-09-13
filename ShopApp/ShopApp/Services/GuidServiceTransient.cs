namespace ShopApp.Services
{
    public class GuidServiceTransient : Interfaces.IGuidService
    {
        public string LayGuidMoi()
        {
            return Guid.NewGuid().ToString(); ;
        }
    }
}
