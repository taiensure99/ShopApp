using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;

namespace ShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestLifetimeController : ControllerBase
    {
        private readonly IGuidService _guidService;
        private readonly IGuidService _guidService2;
        private readonly IGuidService _guidService3;

        public TestLifetimeController(IGuidService guidService, IGuidService guidService2, IGuidService guidService3)
        {
            _guidService = guidService;
            _guidService2 = guidService2;
            _guidService3 = guidService3;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var guid1 = _guidService.LayGuidMoi();
            var guid2 = _guidService2.LayGuidMoi();
            var guid3 = _guidService3.LayGuidMoi();
            return Ok(new { Guid1 = guid1, Guid2 = guid2, Guid3 = guid3 });
        }
    }
}
