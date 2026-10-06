using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        // Dữ liệu tạm (List tĩnh) — sẽ được THAY BẰNG DATABASE THẬT
        // ở buổi 34 khi học Entity Framework Core
        private readonly ISanPhamService _sanPhamService;
        private readonly IThongKeService _thongKeService;

        public SanPhamController(ISanPhamService sanPhamService, IThongKeService thongKeService)
        {
            _sanPhamService = sanPhamService;
            _thongKeService = thongKeService;
        }

        // GET: api/SanPham
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _sanPhamService.GetAllSanPham();
            return Ok(list);
        }

        // GET: api/SanPham/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sp = await _sanPhamService.GetById(id);
            if (sp == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm có ID = {id}" });
            }

            return Ok(sp);
        }


        //[HttpGet("moi-nhat")]
        //public IActionResult GetMoiNhat()
        //{
        //    var moiNhat = _danhSach.TakeLast(3).ToList();
        //    if (!moiNhat.Any())
        //    {
        //        return NotFound(new { message = "Danh sách hiện đang trống." });
        //    }

        //    return Ok(moiNhat);
        //}

        //[HttpGet("het-hang")]
        //public IActionResult GetHetHang()
        //{
        //    var hetHang = _danhSach.Where(x => x.TonKho == 0);

        //    return Ok(hetHang);
        //}

        //[HttpGet("danhmuc/{ten:alpha}")]
        //public IActionResult GetByDanhMuc(string ten)
        //{
        //    var ketQua = _danhSach
        //        .Where(s => s.DanhMuc.Equals(ten, StringComparison.OrdinalIgnoreCase))
        //        .ToList();

        //    if (!ketQua.Any())
        //    {
        //        return NotFound(new { message = $"Không tìm thấy sản phẩm nào thuộc danh mục: {ten}" });
        //    }

        //    return Ok(ketQua);
        //}

        //[HttpGet("timten/{Ten:minlength(3)}")]
        //public IActionResult GetTimKiem(string Ten) {
        //    var KetQua = _danhSach
        //        .Where (s => s.Ten.Contains(Ten, StringComparison.OrdinalIgnoreCase))
        //.ToList();
        //    if (!KetQua.Any())
        //    {
        //        return NotFound(new { message = $"Không tìm thấy sản phẩm nào chứa từ khóa: {Ten}" });
        //    }

        //    return Ok(KetQua);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SanPham sanPham) //bắt buộc phải async Task<T> để dùng await
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ketqua = await _sanPhamService.CreateSanPham(sanPham);
            if (!ketqua)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi thêm sản phẩm.");
            }
            return Ok("Thêm mới sản phẩm thành công");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SanPham sanPham)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ketqua = await _sanPhamService.UpdateSanPham(id, sanPham);
            if (!ketqua)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật sản phẩm.");
            }
            return Ok("Cập nhật sản phẩm thành công");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kq = await _sanPhamService.DeleteSanPham(id);
            if (!kq)
            {
                return NotFound(new { message = $"Xoá  sản phẩm có ID = {id} thất bại" });
            }
            return Ok($"Đã xóa sản phẩm Id={id}");
        }

        //[HttpPatch("{id}/gia")]
        //public IActionResult CapNhatGia(int id, [FromBody] CapNhatGiaRequest req) 
        //{
        //    var sanPham = _danhSach.FirstOrDefault(s => s.Id == id);
        //    if (sanPham == null)
        //    {
        //        return NotFound(new { message = $"Không tìm thấy sản phẩm có ID = {id}" });
        //    }

        //    if (req.GiaBanMoi < 0)
        //    {
        //        return BadRequest(new { message = "Giá sản phẩm không được nhỏ hơn 0." });
        //    }

        //    sanPham.GiaBan = (decimal)req.GiaBanMoi;

        //    return Ok(new
        //    {
        //        message = "Cập nhật giá thành công!",
        //        data = sanPham
        //    });
        //}
        //[HttpPost("nhieu")]
        //public IActionResult CreateNhieu([FromBody] List<SanPham> danhSachMoi)
        //{
        //    // 1. Tạo 2 danh sách để phân loại kết quả
        //    var thanhCong = new List<SanPham>();
        //    var thatBai = new List<object>(); // Dùng object ẩn danh để linh hoạt chứa lỗi

        //    foreach (var sp in danhSachMoi)
        //    {
        //        // 2. Khởi tạo bộ kiểm tra (ValidationContext) cho từng sản phẩm
        //        var validationContext = new ValidationContext(sp);
        //        var validationResults = new List<ValidationResult>();

        //        // Hàm này sẽ kiểm tra sp dựa trên các Data Annotations ([Required], [Range]...)
        //        bool isValid = Validator.TryValidateObject(sp, validationContext, validationResults, true);

        //        if (isValid)
        //        {
        //            // 3. Nếu HỢP LỆ -> Tạo ID mới và thêm vào Database/List
        //            int newId = _danhSach.Any() ? _danhSach.Max(s => s.Id) + 1 : 1;
        //            sp.Id = newId;
        //            _danhSach.Add(sp);

        //            thanhCong.Add(sp);
        //        }
        //        else
        //        {
        //            // 4. Nếu KHÔNG HỢP LỆ -> Gom các thông báo lỗi lại
        //            var errors = validationResults.Select(r => r.ErrorMessage).ToList();
        //            thatBai.Add(new
        //            {
        //                SanPhamLoi = sp,
        //                LyDo = errors
        //            });
        //        }
        //    }

        //    // 5. Trả về kết quả tổng hợp (Dùng Status 200 OK hoặc 207 Multi-Status)
        //    return Ok(new
        //    {
        //        ThongBao = $"Xử lý hoàn tất. Thêm thành công: {thanhCong.Count}, Lỗi: {thatBai.Count}",
        //        DanhSachThanhCong = thanhCong,
        //        DanhSachLoi = thatBai
        //    });
        //}
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

        [HttpGet("loc")]
        public async Task<IActionResult> Loc([FromQuery] string danhMuc, [FromQuery] decimal? giaToiDa)
        {
            var sanPhams = await _sanPhamService.LocAsync(danhMuc, giaToiDa);
            return Ok(sanPhams);
        }

        [HttpGet("thong-ke-theo-danh-muc")]
        public async Task<IActionResult> ThongKeTheoDanhMuc()
        {
            var result = await _sanPhamService.ThongKeTheoDanhMucAsync();
            return Ok(result);
        }

        [HttpGet("loc-gia")]
        public async Task<IActionResult> LocGia([FromQuery] decimal Giamin, [FromQuery] decimal Giamax)
        {
            var sanPhams = await _sanPhamService.LocGiaAsync(Giamin, Giamax);
            return Ok(sanPhams);
        }
        [HttpGet("Danh-sach-rut-gon")]
        public async Task<IActionResult> DanhSachRutGon()
        {
            var sanPhams = await _sanPhamService.DanhSachRutGonAsync();
            return Ok(sanPhams);
        }

        [HttpGet("dem-theo-danh-muc")]
        public async Task<IActionResult> DemTheoDanhMuc()
        {
            var soLuong = await _sanPhamService.DemTheoDanhMucAsync();
            return Ok(new { SoLuong = soLuong });
        }
        [HttpGet("top-dat-nhat-theo-danh-muc")]
        public async Task<IActionResult> TopDatNhatTheoDanhMuc()
        {
            var topSanPhams = await _sanPhamService.TopDatNhatTheoDanhMucAsync();
            return Ok(topSanPhams);
        }
    }


    // Học viên tự thêm POST/PUT/DELETE ở bài tập buổi 31
}
