namespace ShopApp.Services
{
    public class GuidServiceScoped : Interfaces.IGuidService
    {

        public string LayGuidMoi()
        {
            return Guid.NewGuid().ToString(); ;
        }
    }
}
