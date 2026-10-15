using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopApp.Models;
using System.Collections.Generic;
using ShopApp.Services;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatHangController : Controller
    {
        private readonly IDonHangService _donHangService;
        private readonly IDatHangService _datHangService;
        public DatHangController(IDonHangService donHangService, IDatHangService datHangService)
        {
            _donHangService = donHangService;
            _datHangService = datHangService;
        }

        [HttpPost]
        public IActionResult DatHang([FromBody] DatHangRequest request)
        {
            try
            {
                var donHang = _donHangService.DatHang(request);
                return Ok(donHang);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("dat-hang-hoan-chinh")]
        public async Task<IActionResult> DatHangHoanChinh(DatHangRequest data)
        {
            var KetQua = await _datHangService.DatHangHoanChinh(data);
            return Ok(KetQua);
        }
    }
}
