using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
                VaiTro = "KhachHang"          // luôn gán cứng, KHÔNG nhận từ client => admin
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
        
            return await TaoCapTokenAsync(kh);
        }

        private async Task<AuthResponseDTO> TaoCapTokenAsync(KhachHang kh)
        {
            var tokenGoc = TaoRefreshTokenGoc();
            _context.RefreshTokens.Add(new RefreshToken
            {
                MaKhachHang = kh.Id,
                TokenHash = BamToken(tokenGoc),
                NgayTao = DateTime.UtcNow,
                HetHan = DateTime.UtcNow.AddDays(
                    _config.GetValue<int>("Jwt:RefreshTokenDays"))
            });
            await _context.SaveChangesAsync();

            var dto = await SinhAccessToken(kh);   // hàm sinh JWT đã làm ở bước trước
            dto.RefreshToken = tokenGoc;     // trả token GỐC cho client
            return dto;
        }


        private async Task<AuthResponseDTO> SinhAccessToken(KhachHang kh)
        {
            var hetHan = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("Jwt:AccessTokenMinutes"));

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
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                HetHan = hetHan,
                Email = kh.Email,
                VaiTro = kh.VaiTro
            };
        }

        public async Task<AuthResponseDTO?> RefreshAsync(string tokenGoc)
        {
            var hash = BamToken(tokenGoc);
            var rt = _context.RefreshTokens.FirstOrDefault(x => x.TokenHash == hash);
            if (rt == null) return null;

            if (rt.DaThuHoi)                       // token cũ bị dùng lại = đáng ngờ
            {
                await ThuHoiTatCaAsync(rt.MaKhachHang);
                return null;
            }
            if (rt.HetHan < DateTime.UtcNow) return null;

            var kh = await _context.KhachHangs.FindAsync(rt.MaKhachHang);
            if (kh == null) return null;

            rt.DaThuHoi = true;                    // thu hồi token cũ
            var capMoi = await TaoCapTokenAsync(kh);   // cấp cặp mới (có SaveChanges)
            rt.ThayBangTokenHash = BamToken(capMoi.RefreshToken);
            await _context.SaveChangesAsync();
            return capMoi;
        }

        private async Task ThuHoiTatCaAsync(int maKhachHang)
        {
            var ds = await _context.RefreshTokens
                .Where(x => x.MaKhachHang == maKhachHang && !x.DaThuHoi)
                .ToListAsync();
            ds.ForEach(x => x.DaThuHoi = true);
            await _context.SaveChangesAsync();
        }



        private static string TaoRefreshTokenGoc()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
        private static string BamToken(string tokenGoc)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(tokenGoc));
            return Convert.ToHexString(hash);
        }


    }
}
