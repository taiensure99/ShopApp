using Microsoft.AspNetCore.Mvc;
using ShopApp.Models;
namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : Controller 
    {
        private static readonly List<DonHang> _donHangs = new List<DonHang>
        {
            new DonHang { Id = 1, MaDon = "DH001", TongTien = 250000, TrangThai = "Chờ xử lý" ,NgayDat = new  DateTime(2026, 8, 15)},
            new DonHang { Id = 2, MaDon = "DH002", TongTien = 550000, TrangThai = "Đã giao" ,NgayDat = new DateTime(2024, 8, 20)},
            new DonHang { Id = 3, MaDon = "DH003", TongTien = 120000, TrangThai = "Đã hủy",NgayDat = new DateTime(2026, 9, 5) },
            new DonHang { Id = 4, MaDon = "DH004", TongTien = 890000, TrangThai = "Đã giao" , NgayDat = new DateTime(2025, 9, 5) },
            new DonHang { Id = 5, MaDon = "DH005", TongTien = 300000, TrangThai = "Chờ xử lý", NgayDat =  new DateTime(2026, 9, 5)}
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_donHangs);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var donHang = _donHangs.FirstOrDefault(d => d.Id == id);

            if (donHang == null)
            {
                return NotFound(new { message = $"Không tìm thấy đơn hàng với Id = {id}" });
            }
            if (donHang.TongTien <= 0)
            {
                return BadRequest("Tổng tiền phải lớn hơn 0");

            }


            return Ok(donHang);
        }
        [HttpGet("theo-trangthai/{trangThai}")]
        public IActionResult GetByTrangThai(string trangThai)
        {
            if (trangThai is not ("cho_xac_nhan" or "da_giao" or "da_huy"))
            {
                return BadRequest(new { message = "Trạng thái không hợp lệ. Vui lòng nhập: cho_xac_nhan, da_giao, hoặc da_huy" });
            }
            var ketQua = _donHangs
                    .Where(d => d.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!ketQua.Any())
                {
                    return NotFound(new { message = $"Không có đơn hàng nào ở trạng thái: {trangThai}" });

                }
                return Ok(ketQua);
        }

        
        [HttpGet("{nam:int}/{thang:int}")]
        public IActionResult GetByNamThang(int nam, int thang)
        {
            var ketQua = _donHangs.Where(x => x.NgayDat.Year == nam && x.NgayDat.Month == thang);
            if (!ketQua.Any())
            {
                return NotFound(new { message = $"Không có đơn hàng nào trong tháng {thang} năm {nam}" });
            }
            return Ok(ketQua);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var donHang = _donHangs.FirstOrDefault(d => d.Id == id);
            if (donHang == null)
            {
                return NotFound(new { message = $"Không tìm thấy đơn hàng với Id = {id}" });
            }
            _donHangs.Remove(donHang);
            return Ok(new { message = $"Đã xóa đơn hàng với Id = {id}" });
        }

        [HttpPost]
        public IActionResult Create([FromBody] Models.DonHang donHang)
        {
            if (donHang == null)
            {
                return BadRequest("Dữ liệu đơn hàng không được để trống.");
            }

            if (donHang.TongTien <= 0)
            {
                ModelState.AddModelError("TongTien", "Tổng tiền phải lớn hơn 0.");
            }

            if (string.IsNullOrWhiteSpace(donHang.TrangThai))
            {
                ModelState.AddModelError("TrangThai", "Trạng thái không được để trống.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newId = _donHangs.Any() ? _donHangs.Max(d => d.Id) + 1 : 1;
            donHang.Id = newId;

            _donHangs.Add(donHang);

            return CreatedAtAction(nameof(GetById), new { id = donHang.Id }, donHang);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Models.DonHang donHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingDonHang = _donHangs.FirstOrDefault(d => d.Id == id);
            if (existingDonHang == null)
            {
                return NotFound(new { message = $"Không tìm thấy đơn hàng với Id = {id}" });
            }
            // Cập nhật thông tin đơn hàng
            existingDonHang.MaDon = donHang.MaDon;
            existingDonHang.TongTien = donHang.TongTien;
            existingDonHang.TrangThai = donHang.TrangThai;
            existingDonHang.NgayDat = donHang.NgayDat;
            return NoContent();
        }

        [HttpGet("dem/{trangThai}")]
        public IActionResult DemDonHangTheoTrangThai(string trangThai)
        {
            int soLuong = _donHangs.Count(d => d.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase));
            return Ok(new
            {
                TrangThai = trangThai,
                SoLuong = soLuong
            });
        }

    };
}
