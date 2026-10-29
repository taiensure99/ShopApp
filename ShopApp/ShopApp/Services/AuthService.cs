using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly ShopAppDbContext _context;
        private readonly IConfiguration _config;
        public AuthService(ShopAppDbContext context, IConfiguration config)
        { _context = context; _config = config; }

        //đăng ký: nếu email đã tồn tại thì trả về false, ngược lại lưu vào CSDL và trả về true
        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var daCo = await _context.KhachHangs.AnyAsync(k => k.Email == dto.Email);
            if (daCo) return false;

            _context.KhachHangs.Add(new KhachHang
            {
                HoTen = dto.HoTen,
                Email = dto.Email,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau),
                VaiTro = "KhachHang"          // luôn gán cứng, KHÔNG nhận từ client
            });
            await _context.SaveChangesAsync();
            return true;
        }

        //đăng nhập: nếu email không tồn tại hoặc mật khẩu sai thì trả về null, ngược lại trả về token
        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
        {
            var kh = await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.Email == dto.Email);
            if (kh == null || string.IsNullOrEmpty(kh.MatKhau)
                || !BCrypt.Net.BCrypt.Verify(dto.MatKhau, kh.MatKhau))
                return null;                  // 1 thông báo chung, không lộ email có tồn tại

            var hetHan = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("Jwt:ExpireMinutes"));

            var claims = new List<Claim>
          {
              new(ClaimTypes.NameIdentifier, kh.Id.ToString()),
              new(ClaimTypes.Email, kh.Email),
              new(ClaimTypes.Role, kh.VaiTro)
          };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: hetHan,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                HetHan = hetHan,
                Email = kh.Email,
                VaiTro = kh.VaiTro
            };
        }

    }
}
