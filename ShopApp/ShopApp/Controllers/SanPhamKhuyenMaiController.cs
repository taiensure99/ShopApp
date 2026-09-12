using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamKhuyenMaiController : ControllerBase
    {
        private static List<SanPhamKhuyenMai> _danhSach = new List<SanPhamKhuyenMai>
        {
            new SanPhamKhuyenMai { Id = 1, SanPhamId = 1, PhanTramGiam = 20, NgayBatDau = new DateTime(2023, 1, 1), NgayKetThuc = new DateTime(2023, 12, 31) },
            new SanPhamKhuyenMai { Id = 2, SanPhamId = 2, PhanTramGiam = 15, NgayBatDau = new DateTime(2023, 6, 1), NgayKetThuc = new DateTime(2023, 6, 30) },
            new SanPhamKhuyenMai { Id = 3, SanPhamId = 3, PhanTramGiam = 10, NgayBatDau = new DateTime(2023, 7, 1), NgayKetThuc = new DateTime(2027, 7, 31) },
            new SanPhamKhuyenMai { Id = 4, SanPhamId = 4, PhanTramGiam = 25, NgayBatDau = new DateTime(2023, 8, 1), NgayKetThuc = new DateTime(2027, 8, 31) },
            new SanPhamKhuyenMai { Id = 5, SanPhamId = 1, PhanTramGiam = 30, NgayBatDau = new DateTime(2023, 9, 1), NgayKetThuc = new DateTime(2026, 9, 30) }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_danhSach);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var spkm = _danhSach.FirstOrDefault(x => x.Id == id);
            if (spkm == null)
                return NotFound($"Không tìm thấy sản phẩm khuyến mãi Id={id}");
            return Ok(spkm);
        }

        [HttpGet("hieu-luc")]
        public IActionResult GetHieuLuc()
        {
            var spkm = _danhSach.Where(x => x.NgayBatDau <= DateTime.Now && x.NgayKetThuc >= DateTime.Now).ToList();
            return Ok(spkm);
        }

        [HttpGet("{SanPhamId:int}")]
        public IActionResult GetBySanPhamId(int SanPhamId)
        {
            var spkm = _danhSach.Where(x => x.SanPhamId == SanPhamId).ToList();
            if (!spkm.Any())
                return NotFound($"Không tìm thấy sản phẩm khuyến mãi cho sản phẩm Id={SanPhamId}");
            return Ok(spkm);
        }

        [HttpGet("giam-gia/{PhanTramGiam:int}")]
        public IActionResult GetByPhanTramGiam(int PhanTramGiam)
        {
            var spkm = _danhSach.Where(x => x.PhanTramGiam == PhanTramGiam).ToList();
            if (!spkm.Any())
                return NotFound($"Không tìm thấy sản phẩm khuyến mãi với phần trăm giảm {PhanTramGiam}%");
            return Ok(spkm);
        }
        [HttpPost("Khuyen-Mai")]
        public IActionResult CreateKhuyenMai([FromBody] SanPhamKhuyenMai spkm)
        {
            if (spkm.NgayKetThuc <= spkm.NgayBatDau)
            {
                ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc bắt buộc phải sau Ngày bắt đầu.");
                return BadRequest(ModelState);
            }

            bool sanPhamTonTai = _danhSach.Any(sp => sp.Id == spkm.SanPhamId);
            if (!sanPhamTonTai)
            {
                return BadRequest(new { message = $"Sản phẩm với Id = {spkm.SanPhamId} không tồn tại trong hệ thống." });
            }


            int newId = _danhSach.Any() ? _danhSach.Max(s => s.Id) + 1 : 1;
            spkm.Id = newId;
            _danhSach.Add(spkm);
            return Ok(spkm);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateKhuyenMai(int id, [FromBody] SanPhamKhuyenMai req)
        {
            var kmCu = _danhSach.FirstOrDefault(x => x.Id == id);
            if (kmCu == null)
            {
                return NotFound(new { message = "Không tìm thấy khuyến mãi." });
            }

            if (req.SanPhamId != kmCu.SanPhamId)
            {
                return BadRequest(new { message = "Không được đổi sản phẩm áp dụng khuyến mãi" });
            }

            kmCu.PhanTramGiam = req.PhanTramGiam;
            kmCu.NgayKetThuc = req.NgayKetThuc;
            return Ok(kmCu);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteKhuyenMai(int id, [FromBody] SanPhamKhuyenMai red)
        {
            var kmcu = _danhSach.FirstOrDefault(X => X.Id == id);

            if (kmcu == null)
            {
                return NotFound(new { message = "Khong tim thay" });
            }
            if (kmcu.NgayBatDau <= DateTime.Now)
            {
                return BadRequest(new { message = "Không thể xóa khuyến mãi đã/đang áp dụng" });
            }

            _danhSach.Remove(kmcu);
            return Ok(new { message = "Đã xóa thành công khuyến mãi." });
        }
    }
}
