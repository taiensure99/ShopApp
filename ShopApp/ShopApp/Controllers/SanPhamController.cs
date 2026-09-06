using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        // Dữ liệu tạm (List tĩnh) — sẽ được THAY BẰNG DATABASE THẬT
        // ở buổi 34 khi học Entity Framework Core
        private static List<SanPham> _danhSach = new List<SanPham>
        {
            new SanPham { Id = 1, Ten = "Áo thun nam basic", GiaBan = 250000, TonKho = 15, DanhMuc = "Áo" },
            new SanPham { Id = 2, Ten = "Quần jean slim fit", GiaBan = 450000, TonKho = 10, DanhMuc = "ao" },
            new SanPham { Id = 3, Ten = "Giày sneaker trắng", GiaBan = 800000, TonKho = 5,  DanhMuc = "Giày" },
            new SanPham { Id = 4, Ten = "Mũ lưỡi trai", GiaBan = 150000, TonKho = 20, DanhMuc = "Phụ kiện" }
        };

        // GET: api/SanPham
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_danhSach);
        }

        // GET: api/SanPham/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var sp = _danhSach.FirstOrDefault(x => x.Id == id);
            if (sp == null)
                return NotFound($"Không tìm thấy sản phẩm Id={id}");

            return Ok(sp);
        }

        [HttpGet("loc")]    
        public IActionResult LocSanPham([FromQuery] string? danhMuc, [FromQuery] double? giaToiDa)
        {
            var ketQua = _danhSach.AsEnumerable();
            if (!string.IsNullOrEmpty(danhMuc))
            {
                ketQua = ketQua.Where(x => x.DanhMuc.Equals(danhMuc, StringComparison.OrdinalIgnoreCase));
            }
            if (giaToiDa.HasValue)
            {
                ketQua = ketQua.Where(x => x.GiaBan <= giaToiDa.Value);
            }
            return Ok(ketQua);
        }

        [HttpGet("moi-nhat")]
        public IActionResult GetMoiNhat()
        {
            var moiNhat = _danhSach.TakeLast(3).ToList();
            if (!moiNhat.Any())
            {
                return NotFound(new { message = "Danh sách hiện đang trống." });
            }

            return Ok(moiNhat);
        }

        [HttpGet("het-hang")]
        public IActionResult GetHetHang()
        {
            var hetHang = _danhSach.Where(x => x.TonKho == 0);

            return Ok(hetHang);
        }

        [HttpGet("danhmuc/{ten:alpha}")]
        public IActionResult GetByDanhMuc(string ten)
        {
            var ketQua = _danhSach
                .Where(s => s.DanhMuc.Equals(ten, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!ketQua.Any())
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm nào thuộc danh mục: {ten}" });
            }

            return Ok(ketQua);
        }

        [HttpGet("timten/{Ten:minlength(3)}")]
        public IActionResult GetTimKiem(string Ten) {
            var KetQua = _danhSach
                .Where (s => s.Ten.Contains(Ten, StringComparison.OrdinalIgnoreCase))
        .ToList();
            if (!KetQua.Any())
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm nào chứa từ khóa: {Ten}" });
            }

            return Ok(KetQua);
        }


    }

    // Học viên tự thêm POST/PUT/DELETE ở bài tập buổi 31
}
