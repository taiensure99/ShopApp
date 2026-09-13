using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopApp.Models;
using System.ComponentModel.DataAnnotations;

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

        private readonly ISanPhamService _sanPhamService;
        private readonly IThongKeService _thongKeService;

        public SanPhamController(ISanPhamService sanPhamService, IThongKeService thongKeService)
        {
            _sanPhamService = sanPhamService;
            _thongKeService = thongKeService;
        }

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

        [HttpPost]
        public IActionResult Create([FromBody] SanPham sanPham)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Tạo Id mới
            int newId = _danhSach.Max(s => s.Id) + 1;
            sanPham.Id = newId;
            _danhSach.Add(sanPham);
            return CreatedAtAction(nameof(GetById), new { id = sanPham.Id }, sanPham);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SanPham sanPham)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingSanPham = _danhSach.FirstOrDefault(s => s.Id == id);
            if (existingSanPham == null)
            {
                return NotFound($"Không tìm thấy sản phẩm Id={id}");
            }
            // Cập nhật thông tin sản phẩm
            existingSanPham.Ten = sanPham.Ten;
            existingSanPham.GiaBan = sanPham.GiaBan;
            existingSanPham.TonKho = sanPham.TonKho;
            existingSanPham.DanhMuc = sanPham.DanhMuc;
            existingSanPham.IsActive = sanPham.IsActive;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingSanPham = _danhSach.FirstOrDefault(s => s.Id == id);
            if (existingSanPham == null)
            {
                return NotFound($"Không tìm thấy sản phẩm Id={id}");
            }
            _danhSach.Remove(existingSanPham);
            return Ok($"Đã xóa sản phẩm Id={id}");
        }

        [HttpPatch("{id}/gia")]
        public IActionResult CapNhatGia(int id, [FromBody] CapNhatGiaRequest req) 
        {
            var sanPham = _danhSach.FirstOrDefault(s => s.Id == id);
            if (sanPham == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm có ID = {id}" });
            }

            if (req.GiaBanMoi < 0)
            {
                return BadRequest(new { message = "Giá sản phẩm không được nhỏ hơn 0." });
            }

            sanPham.GiaBan = req.GiaBanMoi;

            return Ok(new
            {
                message = "Cập nhật giá thành công!",
                data = sanPham
            });
        }
        [HttpPost("nhieu")]
        public IActionResult CreateNhieu([FromBody] List<SanPham> danhSachMoi)
        {
            // 1. Tạo 2 danh sách để phân loại kết quả
            var thanhCong = new List<SanPham>();
            var thatBai = new List<object>(); // Dùng object ẩn danh để linh hoạt chứa lỗi

            foreach (var sp in danhSachMoi)
            {
                // 2. Khởi tạo bộ kiểm tra (ValidationContext) cho từng sản phẩm
                var validationContext = new ValidationContext(sp);
                var validationResults = new List<ValidationResult>();

                // Hàm này sẽ kiểm tra sp dựa trên các Data Annotations ([Required], [Range]...)
                bool isValid = Validator.TryValidateObject(sp, validationContext, validationResults, true);

                if (isValid)
                {
                    // 3. Nếu HỢP LỆ -> Tạo ID mới và thêm vào Database/List
                    int newId = _danhSach.Any() ? _danhSach.Max(s => s.Id) + 1 : 1;
                    sp.Id = newId;
                    _danhSach.Add(sp);

                    thanhCong.Add(sp);
                }
                else
                {
                    // 4. Nếu KHÔNG HỢP LỆ -> Gom các thông báo lỗi lại
                    var errors = validationResults.Select(r => r.ErrorMessage).ToList();
                    thatBai.Add(new
                    {
                        SanPhamLoi = sp,
                        LyDo = errors
                    });
                }
            }

            // 5. Trả về kết quả tổng hợp (Dùng Status 200 OK hoặc 207 Multi-Status)
            return Ok(new
            {
                ThongBao = $"Xử lý hoàn tất. Thêm thành công: {thanhCong.Count}, Lỗi: {thatBai.Count}",
                DanhSachThanhCong = thanhCong,
                DanhSachLoi = thatBai
            });
        }
        [HttpGet("thong-ke")]
        public IActionResult ThongKe()
        {
            var tongSanPham = _thongKeService.DenTongSanPham();
            var sanPhamHetHang = _thongKeService.DenSanPhamHetHang();

            return Ok(new
            {
                TongSoSanPham = tongSanPham,
                SanPhamHetHang = sanPhamHetHang
            });
        }

    }


    // Học viên tự thêm POST/PUT/DELETE ở bài tập buổi 31
}
