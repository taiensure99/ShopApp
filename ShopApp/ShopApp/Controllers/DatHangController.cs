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
        public DatHangController(IDonHangService donHangService)
        {
            _donHangService = donHangService;
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
    }
}
