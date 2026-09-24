using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopApp.Services;
namespace ShopApp.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]            
    public class TestController : ControllerBase 
    {
        private readonly IServiceProvider _serviceProvider;

        public TestController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet("thong-bao")]
        public IActionResult GuiThongBao([FromQuery] string loai)
        {
            IThongBaoService thongBaoService = loai?.ToLower() switch
            {
                "log" => _serviceProvider.GetRequiredService<IThongBaoService>(),
                "console" => _serviceProvider.GetRequiredService<IThongBaoService>(),
                _ => _serviceProvider.GetRequiredService<IThongBaoService>()
            };

            thongBaoService.Gui("Test chuyển đổi động DI!");
            return Ok($"Đã gửi bằng {loai}");
        }
    }
}