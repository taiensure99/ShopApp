using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly IThongKeService _thongKeService;

        public ThongKeController(IThongKeService thongKeService)
        {
            _thongKeService = thongKeService;
        }


        [HttpGet("don-hang-theo-trang-thai")]
        public async Task<IActionResult> DemDonHangTheoTrangThai([FromQuery] string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                return BadRequest(new { message = "Trạng thái không được để trống." });
            }
            int soLuong = await _thongKeService.DemDonHangTheoTrangThai(trangThai);
            if (soLuong == 0)
            {
                return NotFound(new { message = $"Không có đơn hàng nào ở trạng thái: {trangThai}" });
            }
            return Ok(new { TrangThai = trangThai, SoLuong = soLuong });
        }

        [HttpGet("doanh-thu-theo-thang-nam")]
        public async Task<IActionResult> TinhDoanhThuTheoThangNam([FromQuery] int nam)
        {
            if (nam <= 0)
            {
                return BadRequest(new { message = "Năm phải là một số dương." });
            }
            var ketQua = await _thongKeService.TinhDoanhThuTheoThangNam(nam);
            if (ketQua == null || !ketQua.Any())
            {
                return NotFound(new { message = $"Không có dữ liệu doanh thu cho năm: {nam}" });
            }
            return Ok(ketQua);
        }

        [HttpGet("khach-hang-vip")]
        public async Task<IActionResult> LayDanhSachKhachHangVIP()
        {
            var danhSachKhachHangVIP = await _thongKeService.LayDanhSachKhachHangVIP();
            if (danhSachKhachHangVIP == null || !danhSachKhachHangVIP.Any())
            {
                return NotFound(new { message = "Không có khách hàng VIP." });
            }
            return Ok(danhSachKhachHangVIP);
        }
        [HttpGet("san-pham-ban-chay")]
        public async Task<IActionResult> LaySanPhamBanChay()
        {
            var danhSachSanPhamBanChay = await _thongKeService.LaySanPhamBanChay();
            if (danhSachSanPhamBanChay == null || !danhSachSanPhamBanChay.Any())
            {
                return NotFound(new { message = "Không có sản phẩm bán chạy." });
            }
            return Ok(danhSachSanPhamBanChay);
        }
        [HttpGet("bao-cao-tong-hop")]
        public async Task<IActionResult> LayBaoCaoTongHop()
        {
            var baoCao = await _thongKeService.LayBaoCaoTongHop();
            if (baoCao == null)
            {
                return NotFound(new { message = "Không có dữ liệu báo cáo tổng hợp." });
            }
            return Ok(baoCao);
        }
    }
}
