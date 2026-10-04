using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        [HttpGet("tong-hop")]
        public async Task<IActionResult> TongHopDanhGia([FromQuery] int sanPhamId, [FromServices] Interfaces.IDanhGia danhGiaService)
        {
            if (sanPhamId <= 0)
            {
                return BadRequest(new { message = "ID sản phẩm phải là một số dương." });
            }
            var ketQua = await danhGiaService.TongHopDanhGia(sanPhamId);
            if (ketQua == null || !ketQua.Any())
            {
                return NotFound(new { message = $"Không có đánh giá nào cho sản phẩm với ID: {sanPhamId}" });
            }
            return Ok(ketQua);
        }
    }
}
