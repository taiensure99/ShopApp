using ShopApp.Models;
using ShopApp.Models.DataModels;

namespace ShopApp.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDTO dto);              // false nếu trùng email
        Task<AuthResponseDTO?> LoginAsync(LoginDTO dto);        // null nếu sai thông tin
        Task<AuthResponseDTO?> RefreshAsync(string tokenGoc);


    }
}
