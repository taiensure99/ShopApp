namespace ShopApp.Services
{
    public class GuidServiceSingleton : Interfaces.IGuidService
    {
        public string LayGuidMoi()
        {
            return Guid.NewGuid().ToString(); ;
        }
    }
}
