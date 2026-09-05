using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private static List<Models.KhachHang> _danhSach = new List<Models.KhachHang>
        {
            new Models.KhachHang { Id = 1, HoTen = "Nguyen Van A", Email = "nguyenvana@email.com" },
            new Models.KhachHang { Id = 2, HoTen = "Tran Thi B", Email = "tranthib@email.com" },
            new Models.KhachHang { Id = 3, HoTen = "Le Van C", Email = "levanc@email.com" },
            new Models.KhachHang { Id = 4, HoTen = "Pham Thi D", Email = "phamthid@email.com" }
        };
        // GET: api/KhachHang
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_danhSach);
        }

        [HttpGet("{id}")]

        public IActionResult Get(int id)
        {
            var kh = _danhSach.FirstOrDefault(x => x.Id == id);
            if (kh == null)
                return NotFound($"Không tìm thấy khách hàng Id={id}");

            return Ok(kh);

        }

        [HttpGet("timkiem/{tenCanTim}")]
        public IActionResult TimKiem(string tenCanTim)
        {
            var kh = _danhSach.Where(x => x.HoTen.ToLower().Contains(tenCanTim)).ToList();
            if (kh == null || kh.Count == 0)
                return NotFound($"Không tìm thấy khách hàng có tên chứa '{tenCanTim}'");

            return Ok(kh);
        }
    }
}
