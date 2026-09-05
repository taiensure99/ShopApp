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
            new SanPham { Id = 2, Ten = "Quần jean slim fit", GiaBan = 450000, TonKho = 10, DanhMuc = "Quần" },
            new SanPham { Id = 3, Ten = "Giày sneaker trắng", GiaBan = 800000, TonKho = 5,  DanhMuc = "Giày" },
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

        // Học viên tự thêm POST/PUT/DELETE ở bài tập buổi 31
    }
}
