using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopApp.Models.DataModels;
using System.Security.Claims;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var ok = await _auth.RegisterAsync(dto);
            return ok ? Ok(new { message = "Đăng ký thành công" })
                      : BadRequest(new { message = "Email đã tồn tại" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var kq = await _auth.LoginAsync(dto);
            return kq == null ? Unauthorized(new { message = "Sai email hoặc mật khẩu" })
                              : Ok(kq);
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me() => Ok(new
        {
            maKhachHang = User.FindFirstValue(ClaimTypes.NameIdentifier),
            email = User.FindFirstValue(ClaimTypes.Email),
            vaiTro = User.FindFirstValue(ClaimTypes.Role)
        });

    }
}
